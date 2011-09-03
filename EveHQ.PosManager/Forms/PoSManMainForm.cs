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
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace EveHQ.PosManager
{
    public partial class PoSManMainForm : DevComponents.DotNetBar.Office2007Form
    {
        public ArrayList MonSel_L = new ArrayList();

        POS Design = new POS();                         // New POS Structure
        private FuelBay A_Fuel = new FuelBay();         // Fuel Bay to be used for Adjustments
        private FuelBay D_Fuel = new FuelBay();         // Fuel Bay to be used for the Designer
        public DPListing DPLst = new DPListing();       // Damage Profile Listing
        PoS_Item api;
        FuelBay tt = null;
        bool load = false;
        bool update = false;
        bool timeCheck = false;
        bool UpdateTower = true;
        bool UserSplitMove = false;
        bool SecAddLoad = false;
        private int Mon_dg_indx = 0;
        private int Fil_dg_indx = 0;
        private string Mon_dg_Pos = "";
        private string Sel_React_Pos = "";
        public string selName, AllPosFillText, SelPosFillText, SelReactPos;
        public string CurrentName = "", NewName = "";
        enum MonDG { Name, LocN, FuelR, StrontR, State, Link, Cache, CPU, Power, EnrUr, Oxy, McP, Cool, Rbt, Iso, HvW, LqO, Cht, Strt, useC, React, Owner, FTech, fHrs, sHrs, hCPU, hPow, hIso };
        enum dgPM { Name, Qty, State, Opt, fOff, dmg, rof, dps, trk, prox, swDly, Chg, cost, Cap };
        enum fillDG { Name, Loc, EnrUr, Oxy, McP, Cool, Rbt, Iso, HvW, LqO, Cht, Strt, RunT };
        enum fuelDG { type, amount, vol, cost };
        private bool PosChanged = false;
        private bool ReactChanged = false;
        decimal srcID, dstID;
        string srcNm, dstNm;
        // 20 max possible links for now
        private long MaxLink = 20;


        #region Main Form Handlers

        public PoSManMainForm()
        {
            InitializeComponent();
        }

        private void frmMainForm_Load(object sender, EventArgs e)
        {
            int dgCol;
            load = true;

            DPLst.LoadDPListing();          // Load Damage Profile List

            if (PlugInData.bgw_APIUpdate.IsBusy)
                tp_ModsAndFuel.Visible = false;

            PopulateDPList("Omni-Type");
            PopulateSystemList();
            PopulateCorpList();

            SetChangedStatus(false, "");
            SetReactChangedStatus(false, "");

            // Add PoS designes longo the Designer Combo-Box pull down list for Selection
            UpdatePOSNameSelections();

            BuildPOSListForMonitoring();

            GetTowerItemListData();

            PopulateMonitoredPoSDisplay();
            PopulateReactionList();

            cb_ItemType.SelectedIndex = 1;
            cb_Interval.SelectedIndex = 0;

            Mon_dg_indx = PlugInData.Config.data.MonSelIndex;
            Mon_dg_Pos = PlugInData.Config.data.SelPos;
            if (Mon_dg_Pos != "")
            {
                // Find index to selected POS
                for (int x = 0; x < dg_MonitoredTowers.Rows.Count; x++)
                {
                    if (dg_MonitoredTowers.Rows[x].Cells[(int)MonDG.Name].Value.ToString().Contains(Mon_dg_Pos))
                    {
                        Mon_dg_indx = x;
                        break;
                    }
                }
            }

            if (Mon_dg_indx >= dg_MonitoredTowers.RowCount)
                Mon_dg_indx = dg_MonitoredTowers.RowCount - 1;

            // Set default time period to FILL
            tscb_TimePeriod.SelectedIndex = (int)PlugInData.Config.data.malongTP;
            nud_PeriodValue.Value = Convert.ToInt32(PlugInData.Config.data.malongPV);

            if (PlugInData.Config.data.FuelCat > 0)
                rb_MarketCost.Checked = true;
            else
                rb_CustomCost.Checked = true;

            if (PlugInData.Config.data.AutoAPI > 0)
                rb_AutoAPI.Checked = true;
            else
                rb_ManualAPI.Checked = true;

            if ((PlugInData.Config.data.dgMonBool == null) || (PlugInData.Config.data.dgMonBool.Count < 23))
            {
                if (PlugInData.Config.data.dgMonBool == null)
                {
                    PlugInData.Config.data.dgMonBool = new ArrayList();

                    for (int x = 0; x < 23; x++)
                        PlugInData.Config.data.dgMonBool.Add(true);
                }
                else
                {
                    for (int x = PlugInData.Config.data.dgMonBool.Count; x < 23; x++)
                        PlugInData.Config.data.dgMonBool.Add(true);
                }
                PlugInData.Config.SaveConfiguration();
            }
            else
            {
                dgCol = 0;
                foreach (bool dgcs in PlugInData.Config.data.dgMonBool)
                {
                    if (dgCol <= dg_MonitoredTowers.Columns.Count)
                    {
                        dg_MonitoredTowers.Columns[dgCol].Visible = dgcs;
                        dgCol++;
                    }
                }
            }

            if ((PlugInData.Config.data.dgDesBool == null) || (PlugInData.Config.data.dgDesBool.Count < 14))
            {
                PlugInData.Config.data.dgDesBool = new ArrayList();

                for (int x = 0; x < 14; x++)
                    PlugInData.Config.data.dgDesBool.Add(true);

                PlugInData.Config.SaveConfiguration();
            }
            else
            {
                dgCol = 0;
                foreach (bool dgcs in PlugInData.Config.data.dgDesBool)
                {
                    if (dgCol <= dg_PosMods.Columns.Count)
                    {
                        dg_PosMods.Columns[dgCol].Visible = dgcs;
                        dgCol++;
                    }
                }
            }

            SortMonitorDataGridByColumn(dg_MonitoredTowers, PlugInData.Config.data.SortedColumnIndex);

            if (Mon_dg_indx >= 0)
            {
                dg_MonitoredTowers.CurrentCell = dg_MonitoredTowers.Rows[Mon_dg_indx].Cells[(int)MonDG.Name];
                Object o = new Object();
                EventArgs ea = new EventArgs();
                load = false;
                Mon_dg_indx = -1;
                dg_MonitoredTowers_SelectionChanged(o, ea);
                load = true;
            }

            load = false;
            if (PlugInData.bgw_APIUpdate.IsBusy)
            {
                tsl_APIState.Text = "Updating API Data and Fuel Calculations";
                b_UpdateAPI.Enabled = false;
            }
            else
            {
                tsl_APIState.Text = "";
                b_UpdateAPI.Enabled = true;
            }

            if (PlugInData.Config.data.Extra.Count > 0)
                sc_MainPanels.SplitterDistance = Convert.ToInt32(PlugInData.Config.data.Extra[0]);
            else
            {
                PlugInData.Config.data.Extra.Add(sc_MainPanels.SplitterDistance);
                PlugInData.Config.SaveConfiguration();
            }
        }

        private void UpdatePOSNameSelections()
        {
            ct_PoSName.Nodes.Clear();
            ct_MonTwrForLink.Nodes.Clear();
            foreach (var p in PlugInData.PDL.Designs.Values)
            {
                AddTowerToDesignMenu(p.Name, p.CorpName, p.System);

                if (p.Monitored)
                    AddTowerToModLinkMenu(p.Name, p.CorpName, p.Moon, p.System);
            }

            foreach (DevComponents.AdvTree.Node expN in ct_MonTwrForLink.Nodes)
                expN.ExpandAll();
            foreach (DevComponents.AdvTree.Node expN in ct_PoSName.Nodes)
                expN.ExpandAll();
        }

        private void tc_MainTabs_SelectedIndexChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            int colCt = 1;
            bool colSt = false;
            DataGridViewColumn dgvc;
            string cName;

            if (selName == null)
            {
                selName = "New POS";
            }

            if (tc_MainTabs.SelectedTab == tp_POSDesign) // POS Designer
            {
                SetSelectedTowerNode(selName);
                SaveConfiguration();
            }
            else if (tc_MainTabs.SelectedTab == tp_TowerMonitor) // PoS Monitor
            {
                SaveConfiguration();
                PopulateMonitoredPoSDisplay();
            }
            else if (tc_MainTabs.SelectedTab == tp_MalongainTowers)  // POS Malongenence
            {
                PopulateTowerFillDG();
                //sc_MainPanels.SplitterDistance = (int)PlugInData.Config.data.Extra[0];
                if (dg_TowerFuelList.Rows.Count > Fil_dg_indx)
                {
                    dg_TowerFuelList.CurrentCell = dg_TowerFuelList.Rows[Fil_dg_indx].Cells[(int)fillDG.Name];
                    Object o = new Object();
                    EventArgs ea = new EventArgs();
                    dg_TowerFuelList_SelectionChanged(o, ea);
                }
                SaveConfiguration();
            }
            else if (tc_MainTabs.SelectedTab == tp_Config) // Configuration
            {
                // Config Page - Fill in Checkbox States vs. Datagrid states
                foreach (CheckBox cb in gp_TowerMonitor.Controls)
                {
                    cName = cb.Name;
                    colCt = Convert.ToInt32(cName.Replace("checkBox", ""));
                    if (colCt < 21)
                    {
                        dgvc = dg_MonitoredTowers.Columns[colCt - 1];
                        colSt = dgvc.Visible;
                        cb.Checked = colSt;
                    }
                }

                foreach (CheckBox cb in gp_ModuleDetail.Controls)
                {
                    cName = cb.Name;
                    colCt = Convert.ToInt32(cName.Replace("checkBox", ""));
                    colCt -= 24;
                    if (colCt < 15)
                    {
                        dgvc = dg_PosMods.Columns[colCt - 1];
                        colSt = dgvc.Visible;
                        cb.Checked = colSt;
                    }
                }

                PopulateIGBSecurityDisplay();
            }
            else if (tc_MainTabs.SelectedTab == tp_TowerReactions)  // Reaction Manager
            {
                PopulateTowerModuleDisplay();
            }
            else if (tc_MainTabs.SelectedTab == tp_TowerNotifications)  // Notifications
            {
                PopulatePlayerList();
                PopulateNotificationList();
            }
            else if (tc_MainTabs.SelectedTab == tp_ModsAndFuel)  // Stored Fuel
            {
                PopulateStoredFuelDisplay();
                PopulateIHUBOwners();
            }
        }

        private void PoSManMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((PosChanged) && (Design.Name != "New POS") && (Design.Name != ""))
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("The currently Selected POS has Changed. Do you want to Save it?", "Save POS", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    PlugInData.PDL.UpdateListDesign(Design);
                    PlugInData.PDL.SaveDesignListing();
                    SetChangedStatus(false, "");
                }
                else
                {
                }
            }
            PlugInData.Config.SaveConfiguration();
        }

        private void PopulateSystemList()
        {
            ArrayList syst = new ArrayList();

            cb_System.Items.Clear();

            foreach (string key in PlugInData.SLs.Systems.Keys)
                syst.Add(key);

            if (syst.Count > 0)
                syst.Add("No Systems Found - Update API");

            syst.Sort();

            cb_System.Items.AddRange(syst.ToArray());

            cb_System.SelectedIndex = 0;
        }

        public void PopulateCorpList()
        {
            // Build corp selection listing
            cb_CorpName.Items.Clear();
            cb_CorpName.Items.Add("Undefined");
            foreach (APITowerData atd in PlugInData.API_D.apiTower.Values)
            {
                if (!cb_CorpName.Items.Contains(atd.corpName))
                    cb_CorpName.Items.Add(atd.corpName);
            }
            // Add any other Corps the Player happens to have or belong too
            foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                if (selPilot.Active)
                {
                    if (!cb_CorpName.Items.Contains(selPilot.Corp))
                        cb_CorpName.Items.Add(selPilot.Corp);
                }
            }
        }

        public void PopulateReactionList()
        {
            MoonSiloReactMineral MSRM, msr;
            DevComponents.AdvTree.Node Simple, Complex, Biochem, MedBiochem, Polymer, Reaction, ReactComp;
            DevComponents.AdvTree.Cell atc;

            at_ReactionPlan.Nodes.Clear();

            Simple = new DevComponents.AdvTree.Node("Simple Reaction");
            Complex = new DevComponents.AdvTree.Node("Complex Reaction");
            Biochem = new DevComponents.AdvTree.Node("Biochem Reaction");
            MedBiochem = new DevComponents.AdvTree.Node("Med Biochem Reaction");
            Polymer = new DevComponents.AdvTree.Node("Ploymer Reaction");

            foreach (Module m in PlugInData.ML.Modules.Values)
            {
                if (m.Category.Equals("Mobile Reactor"))
                {
                    foreach (Reaction r in m.ReactList)
                    {
                        if (m.Name.Contains("Simple"))
                        {
                            Reaction = new DevComponents.AdvTree.Node(r.reactName);
                            MSRM = GetInOutForType(((InOutData)r.outputs[0]).typeID, m, false);
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((MSRM.reactQty * ((InOutData)r.outputs[0]).qty).ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString()).ToString("N2"))));

                            decimal compCost = 0;
                            foreach (InOutData iod in r.inputs)
                            {
                                msr = GetInOutForType(iod.typeID, m, true);
                                ReactComp = new DevComponents.AdvTree.Node(msr.name);
                                atc = new DevComponents.AdvTree.Cell((iod.qty * msr.reactQty).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString())).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty)).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                Reaction.Nodes.Add(ReactComp);
                                compCost = compCost + ((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty));
                            }
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell(compCost.ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((((MSRM.reactQty * ((InOutData)r.outputs[0]).qty) * (Decimal)EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString())) - compCost).ToString("N2")));
                            Simple.Nodes.Add(Reaction);
                        }
                        else if (m.Name.Contains("Complex"))
                        {
                            Reaction = new DevComponents.AdvTree.Node(r.reactName);
                            MSRM = GetInOutForType(((InOutData)r.outputs[0]).typeID, m, false);
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((MSRM.reactQty * ((InOutData)r.outputs[0]).qty).ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString()).ToString("N2"))));

                            decimal compCost = 0;
                            foreach (InOutData iod in r.inputs)
                            {
                                msr = GetInOutForType(iod.typeID, m, true);
                                ReactComp = new DevComponents.AdvTree.Node(msr.name);
                                atc = new DevComponents.AdvTree.Cell((iod.qty * msr.reactQty).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString())).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty)).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                Reaction.Nodes.Add(ReactComp);
                                compCost = compCost + ((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty));
                            }
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell(compCost.ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((((MSRM.reactQty * ((InOutData)r.outputs[0]).qty) * (Decimal)EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString())) - compCost).ToString("N2")));
                            Complex.Nodes.Add(Reaction);
                        }
                        else if (m.Name.Contains("Medium"))
                        {
                            int outputSelect = 0;
                            if (r.outputs.Count == 2) { outputSelect = 1; }

                            Reaction = new DevComponents.AdvTree.Node(r.reactName);
                            MSRM = GetInOutForType(((InOutData)r.outputs[outputSelect]).typeID, m, false);
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((MSRM.reactQty * ((InOutData)r.outputs[outputSelect]).qty).ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString()).ToString("N2"))));

                            decimal compCost = 0;
                            foreach (InOutData iod in r.inputs)
                            {
                                msr = GetInOutForType(iod.typeID, m, true);
                                ReactComp = new DevComponents.AdvTree.Node(msr.name);
                                atc = new DevComponents.AdvTree.Cell((iod.qty * msr.reactQty).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString())).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty)).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                Reaction.Nodes.Add(ReactComp);
                                compCost = compCost + ((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty));
                            }
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell(compCost.ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((((MSRM.reactQty * ((InOutData)r.outputs[outputSelect]).qty) * (Decimal)EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString())) - compCost).ToString("N2")));
                            MedBiochem.Nodes.Add(Reaction);
                        }
                        else if (m.Name.Contains("Biochem"))
                        {
                            int outputSelect = 0;
                            if (r.outputs.Count == 2) { outputSelect = 1; }

                            Reaction = new DevComponents.AdvTree.Node(r.reactName);
                            MSRM = GetInOutForType(((InOutData)r.outputs[outputSelect]).typeID, m, false);
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((MSRM.reactQty * ((InOutData)r.outputs[outputSelect]).qty).ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString()).ToString("N2"))));

                            decimal compCost = 0;
                            foreach (InOutData iod in r.inputs)
                            {
                                msr = GetInOutForType(iod.typeID, m, true);
                                ReactComp = new DevComponents.AdvTree.Node(msr.name);
                                atc = new DevComponents.AdvTree.Cell((iod.qty * msr.reactQty).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString())).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty)).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                Reaction.Nodes.Add(ReactComp);
                                compCost = compCost + ((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty));
                            }
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell(compCost.ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((((MSRM.reactQty * ((InOutData)r.outputs[outputSelect]).qty) * (Decimal)EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString())) - compCost).ToString("N2")));
                            Biochem.Nodes.Add(Reaction);
                        }
                        else if (m.Name.Contains("Polymer"))
                        {
                            Reaction = new DevComponents.AdvTree.Node(r.reactName);
                            MSRM = GetInOutForType(((InOutData)r.outputs[0]).typeID, m, false);
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((MSRM.reactQty * ((InOutData)r.outputs[0]).qty).ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString()).ToString("N2"))));

                            decimal compCost = 0;
                            foreach (InOutData iod in r.inputs)
                            {
                                msr = GetInOutForType(iod.typeID, m, true);
                                ReactComp = new DevComponents.AdvTree.Node(msr.name);
                                atc = new DevComponents.AdvTree.Cell((iod.qty * msr.reactQty).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString())).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                atc = new DevComponents.AdvTree.Cell(((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty)).ToString("N2"));
                                ReactComp.Cells.Add(atc);
                                Reaction.Nodes.Add(ReactComp);
                                compCost = compCost + ((Decimal)EveHQ.Core.DataFunctions.GetPrice(msr.typeID.ToString()) * (iod.qty * msr.reactQty));
                            }
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell(compCost.ToString("N2")));
                            Reaction.Cells.Add(new DevComponents.AdvTree.Cell((((MSRM.reactQty * ((InOutData)r.outputs[0]).qty) * (Decimal)EveHQ.Core.DataFunctions.GetPrice(MSRM.typeID.ToString())) - compCost).ToString("N2")));
                            Polymer.Nodes.Add(Reaction);
                        }
                    }
                }
            }

            at_ReactionPlan.Nodes.Add(Simple);
            at_ReactionPlan.Nodes.Add(Complex);
            at_ReactionPlan.Nodes.Add(Biochem);
            at_ReactionPlan.Nodes.Add(MedBiochem);
            at_ReactionPlan.Nodes.Add(Polymer);
        }

        private string GetInputMaterialForIOD(InOutData iod, Module RMod)
        {
            string retStr = "";

            foreach (MoonSiloReactMineral msr in RMod.InputList)
            {
                if (msr.typeID == iod.typeID)
                {
                    return msr.name;
                }
            }
            return retStr;
        }

        private MoonSiloReactMineral GetInOutForType(decimal id, Module RMod, bool inp)
        {
            if (inp)
            {
                foreach (MoonSiloReactMineral r in RMod.InputList)
                {
                    if (r.typeID == id)
                        return r;
                }
            }
            else
            {
                foreach (MoonSiloReactMineral r in RMod.OutputList)
                {
                    if (r.typeID == id)
                        return r;
                }
            }
            return null;
        }

        #endregion


        #region Designer Methods

        private void SetSelectedTowerNode(string twr)
        {
            if (ct_PoSName.Nodes.Count > 0)
            {
                foreach (DevComponents.AdvTree.Node n in ct_PoSName.Nodes)
                {
                    // Corp Name
                    if (n.HasChildNodes)
                    {
                        foreach (DevComponents.AdvTree.Node cn in n.Nodes)
                        {
                            // System
                            if (cn.HasChildNodes)
                            {
                                // Tower
                                foreach (DevComponents.AdvTree.Node ccn in cn.Nodes)
                                {
                                    if (ccn.Text.Equals(twr))
                                    {
                                        ct_PoSName.SelectedNode = ccn;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddTowerToDesignMenu(string twr, string crp, string syst)
        {
            DevComponents.AdvTree.Node corp = null, towr;
            DevComponents.AdvTree.Node systM = null;
            string twrNm;

            if (crp.Length < 1)  // If no corp name, just leave here
                crp = "Undefined";

            if (syst.Length < 1)
                syst = "Not Selected";

            foreach (DevComponents.AdvTree.Node cE in ct_PoSName.Nodes)
            {
                if (cE.Text.Equals(crp))
                {
                    corp = cE;

                    foreach (DevComponents.AdvTree.Node sE in cE.Nodes)
                    {
                        if (sE.Text.Equals(syst))
                        {
                            systM = sE;
                            break;
                        }
                    }
                    break;
                }
            }

            if (corp == null)
            {
                // Corp does not exist, so add it
                corp = new DevComponents.AdvTree.Node();
                corp.Text = crp;
                corp.Selectable = false;
                corp.Expanded = true;
                ct_PoSName.Nodes.Add(corp);
            }

            if (systM == null)
            {
                // Corp does not exist, so add it
                systM = new DevComponents.AdvTree.Node();
                systM.Text = syst;
                systM.Selectable = false;
                systM.Expanded = false;
                corp.Nodes.Add(systM);
            }

            twrNm = twr;
            towr = new DevComponents.AdvTree.Node();
            towr.Text = twrNm;
            // Tower does not exist - add it
            systM.Nodes.Add(towr);
        }

        private void SetChangedStatus(bool st, string ln)
        {
            PosChanged = st;
            l_SaveStatus.Text = ln;
        }

        private void DamageProfile_Change()
        {
            if (!update)
                CalculateAndDisplayEHPData();
        }

        private void nud_ThmRes_ValueChanged(object sender, EventArgs e)
        {
            if (DetermineTotalOverValue(100))
            {
                nud_ThmRes.Value -= 1;
                return;
            }
            DamageProfile_Change();
        }

        private void nud_EMRes_ValueChanged(object sender, EventArgs e)
        {
            if (DetermineTotalOverValue(100))
            {
                nud_EMRes.Value -= 1;
                return;
            }
            DamageProfile_Change();
        }

        private void nud_KinRes_ValueChanged(object sender, EventArgs e)
        {
            if (DetermineTotalOverValue(100))
            {
                nud_KinRes.Value -= 1;
                return;
            }
            DamageProfile_Change();
        }

        private void nud_ExpRes_ValueChanged(object sender, EventArgs e)
        {
            if (DetermineTotalOverValue(100))
            {
                nud_ExpRes.Value -= 1;
                return;
            }
            DamageProfile_Change();
        }

        private bool DetermineTotalOverValue(decimal val)
        {
            decimal emp, exp, kin, thm;

            emp = nud_EMRes.Value;
            exp = nud_ExpRes.Value;
            kin = nud_KinRes.Value;
            thm = nud_ThmRes.Value;

            if ((exp + emp + kin + thm) > val)
                return true;
            else
                return false;
        }

        private void cb_DamageProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            update = true;
            nud_EMRes.Value = 0;
            nud_ExpRes.Value = 0;
            nud_KinRes.Value = 0;
            nud_ThmRes.Value = 0;
            foreach (DamageProfile dl in DPLst.DPL)
            {
                if (cb_DamageProfile.Text == dl.Name)
                {
                    nud_EMRes.Value = dl.EMP;
                    nud_ExpRes.Value = dl.Exp;
                    nud_KinRes.Value = dl.Kin;
                    nud_ThmRes.Value = dl.Thm;
                    break;
                }
            }
            update = false;
            if (!load)
                CalculateAndDisplayEHPData();
        }

        private void PopulateDPList(string sel)
        {
            int ind = 0, newInd = 0;

            cb_DamageProfile.Items.Clear();
            foreach (DamageProfile dl in DPLst.DPL)
            {
                cb_DamageProfile.Items.Add(dl.Name);
                if (dl.Name == sel)
                    newInd = ind;
                ind++;
            }
            cb_DamageProfile.SelectedIndex = newInd;
        }

        private void b_SaveResProf_Click(object sender, EventArgs e)
        {
            DamageProfile dp = new DamageProfile();

            if ((cb_DamageProfile.Text == "Omni-Type") || (cb_DamageProfile.Text == "EM") ||
                (cb_DamageProfile.Text == "Explosive") || (cb_DamageProfile.Text == "Kinetic") ||
                (cb_DamageProfile.Text == "Thermal"))
            {
                MessageBox.Show("Sorry - You cannot Overwrite the Pre-Defined Selections.", "Save Error", MessageBoxButtons.OK);
                return;
            }

            dp.EMP = nud_EMRes.Value;
            dp.Exp = nud_ExpRes.Value;
            dp.Kin = nud_KinRes.Value;
            dp.Thm = nud_ThmRes.Value;
            dp.Name = cb_DamageProfile.Text;

            foreach (DamageProfile dl in DPLst.DPL)
            {
                if (dl.Name == dp.Name)
                {
                    DPLst.DPL.Remove(dl);
                    break;
                }
            }

            DPLst.DPL.Add(dp);
            DPLst.SaveDPListing();
            PopulateDPList(cb_DamageProfile.Text);
        }

        private decimal CalculatePosShieldRecharge()
        {
            decimal retVal = 0;

            if (Design.PosTower.Shield.Extra.Count > 0)
                retVal = (decimal)(Design.PosTower.Shield.Amount / Convert.ToDecimal(Design.PosTower.Shield.Extra[0].ToString()));

            return retVal;
        }

        private void CalculateAndDisplayEHPData()
        {
            decimal shield_ehp = 0, armor_ehp = 0, struct_ehp = 0, total_ehp = 0;
            decimal shield_hp, arm_hp, struct_hp, divis;
            decimal emp_def, exp_def, kin_def, thm_def;
            decimal emp_val, exp_val, kin_val, thm_val;

            emp_def = (nud_EMRes.Value / 100);
            exp_def = (nud_ExpRes.Value / 100);
            kin_def = (nud_KinRes.Value / 100);
            thm_def = (nud_ThmRes.Value / 100);

            emp_val = 100 - (decimal)(Design.PosTower.Shield.EMP + Design.PosTower.Shield.EMP_M);
            exp_val = 100 - (decimal)(Design.PosTower.Shield.Explosive + Design.PosTower.Shield.Explosive_M);
            kin_val = 100 - (decimal)(Design.PosTower.Shield.Kinetic + Design.PosTower.Shield.Kinetic_M);
            thm_val = 100 - (decimal)(Design.PosTower.Shield.Thermal + Design.PosTower.Shield.Thermal_M);

            shield_hp = Design.PosTower.Shield.Amount;
            divis = ((emp_def * emp_val) + (exp_def * exp_val) + (kin_def * kin_val) + (thm_def * thm_val));

            if (divis > 0)
                shield_ehp = (shield_hp * 100) / divis;
            else
                shield_ehp = shield_hp;

            emp_val = 100 - (decimal)(Design.PosTower.Armor.EMP + Design.PosTower.Armor.EMP_M);
            exp_val = 100 - (decimal)(Design.PosTower.Armor.Explosive + Design.PosTower.Armor.Explosive_M);
            kin_val = 100 - (decimal)(Design.PosTower.Armor.Kinetic + Design.PosTower.Armor.Kinetic_M);
            thm_val = 100 - (decimal)(Design.PosTower.Armor.Thermal + Design.PosTower.Armor.Thermal_M);

            arm_hp = Design.PosTower.Armor.Amount;

            if ((emp_val != 100) || (exp_val != 100) || (kin_val != 100) || (thm_val != 00))
                divis = ((emp_def * emp_val) + (exp_def * exp_val) + (kin_def * kin_val) + (thm_def * thm_val));
            else
                divis = 0;

            if (divis > 0)
                armor_ehp = (arm_hp * 100) / divis;
            else
                armor_ehp = arm_hp;

            emp_val = 100 - (decimal)(Design.PosTower.Struct.EMP + Design.PosTower.Struct.EMP_M);
            exp_val = 100 - (decimal)(Design.PosTower.Struct.Explosive + Design.PosTower.Struct.Explosive_M);
            kin_val = 100 - (decimal)(Design.PosTower.Struct.Kinetic + Design.PosTower.Struct.Kinetic_M);
            thm_val = 100 - (decimal)(Design.PosTower.Struct.Thermal + Design.PosTower.Struct.Thermal_M);

            struct_hp = Design.PosTower.Struct.Amount;
            divis = ((emp_def * emp_val) + (exp_def * exp_val) + (kin_def * kin_val) + (thm_def * thm_val));

            if (divis > 0)
                struct_ehp = (struct_hp * 100) / divis;
            else
                struct_ehp = struct_hp;

            total_ehp = shield_ehp + armor_ehp + struct_ehp;

            l_PoS_EHP.Text = String.Format("{0:#,0.#}", total_ehp);
        }

        private void CalculatePOSData()
        {
            decimal cpu_used = 0, power_used = 0, dps = 0, rateofire = 0, dmg = 0;
            decimal total_damage = 0, total_dps = 0;
            ArrayList EM, EXP, KIN, THM;
            decimal[] topt = new decimal[2];
            EM = new ArrayList();
            EXP = new ArrayList();
            KIN = new ArrayList();
            THM = new ArrayList();
            int dg_ind;

            dg_PosMods.Rows.Clear();

            dg_ind = dg_PosMods.Rows.Add();
            if (Design.PosTower.typeID != 0)
            {
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Name].Value = Design.PosTower.Name;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Qty].Value = 1;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Value = Design.PosTower.State;

                if (Design.PosTower.State == "Online")
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LawnGreen;
                else if (Design.PosTower.State == "Offline")
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.Gold;
                else if (Design.PosTower.State == "Reinforced")
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LightCoral;

                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Opt].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.fOff].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dmg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.rof].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dps].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.trk].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.prox].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.swDly].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Chg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.cost].Value = String.Format("{0:#,0.#}", Design.PosTower.Cost) + " isk";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Cap].Value = String.Format("{0:#,0.#}", Design.PosTower.Capacity) + " m3";
            }
            else
            {
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Name].Value = "No Tower";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Qty].Value = 0;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Opt].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.fOff].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dmg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.rof].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dps].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.trk].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.prox].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.swDly].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Chg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.cost].Value = String.Format("{0:#,0.#}", 0) + " isk";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Cap].Value = String.Format("{0:#,0.#}", 0) + " m3";
            }

            foreach (Module PMD in Design.Modules)
            {
                dg_ind = dg_PosMods.Rows.Add();
                dmg = CalculateDamage(PMD);
                rateofire = CalculateRateOfFire(PMD) / 1000;
                dps = CalculateDPS(dmg, rateofire);
                topt = GetTrackOptimalModBonus();

                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Name].Value = PMD.Name;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Qty].Value = PMD.Qty;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Value = PMD.State;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Opt].Value = String.Format("{0:#,0.#}", CalculateOptimalRange(PMD, topt) / 1000) + "km";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.fOff].Value = String.Format("{0:#,0.#}", CalculateFalloffRange(PMD) / 1000) + "km";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dmg].Value = String.Format("{0:#,0.#}", dmg);
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.rof].Value = String.Format("{0:#,0.#}", rateofire) + "s";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dps].Value = String.Format("{0:#,0.#}", dps);
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.trk].Value = String.Format("{0:#,0.#####}", GetTracking(PMD, topt));
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.prox].Value = String.Format("{0:#,0.#}", GetProxRange(PMD));
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.swDly].Value = String.Format("{0:#,0.#}", GetSwitchDelayTime(PMD)) + "s";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Chg].Value = PMD.Charge;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.cost].Value = String.Format("{0:#,0.#}", PMD.Cost) + " isk";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Cap].Value = String.Format("{0:#,0.#}", PMD.Capacity) + " m3";


                if (PMD.State == "Online")
                {
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LawnGreen;
                    cpu_used += PMD.Qty * PMD.CPU_Used;
                    power_used += PMD.Qty * PMD.Power_Used;

                    for (int x = 0; x < PMD.Qty; x++)
                    {
                        if ((PMD.Bonuses.EmpDmgRes > 0) && (PMD.Bonuses.EmpDmgRes < 1))
                            EM.Add(1 - PMD.Bonuses.EmpDmgRes);
                        if ((PMD.Bonuses.ExplosiveDmgRes > 0) && (PMD.Bonuses.ExplosiveDmgRes < 1))
                            EXP.Add(1 - PMD.Bonuses.ExplosiveDmgRes);
                        if ((PMD.Bonuses.KineticDmgRes > 0) && (PMD.Bonuses.KineticDmgRes < 1))
                            KIN.Add(1 - PMD.Bonuses.KineticDmgRes);
                        if ((PMD.Bonuses.ThermalDmgRes > 0) && (PMD.Bonuses.ThermalDmgRes < 1))
                            THM.Add(1 - PMD.Bonuses.ThermalDmgRes);
                    }

                    if (PMD.Name.Contains("Energy Neut"))
                    {
                    }
                    else
                    {
                        total_damage += dmg;
                        total_dps += dps;
                    }
                }
                else if (PMD.State == "Offline")
                {
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.Gold;
                }
                else if (PMD.State == "Reinforced")
                {
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LightCoral;
                }
            }

            Design.PosTower.CPU_Used = cpu_used;
            Design.PosTower.Power_Used = power_used;

            Design.PosTower.Shield.EMP_M = CalcResistanceBonus(Design.PosTower.Shield.EMP, EM);
            Design.PosTower.Shield.Kinetic_M = CalcResistanceBonus(Design.PosTower.Shield.Kinetic, KIN);
            Design.PosTower.Shield.Explosive_M = CalcResistanceBonus(Design.PosTower.Shield.Explosive, EXP);
            Design.PosTower.Shield.Thermal_M = CalcResistanceBonus(Design.PosTower.Shield.Thermal, THM);

            DisplayPOSData();
            l_PoS_DPS.Text = String.Format("{0:#,0.#}", total_damage) + "  /  " + String.Format("{0:#,0.#}", total_dps);
            CalculateAndDisplayEHPData();
            CalculateAndDisplayDesignFuelData();
        }

        private void DisplaySelectedPOSData(POS selTwr)
        {
            decimal cpu_used = 0, power_used = 0, dps = 0, rateofire = 0, dmg = 0;
            decimal total_damage = 0, total_dps = 0;
            ArrayList EM, EXP, KIN, THM;
            decimal[] topt = new decimal[2];
            EM = new ArrayList();
            EXP = new ArrayList();
            KIN = new ArrayList();
            THM = new ArrayList();
            int dg_ind;

            dg_PosMods.Rows.Clear();

            dg_ind = dg_PosMods.Rows.Add();
            if (selTwr.PosTower.typeID != 0)
            {
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Name].Value = selTwr.PosTower.Name;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Qty].Value = 1;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Value = selTwr.PosTower.State;

                if (selTwr.PosTower.State == "Online")
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LawnGreen;
                else if (selTwr.PosTower.State == "Offline")
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.Gold;
                else if (selTwr.PosTower.State == "Reinforced")
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LightCoral;

                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Opt].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.fOff].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dmg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.rof].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dps].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.trk].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.prox].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.swDly].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Chg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.cost].Value = String.Format("{0:#,0.#}", selTwr.PosTower.Cost) + " isk";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Cap].Value = String.Format("{0:#,0.#}", selTwr.PosTower.Capacity) + " m3";
            }
            else
            {
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Name].Value = "No Tower";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Qty].Value = 0;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Opt].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.fOff].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dmg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.rof].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dps].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.trk].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.prox].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.swDly].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Chg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.cost].Value = String.Format("{0:#,0.#}", 0) + " isk";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Cap].Value = String.Format("{0:#,0.#}", 0) + " m3";
            }

            foreach (Module PMD in selTwr.Modules)
            {
                dg_ind = dg_PosMods.Rows.Add();
                dmg = CalculateDamage(PMD);
                rateofire = CalculateRateOfFire(PMD) / 1000;
                dps = CalculateDPS(dmg, rateofire);
                topt = GetTrackOptimalModBonus();

                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Name].Value = PMD.Name;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Qty].Value = PMD.Qty;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Value = PMD.State;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Opt].Value = String.Format("{0:#,0.#}", CalculateOptimalRange(PMD, topt) / 1000) + "km";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.fOff].Value = String.Format("{0:#,0.#}", CalculateFalloffRange(PMD) / 1000) + "km";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dmg].Value = String.Format("{0:#,0.#}", dmg);
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.rof].Value = String.Format("{0:#,0.#}", rateofire) + "s";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dps].Value = String.Format("{0:#,0.#}", dps);
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.trk].Value = String.Format("{0:#,0.#####}", GetTracking(PMD, topt));
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.prox].Value = String.Format("{0:#,0.#}", GetProxRange(PMD));
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.swDly].Value = String.Format("{0:#,0.#}", GetSwitchDelayTime(PMD)) + "s";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Chg].Value = PMD.Charge;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.cost].Value = String.Format("{0:#,0.#}", PMD.Cost) + " isk";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Cap].Value = String.Format("{0:#,0.#}", PMD.Capacity) + " m3";


                if (PMD.State == "Online")
                {
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LawnGreen;
                    cpu_used += PMD.Qty * PMD.CPU_Used;
                    power_used += PMD.Qty * PMD.Power_Used;

                    for (int x = 0; x < PMD.Qty; x++)
                    {
                        if ((PMD.Bonuses.EmpDmgRes > 0) && (PMD.Bonuses.EmpDmgRes < 1))
                            EM.Add(1 - PMD.Bonuses.EmpDmgRes);
                        if ((PMD.Bonuses.ExplosiveDmgRes > 0) && (PMD.Bonuses.ExplosiveDmgRes < 1))
                            EXP.Add(1 - PMD.Bonuses.ExplosiveDmgRes);
                        if ((PMD.Bonuses.KineticDmgRes > 0) && (PMD.Bonuses.KineticDmgRes < 1))
                            KIN.Add(1 - PMD.Bonuses.KineticDmgRes);
                        if ((PMD.Bonuses.ThermalDmgRes > 0) && (PMD.Bonuses.ThermalDmgRes < 1))
                            THM.Add(1 - PMD.Bonuses.ThermalDmgRes);
                    }

                    if (PMD.Name.Contains("Energy Neut"))
                    {
                    }
                    else
                    {
                        total_damage += dmg;
                        total_dps += dps;
                    }
                }
                else if (PMD.State == "Offline")
                {
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.Gold;
                }
                else if (PMD.State == "Reinforced")
                {
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LightCoral;
                }
            }

            selTwr.PosTower.CPU_Used = cpu_used;
            selTwr.PosTower.Power_Used = power_used;

            selTwr.PosTower.Shield.EMP_M = CalcResistanceBonus(selTwr.PosTower.Shield.EMP, EM);
            selTwr.PosTower.Shield.Kinetic_M = CalcResistanceBonus(selTwr.PosTower.Shield.Kinetic, KIN);
            selTwr.PosTower.Shield.Explosive_M = CalcResistanceBonus(selTwr.PosTower.Shield.Explosive, EXP);
            selTwr.PosTower.Shield.Thermal_M = CalcResistanceBonus(selTwr.PosTower.Shield.Thermal, THM);

            DisplayPOSData();
            l_PoS_DPS.Text = String.Format("{0:#,0.#}", total_damage) + "  /  " + String.Format("{0:#,0.#}", total_dps);
            CalculateAndDisplayEHPData();
            CalculateAndDisplayDesignFuelData();
        }

        private decimal[] GetTrackOptimalModBonus()
        {
            decimal[] topt = new decimal[2];
            topt[0] = 0;
            topt[1] = 0;

            foreach (Module PMD in Design.Modules)
            {
                if ((PMD.Category.Contains("Tracking")) && (PMD.State == "Online"))
                {
                    topt[0] += (decimal)PMD.Bonuses.TrtOptimal;
                    topt[1] += (decimal)PMD.Bonuses.TrtTrackSpeed;
                }
            }

            return topt;
        }

        private decimal GetProxRange(Module m)
        {
            decimal retVal = 0;

            if (m.Category.Contains("Missile"))
            {
                retVal = m.Proximity;
            }
            else if (m.Category.Contains("Hybrid"))
            {
                retVal = (m.Proximity * (decimal)((Design.PosTower.Bonuses.HybProxRange / 100) + 1));
            }
            else if (m.Category.Contains("Projectile"))
            {
                retVal = (m.Proximity * (decimal)((Design.PosTower.Bonuses.PrjProxRange / 100) + 1));
            }
            else if (m.Category.Contains("Laser"))
            {
                retVal = (m.Proximity * (decimal)((Design.PosTower.Bonuses.LaserProxRange / 100) + 1));
            }
            else
                retVal = m.Proximity;

            return retVal;
        }

        private decimal GetSwitchDelayTime(Module m)
        {
            decimal retVal = 0;
            decimal swDel;

            swDel = m.SwitchDelay;

            if (Design.PosTower.Bonuses.EWTargetSwitch != 0)
                retVal = swDel * (decimal)((Design.PosTower.Bonuses.EWTargetSwitch / 100) + 1);
            else
                retVal = swDel;

            return retVal;
        }

        private decimal GetTracking(Module m, decimal[] topt)
        {
            decimal retVal = 0;
            decimal track;

            track = (decimal)m.Tracking;

            track *= ((topt[1] / 100) + 1);

            retVal = track;

            return retVal;
        }

        private int GetChargeIndexForModule(Module m)
        {
            int chInd = -1;
            // Get Charge Data if Present
            if ((m.Charge != null) && (m.Charge != ""))
            {
                foreach (Charge ch in m.Charges)
                {
                    chInd++;
                    if (ch.Name == m.Charge)
                        break;
                }
            }

            return chInd;
        }

        private decimal CalculateOptimalRange(Module m, decimal[] topt)
        {
            decimal retVal = 0;
            decimal fltTime, vel;
            decimal ftMod, velMod;
            Charge ch;
            int cI;

            cI = GetChargeIndexForModule(m);
            if (cI < 0)
            {
                if (m.ChargeList.Count <= 0)
                {
                    retVal = (decimal)m.Optimal;
                }
                return retVal;
            }
            ch = (Charge)m.Charges[cI];

            if (m.Category.Contains("Missile"))
            {
                // Missile Battery - charges dictate range entirely, not module type
                fltTime = ch.FlightTime;
                vel = ch.Velocity;
                ftMod = (decimal)m.Bonuses.MslFlightTime;
                velMod = (decimal)m.Bonuses.MslVelocity;

                vel *= velMod;
                fltTime *= ftMod;

                velMod = (decimal)((Design.PosTower.Bonuses.MslVel / 100) + 1);
                vel *= velMod;

                retVal = vel * fltTime;
            }
            else if (m.Category.Contains("Hybrid"))
            {
                vel = m.Optimal;
                velMod = (decimal)((Design.PosTower.Bonuses.HybOpt / 100) + 1);
                retVal = vel * velMod;

                ftMod = ch.RangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                fltTime = (topt[0] / 100) + 1;

                retVal = vel * velMod * ftMod * fltTime;
            }
            else if (m.Category.Contains("Projectile"))
            {
                vel = m.Optimal;
                velMod = (decimal)((Design.PosTower.Bonuses.PrjOpt / 100) + 1);

                ftMod = ch.RangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                fltTime = (topt[0] / 100) + 1;

                retVal = vel * velMod * ftMod * fltTime;
            }
            else if (m.Category.Contains("Laser"))
            {
                vel = m.Optimal;
                velMod = (decimal)((Design.PosTower.Bonuses.LaserOpt / 100) + 1);
                retVal = vel * velMod;

                ftMod = ch.RangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                fltTime = (topt[0] / 100) + 1;

                retVal = vel * velMod * ftMod * fltTime;
            }
            else
                retVal = m.Optimal;

            return retVal;
        }

        private decimal CalculateFalloffRange(Module m)
        {
            decimal retVal = 0;
            decimal vel;
            decimal ftMod, velMod;
            Charge ch;
            int cI;

            cI = GetChargeIndexForModule(m);
            if (cI < 0)
            {
                if (m.ChargeList.Count <= 0)
                {
                    retVal = (decimal)m.FallOff;
                }
                return retVal;
            }
            ch = (Charge)m.Charges[cI];

            if (m.Category.Contains("Projectile"))
            {
                vel = m.FallOff;
                velMod = (decimal)((Design.PosTower.Bonuses.PrjFallOff / 100) + 1);

                ftMod = ch.FlyRangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                retVal = vel * velMod * ftMod;
            }
            else if (m.Category.Contains("Hybrid"))
            {
                vel = m.FallOff;

                ftMod = ch.FlyRangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                retVal = vel * ftMod;
            }
            else if (m.Category.Contains("Laser"))
            {
                vel = m.FallOff;

                ftMod = ch.FlyRangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                retVal = vel * ftMod;
            }
            else
                retVal = m.FallOff;

            return retVal;
        }

        private decimal CalculateDamage(Module m)
        {
            decimal retVal = 0;
            decimal em, exp, kin, thm, arm, shd;
            decimal dmgMod, tDmgMod = 1;

            Charge ch;
            int cI;

            cI = GetChargeIndexForModule(m);
            if (cI < 0)
            {
                if (m.Category.Contains("Energy"))
                {
                    retVal = (decimal)m.EnergyNeut;
                }
                return retVal;
            }

            ch = (Charge)m.Charges[cI];

            dmgMod = (decimal)m.DamageMod;

            if (m.Category.Contains("Missile"))
            {
                dmgMod = (decimal)m.Bonuses.MslDamage;
                tDmgMod = 1;
            }
            else if (m.Category.Contains("Hybrid"))
            {
                tDmgMod = (decimal)((Design.PosTower.Bonuses.HybDmg / 100) + 1);
            }
            else if (m.Category.Contains("Projectile"))
            {
                tDmgMod = 1;
            }
            else if (m.Category.Contains("Laser"))
            {
                tDmgMod = (decimal)((Design.PosTower.Bonuses.LaserDmg / 100) + 1);
            }
            if (dmgMod == 0)
                dmgMod = 1;

            em = ch.EM_Dmg;
            exp = ch.Exp_Dmg;
            kin = ch.Kin_Dmg;
            thm = ch.Thm_Dmg;
            arm = ch.Base_Armor;
            shd = ch.Base_Shield;

            em *= dmgMod * tDmgMod;
            exp *= dmgMod * tDmgMod;
            kin *= dmgMod * tDmgMod;
            thm *= dmgMod * tDmgMod;

            retVal = em + exp + kin + thm;

            retVal *= m.Qty;

            return retVal;
        }

        private decimal CalculateRateOfFire(Module m)
        {
            decimal retVal = 0;
            decimal mROF, rofMod;

            mROF = (decimal)m.ROF;
            retVal = mROF;

            if (mROF <= 0)
                return retVal;

            if (m.Category.Contains("Missile"))
            {
                // Missile weapon
                rofMod = ((100 + (decimal)Design.PosTower.Bonuses.MslROF) / 100);
                retVal = mROF * rofMod;
            }
            else if (m.Category.Contains("Projectile"))
            {
                // Projectiles
                rofMod = ((100 + (decimal)Design.PosTower.Bonuses.PrjROF) / 100);
                retVal = mROF * rofMod;
            }
            else if (m.Category.Contains("Electronic Warfare"))
            {
                // E-War
                rofMod = ((100 + (decimal)Design.PosTower.Bonuses.EWRof) / 100);
                retVal = mROF * rofMod;
            }

            return retVal;
        }

        private decimal CalculateDPS(decimal dmg, decimal rof)
        {
            decimal retVal = 0;

            if (rof == 0)
                retVal = 0;
            else
                retVal = dmg / rof;

            return retVal;
        }

        private decimal CalculateSiloCapacity(Module m, POS p)
        {
            decimal mult;

            if (m.Category.Contains("Silo"))
            {
                mult = 1 + (decimal)(p.PosTower.Bonuses.SiloCap / 100);

                return m.Capacity * mult;
            }
            else
                return m.Capacity;
        }

        private double CalcResistanceBonus(double res, ArrayList rl)
        {
            double pVal, cVal, bVal;

            if (rl.Count < 1)          // Sorry, No bonus to calculate
                return 0;

            bVal = res;
            pVal = res;
            for (int x = 0; x < rl.Count; x++)
            {
                cVal = ((100 - pVal) * (double)rl[x]);
                pVal += cVal;
            }

            return (pVal - bVal);
            //long n;
            //double retVal = 0;
            //double pVal, cVal, bVal;
            //double Xo;

            //n = rl.Count;
            //if (n < 1)          // Sorry, No bonus to calculate
            //    return 0;

            //Xo = (double)((100 - res)/100);
            //bVal = Xo;
            //for (int x = 0; x < n; x++)
            //{
            //    cVal = (1 + ((double)rl[x] * Math.Exp(-(Math.Pow(x, 2) / 7.1289))));
            //    pVal = (bVal * cVal) - bVal;
            //    retVal += pVal;
            //}

            //retVal *= 100;

            //return retVal;
        }

        private void DisplayPOSData()
        {
            decimal cpu, cpu_used, power, power_used, pbc, pbp;
            double calc;
            string Online;

            cpu = Design.PosTower.CPU;
            cpu_used = Design.PosTower.CPU_Used;
            power = Design.PosTower.Power;
            power_used = Design.PosTower.Power_Used;
            Online = Design.PosTower.State;

            if (Online == "Offline")
            {
                cpu = 0;
                power = 0;
            }

            pbc = ComputeBayPercentage(cpu_used, cpu);
            pbp = ComputeBayPercentage(power_used, power);

            pb_CPU.Text = cpu_used + " / " + cpu;
            pb_CPU.Value = Convert.ToInt32(pbc);
            pb_Power.Text = power_used + " / " + power;
            pb_Power.Value = Convert.ToInt32(pbp);

            if (cpu_used > cpu)
                pb_CPU.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error;
            else
                pb_CPU.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;

            if (power_used > power)
                pb_Power.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error;
            else
                pb_Power.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;

            l_TowerShield.Text = String.Format("{0:#,0.#}", Design.PosTower.Shield.Amount);
            calc = Design.PosTower.Shield.EMP;
            calc += Design.PosTower.Shield.EMP_M;
            l_TowerSEmRes.Text = String.Format("{0:#,0.#}", calc) + "%";

            calc = Design.PosTower.Shield.Explosive;
            calc += Design.PosTower.Shield.Explosive_M;
            l_TowerSExRes.Text = String.Format("{0:#,0.#}", calc) + "%";

            calc = Design.PosTower.Shield.Kinetic;
            calc += Design.PosTower.Shield.Kinetic_M;
            l_TowerSKRes.Text = String.Format("{0:#,0.#}", calc) + "%";

            calc = Design.PosTower.Shield.Thermal;
            calc += Design.PosTower.Shield.Thermal_M;
            l_TowerSTRes.Text = String.Format("{0:#,0.#}", calc) + "%";

            l_TowerArmor.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.Amount);
            l_TowerAEmRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.EMP) + "%";
            l_TowerAExRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.Explosive) + "%";
            l_TowerAKRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.Kinetic) + "%";
            l_TowerATRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.Thermal) + "%";

            l_TowerStruct.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.Amount);
            l_TowerStEmRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.EMP) + "%";
            l_TowerStExRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.Explosive) + "%";
            l_TowerStKRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.Kinetic) + "%";
            l_TowerStTRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.Thermal) + "%";

            l_PoS_Tank.Text = CalculatePosShieldRecharge() + " pt/sec";
        }

        private void ClearPosData()
        {
            pb_CPU.Text = "";
            pb_Power.Text = "";
            pb_CPU.Value = 0;
            pb_Power.Value = 0;

            l_TowerShield.Text = String.Format("{0:#,0.#}", 0);
            l_TowerSEmRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerSExRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerSKRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerSTRes.Text = String.Format("{0:#,0.#}", 0) + "%";

            l_TowerArmor.Text = String.Format("{0:#,0.#}", 0);
            l_TowerAEmRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerAExRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerAKRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerATRes.Text = String.Format("{0:#,0.#}", 0) + "%";

            l_TowerStruct.Text = String.Format("{0:#,0.#}", 0);
            l_TowerStEmRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerStExRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerStKRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerStTRes.Text = String.Format("{0:#,0.#}", 0) + "%";

            l_AnchorTime.Text = "";
            l_OnlineTime.Text = "";
            l_Total_UnAnchor_T.Text = "";
            l_SetupTime.Text = "";
            l_PoS_EHP.Text = "";
            l_PoS_DPS.Text = "0 / 0";
            l_PoS_Tank.Text = "";
            l_Fuel_C.Text = "0 isk";
            l_POS_C.Text = "0 isk";
            l_FuelStront_C.Text = "0 isk";

        }

        private void ClearFuelData()
        {
            l_EnrUranium.Text = String.Format("{0:#,0.#}", 0);
            l_Oxygen.Text = String.Format("{0:#,0.#}", 0);
            l_MechParts.Text = String.Format("{0:#,0.#}", 0);
            l_Coolant.Text = String.Format("{0:#,0.#}", 0);
            l_Robotics.Text = String.Format("{0:#,0.#}", 0);
            l_HeavyWater.Text = String.Format("{0:#,0.#}", 0);
            l_LiquidOzone.Text = String.Format("{0:#,0.#}", 0);
            l_FactionCharters.Text = String.Format("{0:#,0.#}", 0);
            nud_StrontInterval.Value = 0;
            nud_StrontInterval.Increment = 0;
            l_Isotopes.Text = String.Format("{0:#,0.#}", 0);
            l_IsotopeType.Text = "Isotopes";
            pb_Fuel.Value = 0;
            pb_Stront.Value = 0;
            pb_Fuel.Text = "0 / 0";
            pb_Stront.Text = "0 / 0";
        }

        private void ClearModListing()
        {
            dg_PosMods.Rows.Clear();
        }

        private void CalculateAndDisplayPosTimesAndCosts()
        {
            decimal totalCost = 0, onlineT = 0, anchorT = 0, unAnchorT = 0;

            totalCost += Design.PosTower.Cost;
            anchorT += Design.PosTower.Anchor_Time;

            if (Design.PosTower.State == "Online")
                onlineT += Design.PosTower.Online_Time;

            unAnchorT += Design.PosTower.UnAnchor_Time;

            foreach (Module m in Design.Modules)
            {
                totalCost += m.Cost;
                anchorT += m.Anchor_Time * m.Qty;
                if (m.State == "Online")
                    onlineT += m.Online_Time * m.Qty;

                unAnchorT += m.UnAnchor_Time * m.Qty;
            }

            l_POS_C.Text = String.Format("{0:#,0.#}", totalCost);
            l_AnchorTime.Text = PlugInData.ConvertSecondsToTextDisplay(anchorT);
            l_OnlineTime.Text = PlugInData.ConvertSecondsToTextDisplay(onlineT);
            l_SetupTime.Text = PlugInData.ConvertSecondsToTextDisplay(anchorT + onlineT);
            l_Total_UnAnchor_T.Text = PlugInData.ConvertSecondsToTextDisplay(unAnchorT);
        }

        private void CalculateAndDisplayDesignFuelData()
        {
            decimal fbu, sbu;
            string lin1, lin2;
            string run_time;

            Design.CalculatePOSDesignFuelValues(PlugInData.Config.data.FuelCosts);

            // Display Calculated Data
            l_EnrUranium.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.EnrUran.Qty);
            //l_EnrUrn_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.EnrUran.CostForQty);

            l_Oxygen.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Oxygen.Qty);
            //l_Oxygen_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Oxygen.CostForQty);

            l_MechParts.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.MechPart.Qty);
            //l_McP_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.MechPart.CostForQty);

            l_Coolant.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Coolant.Qty);
            //l_Cool_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Coolant.CostForQty);

            l_Robotics.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Robotics.Qty);
            //l_Robt_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Robotics.CostForQty);

            l_HeavyWater.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.HvyWater.Qty);
            //l_HvyW_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.HvyWater.CostForQty);

            l_LiquidOzone.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.LiqOzone.Qty);
            //l_LiqO_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.LiqOzone.CostForQty);

            l_FactionCharters.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Charters.Qty);
            //l_Chart_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Charters.CostForQty);

            l_Stront_D.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Strontium.Qty);
            //l_Strnt_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Strontium.CostForQty);

            if (Design.PosTower.D_Fuel.N2Iso.Qty > 0)
            {
                l_Isotopes.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.N2Iso.Qty);
                l_IsotopeType.Text = "N2 Iso";
                //l_Iso_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.N2Iso.CostForQty);
            }
            else if (Design.PosTower.D_Fuel.H2Iso.Qty > 0)
            {
                l_Isotopes.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.H2Iso.Qty);
                l_IsotopeType.Text = "H2 Iso";
                //l_Iso_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.H2Iso.CostForQty);
            }
            else if (Design.PosTower.D_Fuel.HeIso.Qty > 0)
            {
                l_Isotopes.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.HeIso.Qty);
                l_IsotopeType.Text = "He Iso";
                //l_Iso_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.HeIso.CostForQty);
            }
            else if (Design.PosTower.D_Fuel.O2Iso.Qty > 0)
            {
                l_Isotopes.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.O2Iso.Qty);
                l_IsotopeType.Text = "O2 Iso";
                //l_Iso_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.O2Iso.CostForQty);
            }
            else
            {
                l_Isotopes.Text = "0";
                l_IsotopeType.Text = "Unknown";
                //l_Iso_C.Text = String.Format("{0:#,0.#}", 0);
            }

            l_Fuel_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.FuelCost);
            l_FuelStront_C.Text = String.Format("{0:#,0.#}", (Design.PosTower.D_Fuel.FuelCost + Design.PosTower.D_Fuel.Strontium.CostForQty));

            fbu = ComputeBayPercentage(Design.PosTower.D_Fuel.FuelUsed, Design.PosTower.D_Fuel.FuelCap);
            sbu = ComputeBayPercentage(Design.PosTower.D_Fuel.StrontUsed, Design.PosTower.D_Fuel.StrontCap);

            pb_Fuel.Value = Convert.ToInt32(fbu);
            pb_Stront.Value = Convert.ToInt32(sbu);

            lin1 = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.FuelUsed);
            lin2 = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.StrontUsed);
            pb_Fuel.Text = lin1 + " / " + String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.FuelCap);
            pb_Stront.Text = lin2 + " / " + String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.StrontCap);

            if (cb_Interval.SelectedIndex != 4)
            {
                l_AmountForMax.Show();
                l_AmountForMax.BringToFront();
                run_time = PlugInData.ConvertHoursToShortTextDisplay(Design.PosTower.Design_Int_Qty, cb_Interval.SelectedIndex);
                l_AmountForMax.Text = run_time;
            }
            else
            {
                l_AmountForMax.Hide();
                l_AmountForMax.SendToBack();
            }

            CalculateAndDisplayPosTimesAndCosts();
        }

        private void GetTowerItemListData()
        {
            cb_ItemType.Items.Clear();

            foreach (CategoryItem ci in PlugInData.CL.Cats)
            {
                cb_ItemType.Items.Add(ci.CatName);
            }

            if (cb_ItemType.Items.Count > 0)
                cb_ItemType.SelectedIndex = 1;
        }

        private void cb_ItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CategoryItem ci;
            int selIndex;
            long selGroup;
            int imgIndx = 0;
            Image img;
            ListViewItem lvi;

            selIndex = cb_ItemType.SelectedIndex;
            ci = (CategoryItem)PlugInData.CL.Cats[selIndex];
            selGroup = ci.groupID;

            lv_ItemSelList.Items.Clear();
            il_SelCat.Images.Clear();

            if (cb_ItemType.Text.Contains("Tower"))
            {
                foreach (Tower t in PlugInData.TL.Towers.Values)
                {
                    img = EveHQ.Core.ImageHandler.GetImage(t.typeID.ToString());

                    if (img == null)
                    {
                        il_SelCat.Images.Add(il_system.Images[0]);
                    }
                    else
                        il_SelCat.Images.Add(img);

                    lvi = new ListViewItem(t.Name);
                    lvi.Name = cb_ItemType.Text;
                    lvi.ImageIndex = imgIndx;
                    lvi.SubItems.Add(t.typeID.ToString());
                    lvi.SubItems[1].Name = t.groupID.ToString();
                    lv_ItemSelList.Items.Add(lvi);
                    imgIndx++;
                }
            }
            else
            {
                if (PlugInData.ML.Modules.Count < 1)
                {
                    // No modules found, attempt to retrieve them
                    MessageBox.Show("Did not find any modules, Attempting to find them - this will take a bit, please wait.", "Modules Error", MessageBoxButtons.OK);
                    PlugInData.ML.PopulateModuleListing(1);
                }

                foreach (Module m in PlugInData.ML.Modules.Values)
                {
                    if (selGroup == m.groupID)
                    {
                        img = EveHQ.Core.ImageHandler.GetImage(m.typeID.ToString());

                        if (img == null)
                        {
                            il_SelCat.Images.Add(il_system.Images[0]);
                        }
                        else
                            il_SelCat.Images.Add(img);

                        lvi = new ListViewItem(m.Name);
                        lvi.Name = cb_ItemType.Text;
                        lvi.ImageIndex = imgIndx;
                        lvi.SubItems.Add(m.typeID.ToString());
                        lvi.SubItems[1].Name = m.groupID.ToString();
                        lv_ItemSelList.Items.Add(lvi);
                        imgIndx++;
                    }
                }
            }
        }

        private void lv_ItemSelList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DragDropEffects DDE;
            ListViewItem LVI;
            long typeID;
            string location;

            if (CurrentName == "")
            {
                MessageBox.Show("You MUST select New to create a tower or POS design.", "Invalid Creation");
                Design.ClearAllPOSData();
                return;
            }

            DDE = DoDragDrop(e.Item, DragDropEffects.Copy);

            if (DDE == DragDropEffects.Copy)
            {
                LVI = (ListViewItem)e.Item;
                typeID = Convert.ToInt32(LVI.SubItems[1].Text);
                location = LVI.SubItems[1].Name.ToString();

                if (LVI.Name.Contains("Tower"))
                {
                    if (PlugInData.TL.Towers.ContainsKey(typeID))
                    {
                        Design.PosTower = new Tower(PlugInData.TL.Towers[typeID]);
                        Design.PosTower.Location = location;
                        Design.PosTower.Category = cb_ItemType.Text;
                        Design.PosTower.Design_Int_Qty = 1;
                        Design.PosTower.Design_Interval = cb_Interval.SelectedIndex;

                        nud_StrontInterval.Maximum = Design.ComputeMaxPosStrontTime();
                        nud_DesFuelPeriod.Maximum = Design.ComputeMaxPosRunTimeForLoad();

                        Design.PosTower.Design_Stront_Qty = 0;
                        if (!load)
                        {
                            SetChangedStatus(true, "Unsaved Changes - Save Needed");
                        }
                    }
                }
                else
                {
                    if (PlugInData.ML.Modules.ContainsKey(typeID))
                    {
                        Module nw = new Module(PlugInData.ML.Modules[typeID]);
                        nw.Location = location;
                        nw.Category = cb_ItemType.Text;
                        Design.Modules.Add(nw);
                        if (!load)
                        {
                            SetChangedStatus(true, "Unsaved Changes - Save Needed");
                        }
                    }
                }
                CalculatePOSData();
            }
        }

        private void GetItemData(ListViewItem LVI)
        {
            long typeID;

            typeID = Convert.ToInt32(LVI.SubItems[1].Text);

            GetItemData(typeID, cb_ItemType.Text);
        }

        private void GetItemData(long typeID, string cat)
        {
            string fuel = "";
            Tower t;
            Module m;

            if (cat.Contains("Tower"))
            {
                if (PlugInData.TL.Towers.ContainsKey(typeID))
                {
                    t = PlugInData.TL.Towers[typeID];
                    if (t.CPU > t.CPU_Used)
                    {
                        l_cpu.ForeColor = Color.Green;
                        l_cpu.Text = String.Format("{0:#,0.#}", t.CPU);
                    }
                    else
                    {
                        l_cpu.ForeColor = Color.Red;
                        l_cpu.Text = String.Format("{0:#,0.#}", t.CPU_Used);
                    }

                    if (t.Power > t.Power_Used)
                    {
                        l_power.ForeColor = Color.Green;
                        l_power.Text = String.Format("{0:#,0.#}", t.Power);
                    }
                    else
                    {
                        l_power.ForeColor = Color.Red;
                        l_power.Text = String.Format("{0:#,0.#}", t.Power_Used);
                    }

                    l_armor_hp.Text = String.Format("{0:#,0.#}", t.Armor.Amount);
                    l_shield_hp.Text = String.Format("{0:#,0.#}", t.Shield.Amount);
                    l_shield_em.Text = String.Format("{0:#,0.#}", t.Shield.EMP) + "%";
                    l_shield_exp.Text = String.Format("{0:#,0.#}", t.Shield.Explosive) + "%";
                    l_shield_kin.Text = String.Format("{0:#,0.#}", t.Shield.Kinetic) + "%";
                    l_shield_therm.Text = String.Format("{0:#,0.#}", t.Shield.Thermal) + "%";
                    l_struct_hp.Text = String.Format("{0:#,0.#}", t.Struct.Amount);
                    l_struct_emp.Text = String.Format("{0:#,0.#}", t.Struct.EMP) + "%";
                    l_struct_exp.Text = String.Format("{0:#,0.#}", t.Struct.Explosive) + "%";
                    l_struct_kin.Text = String.Format("{0:#,0.#}", t.Struct.Kinetic) + "%";
                    l_struct_therm.Text = String.Format("{0:#,0.#}", t.Struct.Thermal) + "%";
                    l_SI_Cost.Text = String.Format("{0:#,0.#}", t.Cost) + " isk";

                    fuel += "Fuel Usage -->\n";
                    fuel += "Enriched Uranium: " + t.Fuel.EnrUran.PeriodQty + "\n";
                    fuel += "Oxygen: " + t.Fuel.Oxygen.PeriodQty + "\n";
                    fuel += "Mechanical Parts: " + t.Fuel.MechPart.PeriodQty + "\n";
                    fuel += "Coolant: " + t.Fuel.Coolant.PeriodQty + "\n";
                    fuel += "Robotics: " + t.Fuel.Robotics.PeriodQty + "\n";
                    fuel += "Heavy Water: " + t.Fuel.HvyWater.PeriodQty + "\n";
                    fuel += "Liquid Ozone: " + t.Fuel.LiqOzone.PeriodQty + "\n";
                    fuel += "Charters: " + t.Fuel.Charters.PeriodQty + "\n";
                    fuel += "Strontium: " + t.Fuel.Strontium.PeriodQty + "\n";

                    if (t.Fuel.N2Iso.PeriodQty > 0)
                    {
                        fuel += "Nitrogen Isotopes: " + t.Fuel.N2Iso.PeriodQty + "\n";
                    }
                    else if (t.Fuel.HeIso.PeriodQty > 0)
                    {
                        fuel += "Helium Isotopes: " + t.Fuel.HeIso.PeriodQty + "\n";
                    }
                    else if (t.Fuel.H2Iso.PeriodQty > 0)
                    {
                        fuel += "Hydrogen Isotopes: " + t.Fuel.H2Iso.PeriodQty + "\n";
                    }
                    else if (t.Fuel.O2Iso.PeriodQty > 0)
                    {
                        fuel += "Oxygen Isotopes: " + t.Fuel.O2Iso.PeriodQty + "\n";
                    }

                    rtb_Other.Text = t.Desc + "\n\n" + fuel + "\n" + t.OtherInfo;
                    rtb_Other.Text += "Volume   <" + t.Volume + " m3>\n";
                    rtb_Other.Text += "Capacity <" + t.Capacity + " m3>\n";
                }
            }
            else
            {
                if (PlugInData.ML.Modules.ContainsKey(typeID))
                {
                    m = PlugInData.ML.Modules[typeID];
                    if (m.CPU > m.CPU_Used)
                    {
                        l_cpu.ForeColor = Color.Green;
                        l_cpu.Text = String.Format("{0:#,0.#}", m.CPU);
                    }
                    else
                    {
                        l_cpu.ForeColor = Color.Red;
                        l_cpu.Text = String.Format("{0:#,0.#}", m.CPU_Used);
                    }

                    if (m.Power > m.Power_Used)
                    {
                        l_power.ForeColor = Color.Green;
                        l_power.Text = String.Format("{0:#,0.#}", m.Power);
                    }
                    else
                    {
                        l_power.ForeColor = Color.Red;
                        l_power.Text = String.Format("{0:#,0.#}", m.Power_Used);
                    }

                    l_armor_hp.Text = String.Format("{0:#,0.#}", m.Armor.Amount);
                    l_shield_hp.Text = String.Format("{0:#,0.#}", m.Shield.Amount);
                    l_shield_em.Text = String.Format("{0:#,0.#}", m.Shield.EMP) + "%";
                    l_shield_exp.Text = String.Format("{0:#,0.#}", m.Shield.Explosive) + "%";
                    l_shield_kin.Text = String.Format("{0:#,0.#}", m.Shield.Kinetic) + "%";
                    l_shield_therm.Text = String.Format("{0:#,0.#}", m.Shield.Thermal) + "%";
                    l_struct_hp.Text = String.Format("{0:#,0.#}", m.Struct.Amount);
                    l_struct_emp.Text = String.Format("{0:#,0.#}", m.Struct.EMP) + "%";
                    l_struct_exp.Text = String.Format("{0:#,0.#}", m.Struct.Explosive) + "%";
                    l_struct_kin.Text = String.Format("{0:#,0.#}", m.Struct.Kinetic) + "%";
                    l_struct_therm.Text = String.Format("{0:#,0.#}", m.Struct.Thermal) + "%";
                    l_SI_Cost.Text = String.Format("{0:#,0.#}", m.Cost) + " isk";

                    rtb_Other.Text = m.Desc + "\n\n" + fuel + "\n" + m.OtherInfo;
                    rtb_Other.Text += "Volume   <" + m.Volume + " m3>\n";
                    rtb_Other.Text += "Capacity <" + m.Capacity + " m3>\n";
                }
            }
        }

        private void GetDesignItemData(long typeID, string cat)
        {
            string fuel = "", tName;
            decimal cap;
            POS p;

            tName = ct_PoSName.Text;

            if (PlugInData.PDL.Designs.ContainsKey(tName))
            {
                p = PlugInData.PDL.Designs[tName];
                if (cat.Contains("Tower"))
                {
                    if (p.PosTower.CPU > p.PosTower.CPU_Used)
                    {
                        l_cpu.ForeColor = Color.Green;
                        l_cpu.Text = String.Format("{0:#,0.#}", p.PosTower.CPU);
                    }
                    else
                    {
                        l_cpu.ForeColor = Color.Red;
                        l_cpu.Text = String.Format("{0:#,0.#}", p.PosTower.CPU_Used);
                    }

                    if (p.PosTower.Power > p.PosTower.Power_Used)
                    {
                        l_power.ForeColor = Color.Green;
                        l_power.Text = String.Format("{0:#,0.#}", p.PosTower.Power);
                    }
                    else
                    {
                        l_power.ForeColor = Color.Red;
                        l_power.Text = String.Format("{0:#,0.#}", p.PosTower.Power_Used);
                    }

                    l_armor_hp.Text = String.Format("{0:#,0.#}", p.PosTower.Armor.Amount);
                    l_shield_hp.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.Amount);
                    l_shield_em.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.EMP) + "%";
                    l_shield_exp.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.Explosive) + "%";
                    l_shield_kin.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.Kinetic) + "%";
                    l_shield_therm.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.Thermal) + "%";
                    l_struct_hp.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.Amount);
                    l_struct_emp.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.EMP) + "%";
                    l_struct_exp.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.Explosive) + "%";
                    l_struct_kin.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.Kinetic) + "%";
                    l_struct_therm.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.Thermal) + "%";
                    l_SI_Cost.Text = String.Format("{0:#,0.#}", p.PosTower.Cost) + " isk";

                    fuel += "Fuel Usage -->\n";
                    fuel += "Enriched Uranium: " + p.PosTower.Fuel.EnrUran.PeriodQty + "\n";
                    fuel += "Oxygen: " + p.PosTower.Fuel.Oxygen.PeriodQty + "\n";
                    fuel += "Mechanical Parts: " + p.PosTower.Fuel.MechPart.PeriodQty + "\n";
                    fuel += "Coolant: " + p.PosTower.Fuel.Coolant.PeriodQty + "\n";
                    fuel += "Robotics: " + p.PosTower.Fuel.Robotics.PeriodQty + "\n";
                    fuel += "Heavy Water: " + p.PosTower.Fuel.HvyWater.PeriodQty + "\n";
                    fuel += "Liquid Ozone: " + p.PosTower.Fuel.LiqOzone.PeriodQty + "\n";
                    fuel += "Charters: " + p.PosTower.Fuel.Charters.PeriodQty + "\n";
                    fuel += "Strontium: " + p.PosTower.Fuel.Strontium.PeriodQty + "\n";

                    if (p.PosTower.Fuel.N2Iso.PeriodQty > 0)
                    {
                        fuel += "Nitrogen Isotopes: " + p.PosTower.Fuel.N2Iso.PeriodQty + "\n";
                    }
                    else if (p.PosTower.Fuel.HeIso.PeriodQty > 0)
                    {
                        fuel += "Helium Isotopes: " + p.PosTower.Fuel.HeIso.PeriodQty + "\n";
                    }
                    else if (p.PosTower.Fuel.H2Iso.PeriodQty > 0)
                    {
                        fuel += "Hydrogen Isotopes: " + p.PosTower.Fuel.H2Iso.PeriodQty + "\n";
                    }
                    else if (p.PosTower.Fuel.O2Iso.PeriodQty > 0)
                    {
                        fuel += "Oxygen Isotopes: " + p.PosTower.Fuel.O2Iso.PeriodQty + "\n";
                    }

                    rtb_Other.Text = p.PosTower.Desc + "\n\n" + fuel + "\n" + p.PosTower.OtherInfo;
                    rtb_Other.Text += "Volume   <" + p.PosTower.Volume + " m3>\n";
                    rtb_Other.Text += "Capacity <" + p.PosTower.Capacity + " m3>\n";
                }
                else
                {
                    foreach (Module m in p.Modules)
                    {
                        if (m.typeID == typeID)
                        {
                            if (m.CPU > m.CPU_Used)
                            {
                                l_cpu.ForeColor = Color.Green;
                                l_cpu.Text = String.Format("{0:#,0.#}", m.CPU);
                            }
                            else
                            {
                                l_cpu.ForeColor = Color.Red;
                                l_cpu.Text = String.Format("{0:#,0.#}", m.CPU_Used);
                            }

                            if (m.Power > m.Power_Used)
                            {
                                l_power.ForeColor = Color.Green;
                                l_power.Text = String.Format("{0:#,0.#}", m.Power);
                            }
                            else
                            {
                                l_power.ForeColor = Color.Red;
                                l_power.Text = String.Format("{0:#,0.#}", m.Power_Used);
                            }

                            l_armor_hp.Text = String.Format("{0:#,0.#}", m.Armor.Amount);
                            l_shield_hp.Text = String.Format("{0:#,0.#}", m.Shield.Amount);
                            l_shield_em.Text = String.Format("{0:#,0.#}", m.Shield.EMP) + "%";
                            l_shield_exp.Text = String.Format("{0:#,0.#}", m.Shield.Explosive) + "%";
                            l_shield_kin.Text = String.Format("{0:#,0.#}", m.Shield.Kinetic) + "%";
                            l_shield_therm.Text = String.Format("{0:#,0.#}", m.Shield.Thermal) + "%";
                            l_struct_hp.Text = String.Format("{0:#,0.#}", m.Struct.Amount);
                            l_struct_emp.Text = String.Format("{0:#,0.#}", m.Struct.EMP) + "%";
                            l_struct_exp.Text = String.Format("{0:#,0.#}", m.Struct.Explosive) + "%";
                            l_struct_kin.Text = String.Format("{0:#,0.#}", m.Struct.Kinetic) + "%";
                            l_struct_therm.Text = String.Format("{0:#,0.#}", m.Struct.Thermal) + "%";
                            l_SI_Cost.Text = String.Format("{0:#,0.#}", m.Cost) + " isk";

                            rtb_Other.Text = m.Desc + "\n\n" + fuel + "\n" + m.OtherInfo;
                            rtb_Other.Text += "Volume   <" + m.Volume + " m3>\n";
                            cap = CalculateSiloCapacity(m, p);
                            rtb_Other.Text += "Capacity <" + cap + " m3>\n";

                            break;
                        }
                    }
                }
            }
        }

        private void lv_ItemSelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView LV;
            ListViewItem LVI;

            LV = (ListView)sender;
            LVI = (ListViewItem)LV.FocusedItem;

            if (LVI != null)
            {
                GetItemData(LVI);
            }
        }

        private void PoS_Item_MouseDown(object sender, MouseEventArgs e)
        {
            DragDropEffects DDE;
            string srcName, location;
            long typeID;
            PoS_Item pi = (PoS_Item)sender;
            PoS_Item piOld;

            if (pi.typeID == 0)
                return;

            if (e.Button == MouseButtons.Right)
                return;

            if (e.Button == MouseButtons.Middle)
                Mouse_Online_Offline_Click(sender, e);

            srcName = pi.contName;
            DDE = DoDragDrop(pi, DragDropEffects.Copy);

            if (DDE == DragDropEffects.Copy)
            {
                typeID = pi.typeID;
                location = pi.contName;

                if (!pi.ModName.Contains("Tower"))
                {
                    if (pi.DragType == "Move")
                    {
                        foreach (Module m in Design.Modules)
                        {
                            if ((m.typeID == typeID) && (m.Location == srcName))
                            {
                                m.Location = location;
                                break;
                            }
                        }
                        piOld = (PoS_Item)p_Tower.Controls[srcName];
                        piOld.RemoveItemFromControl();
                    }
                    else   // Copy
                    {
                        foreach (Module m in Design.Modules)
                        {
                            if (m.typeID == typeID)
                            {
                                Module nw = new Module(m);
                                nw.Location = pi.dstName;
                                Design.Modules.Add(nw);
                                break;
                            }
                        }
                        foreach (Control c in p_Tower.Controls)
                        {
                            if (c.GetType().Name == "PoS_Item")
                            {
                                PoS_Item piC = (PoS_Item)c;
                                if (piC.typeID != 0)
                                {
                                    piC.DragType = "Move";
                                    piC.dstName = "";
                                }
                            }
                        }
                    }

                    if (!load)
                    {
                        SetChangedStatus(true, "Unsaved Changes - Save Needed");
                    }
                }
                CalculatePOSData();
            }
            else if (e.Button == MouseButtons.Left)
            {
                GetDesignItemData(pi.typeID, pi.catName);
            }
        }

        private void ct_PoSName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((PosChanged) && (Design.Name != "New POS") && (Design.Name != ""))
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("The currently Selected POS has Changed. Do you want to Save it?", "Save POS", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    PlugInData.PDL.UpdateListDesign(Design);
                    PlugInData.PDL.SaveDesignListing();
                    BuildPOSListForMonitoring();
                    PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
                }
                else
                {
                    PlugInData.PDL.LoadDesignListing();
                }
            }

            SetChangedStatus(false, "");
            load = true;

            CurrentName = ct_PoSName.Text;
            if (CurrentName.Contains(" <"))
                CurrentName = CurrentName.Substring(0, CurrentName.IndexOf(" <"));

            // 1. Get new PoS Data from List
            if (PlugInData.PDL.Designs.ContainsKey(CurrentName))
            {
                // 2. Clear Current PoS Display
                foreach (Control c in p_Tower.Controls)
                {
                    if (c.GetType().Name == "PoS_Item")
                    {
                        PoS_Item pi = (PoS_Item)c;
                        pi.RemoveItemFromControl();
                    }
                }

                // 3. Populate PoS Display with PoS Information
                Design = new POS(PlugInData.PDL.Designs[CurrentName]);

                // Populate POS
                if (Design.PosTower.typeID != 0)
                {
                    // First Populate the Tower itself - as there is only 1
                    pi_Tower.typeID = Design.PosTower.typeID;
                    pi_Tower.contName = "pi_Tower";
                    pi_Tower.number = 1;

                    if (Design.PosTower.State == "Online")
                        pi_Tower.onOff = Color.Green;
                    else
                        pi_Tower.onOff = Color.Red;

                    pi_Tower.catName = Design.PosTower.Category;
                    pi_Tower.ModName = Design.PosTower.Name;
                    pi_Tower.SetToolTipForItem(Design.PosTower.Name);
                    pi_Tower.UpdateItemState();

                    nud_StrontInterval.Maximum = Design.ComputeMaxPosStrontTime();

                    foreach (Module mc in Design.Modules)
                    {
                        // Now Populate each Module in the POS design
                        PoS_Item pi;

                        pi = (PoS_Item)p_Tower.Controls[mc.Location];

                        pi.typeID = mc.typeID;
                        pi.contName = mc.Location;
                        pi.number = (int)mc.Qty;

                        if (mc.State == "Online")
                            pi.onOff = Color.Green;
                        else
                            pi.onOff = Color.Red;

                        pi.catName = mc.Category;
                        pi.ModName = mc.Name;
                        pi.SetToolTipForItem(mc.Name);
                        pi.UpdateItemState();

                    }
                }
            }

            cb_SovLevel.SelectedIndex = Design.SovLevel;
            cb_System.Text = Design.System;
            cb_CorpName.Text = Design.CorpName;
            cb_systemMoon.Text = Design.Moon;

            nud_DesFuelPeriod.Maximum = Design.ComputeMaxPosRunTimeForLoad();
            if (nud_DesFuelPeriod.Minimum < 0)
                nud_DesFuelPeriod.Minimum = 0;
            if (nud_DesFuelPeriod.Maximum <= 0)
                nud_DesFuelPeriod.Maximum = 100;
            if (nud_StrontInterval.Minimum < 0)
                nud_StrontInterval.Minimum = 0;
            if (nud_StrontInterval.Maximum <= 0)
                nud_StrontInterval.Maximum = 100;

            if (Design.PosTower.typeID != 0)
            {
                cb_Interval.SelectedIndex = (int)Design.PosTower.Design_Interval;
                if (Design.PosTower.Design_Int_Qty > 0)
                    nud_DesFuelPeriod.Value = Design.PosTower.Design_Int_Qty;
                else
                    nud_DesFuelPeriod.Value = 1;
                if (Design.PosTower.Design_Stront_Qty > nud_StrontInterval.Maximum)
                    nud_StrontInterval.Value = nud_StrontInterval.Maximum;
                else
                    nud_StrontInterval.Value = Design.PosTower.Design_Stront_Qty;
            }
            else
            {
                cb_Interval.SelectedIndex = 1;
                nud_DesFuelPeriod.Minimum = 0;
                nud_DesFuelPeriod.Maximum = 1;
                nud_DesFuelPeriod.Value = 0;
                nud_StrontInterval.Minimum = 0;
                nud_StrontInterval.Maximum = 1;
                nud_StrontInterval.Value = 0;
            }

            load = false;
            CalculatePOSData();
        }

        private void SetModuleQuantity(object sender, EventArgs e)
        {
            long newNum;

            if (api.typeID == 0)
                return;

            newNum = Convert.ToInt32(sender.ToString());

            if (api.TypeKey != EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                api.number = newNum;
                api.UpdateItemState();

                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        m.Qty = api.number;
                    }
                }
                if (!load)
                {
                    SetChangedStatus(true, "Unsaved Changes - Save Needed");
                }
                CalculatePOSData();
            }
        }

        private decimal ComputeBayPercentage(decimal used, decimal cap)
        {
            decimal retVal = 0;

            if (cap > 0)
                retVal = ((used / cap) * 100);

            if (retVal > 100)
                retVal = 100;

            return retVal;
        }

        private void cb_SovLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Design.SovLevel = cb_SovLevel.SelectedIndex;

            if (!load)
            {
                SetChangedStatus(true, "Unsaved Changes - Save Needed");
            }
            CalculatePOSData();
        }

        private void cb_systemMoon_SelectedIndexChanged(object sender, EventArgs e)
        {
            Design.Moon = cb_systemMoon.Text;

            if (!load)
            {
                SetChangedStatus(true, "Unsaved Changes - Save Needed");
            }
        }

        private void cb_System_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sov_Data syst;

            Design.System = cb_System.Text;

            if (!load)
            {
                SetChangedStatus(true, "Unsaved Changes - Save Needed");
            }

            FindAndGetSystemMoons();

            syst = PlugInData.SLs.Systems[Design.System];

            if (Decimal.Compare(syst.secLevel, Convert.ToDecimal(0.45)) > 0)
                Design.UseChart = true;
            else
                Design.UseChart = false;

            CalculatePOSData();
        }

        private void FindAndGetSystemMoons()
        {
            string strSQL;
            decimal sysID;
            Sov_Data sd;
            DataSet ml;

            sd = PlugInData.SLs.Systems[Design.System];
            sysID = sd.systemID;

            strSQL = "SELECT mapDenormalize.itemName FROM mapDenormalize WHERE (mapDenormalize.solarSystemID=" + sysID + " AND mapDenormalize.groupID=8);";
            ml = EveHQ.Core.DataFunctions.GetData(strSQL);

            cb_systemMoon.Items.Clear();
            if (ml.Tables.Count > 0)
            {
                foreach (DataRow dr in ml.Tables[0].Rows)
                {
                    cb_systemMoon.Items.Add(dr[0].ToString());
                }
            }
        }

        private void cb_CorpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Design.itemID != 0)
            {
                cb_CorpName.Text = Design.CorpName;
                return;
            }
            if (Design.CorpName != cb_CorpName.Text)
            {
                Design.CorpName = cb_CorpName.Text;

                foreach (APITowerData apid in PlugInData.API_D.apiTower.Values)
                {
                    if (apid.corpName == Design.CorpName)
                    {
                        if (Design.corpID != apid.corpID)
                        {
                            // Update the corpID for the POS and Save to Disk
                            Design.corpID = apid.corpID;
                            PlugInData.PDL.SaveDesignListing();
                            SetChangedStatus(false, "");
                        }
                        break;
                    }
                }
            }

            UpdateLinkedTowerSovLevel(Design);

            if (!load)
            {
                SetChangedStatus(true, "Unsaved Changes - Save Needed");
            }

            CalculatePOSData();
        }


        #endregion


        #region PoS Monitor Routines

        private void ExportToClipboard(object sender, EventArgs e)
        {
            POS p;
            long mCnt = 0, rlCnt = 0;
            string TowerExport;

            if (PlugInData.PDL.Designs.ContainsKey(selName))
            {
                p = PlugInData.PDL.Designs[selName];
            }
            else
            {
                MessageBox.Show("You Must Select a Tower to Export first.", "Export Tower", MessageBoxButtons.OK);
                return;
            }

            TowerExport = p.Name + "," + p.PosTower.typeID.ToString() + "\n";

            if (p.Modules.Count > 0)
            {
                TowerExport += "MODL|";
                foreach (Module m in p.Modules)
                {
                    if (mCnt > 0)
                        TowerExport += "|";

                    TowerExport += m.typeID.ToString() + "," + m.Qty.ToString() + "," + m.ModuleID + "," + m.selReact.typeID.ToString() + "," + m.selMineral.typeID + "," + m.State + "," + m.Location;
                    mCnt++;
                }
            }

            TowerExport += "\n";
            if (p.ReactionLinks.Count > 0)
            {
                TowerExport += "RLNK|";
                foreach (ReactionLink rl in p.ReactionLinks)
                {
                     if (rlCnt > 0)
                        TowerExport += "|";

                     TowerExport += rl.LinkID.ToString() + "," + rl.InpID.ToString() + "," + rl.OutID.ToString() + "," + rl.XferQty + "," + rl.XferVol.ToString() + "," + rl.srcNm + "," + rl.dstNm;
                
                    rlCnt++;
                }
            }

            Clipboard.SetText(TowerExport);

        }

        private void tm_ExportTower_Click(object sender, EventArgs e)
        {
            POS p;
            string fname;

            if (PlugInData.PDL.Designs.ContainsKey(selName))
            {
                p = PlugInData.PDL.Designs[selName];
            }
            else
            {
                MessageBox.Show("You Must Select a Tower to export first.", "Export Tower", MessageBoxButtons.OK);
                return;
            }

            sfd_Export.InitialDirectory = PlugInData.PosExport_Path;
            sfd_Export.Filter = "Tower (*.twr)|*.twr";
            sfd_Export.FilterIndex = 0;
            sfd_Export.ShowDialog();

            fname = sfd_Export.FileName;

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, p);
            pStream.Close();
        }

        private void tm_ImportTower_Click(object sender, EventArgs e)
        {
            string fname;
            POS p = new POS();
            Stream cStr;
            BinaryFormatter myBf;

            ofd_Import.InitialDirectory = PlugInData.PosExport_Path;
            ofd_Import.Filter = "Tower (*.twr)|*.twr";
            ofd_Import.FilterIndex = 0;
            ofd_Import.ShowDialog();

            fname = ofd_Import.FileName;
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    p = (POS)myBf.Deserialize(cStr);
                    if (PlugInData.PDL.Designs.ContainsKey(p.Name))
                    {
                        PlugInData.PDL.Designs.Remove(p.Name);
                    }
                    PlugInData.PDL.Designs.Add(p.Name, p);
                    PlugInData.PDL.SaveDesignListing();
                    UpdatePOSForNewData();
                }
                catch
                {
                    // Cannot load tower for some reason!
                    MessageBox.Show("Error encountered trying to load Tower File.", "Import Tower", MessageBoxButtons.OK);
                    cStr.Close();
                    return;
                }

                cStr.Close();
            }
        }

        private void UpdateAllTowerSovLevels()
        {
            foreach (POS p in PlugInData.PDL.Designs.Values)
            {
                UpdateLinkedTowerSovLevel(p);
            }

            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");
        }

        private void UpdateLinkedTowerSovLevel(POS p)
        {
            decimal corpID;
            string strSQL;
            Sov_Data sd;
            Alliance_Data ad;
            APITowerData td;
            DataSet ml;

            if (p.corpID != 0)
            {
                // This POS is Linked to a Corp
                corpID = p.corpID;

                ad = (Alliance_Data)PlugInData.AL.alliances[corpID];
                if (ad != null)
                {
                    // Corp is in an alliance
                    // Find system in system list, update SOV level accordingly
                    if (PlugInData.SLs.Systems.ContainsKey(p.System))
                        sd = PlugInData.SLs.Systems[p.System];
                    else
                        sd = null;

                    if (sd != null)
                    {
                        if ((sd.corpID == corpID) || (sd.allianceID == ad.allianceID))
                        {
                            // Found the correct system and alliance ID
                            p.SovLevel = 1;
                        }
                        else
                            p.SovLevel = 0;
                    }
                }
            }

            if (p.itemID != 0)
            {
                // Tower is linked to CORP Data
                td = PlugInData.API_D.GetAPIDataMemberForTowerID(p.itemID);

                if (td != null)
                {
                    // Update Tower Moon from API Data
                    strSQL = "SELECT mapDenormalize.itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + td.moonID + ";";
                    ml = EveHQ.Core.DataFunctions.GetData(strSQL);

                    if (ml != null)
                        if ((ml.Tables != null) && (ml.Tables.Count > 0))
                            if ((ml.Tables[0].Rows != null) && (ml.Tables[0].Rows.Count > 0))
                                p.Moon = ml.Tables[0].Rows[0].ItemArray[0].ToString();
                            else
                                p.Moon = "Unknown";
                        else
                            p.Moon = "Unknown";
                    else
                        p.Moon = "Unknown";

                    switch (td.stateV)
                    {
                        case 0:
                            p.PosTower.State = "Unanchored";
                            break;
                        case 1:
                            p.PosTower.State = "Offline";
                            break;
                        case 2:
                            p.PosTower.State = "Onlining";
                            break;
                        case 3:
                            p.PosTower.State = "Reinforced";
                            break;
                        case 4:
                            p.PosTower.State = "Online";
                            break;
                    }

                    if (p.corpID == 0)
                        p.corpID = td.corpID;
                }
            }

            // Update Tower Charter Requirements
            if (PlugInData.SLs.Systems.ContainsKey(p.System))
            {
                sd = PlugInData.SLs.Systems[p.System];
                if (Decimal.Compare(sd.secLevel, Convert.ToDecimal(0.45)) > 0)
                    p.UseChart = true;
                else
                    p.UseChart = false;
            }
        }

        public POS GetPoSListingForPoS(string name)
        {
            if (PlugInData.PDL.Designs.ContainsKey(name))
            {
                return PlugInData.PDL.Designs[name];
            }
            return null;
        }

        private void BuildPOSListForMonitoring()
        {
            MonSel_L.Clear();
            foreach (POS pl in PlugInData.PDL.Designs.Values)
                MonSel_L.Add(pl.Name);
        }

        public void UpdatePOSForNewData()
        {
            BuildPOSListForMonitoring();
            UpdateAllTowerSovLevels();
            RunCalculationsWithUpdatedInformation();
            dg_MonitoredTowers_SelectionChanged(1, new EventArgs());
            SetChangedStatus(false, "");
            tsl_APIState.Text = "";
            b_UpdateAPI.Enabled = true;
        }

        private void PopulateMonitoredPoSDisplay()
        {
            APITowerData apid;
            int dg_ind, ind_ct;
            string line, cache, strSQL, loc;
            DateTime now, cTim, ref_TM;
            TimeSpan diffT;
            decimal ReactTime = 0;
            DataSet locData;
            ArrayList ReactRet;

            if (dg_MonitoredTowers.ColumnCount <= 0)
                return;

            foreach (POS p in PlugInData.PDL.Designs.Values)
            {
                if (p.Monitored)
                {
                    dg_ind = -1;
                    ind_ct = 0;
                    p.CalculatePOSFuelRunTime(PlugInData.API_D, p.PosTower.Fuel);

                    foreach (DataGridViewRow dr in dg_MonitoredTowers.Rows)
                    {
                        if (dr.Cells[(int)MonDG.Name].Value.ToString().Contains(p.Name))
                        {
                            dg_ind = ind_ct;
                            break;
                        }

                        ind_ct++;
                    }

                    if (dg_ind < 0)
                    {
                        // Did not find the tower in the existing list, so Add new row data to DG
                        dg_ind = dg_MonitoredTowers.Rows.Add();
                    }

                    for (int cl = 0; cl < 28; cl++)
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[cl].Style.BackColor = Color.Gainsboro;
                    }

                    line = p.Name + " [Sov" + p.SovLevel + "]";
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Name].Value = line;

                    if (p.Moon != "")
                        line = p.Moon;
                    else
                        line = p.System;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LocN].Value = line;

                    line = PlugInData.ConvertHoursToTextDisplay(p.PosTower.F_RunTime);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FuelR].Value = line;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.fHrs].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.fHrs].Value = p.PosTower.F_RunTime;
                    line = PlugInData.ConvertHoursToTextDisplay(p.PosTower.Fuel.Strontium.RunTime);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.StrontR].Value = line;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.sHrs].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.sHrs].Value = p.PosTower.Fuel.Strontium.RunTime;

                    ref_TM = DateTime.Now;
                    if (p.itemID != 0)
                    {
                        apid = PlugInData.API_D.GetAPIDataMemberForTowerID(p.itemID);
                        // Get Table With Tower or Tower Item Information
                        if (apid != null)
                        {
                            if (apid.locName == "Unknown")
                            {
                                strSQL = "SELECT itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + apid.locID + ";";
                                locData = EveHQ.Core.DataFunctions.GetData(strSQL);
                                if (locData.Tables[0].Rows.Count > 0)
                                {
                                    loc = locData.Tables[0].Rows[0].ItemArray[0].ToString();
                                    apid.locName = loc;
                                    PlugInData.API_D.SaveAPIListing();
                                }
                            }

                            line = "<" + apid.locName + "> ";
                            line += apid.towerName;

                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Link].Value = line;

                            cache = apid.cacheUntil;
                            cTim = Convert.ToDateTime(cache);
                            cTim = TimeZone.CurrentTimeZone.ToLocalTime(cTim);
                            now = DateTime.Now;
                            diffT = now.Subtract(cTim);

                            switch (apid.stateV)
                            {
                                case 0:
                                    p.PosTower.State = "Unanchored";
                                    break;
                                case 1:
                                    p.PosTower.State = "Offline";
                                    break;
                                case 2:
                                    p.PosTower.State = "Onlining";
                                    break;
                                case 3:
                                    p.PosTower.State = "Reinforced";
                                    ref_TM = Convert.ToDateTime(apid.stateTS);
                                    break;
                                case 4:
                                    p.PosTower.State = "Online";
                                    break;
                            }

                            if (diffT.Hours > 1)
                            {
                                dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Value = cTim.ToString();
                                // Cache can be updated, highlight red
                                dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Style.ForeColor = Color.Red;
                            }
                            else
                            {
                                dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Value = cache;
                                // Cache is up to date, highlight green
                                dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Style.ForeColor = Color.Green;
                            }
                        }
                    }
                    else
                    {
                        line = "<Not Linked>";
                        cache = "";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Link].Value = line;
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Value = cache;
                    }

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Value = p.PosTower.State;

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.CPU].Value = String.Format("{0:#,0.#}", p.PosTower.CPU_Used) + " / " + String.Format("{0:#,0.#}", p.PosTower.CPU);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hCPU].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hCPU].Value = p.PosTower.CPU_Used;

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Power].Value = String.Format("{0:#,0.#}", p.PosTower.Power_Used) + " / " + String.Format("{0:#,0.#}", p.PosTower.Power);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hPow].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hPow].Value = p.PosTower.Power_Used;

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].ValueType = typeof(decimal);

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].Value = p.PosTower.Fuel.EnrUran.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].Value = p.PosTower.Fuel.Oxygen.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].Value = p.PosTower.Fuel.MechPart.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].Value = p.PosTower.Fuel.Coolant.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].Value = p.PosTower.Fuel.Robotics.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].Value = p.PosTower.Fuel.HvyWater.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].Value = p.PosTower.Fuel.LiqOzone.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].Value = p.PosTower.Fuel.Charters.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].Value = p.PosTower.Fuel.Strontium.Qty;

                    if (p.PosTower.Fuel.HeIso.PeriodQty > 0)
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.HeIso.Qty) + " He";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = p.PosTower.Fuel.HeIso.Qty;
                    }
                    else if (p.PosTower.Fuel.H2Iso.PeriodQty > 0)
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.H2Iso.Qty) + " H2";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = p.PosTower.Fuel.H2Iso.Qty;
                    }
                    else if (p.PosTower.Fuel.N2Iso.PeriodQty > 0)
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.N2Iso.Qty) + " N2";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = p.PosTower.Fuel.N2Iso.Qty;
                    }
                    else if (p.PosTower.Fuel.O2Iso.PeriodQty > 0)
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.O2Iso.Qty) + " O2";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = p.PosTower.Fuel.O2Iso.Qty;
                    }
                    else
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", 0) + " ??";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = 0;
                    }

                    if (p.PosTower.State == "Online")
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Style.BackColor = Color.LawnGreen;
                        if (p.PosTower.F_RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FuelR].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FuelR].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.EnrUran.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.Oxygen.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.MechPart.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.Coolant.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.Robotics.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.HvyWater.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.LiqOzone.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].Style.BackColor = Color.Gainsboro;

                        if (p.UseChart)
                            if (p.PosTower.Fuel.Charters.RunTime < 24)
                                dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].Style.BackColor = Color.Red;
                            else
                                dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.Strontium.RunTime < 4)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].Style.BackColor = Color.Gainsboro;

                        if ((p.PosTower.Fuel.HeIso.PeriodQty > 0) && (p.PosTower.Fuel.HeIso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else if ((p.PosTower.Fuel.N2Iso.PeriodQty > 0) && (p.PosTower.Fuel.N2Iso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else if ((p.PosTower.Fuel.H2Iso.PeriodQty > 0) && (p.PosTower.Fuel.H2Iso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else if ((p.PosTower.Fuel.O2Iso.PeriodQty > 0) && (p.PosTower.Fuel.O2Iso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Gainsboro;

                    }
                    else if (p.PosTower.State == "Offline")
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Style.BackColor = Color.Gold;
                    }
                    else
                    {
                        // Tower is Reinforced !
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.StrontR].Value = "Reinforced Until";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Value = ref_TM.ToString() + "(Eve Time)";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Style.BackColor = Color.LightCoral;
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.StrontR].Style.BackColor = Color.LightCoral;
                    }

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.useC].Value = p.UseChart;

                    ReactRet = PlugInData.GetLongestSiloRunTime(p);
                    ReactTime = (decimal)ReactRet[0];
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.React].Value = (string)ReactRet[1];
                    if (ReactTime < 6)
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.React].Style.BackColor = Color.Red;
                    else if (ReactTime < 24)
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.React].Style.BackColor = Color.Gold;
                    else
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.React].Style.BackColor = Color.Gainsboro;

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Owner].Value = p.Owner;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FTech].Value = p.FuelTech;
                }
            }

            if (Mon_dg_Pos != "")
            {
                // Find index to selected POS
                for (int x = 0; x < dg_MonitoredTowers.Rows.Count; x++)
                {
                    if (dg_MonitoredTowers.Rows[x].Cells[(int)MonDG.Name].Value.ToString().Contains(Mon_dg_Pos))
                    {
                        Mon_dg_indx = x;
                        break;
                    }
                }
            }

            if (Mon_dg_indx >= dg_MonitoredTowers.RowCount)
                Mon_dg_indx = dg_MonitoredTowers.RowCount - 1;

            if (Mon_dg_indx >= 0)
            {
                dg_MonitoredTowers.CurrentCell = dg_MonitoredTowers.Rows[Mon_dg_indx].Cells[(int)MonDG.Name];
                Object o = new Object();
                EventArgs ea = new EventArgs();
                dg_MonitoredTowers_SelectionChanged(o, ea);
            }
        }

        private void SortMonitorDataGridByColumn(DataGridView dgv, long ColIndex)
        {
            if (load)
            {
                if (PlugInData.Config.data.MonSortOrder == SortOrder.Descending)
                    PlugInData.Config.data.MonSortOrder = SortOrder.Ascending;
                else
                    PlugInData.Config.data.MonSortOrder = SortOrder.Descending;
            }

            if ((ColIndex == (int)MonDG.FuelR) && (dgv.Columns[(int)MonDG.FuelR].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                // Sort on hidden fuel hours column
                if (PlugInData.Config.data.MonSortOrder == SortOrder.Descending)
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.fHrs], ListSortDirection.Ascending);
                }
                else
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.fHrs], ListSortDirection.Descending);
                }
            }
            else if ((ColIndex == (int)MonDG.CPU) && (dgv.Columns[(int)MonDG.CPU].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                if (PlugInData.Config.data.MonSortOrder == SortOrder.Descending)
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hCPU], ListSortDirection.Ascending);
                }
                else
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hCPU], ListSortDirection.Descending);
                }
            }
            else if ((ColIndex == (int)MonDG.Power) && (dgv.Columns[(int)MonDG.Power].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                if (PlugInData.Config.data.MonSortOrder == SortOrder.Descending)
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hPow], ListSortDirection.Ascending);
                }
                else
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hPow], ListSortDirection.Descending);
                }
            }
            else if ((ColIndex == (int)MonDG.StrontR) && (dgv.Columns[(int)MonDG.StrontR].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                // Sort on hidden stront hours column
                if (PlugInData.Config.data.MonSortOrder == SortOrder.Descending)
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.sHrs], ListSortDirection.Ascending);
                }
                else
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.sHrs], ListSortDirection.Descending);
                }
            }
            else if ((ColIndex == (int)MonDG.Iso) && (dgv.Columns[(int)MonDG.Iso].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                // Sort on hidden stront hours column
                if (PlugInData.Config.data.MonSortOrder == SortOrder.Descending)
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hIso], ListSortDirection.Ascending);
                }
                else
                {
                    PlugInData.Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hIso], ListSortDirection.Descending);
                }
            }
            else
            {
                PlugInData.Config.data.MonSortOrder = dgv.SortOrder;
            }
        }

        private void dg_MonitoredTowers_SelectionChanged(object sender, EventArgs e)
        {
            //decimal qty;
            string posName; //posItm, line;
            APITowerData td;
            POS pl;

            if (dg_MonitoredTowers.CurrentRow == null)
                return;

            if (dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value != null)
            {
                selName = dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value.ToString();
                posName = selName.Substring(0, selName.IndexOf(" ["));
                selName = posName;
                Mon_dg_Pos = posName;

                if (Mon_dg_indx == dg_MonitoredTowers.CurrentRow.Index)
                    return;

                Mon_dg_indx = dg_MonitoredTowers.CurrentRow.Index;

                if (PlugInData.PDL.Designs.ContainsKey(selName))
                {
                    pl = PlugInData.PDL.Designs[selName];
                    // OK, this is the PoS that was just selected. Need to work the display.
                    // 1. Display current fuel levels for the POS & Run times for each type of fuel
                    // 2. Calculate and Display Bay usage (percentage on bars)
                    // 3. Display PoS module listing in listbox
                    // 4. Display PoS modules on picture screen section
                    DisplaySelectedPOSData(pl);

                    nud_EnrUran.Value = Convert.ToInt32(pl.PosTower.Fuel.EnrUran.Qty);
                    nud_Oxy.Value = Convert.ToInt32(pl.PosTower.Fuel.Oxygen.Qty);
                    nud_MechPart.Value = Convert.ToInt32(pl.PosTower.Fuel.MechPart.Qty);
                    nud_Robotic.Value = Convert.ToInt32(pl.PosTower.Fuel.Robotics.Qty);
                    nud_Coolant.Value = Convert.ToInt32(pl.PosTower.Fuel.Coolant.Qty);
                    nud_HvyWtr.Value = Convert.ToInt32(pl.PosTower.Fuel.HvyWater.Qty);
                    nud_LiqOzn.Value = Convert.ToInt32(pl.PosTower.Fuel.LiqOzone.Qty);
                    nud_Charter.Value = Convert.ToInt32(pl.PosTower.Fuel.Charters.Qty);
                    nud_Stront.Value = Convert.ToInt32(pl.PosTower.Fuel.Strontium.Qty);
                    if (pl.PosTower.Fuel.HeIso.PeriodQty > 0)
                        nud_Isotope.Value = Convert.ToInt32(pl.PosTower.Fuel.HeIso.Qty);
                    if (pl.PosTower.Fuel.H2Iso.PeriodQty > 0)
                        nud_Isotope.Value = Convert.ToInt32(pl.PosTower.Fuel.H2Iso.Qty);
                    if (pl.PosTower.Fuel.O2Iso.PeriodQty > 0)
                        nud_Isotope.Value = Convert.ToInt32(pl.PosTower.Fuel.O2Iso.Qty);
                    if (pl.PosTower.Fuel.N2Iso.PeriodQty > 0)
                        nud_Isotope.Value = Convert.ToInt32(pl.PosTower.Fuel.N2Iso.Qty);

                    // Tower is linked to CORP Data
                    td = PlugInData.API_D.GetAPIDataMemberForTowerID(pl.itemID);
                    if (td != null)
                    {
                        if (td.allowCorp)
                            cbx_AllowCorp.Checked = true;
                        else
                            cbx_AllowCorp.Checked = false;

                        if (td.allowAlliance)
                            cbk_AllowAlly.Checked = true;
                        else
                            cbk_AllowAlly.Checked = false;

                        if (td.standDrop > 0)
                        {
                            cbx_OnStdDrop.Checked = true;
                            cbx_OnStdDrop.Text = "On Standing Drop < " + td.standDrop / 10 + " >";
                        }
                        else
                        {
                            cbx_OnStdDrop.Checked = false;
                            cbx_OnStdDrop.Text = "On Standing Drop < 0 >";
                        }


                        if (td.onStatusDrop)
                        {
                            cbx_OnStatusDrop.Checked = true;
                            cbx_OnStatusDrop.Text = "On Status Drop < " + td.statusDrop + " >";
                        }
                        else
                        {
                            cbx_OnStatusDrop.Checked = false;
                            cbx_OnStatusDrop.Text = "On Status Drop < 0 >";
                        }
                        
                        if (td.onAgression)
                            cbx_OnAgression.Checked = true;
                        else
                            cbx_OnAgression.Checked = false;

                        if (td.onWar)
                            cbx_OnWar.Checked = true;
                        else
                            cbx_OnWar.Checked = false;
                    }

                    UpdateTower = true;
                    UpdateTowerMonitorDisplay();
                }
            }
            if (!load)
            {
                PlugInData.Config.data.MonSelIndex = Mon_dg_indx;
                PlugInData.Config.data.SelPos = Mon_dg_Pos;
                PlugInData.Config.SaveConfiguration();
            }
        }

        private void UpdateTowerMonitorDisplay()
        {
            FuelBay nud_fuel;
            decimal bay_p;
            decimal sov_mod, increment;
            POS pl;

            if ((load) || (timeCheck))
                return;

            if (dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value != null)
            {
                if (PlugInData.PDL.Designs.ContainsKey(selName))
                {
                    pl = PlugInData.PDL.Designs[selName];
                    nud_fuel = new FuelBay(pl.PosTower.Fuel);
                    nud_fuel.EnrUran.Qty = nud_EnrUran.Value;
                    nud_fuel.Oxygen.Qty = nud_Oxy.Value;
                    nud_fuel.MechPart.Qty = nud_MechPart.Value;
                    nud_fuel.Robotics.Qty = nud_Robotic.Value;
                    nud_fuel.Coolant.Qty = nud_Coolant.Value;
                    nud_fuel.HvyWater.Qty = nud_HvyWtr.Value;
                    nud_fuel.LiqOzone.Qty = nud_LiqOzn.Value;
                    nud_fuel.Charters.Qty = nud_Charter.Value;
                    if (pl.PosTower.Fuel.N2Iso.PeriodQty > 0)
                        nud_fuel.N2Iso.Qty = nud_Isotope.Value;
                    else if (pl.PosTower.Fuel.HeIso.PeriodQty > 0)
                        nud_fuel.HeIso.Qty = nud_Isotope.Value;
                    else if (pl.PosTower.Fuel.H2Iso.PeriodQty > 0)
                        nud_fuel.H2Iso.Qty = nud_Isotope.Value;
                    else if (pl.PosTower.Fuel.O2Iso.PeriodQty > 0)
                        nud_fuel.O2Iso.Qty = nud_Isotope.Value;
                    nud_fuel.Strontium.Qty = nud_Stront.Value;

                    pl.CalculatePOSAdjustRunTime(PlugInData.Config.data.FuelCosts, nud_fuel);

                    sov_mod = pl.GetSovMultiple();

                    // Enr Uranium
                    l_C_EnUr.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.EnrUran.Qty);
                    l_R_EnUr.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.EnrUran.RunTime);
                    increment = pl.PosTower.Fuel.EnrUran.GetFuelQtyForPeriod(sov_mod, 1, 1);
                    nud_EnrUran.Increment = Convert.ToInt32(increment);
                    l_QH_EnUr.Text = String.Format("{0:#,0.#}", increment);
                    l_AR_EnUr.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.EnrUran.RunTime);
                    if (pl.PosTower.A_Fuel.EnrUran.RunTime < pl.PosTower.Fuel.EnrUran.RunTime)
                        l_AR_EnUr.ForeColor = Color.Red;
                    else
                        l_AR_EnUr.ForeColor = Color.Green;
                    if (nud_EnrUran.Value < pl.PosTower.Fuel.EnrUran.Qty)
                        nud_EnrUran.ForeColor = Color.Red;
                    else if (nud_EnrUran.Value > pl.PosTower.Fuel.EnrUran.Qty)
                        nud_EnrUran.ForeColor = Color.Green;
                    else
                        nud_EnrUran.ForeColor = Color.Blue;

                    // Oxygen
                    l_C_Oxyg.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Oxygen.Qty);
                    l_R_Oxyg.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.Oxygen.RunTime);
                    increment = pl.PosTower.Fuel.Oxygen.GetFuelQtyForPeriod(sov_mod, 1, 1);
                    nud_Oxy.Increment = Convert.ToInt32(increment);
                    l_QH_Oxyg.Text = String.Format("{0:#,0.#}", increment);
                    l_AR_Oxyg.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Oxygen.RunTime);
                    if (pl.PosTower.A_Fuel.Oxygen.RunTime < pl.PosTower.Fuel.Oxygen.RunTime)
                        l_AR_Oxyg.ForeColor = Color.Red;
                    else
                        l_AR_Oxyg.ForeColor = Color.Green;
                    if (nud_Oxy.Value > pl.PosTower.Fuel.Oxygen.Qty)
                        nud_Oxy.ForeColor = Color.Green;
                    else if (nud_Oxy.Value < pl.PosTower.Fuel.Oxygen.Qty)
                        nud_Oxy.ForeColor = Color.Red;
                    else
                        nud_Oxy.ForeColor = Color.Blue;

                    // Mechanical Parts
                    l_C_McP.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.MechPart.Qty);
                    l_R_McP.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.MechPart.RunTime);
                    increment = pl.PosTower.Fuel.MechPart.GetFuelQtyForPeriod(sov_mod, 1, 1);
                    nud_MechPart.Increment = Convert.ToInt32(increment);
                    l_QH_McP.Text = String.Format("{0:#,0.#}", increment);
                    l_AR_McP.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.MechPart.RunTime);
                    if (pl.PosTower.A_Fuel.MechPart.RunTime < pl.PosTower.Fuel.MechPart.RunTime)
                        l_AR_McP.ForeColor = Color.Red;
                    else
                        l_AR_McP.ForeColor = Color.Green;
                    if (nud_MechPart.Value > pl.PosTower.Fuel.MechPart.Qty)
                        nud_MechPart.ForeColor = Color.Green;
                    else if (nud_MechPart.Value < pl.PosTower.Fuel.MechPart.Qty)
                        nud_MechPart.ForeColor = Color.Red;
                    else
                        nud_MechPart.ForeColor = Color.Blue;

                    // Coolant
                    l_C_Cool.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Coolant.Qty);
                    l_R_Cool.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.Coolant.RunTime);
                    increment = pl.PosTower.Fuel.Coolant.GetFuelQtyForPeriod(sov_mod, 1, 1);
                    nud_Coolant.Increment = Convert.ToInt32(increment);
                    l_QH_Cool.Text = String.Format("{0:#,0.#}", increment);
                    l_AR_Cool.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Coolant.RunTime);
                    if (pl.PosTower.A_Fuel.Coolant.RunTime < pl.PosTower.Fuel.Coolant.RunTime)
                        l_AR_Cool.ForeColor = Color.Red;
                    else
                        l_AR_Cool.ForeColor = Color.Green;
                    if (nud_Coolant.Value > pl.PosTower.Fuel.Coolant.Qty)
                        nud_Coolant.ForeColor = Color.Green;
                    else if (nud_Coolant.Value < pl.PosTower.Fuel.Coolant.Qty)
                        nud_Coolant.ForeColor = Color.Red;
                    else
                        nud_Coolant.ForeColor = Color.Blue;

                    // Robotics
                    l_C_Robt.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Robotics.Qty);
                    l_R_Robt.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.Robotics.RunTime);
                    increment = pl.PosTower.Fuel.Robotics.GetFuelQtyForPeriod(sov_mod, 1, 1);
                    nud_Robotic.Increment = Convert.ToInt32(increment);
                    l_QH_Robt.Text = String.Format("{0:#,0.#}", increment);
                    l_AR_Robt.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Robotics.RunTime);
                    if (pl.PosTower.A_Fuel.Robotics.RunTime < pl.PosTower.Fuel.Robotics.RunTime)
                        l_AR_Robt.ForeColor = Color.Red;
                    else
                        l_AR_Robt.ForeColor = Color.Green;
                    if (nud_Robotic.Value > pl.PosTower.Fuel.Robotics.Qty)
                        nud_Robotic.ForeColor = Color.Green;
                    else if (nud_Robotic.Value < pl.PosTower.Fuel.Robotics.Qty)
                        nud_Robotic.ForeColor = Color.Red;
                    else
                        nud_Robotic.ForeColor = Color.Blue;

                    // Faction Charters
                    l_C_Chrt.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Charters.Qty);
                    l_R_Chrt.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.Charters.RunTime);
                    increment = pl.PosTower.Fuel.Charters.GetFuelQtyForPeriod(sov_mod, 1, 1);
                    nud_Charter.Increment = Convert.ToInt32(increment);
                    l_QH_Chrt.Text = String.Format("{0:#,0.#}", increment);
                    if (!pl.UseChart)
                    {
                        l_AR_Chrt.ForeColor = Color.Blue;
                        l_AR_Chrt.Text = "NA";
                    }
                    else
                    {
                        l_AR_Chrt.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Charters.RunTime);
                        if (pl.PosTower.A_Fuel.Charters.RunTime < pl.PosTower.Fuel.Charters.RunTime)
                            l_AR_Chrt.ForeColor = Color.Red;
                        else
                            l_AR_Chrt.ForeColor = Color.Green;
                    }
                    if (nud_Charter.Value > pl.PosTower.Fuel.Charters.Qty)
                        nud_Charter.ForeColor = Color.Green;
                    if (nud_Charter.Value < pl.PosTower.Fuel.Charters.Qty)
                        nud_Charter.ForeColor = Color.Red;
                    else
                        nud_Charter.ForeColor = Color.Blue;

                    // Strontium
                    l_C_Strn.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Strontium.Qty);
                    l_R_Strn.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.Strontium.RunTime);
                    increment = pl.PosTower.Fuel.Strontium.GetFuelQtyForPeriod(sov_mod, 1, 1);
                    nud_Stront.Increment = Convert.ToInt32(increment);
                    l_QH_Strn.Text = String.Format("{0:#,0.#}", increment);
                    l_AR_Strn.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Strontium.RunTime);
                    if (pl.PosTower.A_Fuel.Strontium.RunTime < pl.PosTower.Fuel.Strontium.RunTime)
                        l_AR_Strn.ForeColor = Color.Red;
                    else
                        l_AR_Strn.ForeColor = Color.Green;
                    if (nud_Stront.Value > pl.PosTower.Fuel.Strontium.Qty)
                        nud_Stront.ForeColor = Color.Green;
                    else if (nud_Stront.Value < pl.PosTower.Fuel.Strontium.Qty)
                        nud_Stront.ForeColor = Color.Red;
                    else
                        nud_Stront.ForeColor = Color.Blue;

                    // Heavy Water
                    l_C_HvyW.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.HvyWater.Qty);
                    l_R_HvyW.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.HvyWater.RunTime);
                    increment = pl.PosTower.Fuel.HvyWater.GetFuelQtyForPeriod(sov_mod, pl.PosTower.CPU, pl.PosTower.CPU_Used);
                    nud_HvyWtr.Increment = Convert.ToInt32(increment);
                    l_QH_HvyW.Text = String.Format("{0:#,0.#}", increment);
                    l_AR_HvyW.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.HvyWater.RunTime);
                    if (pl.PosTower.A_Fuel.HvyWater.RunTime < pl.PosTower.Fuel.HvyWater.RunTime)
                        l_AR_HvyW.ForeColor = Color.Red;
                    else
                        l_AR_HvyW.ForeColor = Color.Green;
                    if (nud_HvyWtr.Value > pl.PosTower.Fuel.HvyWater.Qty)
                        nud_HvyWtr.ForeColor = Color.Green;
                    else if (nud_HvyWtr.Value < pl.PosTower.Fuel.HvyWater.Qty)
                        nud_HvyWtr.ForeColor = Color.Red;
                    else
                        nud_HvyWtr.ForeColor = Color.Blue;

                    // Liquid Ozone
                    l_C_LiqO.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.LiqOzone.Qty);
                    l_R_LiqO.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.LiqOzone.RunTime);
                    increment = pl.PosTower.Fuel.LiqOzone.GetFuelQtyForPeriod(sov_mod, pl.PosTower.Power, pl.PosTower.Power_Used);
                    nud_LiqOzn.Increment = Convert.ToInt32(increment);
                    l_QH_LiqO.Text = String.Format("{0:#,0.#}", increment);
                    l_AR_LiqO.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.LiqOzone.RunTime);
                    if (pl.PosTower.A_Fuel.LiqOzone.RunTime < pl.PosTower.Fuel.LiqOzone.RunTime)
                        l_AR_LiqO.ForeColor = Color.Red;
                    else
                        l_AR_LiqO.ForeColor = Color.Green;
                    if (nud_LiqOzn.Value > pl.PosTower.Fuel.LiqOzone.Qty)
                        nud_LiqOzn.ForeColor = Color.Green;
                    else if (nud_LiqOzn.Value < pl.PosTower.Fuel.LiqOzone.Qty)
                        nud_LiqOzn.ForeColor = Color.Red;
                    else
                        nud_LiqOzn.ForeColor = Color.Blue;

                    // Isotopes
                    if (pl.PosTower.Fuel.N2Iso.PeriodQty > 0)
                    {
                        l_M_IsoType.Text = "N2";
                        l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.N2Iso.Qty);
                        l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.N2Iso.RunTime);
                        increment = pl.PosTower.Fuel.N2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_Isotope.Increment = Convert.ToInt32(increment);
                        l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.N2Iso.RunTime);
                        if (pl.PosTower.A_Fuel.N2Iso.RunTime < pl.PosTower.Fuel.N2Iso.RunTime)
                            l_AR_Iso.ForeColor = Color.Red;
                        else
                            l_AR_Iso.ForeColor = Color.Green;
                        if (nud_Isotope.Value > pl.PosTower.Fuel.N2Iso.Qty)
                            nud_Isotope.ForeColor = Color.Green;
                        else if (nud_Isotope.Value < pl.PosTower.Fuel.N2Iso.Qty)
                            nud_Isotope.ForeColor = Color.Red;
                        else
                            nud_Isotope.ForeColor = Color.Blue;
                    }
                    else if (pl.PosTower.Fuel.H2Iso.PeriodQty > 0)
                    {
                        l_M_IsoType.Text = "H2";
                        l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.H2Iso.Qty);
                        l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.H2Iso.RunTime);
                        increment = pl.PosTower.Fuel.H2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_Isotope.Increment = Convert.ToInt32(increment);
                        l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.H2Iso.RunTime);
                        if (pl.PosTower.A_Fuel.H2Iso.RunTime < pl.PosTower.Fuel.H2Iso.RunTime)
                            l_AR_Iso.ForeColor = Color.Red;
                        else
                            l_AR_Iso.ForeColor = Color.Green;
                        if (nud_Isotope.Value > pl.PosTower.Fuel.H2Iso.Qty)
                            nud_Isotope.ForeColor = Color.Green;
                        else if (nud_Isotope.Value < pl.PosTower.Fuel.H2Iso.Qty)
                            nud_Isotope.ForeColor = Color.Red;
                        else
                            nud_Isotope.ForeColor = Color.Blue;
                    }
                    else if (pl.PosTower.Fuel.O2Iso.PeriodQty > 0)
                    {
                        l_M_IsoType.Text = "O2";
                        l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.O2Iso.Qty);
                        l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.O2Iso.RunTime);
                        increment = pl.PosTower.Fuel.O2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_Isotope.Increment = Convert.ToInt32(increment);
                        l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.O2Iso.RunTime);
                        if (pl.PosTower.A_Fuel.O2Iso.RunTime < pl.PosTower.Fuel.O2Iso.RunTime)
                            l_AR_Iso.ForeColor = Color.Red;
                        else
                            l_AR_Iso.ForeColor = Color.Green;
                        if (nud_Isotope.Value > pl.PosTower.Fuel.O2Iso.Qty)
                            nud_Isotope.ForeColor = Color.Green;
                        else if (nud_Isotope.Value < pl.PosTower.Fuel.O2Iso.Qty)
                            nud_Isotope.ForeColor = Color.Red;
                        else
                            nud_Isotope.ForeColor = Color.Blue;
                    }
                    else if (pl.PosTower.Fuel.HeIso.PeriodQty > 0)
                    {
                        l_M_IsoType.Text = "He";
                        l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.HeIso.Qty);
                        l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.Fuel.HeIso.RunTime);
                        increment = pl.PosTower.Fuel.HeIso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_Isotope.Increment = Convert.ToInt32(increment);
                        l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.HeIso.RunTime);
                        if (pl.PosTower.A_Fuel.HeIso.RunTime < pl.PosTower.Fuel.HeIso.RunTime)
                            l_AR_Iso.ForeColor = Color.Red;
                        else
                            l_AR_Iso.ForeColor = Color.Green;
                        if (nud_Isotope.Value > pl.PosTower.Fuel.HeIso.Qty)
                            nud_Isotope.ForeColor = Color.Green;
                        else if (nud_Isotope.Value < pl.PosTower.Fuel.HeIso.Qty)
                            nud_Isotope.ForeColor = Color.Red;
                        else
                            nud_Isotope.ForeColor = Color.Blue;
                    }
                    else
                    {
                        l_M_IsoType.Text = "";
                        l_C_Iso.Text = "0";
                        l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(0);
                        l_QH_Iso.Text = "0";
                        l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(0);
                        l_AR_Iso.ForeColor = Color.Green;
                        nud_Isotope.Increment = 0;
                        nud_Isotope.ForeColor = Color.Blue;
                    }

                    bay_p = ComputeBayPercentage(pl.PosTower.A_Fuel.FuelUsed, pl.PosTower.A_Fuel.FuelCap);
                    pb_FuelBayFill.Value = Convert.ToInt32(bay_p);
                    pb_FuelBayFill.Text = pl.PosTower.A_Fuel.FuelUsed + " / " + pl.PosTower.A_Fuel.FuelCap;

                    bay_p = ComputeBayPercentage(pl.PosTower.A_Fuel.StrontUsed, pl.PosTower.A_Fuel.StrontCap);
                    pb_StrontBayFill.Value = Convert.ToInt32(bay_p);
                    pb_StrontBayFill.Text = pl.PosTower.A_Fuel.StrontUsed + " / " + pl.PosTower.A_Fuel.StrontCap;
                }
            }
        }

        public void DataUpdateInProgress()
        {
            tsl_APIState.Text = "Updating API Data and Fuel Calculations";
            b_UpdateAPI.Enabled = false;
        }
       
        private void RunCalculationsWithUpdatedInformation()
        {
            timeCheck = true;

            SetReactionValuesAndUpdateDisplay();
            PopulateMonitoredPoSDisplay();

            if ((Mon_dg_indx >= 0) && (dg_MonitoredTowers.Rows.Count >= Mon_dg_indx))
            {
                dg_MonitoredTowers.CurrentCell = dg_MonitoredTowers.Rows[Mon_dg_indx].Cells[(int)MonDG.Name];
                Object o = new Object();
                EventArgs ea = new EventArgs();
                dg_MonitoredTowers_SelectionChanged(o, ea);
            }

            //PlugInData.bgw_SendNotify.RunWorkerAsync();

            timeCheck = false;
        }

        private void UpdateMonitoredTowerState(string state)
        {
            update = true;

            if (dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value != null)
            {
                if (PlugInData.PDL.Designs.ContainsKey(selName))
                {
                    PlugInData.PDL.Designs[selName].PosTower.State = state;

                    if (state == "Reinforced")
                        PlugInData.PDL.Designs[selName].Stront_TS = DateTime.Now;
                    else
                        PlugInData.PDL.Designs[selName].Fuel_TS = DateTime.Now;
                }
            }

            update = false;
        }

        #endregion


        #region Button and Menu Controls

        private void b_ImportTower_Click(object sender, EventArgs e)
        {
            string line;
            StreamReader SR;
            string fname;
            string[] kv;
            POS it = new POS();
            Module im = new Module();
            long ID;

            ofd_Import.InitialDirectory = PlugInData.PosExport_Path;
            ofd_Import.Filter = "POS (*.pos)|*.pos";
            ofd_Import.FilterIndex = 0;
            DialogResult DR = ofd_Import.ShowDialog();

            if (DR == DialogResult.Cancel)
                return;

            fname = ofd_Import.FileName;

            SR = File.OpenText(fname);

            while ((line = SR.ReadLine()) != null)
            {
                // Read in each line, and the pull in the individual data pieces
                kv = line.Split(':');
                switch (kv[0])
                {
                    case "TNAME":
                        it.Name = kv[1];
                        break;
                    case "TSYST":
                        it.System = kv[1];
                        break;
                    case "TSOVL":
                        it.SovLevel = Convert.ToInt32(kv[1]);
                        break;
                    case "TMOON":
                        it.Moon = kv[1];
                        break;
                    case "TTOWR":
                        ID = Convert.ToInt32(kv[1]);
                        it.PosTower = new Tower(PlugInData.TL.Towers[ID]);
                        break;
                    case "TTOWS":
                        it.PosTower.State = kv[1];
                        break;
                    case "TMODT":
                        ID = Convert.ToInt32(kv[1]);
                        im = new Module(PlugInData.ML.Modules[ID]);
                        break;
                    case "TMODS":
                        im.State = kv[1];
                        break;
                    case "TMODQ":
                        im.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TMODC":
                        im.Charge = kv[1];
                        break;
                    case "TMODL":
                        im.Location = kv[1];
                        it.Modules.Add(im);
                        break;
                    case "TF_EU":
                        it.PosTower.Fuel.EnrUran.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_OX":
                        it.PosTower.Fuel.Oxygen.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_MP":
                        it.PosTower.Fuel.MechPart.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_CL":
                        it.PosTower.Fuel.Coolant.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_RB":
                        it.PosTower.Fuel.Robotics.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_HW":
                        it.PosTower.Fuel.HvyWater.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_LO":
                        it.PosTower.Fuel.LiqOzone.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_CH":
                        it.PosTower.Fuel.Charters.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_ST":
                        it.PosTower.Fuel.Strontium.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_O2":
                        it.PosTower.Fuel.O2Iso.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_N2":
                        it.PosTower.Fuel.N2Iso.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_H2":
                        it.PosTower.Fuel.H2Iso.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_HE":
                        it.PosTower.Fuel.HeIso.Qty = Convert.ToDecimal(kv[1]);
                        break;
                    case "TF_UC":
                        if (kv[1].Equals("true"))
                            it.UseChart = true;
                        else
                            it.UseChart = false;
                            break;
                    case "TWMON":
                        if (kv[1].Equals("true"))
                            it.Monitored = true;
                        else
                            it.Monitored = false;
                        break;
                    default:
                        break;
                }
            }

            // Add new tower to list
            PlugInData.PDL.AddDesignToList(it);

            // Set selected tower to new tower name
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");

            BuildPOSListForMonitoring();

            UpdatePOSNameSelections();
            SetSelectedTowerNode(it.Name);

            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_ExportTower_Click(object sender, EventArgs e)
        {
            // 1. Get desired export location from user
            // 2. Export
            string line;
            StreamWriter SW;
            string fname;

            if (Design.Name == "")
            {
                MessageBox.Show("No Tower Selected to Export.", "Input Missing");
                return;
            }

            sfd_Export.InitialDirectory = PlugInData.PosExport_Path;
            sfd_Export.Filter = "POS (*.pos)|*.pos";
            sfd_Export.FilterIndex = 0;
            DialogResult DR = sfd_Export.ShowDialog();

            if (DR == DialogResult.Cancel)
                return;

            fname = sfd_Export.FileName;

            SW = File.CreateText(fname);

            line = "TNAME:" + Design.Name;
            SW.WriteLine(line);

            line = "TSYST:" + Design.System;
            SW.WriteLine(line);

            line = "TSOVL:" + Design.SovLevel;
            SW.WriteLine(line);

            line = "TMOON:" + Design.Moon;
            SW.WriteLine(line);

            line = "TTOWR:" + Design.PosTower.typeID;
            SW.WriteLine(line);
            line = "TTOWS:" + Design.PosTower.State;
            SW.WriteLine(line);

            foreach (Module m in Design.Modules)
            {
                line = "TMODT:" + m.typeID;
                SW.WriteLine(line);
                line = "TMODS:" + m.State;
                SW.WriteLine(line);
                line = "TMODQ:" + m.Qty;
                SW.WriteLine(line);
                line = "TMODC:" + m.Charge;
                SW.WriteLine(line);
                line = "TMODL:" + m.Location;
                SW.WriteLine(line);
            }

            line = "TF_EU:" + Design.PosTower.Fuel.EnrUran.Qty;
            SW.WriteLine(line);
            line = "TF_OX:" + Design.PosTower.Fuel.Oxygen.Qty;
            SW.WriteLine(line);
            line = "TF_MP:" + Design.PosTower.Fuel.MechPart.Qty;
            SW.WriteLine(line);
            line = "TF_CL:" + Design.PosTower.Fuel.Coolant.Qty;
            SW.WriteLine(line);
            line = "TF_RB:" + Design.PosTower.Fuel.Robotics.Qty;
            SW.WriteLine(line);
            line = "TF_HW:" + Design.PosTower.Fuel.HvyWater.Qty;
            SW.WriteLine(line);
            line = "TF_LO:" + Design.PosTower.Fuel.LiqOzone.Qty;
            SW.WriteLine(line);
            line = "TF_CH:" + Design.PosTower.Fuel.Charters.Qty;
            SW.WriteLine(line);
            line = "TF_ST:" + Design.PosTower.Fuel.Strontium.Qty;
            SW.WriteLine(line);
            line = "TF_O2:" + Design.PosTower.Fuel.O2Iso.Qty;
            SW.WriteLine(line);
            line = "TF_N2:" + Design.PosTower.Fuel.N2Iso.Qty;
            SW.WriteLine(line);
            line = "TF_H2:" + Design.PosTower.Fuel.H2Iso.Qty;
            SW.WriteLine(line);
            line = "TF_HE:" + Design.PosTower.Fuel.HeIso.Qty;
            SW.WriteLine(line);

            if (Design.UseChart)
                line = "TF_UC:true";
            else
                line = "TF_UC:false";
            SW.WriteLine(line);

            if (Design.Monitored)
                line = "TWMON:true";
            else
                line = "TWMON:false";
            SW.WriteLine(line);


            SW.Close();
        }

        private void b_CopyToCB_Click(object sender, EventArgs e)
        {
            string TowerExport, paddStr;
            string[,] fVal;
            decimal totalCost = 0, totalVol = 0, anchorTime = 0, onlineTime = 0;

            TowerExport = Design.Name + " < " + Design.System + " >[" + Design.SovLevel + "]{" + Design.Moon + "}\n";
            TowerExport += "--------------------------------------------------------\n";
            // Do Tower Itself and The Modules w/ Qty and Charges
            TowerExport += Design.PosTower.Name + "\n";
            totalVol += Design.PosTower.Volume;
            totalCost += Design.PosTower.Cost;
            anchorTime += Design.PosTower.Anchor_Time;
            onlineTime += Design.PosTower.Online_Time;

            foreach (Module m in Design.Modules)
            {
                TowerExport += m.Name + " [ " + m.Qty + " ]";
                if (m.Charge.Length > 0)
                    TowerExport += "< " + m.Charge + " > -- " + m.State + "\n";
                else
                    TowerExport += " -- " + m.State + "\n";

                totalVol += m.Volume;
                totalCost += m.Cost;
                anchorTime += m.Anchor_Time;
                onlineTime += m.Online_Time;
            }

            // Do Fuel For Design Period
            fVal = Design.PosTower.D_Fuel.GetFuelBayTotals();
            TowerExport += "--------------------------------------------------------\n";
            for (int x = 0; x < 13; x++)
            {
                if ((!Design.UseChart) && (x == 11))
                    continue;

                if (Convert.ToDecimal(fVal[x, 1]) > 0)
                {
                    paddStr = fVal[x, 0].PadRight(20, ' ').Substring(0, 18) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                    paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3 ]\n";
                    TowerExport += paddStr;
                    totalVol += Convert.ToDecimal(fVal[x, 2]);
                    totalCost += Convert.ToDecimal(fVal[x, 3]);
                }
            }

            // Do Total Volume, Cost, Anchor, and Online Time
            TowerExport += "--------------------------------------------------------\n";
            TowerExport += "Transport Volume: " + String.Format("{0:#,0.#}", totalVol) + " m3\n";
            TowerExport += "Total Cost       : " + String.Format("{0:#,0.#}", totalCost) + " isk\n";
            TowerExport += "Anchor Time     : " + PlugInData.ConvertSecondsToTextDisplay(anchorTime) + "\n";
            TowerExport += "Online Time      : " + PlugInData.ConvertSecondsToTextDisplay(onlineTime) + "\n";
            TowerExport += "Total Assembly   : " + PlugInData.ConvertSecondsToTextDisplay(anchorTime + onlineTime) + "\n";

            Clipboard.SetText(TowerExport);
        }

        private void cms_PosItem_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip Mnu = (ContextMenuStrip)sender;
            api = (PoS_Item)Mnu.SourceControl;
            ToolStripMenuItem tsmi, tsmm;

            tsmi = (ToolStripMenuItem)cms_PosItem.Items[4];
            tsmm = (ToolStripMenuItem)cms_PosItem.Items[3];
            tsmi.DropDownItems.Clear();
            tsmi.Available = true;

            if (api.typeID == 0)
                // Empty Block, do not show Context menu / disable all
                Mnu.Enabled = false;
            else
                Mnu.Enabled = true;

            if (api.TypeKey != EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        // found the module, populate the charge list
                        if ((m.ChargeList == null) || (m.ChargeList.Count < 1))
                        {
                            tsmi.Available = false;
                            break;
                        }

                        foreach (string s in m.ChargeList)
                        {
                            tsmi.DropDownItems.Add(s);
                        }
                    }
                }
            }

            if (tsmi.Available == true)
                tsmi.Enabled = true;
            else
                tsmi.Enabled = false;

            if ((api.catName == "Silo") || (api.catName == "Moon Mining") || (api.catName == "Mobile Reactor"))
                tsmm.Enabled = false;
            else
                tsmm.Enabled = true;
        }

        private void tsm_SetModuleCharge_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (api.TypeKey != EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        m.Charge = e.ClickedItem.Text.ToString();
                        break;
                    }
                }
                CalculatePOSData();
            }
        }

        private void b_SaveTower_Click(object sender, EventArgs e)
        {
            POS pli;
            DateTime cur;

            l_SaveStatus.Text = "Saving POS Listing";

            if ((ct_PoSName.Text == "") && (NewName == ""))
            {
                // No name - request one from user
                POS_Name NewPos = new POS_Name();
                NewPos.myData = this;
                NewPos.NewPOS = true;
                NewPos.ShowDialog();
            }

            if ((ct_PoSName.Text == "") && (NewName == ""))
                return;

            if (ct_PoSName.Text == "")
            {
                POS_Name NewPos = new POS_Name();
                NewPos.myData = this;
                NewPos.NewPOS = true;
                NewPos.ShowDialog();

                CurrentName = NewName;
                NewName = "";

                if (CurrentName != "")
                    Design.Name = CurrentName;
                else
                {
                    // Still no name for the PoS, so just exit
                    return;
                }
            }

            pli = new POS(Design);
            pli.Name = CurrentName;

            PlugInData.PDL.UpdateListDesign(pli);
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");

            BuildPOSListForMonitoring();
            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
            SetSelectedTowerNode(pli.Name);
            cur = DateTime.Now;
            l_SaveStatus.Text = "POS Listing Saved (" + cur.ToString() + ")";
        }

        private void tsm_Online_Click(object sender, EventArgs e)
        {
            if (api.typeID == 0)
                return;

            api.onOff = Color.Green;
            api.UpdateItemState();

            if (api.TypeKey == EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                Design.PosTower.State = "Online";
            }
            else
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        m.State = "Online";
                    }
                }
            }
            if (!load)
            {
                SetChangedStatus(true, "Unsaved Changes - Save Needed");
            }
            CalculatePOSData();
        }

        private void tsm_Offline_Click(object sender, EventArgs e)
        {
            if (api.typeID == 0)
                return;

            api.onOff = Color.Red;
            api.UpdateItemState();

            if (api.TypeKey == EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                Design.PosTower.State = "Offline";
            }
            else
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        m.State = "Offline";
                    }
                }
            }
            if (!load)
            {
                SetChangedStatus(true, "Unsaved Changes - Save Needed");
            }
            CalculatePOSData();
        }

        private void Mouse_Online_Offline_Click(object sender, MouseEventArgs e)
        {
            bool online = false;

            api = (PoS_Item)sender;

            if (api.typeID == 0)
                return;

            if (api.TypeKey == EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                if (Design.PosTower.State == "Online")
                    Design.PosTower.State = "Offline";
                else
                {
                    online = true;
                    Design.PosTower.State = "Online";
                }
            }
            else
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        if (m.State == "Online")
                            m.State = "Offline";
                        else
                        {
                            online = true;
                            m.State = "Online";
                        }
                    }
                }
            }

            if (online)
                api.onOff = Color.Green;
            else
                api.onOff = Color.Red;

            api.UpdateItemState();

            if (!load)
            {
                SetChangedStatus(true, "Unsaved Changes - Save Needed");
            }

            CalculatePOSData();
        }

        private void tsm_Remove_Click(object sender, EventArgs e)
        {
            int ind = 0;

            if (api.typeID == 0)
                return;

            if (api.TypeKey == EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                Design.RemoveTowerFromPOS();
            }
            else
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        Design.RemoveModuleFromPOS(ind);
                        break;
                    }
                    ind++;
                }
            }

            api.RemoveItemFromControl();
            if (!load)
                SetChangedStatus(true, "Unsaved Changes - Save Needed");

            CalculatePOSData();
        }

        private void b_NewTower_Click(object sender, EventArgs e)
        {
            if ((PosChanged) && (Design.Name != ""))
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("The currently Selected POS has Changed. Do you want to Save it?", "Save POS", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    PlugInData.PDL.UpdateListDesign(Design);
                }
            }

            POS_Name NewPos = new POS_Name();
            NewPos.myData = this;
            NewPos.NewPOS = true;
            NewPos.ShowDialog();

            // The user did not enter a new name, just get out
            if (NewName == "")
                return;

            CurrentName = NewName;
            NewName = "";
            this.Focus();
            PlugInData.PDL.AddDesignToList(new POS(CurrentName));
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");
            BuildPOSListForMonitoring();
            UpdatePOSNameSelections();
            SetSelectedTowerNode(CurrentName);

            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();

            cb_System.Text = "Select POS System";
            cb_SovLevel.Text = "Select Sov Level";
            cb_CorpName.Text = "Select Corp Name";
        }

        private void b_RenameTower_Click(object sender, EventArgs e)
        {
            POS_Name RenPos = new POS_Name();
            RenPos.myData = this;
            RenPos.NewPOS = false;
            RenPos.ShowDialog();

            if (CurrentName.Equals(NewName))
                return;

            PlugInData.PDL.RemoveDesignFromList(CurrentName);
            Design.Name = NewName;
            CurrentName = NewName;
            NewName = "";
            this.Focus();
            PlugInData.PDL.AddDesignToList(Design);
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");

            BuildPOSListForMonitoring();
            UpdatePOSNameSelections();
            SetSelectedTowerNode(CurrentName); 
            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            dg_MonitoredTowers.Rows.Clear();
            PopulateMonitoredPoSDisplay();
        }

        private void b_CopyTower_Click(object sender, EventArgs e)
        {
            POS newCopy;
            POS_Name CpyPos = new POS_Name();

            CpyPos.myData = this;
            CpyPos.NewPOS = false;
            CpyPos.CopyPOS = true;
            CpyPos.ShowDialog();

            if (CurrentName.Equals(NewName))
                return;
            if (NewName.Length <= 0)
                return;

            newCopy = new POS(NewName, Design);
            newCopy.itemID = 0;
            newCopy.locID = 0;
            newCopy.ReactionLinks.Clear();
            foreach (Module m in newCopy.Modules)
                m.CopyMissingReactData();

            PlugInData.PDL.AddDesignToList(newCopy);
            CurrentName = NewName;
            NewName = "";
            this.Focus();
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");

            BuildPOSListForMonitoring();
            UpdatePOSNameSelections();
            SetSelectedTowerNode(NewName); 
            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            dg_MonitoredTowers.Rows.Clear();
            PopulateMonitoredPoSDisplay();
        }

        private void b_ClearMods_Click(object sender, EventArgs e)
        {
            // Needs Pop-Up confirmation
            DialogResult dr = MessageBox.Show("Are you sure that you want to Clear  the POS Modules from [" + CurrentName + "] ?", "Clear POS", MessageBoxButtons.YesNo);

            if (dr == DialogResult.No)
                return;

            // Clear Current PoS Display
            foreach (Control c in p_Tower.Controls)
            {
                if (c.GetType().Name == "PoS_Item")
                {
                    PoS_Item pi = (PoS_Item)c;
                    pi.RemoveItemFromControl();
                }
            }

            Design.ClearAllPOSData();

            b_SaveTower_Click(sender, e);

            // Needs to clear all Data (fuel, mod list, etc)
            ClearPosData();
            ClearFuelData();
            ClearModListing();
        }

        private void b_DeleteTower_Click(object sender, EventArgs e)
        {
            // Needs Pop-Up confirmation
            DialogResult dr = MessageBox.Show("Are you sure that you want to Delete the POS [" + CurrentName + "] ?", "Delete POS", MessageBoxButtons.YesNo);

            if (dr == DialogResult.No)
                return;

            if (PlugInData.PDL.Designs.ContainsKey(CurrentName))
                PlugInData.PDL.Designs.Remove(CurrentName);

            // 2. Clear Current PoS Display
            foreach (Control c in p_Tower.Controls)
            {
                if (c.GetType().Name == "PoS_Item")
                {
                    PoS_Item pi = (PoS_Item)c;
                    pi.RemoveItemFromControl();
                }
            }

            Design.ClearAllPOSData();
            UpdatePOSNameSelections();

            CalculatePOSData();

            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");

            // Needs to clear all Data (fuel, mod list, etc)
            ClearPosData();
            ClearFuelData();
            ClearModListing();
            BuildPOSListForMonitoring();
            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            dg_MonitoredTowers.Rows.Clear();
            PopulateMonitoredPoSDisplay();
        }

        private void b_MonitorList_Click(object sender, EventArgs e)
        {
            MonitorListSelect mlist_sel = new MonitorListSelect();
            mlist_sel.myData = this;
            mlist_sel.ShowDialog();

            dg_MonitoredTowers.Rows.Clear();
            BuildPOSListForMonitoring();
            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");
        }

        private void b_SetFuelLevel_Click(object sender, EventArgs e)
        {
            POS pl;
            update = true;

            b_SetFuelLevel.Enabled = false;
            if (dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value != null)
            {
                if (PlugInData.PDL.Designs.ContainsKey(selName))
                {
                    pl = PlugInData.PDL.Designs[selName];
                    // Enr Uranium
                    pl.PosTower.Fuel.EnrUran.Qty = pl.PosTower.A_Fuel.EnrUran.Qty;

                    // Oxygen
                    pl.PosTower.Fuel.Oxygen.Qty = pl.PosTower.A_Fuel.Oxygen.Qty;

                    // Mechanical Parts
                    pl.PosTower.Fuel.MechPart.Qty = pl.PosTower.A_Fuel.MechPart.Qty;

                    // Coolant
                    pl.PosTower.Fuel.Coolant.Qty = pl.PosTower.A_Fuel.Coolant.Qty;

                    // Robotics
                    pl.PosTower.Fuel.Robotics.Qty = pl.PosTower.A_Fuel.Robotics.Qty;

                    // Faction Charters
                    pl.PosTower.Fuel.Charters.Qty = pl.PosTower.A_Fuel.Charters.Qty;

                    // Strontium
                    pl.PosTower.Fuel.Strontium.Qty = pl.PosTower.A_Fuel.Strontium.Qty;

                    // Heavy Water
                    pl.PosTower.Fuel.HvyWater.Qty = pl.PosTower.A_Fuel.HvyWater.Qty;

                    // Liquid Ozone
                    pl.PosTower.Fuel.LiqOzone.Qty = pl.PosTower.A_Fuel.LiqOzone.Qty;

                    // Isotope
                    if (pl.PosTower.Fuel.HeIso.PeriodQty > 0)
                        pl.PosTower.Fuel.HeIso.Qty = pl.PosTower.A_Fuel.HeIso.Qty;
                    if (pl.PosTower.Fuel.H2Iso.PeriodQty > 0)
                        pl.PosTower.Fuel.H2Iso.Qty = pl.PosTower.A_Fuel.H2Iso.Qty;
                    if (pl.PosTower.Fuel.N2Iso.PeriodQty > 0)
                        pl.PosTower.Fuel.N2Iso.Qty = pl.PosTower.A_Fuel.N2Iso.Qty;
                    if (pl.PosTower.Fuel.O2Iso.PeriodQty > 0)
                        pl.PosTower.Fuel.O2Iso.Qty = pl.PosTower.A_Fuel.O2Iso.Qty;

                }
            }

            update = false;

            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");
            BuildPOSListForMonitoring();
            PopulateMonitoredPoSDisplay();
            UpdateTowerMonitorDisplay();
            b_SetFuelLevel.Enabled = true;
        }

        private void b_State_Reinforced_Click(object sender, EventArgs e)
        {
            if (dg_MonitoredTowers.RowCount <= 0)
                return;

            UpdateMonitoredTowerState("Reinforced");
            BuildPOSListForMonitoring();
            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_State_Offline_Click(object sender, EventArgs e)
        {
            if (dg_MonitoredTowers.RowCount <= 0)
                return;

            UpdateMonitoredTowerState("Offline");
            BuildPOSListForMonitoring();
            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_State_Online_Click(object sender, EventArgs e)
        {
            if (dg_MonitoredTowers.RowCount <= 0)
                return;

            UpdateMonitoredTowerState("Online");
            BuildPOSListForMonitoring();
            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_SetOwner_Click(object sender, EventArgs e)
        {
            // Setting of the Tower Owner and Fuel Tech
            // Need to have Entry / Selection for:
            //  Owner (corp / personal) type
            //  Owner Name (corp name or person)
            //  Fuel Tech Name
            //  Player ID's for players if known, Corp ID's if corp
            OwnerFuelTech ownFT;

            ownFT = new OwnerFuelTech(this);
            ownFT.ShowDialog();
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");
            PopulateMonitoredPoSDisplay();
        }


        #endregion


        #region POS Maintenance

        private void cb_ShowFuelNeed_CheckedChanged(object sender, EventArgs e)
        {
            PopulateTowerFillDG();
            UpdateSelectedTowerList();
        }

        private void b_CopySelected_Click(object sender, EventArgs e)
        {
            string malongName = "", paddStr;
            long dgi, count = 0;
            FuelBay sfb = new FuelBay();
            decimal totVol, totCost;
            decimal[,] tval = new decimal[13, 3];
            string[,] fVal;
            POS pl;

            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 3; y++)
                    tval[x, y] = 0;
            }
            SelPosFillText = "";
            dgi = cb_SelectedDataForCopy.SelectedIndex;

            foreach (DataGridViewRow dr in dg_TowerFuelList.SelectedRows)
            {
                if (dr.Cells[0].Value == null)
                    continue;

                malongName = dr.Cells[(int)fillDG.Name].Value.ToString();
                SelPosFillText += malongName + " < " + dr.Cells[(int)fillDG.Loc].Value.ToString() + " >\n";

                if (PlugInData.PDL.Designs.ContainsKey(malongName))
                {
                    pl = PlugInData.PDL.Designs[malongName];
                    totVol = 0;
                    totCost = 0;

                    if (count < 1)
                        sfb = new FuelBay(pl.PosTower.T_Fuel);
                    else
                        sfb.AddFuelQty(pl.PosTower.T_Fuel);

                    pl.PosTower.T_Fuel.SetCurrentFuelVolumes();
                    pl.PosTower.T_Fuel.SetCurrentFuelCosts(PlugInData.Config.data.FuelCosts);
                    fVal = pl.PosTower.T_Fuel.GetFuelBayTotals();

                    for (int x = 0; x < 13; x++)
                    {
                        if ((!PlugInData.Config.data.malongChart) && (x == 11))
                            continue;
                        if ((!PlugInData.Config.data.malongStront) && (x == 12))
                            continue;

                        if (Convert.ToDecimal(fVal[x, 1]) > 0)
                        {
                            switch (dgi)
                            {
                                case 0:
                                    paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + " ]\n";
                                    totVol += Convert.ToDecimal(fVal[x, 2]);
                                    totCost += Convert.ToDecimal(fVal[x, 3]);
                                    break;
                                case 1:
                                    paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                                    paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3 ]\n";
                                    totVol += Convert.ToDecimal(fVal[x, 2]);
                                    totCost += Convert.ToDecimal(fVal[x, 3]);
                                    break;
                                case 2:
                                    paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                                    paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3";
                                    paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + "isk ]\n";
                                    totVol += Convert.ToDecimal(fVal[x, 2]);
                                    totCost += Convert.ToDecimal(fVal[x, 3]);
                                    break;
                                default:
                                    paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + " ]\n";
                                    totVol += Convert.ToDecimal(fVal[x, 2]);
                                    totCost += Convert.ToDecimal(fVal[x, 3]);
                                    break;
                            }
                            tval[x, 0] += Convert.ToDecimal(fVal[x, 1]);
                            tval[x, 1] += Convert.ToDecimal(fVal[x, 2]);
                            tval[x, 2] += Convert.ToDecimal(fVal[x, 3]);
                            SelPosFillText += paddStr;
                        }
                    }
                    count++;
                    SelPosFillText += "Tport Vol  [ " + String.Format("{0:#,0.#}", totVol) + "m3 ]\n";
                    if (dgi == 2)
                        SelPosFillText += "Fuel Cost  [ " + String.Format("{0:#,0.#}", totCost) + "isk ]\n";

                    SelPosFillText += "----\n";
                }
            }

            if (dg_TowerFuelList.SelectedRows.Count > 1)
            {
                sfb.SetCurrentFuelVolumes();
                sfb.SetCurrentFuelCosts(PlugInData.Config.data.FuelCosts);
                fVal = sfb.GetFuelBayTotals();
                totCost = 0;
                totVol = 0;

                SelPosFillText += " < Total for Selected Towers >\n";
                for (int x = 0; x < 13; x++)
                {
                    if ((!PlugInData.Config.data.malongChart) && (x == 11))
                        continue;
                    if ((!PlugInData.Config.data.malongStront) && (x == 12))
                        continue;

                    if (Convert.ToDecimal(fVal[x, 1]) > 0)
                    {
                        switch (dgi)
                        {
                            case 0:
                                paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0])) + " ]\n";
                                totVol += Convert.ToDecimal(tval[x, 1]);
                                totCost += Convert.ToDecimal(tval[x, 2]);
                                break;
                            case 1:
                                paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0]));
                                paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 1])) + "m3 ]\n";
                                totVol += Convert.ToDecimal(tval[x, 1]);
                                totCost += Convert.ToDecimal(tval[x, 2]);
                                break;
                            case 2:
                                paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0]));
                                paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 1])) + "m3";
                                paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 2])) + "isk ]\n";
                                totVol += Convert.ToDecimal(tval[x, 1]);
                                totCost += Convert.ToDecimal(tval[x, 2]);
                                break;
                            default:
                                paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0])) + " ]\n";
                                totVol += Convert.ToDecimal(tval[x, 1]);
                                totCost += Convert.ToDecimal(tval[x, 2]);
                                break;
                        }
                        SelPosFillText += paddStr;
                    }
                }
                SelPosFillText += "Tport Vol  [ " + String.Format("{0:#,0.#}", totVol) + "m3 ]\n";
                if (dgi == 2)
                    SelPosFillText += "Fuel Cost  [ " + String.Format("{0:#,0.#}", totCost) + "isk ]\n";
            }
            Clipboard.SetText(SelPosFillText);
        }

        private void b_CopyAllPos_Click(object sender, EventArgs e)
        {
            string malongName = "", paddStr;
            long dgi, count = 0;
            FuelBay sfb = new FuelBay();
            decimal totVol, totCost;
            decimal[,] tval = new decimal[13, 3];
            string[,] fVal;
            POS pl;

            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 3; y++)
                    tval[x, y] = 0;
            }
            AllPosFillText = "";
            dgi = cb_AllDataForCopy.SelectedIndex;

            foreach (DataGridViewRow dr in dg_TowerFuelList.Rows)
            {
                if (dr.Cells[0].Value == null)
                    continue;

                malongName = dr.Cells[(int)fillDG.Name].Value.ToString();
                AllPosFillText += malongName + " < " + dr.Cells[(int)fillDG.Loc].Value.ToString() + " >\n";

                if (PlugInData.PDL.Designs.ContainsKey(malongName))
                {
                    pl = PlugInData.PDL.Designs[malongName];
                    totVol = 0;
                    totCost = 0;

                    if (count < 1)
                        sfb = new FuelBay(pl.PosTower.T_Fuel);
                    else
                        sfb.AddFuelQty(pl.PosTower.T_Fuel);

                    pl.PosTower.T_Fuel.SetCurrentFuelVolumes();
                    pl.PosTower.T_Fuel.SetCurrentFuelCosts(PlugInData.Config.data.FuelCosts);
                    fVal = pl.PosTower.T_Fuel.GetFuelBayTotals();

                    for (int x = 0; x < 13; x++)
                    {
                        if ((!PlugInData.Config.data.malongChart) && (x == 11))
                            continue;
                        if ((!PlugInData.Config.data.malongStront) && (x == 12))
                            continue;

                        if (Convert.ToDecimal(fVal[x, 1]) > 0)
                        {
                            switch (dgi)
                            {
                                case 0:
                                    paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + " ]\n";
                                    totVol += Convert.ToDecimal(fVal[x, 2]);
                                    totCost += Convert.ToDecimal(fVal[x, 3]);
                                    break;
                                case 1:
                                    paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                                    paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3 ]\n";
                                    totVol += Convert.ToDecimal(fVal[x, 2]);
                                    totCost += Convert.ToDecimal(fVal[x, 3]);
                                    break;
                                case 2:
                                    paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                                    paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3";
                                    paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + "isk ]\n";
                                    totVol += Convert.ToDecimal(fVal[x, 2]);
                                    totCost += Convert.ToDecimal(fVal[x, 3]);
                                    break;
                                default:
                                    paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + " ]\n";
                                    totVol += Convert.ToDecimal(fVal[x, 2]);
                                    totCost += Convert.ToDecimal(fVal[x, 3]);
                                    break;
                            }
                            tval[x, 0] += Convert.ToDecimal(fVal[x, 1]);
                            tval[x, 1] += Convert.ToDecimal(fVal[x, 2]);
                            tval[x, 2] += Convert.ToDecimal(fVal[x, 3]);
                            AllPosFillText += paddStr;
                        }
                    }
                    count++;
                    AllPosFillText += "Tport Vol  [ " + String.Format("{0:#,0.#}", totVol) + "m3 ]\n";
                    if (dgi == 2)
                        AllPosFillText += "Fuel Cost  [ " + String.Format("{0:#,0.#}", totCost) + "isk ]\n";

                    AllPosFillText += "-------------------------------------------------\n";
                }
            }

            sfb.SetCurrentFuelVolumes();
            sfb.SetCurrentFuelCosts(PlugInData.Config.data.FuelCosts);
            fVal = sfb.GetFuelBayTotals();
            totCost = 0;
            totVol = 0;

            AllPosFillText += " < Total for All Towers >\n";
            for (int x = 0; x < 13; x++)
            {
                if ((!PlugInData.Config.data.malongChart) && (x == 11))
                    continue;
                if ((!PlugInData.Config.data.malongStront) && (x == 12))
                    continue;

                if (Convert.ToDecimal(fVal[x, 1]) > 0)
                {
                    switch (dgi)
                    {
                        case 0:
                            paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0])) + " ]\n";
                            totVol += Convert.ToDecimal(tval[x, 1]);
                            totCost += Convert.ToDecimal(tval[x, 2]);
                            break;
                        case 1:
                            paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0]));
                            paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 1])) + "m3 ]\n";
                            totVol += Convert.ToDecimal(tval[x, 1]);
                            totCost += Convert.ToDecimal(tval[x, 2]);
                            break;
                        case 2:
                            paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0]));
                            paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 1])) + "m3";
                            paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 2])) + "isk ]\n";
                            totVol += Convert.ToDecimal(tval[x, 1]);
                            totCost += Convert.ToDecimal(tval[x, 2]);
                            break;
                        default:
                            paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0])) + " ]\n";
                            totVol += Convert.ToDecimal(tval[x, 1]);
                            totCost += Convert.ToDecimal(tval[x, 2]);
                            break;
                    }
                    AllPosFillText += paddStr;
                }
            }
            AllPosFillText += "Tport Vol  [ " + String.Format("{0:#,0.#}", totVol) + "m3 ]\n";
            if (dgi == 2)
                AllPosFillText += "Fuel Cost  [ " + String.Format("{0:#,0.#}", totCost) + "isk ]\n";

            Clipboard.SetText(AllPosFillText);
        }

        private void b_ApplyIntervalPeriod_Click(object sender, EventArgs e)
        {
            PlugInData.Config.data.malongTP = tscb_TimePeriod.SelectedIndex;
            PlugInData.Config.data.malongPV = nud_PeriodValue.Value;
            PlugInData.Config.SaveConfiguration();

            PopulateTowerFillDG();
            UpdateSelectedTowerList();
        }

        private void UpdateSelectedTowerList()
        {
            if (dg_TowerFuelList.Rows.Count > Fil_dg_indx)
            {
                dg_TowerFuelList.CurrentCell = dg_TowerFuelList.Rows[Fil_dg_indx].Cells[(int)fillDG.Name];
                Object o = new Object();
                EventArgs ea = new EventArgs();
                dg_TowerFuelList_SelectionChanged(o, ea);
            }

        }

        private void dg_TowerFuelList_SelectionChanged(object sender, EventArgs e)
        {
            string malongName = "";
            int dgi, count = 0;
            FuelBay sfb = new FuelBay();
            decimal totVol, totCost;
            string[,] fVal;

            foreach (DataGridViewRow dr in dg_TowerFuelList.SelectedRows)
            {
                if ((dr.Cells[0].Value == null) || (dg_TowerFuelList.CurrentRow == null))
                    continue;

                Fil_dg_indx = dg_TowerFuelList.CurrentRow.Index;
                malongName = dr.Cells[(int)fillDG.Name].Value.ToString();

                if (PlugInData.PDL.Designs.ContainsKey(malongName))
                {
                    if (count < 1)
                        sfb = new FuelBay(PlugInData.PDL.Designs[malongName].PosTower.T_Fuel);
                    else
                        sfb.AddFuelQty(PlugInData.PDL.Designs[malongName].PosTower.T_Fuel);

                    count++;
                }
            }

            dg_SelectedFuel.Rows.Clear();
            sfb.SetCurrentFuelVolumes();
            sfb.SetCurrentFuelCosts(PlugInData.Config.data.FuelCosts);
            fVal = sfb.GetFuelBayTotals();

            totCost = 0;
            totVol = 0;

            for (int x = 0; x < 13; x++)
            {
                if ((!PlugInData.Config.data.malongChart) && (x == 11))
                    continue;
                if ((!PlugInData.Config.data.malongStront) && (x == 12))
                    continue;

                dgi = dg_SelectedFuel.Rows.Add();
                dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = fVal[x, 0];
                dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.amount].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                if (Convert.ToDecimal(fVal[x, 2]) > 0)
                    totVol += Convert.ToDecimal(fVal[x, 2]);
                dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + " m3";
                totCost += Convert.ToDecimal(fVal[x, 3]);
                dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + " isk";
            }
            dgi = dg_SelectedFuel.Rows.Add();
            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = "Total Volume & Cost";
            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", totVol) + " m3";
            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", totCost) + " isk";
        }

        private void PopulateTotalsDG()
        {
            int dgi;
            decimal totVol, totCost;
            string[,] fVal;

            if (tt == null)
                return;

            tt.SetCurrentFuelVolumes();
            tt.SetCurrentFuelCosts(PlugInData.Config.data.FuelCosts);
            dg_TotalFuel.Rows.Clear();
            fVal = tt.GetFuelBayTotals();

            totVol = 0;
            totCost = 0;
            for (int x = 0; x < 13; x++)
            {
                if ((!PlugInData.Config.data.malongChart) && (x == 11))
                    continue;
                if ((!PlugInData.Config.data.malongStront) && (x == 12))
                    continue;

                dgi = dg_TotalFuel.Rows.Add();
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = fVal[x, 0];
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.amount].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                if (Convert.ToDecimal(fVal[x, 2]) > 0)
                    totVol += Convert.ToDecimal(fVal[x, 2]);
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + " m3";
                totCost += Convert.ToDecimal(fVal[x, 3]);
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + " isk";
            }
            dgi = dg_TotalFuel.Rows.Add();
            dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = "Total Volume & Cost";
            dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", totVol) + " m3";
            dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", totCost) + " isk";
        }

        private void PopulateTowerFillDG()
        {
            int dgi, count = 0;
            string line;
            decimal period;

            tt = null;
            dg_TowerFuelList.Rows.Clear();
            update = true;
            cb_FactChartTotal.Checked = PlugInData.Config.data.malongChart;
            cb_UseStrontTotals.Checked = PlugInData.Config.data.malongStront;
            update = false;

            foreach (POS p in PlugInData.PDL.Designs.Values)
            {
                if (!p.Monitored)
                    continue;

                if (!cb_ShowFuelNeed.Checked)
                    period = p.ComputePosFuelUsageForFillTracking((int)PlugInData.Config.data.malongTP, PlugInData.Config.data.malongPV, PlugInData.Config.data.FuelCosts);
                else
                    period = p.ComputePosFuelNeedForFillTracking((int)PlugInData.Config.data.malongTP, PlugInData.Config.data.malongPV, PlugInData.Config.data.FuelCosts);

                dgi = dg_TowerFuelList.Rows.Add();

                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Name].Value = p.Name;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Loc].Value = p.Moon;

                if (count < 1)
                    tt = new FuelBay(p.PosTower.T_Fuel);
                else
                    tt.AddFuelQty(p.PosTower.T_Fuel);

                count++;

                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.EnrUr].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.EnrUran.Qty);
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Oxy].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Oxygen.Qty);
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.McP].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.MechPart.Qty);
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Cool].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Coolant.Qty);
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Rbt].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Robotics.Qty);

                if (p.PosTower.T_Fuel.N2Iso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.N2Iso.Qty) + " N2";
                }
                else if (p.PosTower.T_Fuel.H2Iso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.H2Iso.Qty) + " H2";
                }
                else if (p.PosTower.T_Fuel.O2Iso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.O2Iso.Qty) + " O2";
                }
                else if (p.PosTower.T_Fuel.HeIso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.HeIso.Qty) + " He";
                }
                else
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = "No F'in Clue";
                }
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.HvW].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.HvyWater.Qty);
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.LqO].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.LiqOzone.Qty);
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Cht].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Charters.Qty);
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Strt].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Strontium.Qty);

                line = PlugInData.ConvertHoursToTextDisplay(period);
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.RunT].Value = line;
            }
            PopulateTotalsDG();
        }

        private void cb_FactChartTotal_CheckedChanged(object sender, EventArgs e)
        {
            if (update)
                return;
            PlugInData.Config.data.malongChart = cb_FactChartTotal.Checked;
            PlugInData.Config.SaveConfiguration();
            PopulateTotalsDG();
            UpdateSelectedTowerList();
        }

        private void cb_UseStrontTotals_CheckedChanged(object sender, EventArgs e)
        {
            if (update)
                return;
            PlugInData.Config.data.malongStront = cb_UseStrontTotals.Checked;
            PlugInData.Config.SaveConfiguration();
            PopulateTotalsDG();
            UpdateSelectedTowerList();
        }

        private void b_SetSelectedFull_Click(object sender, EventArgs e)
        {
            string malongName;

            b_SetSelectedFull.Enabled = false; // Disable while processing to prevent multiple-clicks.

            if (dg_TowerFuelList.SelectedRows.Count > 1)
            {
                // Post a Pop-Up error and exit
                MessageBox.Show("Sorry - You can Only have ONE tower selected to use this feature.", "Set Selected Tower Full", MessageBoxButtons.OK);
                return;
            }

            if (dg_TowerFuelList.CurrentRow.Cells[(int)fillDG.Name].Value != null)
            {
                malongName = dg_TowerFuelList.CurrentRow.Cells[(int)fillDG.Name].Value.ToString();
                if (PlugInData.PDL.Designs.ContainsKey(malongName))
                {
                    PlugInData.PDL.Designs[malongName].PosTower.Fuel.AddFuelQty(PlugInData.PDL.Designs[malongName].PosTower.T_Fuel);
                }
            }
            PopulateTowerFillDG();
            dg_TowerFuelList_SelectionChanged(sender, e);
            PopulateTotalsDG();
            UpdateSelectedTowerList();

            PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");
            BuildPOSListForMonitoring();
            PopulateMonitoredPoSDisplay();
            UpdateTowerMonitorDisplay();

            b_SetSelectedFull.Enabled = true;

        }


        #endregion


        #region Fuel Amount Change Control

        private void nud_DesFuelPeriod_ValueChanged(object sender, EventArgs e)
        {
            // If loading a POS and updating fields, do not pay attention.
            if (load)
                return;

            if (Design.PosTower.typeID == 0)
                return;

            Design.PosTower.Design_Int_Qty = nud_DesFuelPeriod.Value;
            CalculateAndDisplayDesignFuelData();
            nud_DesFuelPeriod.Value = Design.PosTower.Design_Int_Qty;
        }

        private void cb_Interval_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If loading a POS and updating fields, do not pay attention.
            if (load)
                return;

            if (Design.PosTower.typeID == 0)
                return;

            Design.PosTower.Design_Interval = cb_Interval.SelectedIndex;
            CalculateAndDisplayDesignFuelData();

            if (Design.PosTower.Design_Int_Qty > nud_DesFuelPeriod.Maximum)
                nud_DesFuelPeriod.Maximum = Design.PosTower.Design_Int_Qty;
            if (Design.PosTower.Design_Int_Qty < nud_DesFuelPeriod.Minimum)
                nud_DesFuelPeriod.Minimum = Design.PosTower.Design_Int_Qty;

            nud_DesFuelPeriod.Value = Design.PosTower.Design_Int_Qty;
        }

        private void nud_StrontInterval_ValueChanged(object sender, EventArgs e)
        {
            // If loading a POS and updating fields, do not pay attention.
            if (load)
                return;

            if (Design.PosTower.typeID == 0)
                return;

            Design.PosTower.Design_Stront_Qty = nud_StrontInterval.Value;
            CalculateAndDisplayDesignFuelData();
        }

        private void MonitoredFuelChange(object sender, EventArgs e)
        {
            if ((!update) && (UpdateTower))
                UpdateTowerMonitorDisplay();
        }

        #endregion


        #region API Routines

        private void b_API_Populate_Click(object sender, EventArgs e)
        {
            PopulateTowersFromAPI pti = new PopulateTowersFromAPI();
            pti.myData = this;
            pti.ShowDialog();

            UpdatePOSNameSelections();

            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");
            BuildPOSListForMonitoring();
            RunCalculationsWithUpdatedInformation();
        }

        private void b_API_Link_Click(object sender, EventArgs e)
        {
            Tower_API_Linker TowerLink = new Tower_API_Linker();
            TowerLink.myData = this;
            TowerLink.ShowDialog();
            PlugInData.API_D.SaveAPIListing();
            BuildPOSListForMonitoring();
            RunCalculationsWithUpdatedInformation();
        }

        private void b_UpdateAPI_Click(object sender, EventArgs e)
        {
            if (!PlugInData.bgw_APIUpdate.IsBusy)
            {
                tsl_APIState.Text = "Updating API Data and Fuel Calculations";
                b_UpdateAPI.Enabled = false;
                PlugInData.ManualUpdate = true;
                PlugInData.bgw_APIUpdate.RunWorkerAsync();
            }
        }


        #endregion


        #region Configuration Stuff

        private void DGColumnCBStateChange(object sender, EventArgs e)
        {
            int colCt = 1;
            DataGridViewColumn dgvc;
            CheckBox cb;
            string cName;

            // Configuration Change for Combo-Box
            cb = (CheckBox)sender;

            cName = cb.Name;
            colCt = Convert.ToInt32(cName.Replace("checkBox", ""));

            if (colCt < 24)
            {
                dgvc = dg_MonitoredTowers.Columns[colCt - 1];
                dgvc.Visible = cb.Checked;
                PlugInData.Config.data.dgMonBool[colCt - 1] = cb.Checked;
            }
            else
            {
                colCt -= 24;
                if (colCt < 15)
                {
                    dgvc = dg_PosMods.Columns[colCt - 1];
                    dgvc.Visible = cb.Checked;
                    PlugInData.Config.data.dgDesBool[colCt - 1] = cb.Checked;
                }
            }

            SaveConfiguration();
        }

        private void SaveConfiguration()
        {
            if (rb_MarketCost.Checked)
                PlugInData.Config.data.FuelCat = 1;
            else
                PlugInData.Config.data.FuelCat = 0;

            if (rb_AutoAPI.Checked)
                PlugInData.Config.data.AutoAPI = 1;
            else
                PlugInData.Config.data.AutoAPI = 0;

            PlugInData.Config.SaveConfiguration();
            CalculateAndDisplayDesignFuelData();
        }

        private void rb_MarketCost_CheckedChanged(object sender, EventArgs e)
        {
            SaveConfiguration();
        }

        private void rb_AutoAPI_CheckedChanged(object sender, EventArgs e)
        {
            SaveConfiguration();
        }

        private void dg_MonitoredTowers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv;
            string name;

            dgv = (DataGridView)sender;

            PlugInData.Config.data.SortedColumnIndex = e.ColumnIndex;

            if (e.Button == MouseButtons.Left)
            {
                SortMonitorDataGridByColumn(dgv, PlugInData.Config.data.SortedColumnIndex);
                PlugInData.Config.SaveConfiguration();
            }
            else if (e.Button == MouseButtons.Right)
            {
                name = dgv.Columns[e.ColumnIndex].HeaderText;
            }
        }

        private void sc_MainPanels_SplitterMoved(object sender, SplitterEventArgs e)
        {
            long splitPos;

            if (!UserSplitMove)
                return;

            splitPos = sc_MainPanels.SplitterDistance;

            //if (PlugInData.Config.data.Extra.Count > 0)
            PlugInData.Config.data.Extra[0] = splitPos;
            //else
            //    PlugInData.Config.data.Extra.Add(splitPos);

            PlugInData.Config.SaveConfiguration();
            UserSplitMove = false;
        }

        private void sc_MainPanels_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            UserSplitMove = true;
        }

        #endregion


        #region Set Module Charge - Handling Routines

        private void tsm_SetModuleCharge_Click(object sender, EventArgs e)
        {

        }


        #endregion


        #region Reaction Monitor

        private void SetReactChangedStatus(bool st, string ln)
        {
            ReactChanged = st;
            lb_ReacSave.Text = ln;
        }

        public void SetModuleWarnOnValueAndTime()
        {
            bool fInp, fOutp;
            decimal xfIn, xfOut;

            foreach (POS p in PlugInData.PDL.Designs.Values)
            {
                foreach (Module m in p.Modules)
                {
                    switch (Convert.ToInt64(m.ModType))
                    {
                        case 2:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                            // Silo type module here
                            fInp = false;
                            fOutp = false;
                            xfIn = 0;
                            xfOut = 0;
                            foreach (ReactionLink rl in p.ReactionLinks)
                            {
                                if (rl.InpID == m.ModuleID)
                                {
                                    fInp = true;
                                    xfIn = rl.XferQty;
                                }

                                if (rl.OutID == m.ModuleID)
                                {
                                    fOutp = true;
                                    xfOut = rl.XferQty;
                                }
                            }

                            if (xfIn <= 0)
                                xfIn = 1;
                            if (xfOut <= 0)
                                xfOut = 1;
                            if ((fInp) && (!fOutp))
                            {
                                // Silo is an input to another module only - WarnOn Empty (2)
                                m.WarnOn = 2;
                                m.FillEmptyTime = Math.Floor(m.CapQty / xfIn);
                            }
                            else if ((!fInp) && (fOutp))
                            {
                                // Silo is an output from another module only - WarnOn Full (1)
                                m.WarnOn = 1;
                                m.FillEmptyTime = Math.Floor((m.MaxQty - m.CapQty) / xfOut);
                            }
                            else if (fInp && fOutp)
                            {
                                if (xfIn >= xfOut)
                                {
                                    // Silo is both an input and an output - WarnOn Full (1)
                                    m.WarnOn = 1;
                                    if (xfIn > xfOut)
                                        m.FillEmptyTime = Math.Floor((m.MaxQty - m.CapQty) / (xfIn - xfOut));
                                    else
                                        m.FillEmptyTime = 9999999999999;
                                }
                                else if (xfOut > xfIn)
                                {
                                    // Silo is both an input and an output - WarnOn Empty (2)
                                    m.WarnOn = 2;
                                    m.FillEmptyTime = Math.Floor(m.CapQty / (xfOut - xfIn));
                                }
                            }
                            else
                            {
                                // Silo is not connected to anything - disable
                                m.WarnOn = 0;
                                m.FillEmptyTime = 0;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void SetReactionValuesAndUpdateDisplay()
        {
            // OK, new fill numbers, etc have been set for towers. Now I need to update the display
            // accordingly
            TowerReactMod tr;
            decimal sMult;
            POS pl;

            SetModuleWarnOnValueAndTime();

            // Now - Get / find this POS in the list of towers
            // If tower cannot be found, leave
            if (!PlugInData.PDL.Designs.ContainsKey(Sel_React_Pos))
                return;

            pl = PlugInData.PDL.Designs[Sel_React_Pos];
            // Found Selected Tower - Update all Modules
            sMult = 1 + (decimal)(pl.PosTower.Bonuses.SiloCap / 100);

            foreach (Control c in p_PosMods.Controls)
            {
                tr = (TowerReactMod)c;

                foreach (Module m in pl.Modules)
                {
                    if (m.ModuleID == tr.ReactMod.ModuleID)
                    {
                        tr.SetModuleData(m, sMult);
                        break;
                    }
                }
            }
        }

        private void gp_ReacTower_Resize(object sender, EventArgs e)
        {
            PopulateTowerModuleDisplay();
        }

        public void PopulateTowerModuleDisplay()
        {
            // Need to populate the tower display with Monitored Towers and Thier applicable modules
            // IE: Silos, Juntions, Reactors, Harvesters
            int width, numWide, countV = 0, countH = 0;
            ReactMod smd;
            ArrayList SiloMods = new ArrayList();
            ReactionTower RT;

            SetModuleWarnOnValueAndTime();

            gp_ReacTower.Controls.Clear();
            foreach (POS pl in PlugInData.PDL.Designs.Values)
            {
                if (!pl.Monitored)
                    continue;

                SiloMods.Clear();
                foreach (Module m in pl.Modules)
                {
                    if ((m.Category == "Mobile Reactor") || (m.Category == "Moon Mining") ||
                        (m.Category == "Silo"))
                    {
                        if (m.Category == "Silo")
                        {
                            if (m.FillEmptyTime == 0)
                                smd = new ReactMod(m.selMineral.name, PlugInData.ConvertReactionHoursToTextDisplay(m.FillEmptyTime), 999999, Convert.ToInt32(m.CapQty), Convert.ToInt32(m.MaxQty));
                            else
                                smd = new ReactMod(m.selMineral.name, PlugInData.ConvertReactionHoursToTextDisplay(m.FillEmptyTime), m.FillEmptyTime, Convert.ToInt32(m.CapQty), Convert.ToInt32(m.MaxQty));

                            SiloMods.Add(smd);
                        }
                    }
                }

                if (SiloMods.Count > 0)
                {
                    SiloMods.Sort();
                    // Populate the controls display
                    RT = new ReactionTower();
                    RT.UpdateReactionInformation(pl.Name, pl.Moon, SiloMods, this, pl.itemID);
                    width = gp_ReacTower.Width;
                    numWide = (int)(width / RT.Width);

                    if (countH < numWide)
                    {
                        RT.Location = new Point(countH * (RT.Width + 1), countV * (RT.Height + 2));
                        gp_ReacTower.Controls.Add(RT);
                        countH++;
                    }
                    else
                    {
                        countH = 0;
                        countV++;
                        RT.Location = new Point(countH * (RT.Width + 1), countV * (RT.Height + 2));
                        gp_ReacTower.Controls.Add(RT);
                        countH++;
                    }
                }
            }
        }

        private void DisplayReactionModules()
        {
            TowerReactMod trm;
            int num = 0;
            decimal sMult;
            POS pl;
            // Get corret POS Object
            pl = PlugInData.PDL.Designs[SelReactPos];

            // Clear current module display
            p_PosMods.Controls.Clear();
            p_PosMods.Refresh();
            SetModuleWarnOnValueAndTime();

            SetReactionModuleID(pl);

            foreach (Module m in pl.Modules)
            {
                if (((m.Category == "Mobile Reactor") || (m.Category == "Moon Mining") ||
                    (m.Category == "Silo")) && (m.State == "Online"))
                {
                    trm = new TowerReactMod(m.Name);
                    trm.myData = this;
                    trm.Location = new Point(0, (num * 86));
                    p_PosMods.Controls.Add(trm);
                    sMult = 1 + (decimal)(pl.PosTower.Bonuses.SiloCap / 100);
                    trm.SetModuleData(m, sMult);

                    num++;
                }
            }
            SetActiveLinkColorBG(pl);
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");

        }

        private void SetActiveLinkColorBG(POS pl)
        {
            TowerReactMod tr;
            long lnkCnt = 0;

            // Now populate links
            foreach (ReactionLink rl in pl.ReactionLinks)
            {
                foreach (Control c in p_PosMods.Controls)
                {
                    tr = (TowerReactMod)c;

                    if (tr.ReactMod.ModuleID == rl.InpID)
                        tr.SetLinkColor(PlugInData.LColor[lnkCnt], rl.srcNm);
                    if (tr.ReactMod.ModuleID == rl.OutID)
                        tr.SetLinkColor(PlugInData.LColor[lnkCnt], rl.dstNm);
                }
                lnkCnt++;
            }
        }

        public void SetReactionModuleID(POS p)
        {
            decimal cnt = 0;
            SortedList SLs = new SortedList();
            // Need to give each and every tower module a different unique ID number as it comes in.
            //      Note: ID only needs to be Unique per the tower, not overall
            // BUT - only want to do this if the module does not already have such an ID - do not want
            // to change or modify an existing ID

            // 1. Scan for existing ID's - Add to list
            foreach (Module m in p.Modules)
            {
                if ((m.Category == "Mobile Reactor") || (m.Category == "Moon Mining") ||
                    (m.Category == "Silo"))
                {
                    // I only care about Reaction Modules with IDs
                    if (m.ModuleID != 0)
                    {
                        SLs.Add(m.ModuleID, m.Name);
                    }

                    if (m.Category == "Mobile Reactor")
                    {
                        if (m.Name.Contains("Complex"))
                            m.ModType = 4;
                        else if (m.Name.Contains("Medium Biochemical"))
                            m.ModType = 5;
                        else if (m.Name.Contains("Biochemical"))
                            m.ModType = 6;
                        else if (m.Name.Contains("Polymer"))
                            m.ModType = 7;
                        else
                            m.ModType = 3;  // Simple
                    }
                    else if (m.Category == "Moon Mining")
                    {
                        m.ModType = 1;
                    }
                    else if (m.Category == "Silo")
                    {
                        if (m.Name == "Silo")
                            m.ModType = 2;
                        else if (m.Name.Contains("Biochemical"))
                            m.ModType = 8;
                        else if (m.Name.Contains("Catalyst"))
                            m.ModType = 9;
                        else if (m.Name.Contains("General Storage"))
                            m.ModType = 11;
                        else if (m.Name.Contains("Hazardous Chemical"))
                            m.ModType = 12;
                        else if (m.Name.Contains("Hybrid Polymer"))
                            m.ModType = 13;
                        else
                            m.ModType = 10; // Junction
                    }
                }
            }

            // 2. Scan for un-assigned Modules : ID = 0
            // 3. Assign Unique ID to modules with existing ID of 0
            foreach (Module m in p.Modules)
            {
                cnt = 0;
                if ((m.Category == "Mobile Reactor") || (m.Category == "Moon Mining") ||
                    (m.Category == "Silo"))
                {
                    // I only care about Reaction Modules w/o IDs
                    if (m.ModuleID == 0)
                    {
                        cnt++;
                        // Find an unused Key value
                        while (SLs.ContainsKey(cnt))
                            cnt++;

                        m.ModuleID = cnt;
                        SLs.Add(cnt, m.Name);
                    }
                }
            }
        }

        public void TowerReactModuleUpdated(Module m, long cTyp, long linkID, string pbNm)
        {
            POS pl;
            // Get corret POS Object
            pl = PlugInData.PDL.Designs[SelReactPos];

            // A module reaction, or Link has been set
            switch (cTyp)
            {
                case 1:
                case 11:
                    // Mineral has been set
                    foreach (Module pm in pl.Modules)
                    {
                        if (pm.ModuleID == m.ModuleID)
                        {
                            pm.selMineral = new MoonSiloReactMineral(m.selMineral);
                            pm.MaxQty = m.MaxQty;
                            if (cTyp == 11)
                                ClearReactionLink(pm);
                            break;
                        }
                    }
                    break;
                case 2:
                case 12:
                    // Reaction has been set
                    foreach (Module pm in pl.Modules)
                    {
                        if (pm.ModuleID == m.ModuleID)
                        {
                            pm.selReact = new Reaction(m.selReact);
                            if (cTyp == 12)
                                ClearReactionLink(pm);
                            break;
                        }
                    }
                    break;
                case 3:
                    if (srcID != 0)
                    {
                        // selection of 2nd input has taken place.
                        ClearSelectData();
                    }
                    // Source has been selected
                    srcID = m.ModuleID;
                    srcNm = pbNm;

                    if (dstID != 0)
                        CheckAndSetModuleLinkIfCompatible();
                    break;
                case 4:
                    if (dstID != 0)
                    {
                        // selection of 2nd output has taken place.
                        ClearSelectData();
                    }
                    // Destination has been selected
                    dstID = m.ModuleID;
                    dstNm = pbNm;

                    if (srcID != 0)
                        CheckAndSetModuleLinkIfCompatible();
                    break;
                case 5:
                    // We Have a Silo Fill Level Change
                    foreach (Module pm in pl.Modules)
                    {
                        if (pm.ModuleID == m.ModuleID)
                        {
                            pm.CapQty = m.CapQty;
                            pm.CapVol = m.CapVol;
                        }
                    }
                    // Values updated, change tower timestamp
                    pl.React_TS = DateTime.Now;
                    break;
                case 6:
                    // User wants to clear this reaction link
                    foreach (ReactionLink rl in pl.ReactionLinks)
                    {
                        if (((rl.InpID == m.ModuleID) && (rl.srcNm == pbNm)) || ((rl.OutID == m.ModuleID) && (rl.dstNm == pbNm)))
                        {
                            pl.ReactionLinks.Remove(rl);
                            DisplayReactionModules();
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }

            SetReactChangedStatus(true, "Reaction Data Changed - Save Needed");
        }

        public bool CheckAndSetModuleLinkIfCompatible()
        {
            Module src = new Module();
            Module dst = new Module();
            long numFound = 0;
            int srcNum = 0, dstNum = 0;
            InOutData dio, sio;
            POS pl;
            // Get corret POS Object
            pl = PlugInData.PDL.Designs[SelReactPos];

            if (srcNm.Length > 0)
            {
                srcNum = Convert.ToInt32(srcNm.Substring(6, 1)) - 1;
            }

            if (dstNm.Length > 0)
            {
                dstNum = Convert.ToInt32(dstNm.Substring(5, 1)) - 1;
            }

            // Reaction has been set
            if (pl.ReactionLinks.Count >= MaxLink)
            {
                MessageBox.Show("Sorry, Maximum [20] Links has been Reached. Pester Sherk to allow more!");
                ClearSelectData();
                return false;
            }
            // Found tower - now find modules
            foreach (Module pm in pl.Modules)
            {
                if (pm.ModuleID == srcID)
                {
                    src = new Module(pm);
                    numFound++;
                }
                if (pm.ModuleID == dstID)
                {
                    dst = new Module(pm);
                    numFound++;
                }

                if (numFound == 2)
                    break;
            }

            if (src.ModuleID == dst.ModuleID)
            {
                MessageBox.Show("Invalid Link - Source and Destination of Link cannot be the Same Module.");
                ClearSelectData();
                return false;
            }

            // Verify location is not already linked
            foreach (ReactionLink rl in pl.ReactionLinks)
            {
                if (((rl.srcNm == srcNm) && (rl.InpID == src.ModuleID)) || ((rl.dstNm == dstNm) && (rl.OutID == dst.ModuleID)))
                {
                    MessageBox.Show("Invalid Link - Input or Output is Already Linked.");
                    ClearSelectData();
                    return false;
                }
            }

            // src == Module providing link source material
            // dst == Module providing link desctination for material
            if (src.Category == "Mobile Reactor")
            {
                // Source is a Mobile Reactor, Dest Can be: A Reactor or A Silo/Junction
                // Source Has different Outputs, make sure we check correct one
                sio = new InOutData((InOutData)src.selReact.outputs[srcNum]);

                if (dst.Category == "Silo")
                {
                    if (sio.typeID == dst.selMineral.typeID)
                    {
                        // Valid - complete the linkup
                        SetModuleReactionLink(pl);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Link - Mineral Output Type and Target Module Type are not compatible.");
                        ClearSelectData();
                        return false;
                        // Not valid
                    }
                }
                else if (dst.Category == "Mobile Reactor")
                {
                    dio = new InOutData((InOutData)dst.selReact.inputs[dstNum]);
                    if (dio.typeID == sio.typeID)
                    {
                        // Valid but not ideal - warn
                        // This is allowed, but more comes in than the reaction will use, so
                        // it will cost to do it - not ideal use of resources
                        DialogResult dr = MessageBox.Show("You will lose mineral / reaction production with this link, do you want to continue?", "Make Link", MessageBoxButtons.YesNo);

                        if (dr == DialogResult.Yes)
                        {
                            // Well, stupid things do happen
                            SetModuleReactionLink(pl);
                        }
                        else
                        {
                            ClearSelectData();
                            return false;
                        }
                    }
                    else
                    {
                        // Not valid
                        MessageBox.Show("Invalid Link - Mineral Output Type and Reaction Input Types are not compatible.");
                        ClearSelectData();
                        return false;
                    }
                }
                else
                {
                    // Invalid fitting
                    MessageBox.Show("Invalid Link - Module Types are Not Compatible.");
                    ClearSelectData();
                    return false;
                }
            }
            else if (src.Category == "Moon Mining")
            {
                // Source is a Moon miner, Dest can be: A Mobile Reactor or a Silo / Junction
                // Only one possible output
                if (dst.Category == "Silo")
                {
                    if (src.selMineral.typeID == dst.selMineral.typeID)
                    {
                        // Valid Link
                        SetModuleReactionLink(pl);
                    }
                    else
                    {
                        // Not valid
                        MessageBox.Show("Invalid Link - Mineral Output Type and Silo Types are not compatible.");
                        ClearSelectData();
                        return false;
                    }
                }
                else if (dst.Category == "Mobile Reactor")
                {
                    dio = new InOutData((InOutData)dst.selReact.inputs[dstNum]);
                    if (src.selMineral.typeID == dio.typeID)
                    {
                        // Valid Link
                        SetModuleReactionLink(pl);
                    }
                    else
                    {
                        // Not valid
                        MessageBox.Show("Invalid Link - Mineral Output Type and Reaction Input Types are not compatible.");
                        ClearSelectData();
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Link - Moon miner input is always the same as Selected Mineral. No link possible here.");
                    ClearSelectData();
                    return false;
                }
            }
            else if (src.Category == "Silo")
            {
                // Input is a Silo type - ensure compatibility with output of the other module in the link
                // Silo Outputs can be: Reaction, Silo / Junction
                if (dst.Category == "Mobile Reactor")
                {
                    dio = new InOutData((InOutData)dst.selReact.inputs[dstNum]);
                    if (src.selMineral.typeID == dio.typeID)
                    {
                        // Valid Link
                        SetModuleReactionLink(pl);
                    }
                    else
                    {
                        // Not valid
                        MessageBox.Show("Invalid Link - Mineral Output Type and Reaction Input Types are not compatible.");
                        ClearSelectData();
                        return false;
                    }
                }
                else if (dst.Category == "Silo")
                {
                    // Silo or Moon Mining type - check mineral compabitility
                    if (src.selMineral.typeID == dst.selMineral.typeID)
                    {
                        // Valid
                        SetModuleReactionLink(pl);
                    }
                    else
                    {
                        // Not valid
                        MessageBox.Show("Invalid Link - Mineral Output Type and Mineral Input Types are not compatible.");
                        ClearSelectData();
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Link - Silo output cannot be used as input for this Module.");
                    ClearSelectData();
                    return false;
                }
            }
            ClearSelectData();
            return false;
        }

        private decimal[] GetQtyVolForLink()
        {
            decimal mult, qty, vol, bPrice, rQty;
            decimal[] retV = new decimal[2];
            retV[0] = 0;
            retV[1] = 100;
            Module SrcMod = new Module();
            Module DstMod = new Module();
            POS pl;
            // Get corret POS Object
            pl = PlugInData.PDL.Designs[SelReactPos];

            foreach (Module m in pl.Modules)
            {
                if (m.ModuleID == srcID)
                {
                    SrcMod = new Module(m);
                    break;
                }
            }
            foreach (Module m in pl.Modules)
            {
                if (m.ModuleID == dstID)
                {
                    DstMod = new Module(m);
                    break;
                }
            }

            if ((SrcMod.Category == "Silo") || (SrcMod.Category == "Moon Mining"))
            {
                // Plain mineral incomming

                // There are some cases, where the destination is a reaction, that the reaction
                // should override on silo due to the selMineral.reactQty being just plain Wrong!
                if ((DstMod.ModType == 5) || (DstMod.ModType == 6) || (DstMod.ModType == 7))
                {
                    foreach (InOutData iod in DstMod.selReact.inputs)
                    {
                        if (iod.typeID == SrcMod.selMineral.typeID)
                        {
                            qty = iod.qty;
                            mult = SrcMod.selMineral.portionSize;
                            vol = SrcMod.selMineral.volume;
                            bPrice = SrcMod.selMineral.basePrice;
                            rQty = SrcMod.selMineral.reactQty;

                            retV[0] = qty * rQty;
                            retV[1] = retV[0] * vol * mult;
                        }
                    }
                }
                else
                {
                    qty = SrcMod.selMineral.reactQty;
                    mult = SrcMod.selMineral.portionSize;
                    vol = SrcMod.selMineral.volume;
                    bPrice = SrcMod.selMineral.basePrice;

                    retV[0] = qty;
                    retV[1] = retV[0] * vol * mult;
                }
            }
            else if (SrcMod.Category == "Mobile Reactor")
            {
                // Reaction mineral incomming. Use IO mod Qty as multiplier
                foreach (InOutData iod in SrcMod.selReact.outputs)
                {
                    if (iod.typeID == DstMod.selMineral.typeID)
                    {
                        qty = iod.qty;
                        mult = DstMod.selMineral.portionSize;
                        vol = DstMod.selMineral.volume;
                        bPrice = DstMod.selMineral.basePrice;
                        rQty = DstMod.selMineral.reactQty;

                        retV[0] = qty * rQty;
                        retV[1] = retV[0] * vol * mult;
                    }
                }
            }
            else
            {
            }

            return retV;
        }

        private void SetModuleReactionLink(POS pl)
        {
            ReactionLink nrl;
            decimal[] qv;

            qv = GetQtyVolForLink();

            nrl = new ReactionLink(1, srcID, dstID, qv[0], qv[1], Color.Green, srcNm, dstNm);

            pl.ReactionLinks.Add(nrl);

            SetLinkColorBG(pl);

            ClearSelectData();
        }

        private void b_SaveStatus_Click(object sender, EventArgs e)
        {
            b_SaveStatus.Enabled = false;
            PlugInData.PDL.SaveDesignListing();
            SetChangedStatus(false, "");
            SetReactChangedStatus(false, "");

            SetReactionValuesAndUpdateDisplay();
            PopulateTowerModuleDisplay();
            b_SaveStatus.Enabled = true;
        }

        private void b_ClearLinks_Click(object sender, EventArgs e)
        {
            POS pl;
            // Get corret POS Object
            pl = PlugInData.PDL.Designs[SelReactPos];
            // Get corret POS Object

            pl.ReactionLinks.Clear();
            DisplayReactionModules();

            SetReactChangedStatus(true, "Reaction Data Changed - Save Needed");
        }

        private void ClearReactionLink(Module m)
        {
            ArrayList tmpLst;
            POS pl;
            bool linkCleared = false;
            // Get corret POS Object
            pl = PlugInData.PDL.Designs[SelReactPos];

            tmpLst = new ArrayList();
            foreach (ReactionLink rl in pl.ReactionLinks)
            {
                if ((rl.InpID != m.ModuleID) && (rl.OutID != m.ModuleID))
                {
                    tmpLst.Add(rl);
                }
                else
                    linkCleared = true;
            }
            pl.ReactionLinks = new ArrayList(tmpLst);
            if (linkCleared)
                DisplayReactionModules();
        }

        private void ClearSelectData()
        {
            TowerReactMod tr;

            foreach (Control c in p_PosMods.Controls)
            {
                tr = (TowerReactMod)c;

                if (tr.ReactMod.ModuleID == srcID)
                    tr.ClearLinkSel(srcNm);
                if (tr.ReactMod.ModuleID == dstID)
                    tr.ClearLinkSel(dstNm);
            }

            srcID = 0;
            dstID = 0;
            srcNm = "";
            dstNm = "";
        }

        private void SetLinkColorBG(POS pl)
        {
            TowerReactMod tr;

            foreach (Control c in p_PosMods.Controls)
            {
                tr = (TowerReactMod)c;

                if (tr.ReactMod.ModuleID == srcID)
                    tr.SetLinkColor(PlugInData.LColor[(pl.ReactionLinks.Count - 1)], srcNm);
                if (tr.ReactMod.ModuleID == dstID)
                    tr.SetLinkColor(PlugInData.LColor[(pl.ReactionLinks.Count - 1)], dstNm);
            }
        }

        public Bitmap GetIcon(string icon)
        {
            Bitmap bmp;
            Image img;

            img = EveHQ.Core.ImageHandler.GetImage(icon);

            if (img == null)
            {
                bmp = new Bitmap(il_system.Images[0]);
            }
            else
                bmp = new Bitmap(img);

            return bmp;
        }

        public void TowerSelectedForReactionLinks(string name)
        {
            Sel_React_Pos = name;

            // Populate Module list on RH side with correct controls, etc... for this POS
            if (ReactChanged == true)
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("The currently Selected Reaction has Changed. Do you want to Save it?", "Save Reaction State", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    PlugInData.PDL.SaveDesignListing();
                    SetChangedStatus(false, "");
                }
                else
                {
                    PlugInData.PDL.LoadDesignListing();
                }
            }
            SetReactChangedStatus(false, "");

            SelReactPos = name;

            DisplayReactionModules();
        }

        #endregion


        #region Tower Notifications

        private TreeNode FindTreeNode(TreeNodeCollection tnc, string node)
        {
            foreach (TreeNode tn in tnc)
            {
                if (tn.Text == node)
                    return tn;
            }

            return null;
        }

        private void PopulatePlayerList()
        {
            TreeNode mtn, tn;

            tv_Players.Nodes.Clear();

            foreach (Player p in PlugInData.PL.Players)
            {
                mtn = tv_Players.Nodes.Add(p.Name);
                mtn.Name = p.Name;
                if (p.Email1 != "")
                    tn = mtn.Nodes.Add(p.Email1);
                if (p.Email2 != "")
                    tn = mtn.Nodes.Add(p.Email2);
                if (p.Email3 != "")
                    tn = mtn.Nodes.Add(p.Email3);
            }
        }

        private void PopulateNotificationList()
        {
            TreeNode mtn, tn, pn;
            string line;

            tv_Notifications.Nodes.Clear();

            foreach (PosNotify p in PlugInData.NL.NotifyList)
            {
                mtn = FindTreeNode(tv_Notifications.Nodes, p.Tower);
                if (mtn == null)
                {
                    mtn = tv_Notifications.Nodes.Add(p.Tower);
                    mtn.Name = p.Tower;
                }
                line = p.Type + " [ Start at " + p.InitQty + " " + p.Initial + " | Every " + p.FreqQty + " " + p.Frequency + " after.]";
                tn = mtn.Nodes.Add(line);

                foreach (Player pl in p.PList.Players)
                {
                    pn = tn.Nodes.Add(pl.Name);
                }
            }
        }

        private void tsb_NewPlayer_Click(object sender, EventArgs e)
        {
            AddPlayer apf = new AddPlayer();
            apf.myData = this;
            apf.SetupData("Add a New Player", "");

            apf.ShowDialog();
            PopulatePlayerList();
        }

        private void tsb_NewNotification_Click(object sender, EventArgs e)
        {
            Notification not = new Notification();
            not.myData = this;
            not.SetupData("Add a New Notification", "", "");

            not.ShowDialog();
            PopulateNotificationList();
        }

        private void tsb_Test_Click(object sender, EventArgs e)
        {
            PlugInData.NL.mailSendErr = false;
            bgw_ManualSend.RunWorkerAsync();
        }

        private void tsm_EditNotify_Click(object sender, EventArgs e)
        {
            TreeNode sel;
            string root, rule;

            sel = tv_Notifications.SelectedNode;
            if (sel == null)
                return;

            if (sel.Parent == null)
            {
                if (sel.Nodes.Count > 1)
                {
                    DialogResult dr = MessageBox.Show("You Must Select the Rule you wish to Edit, Not the Tower.", "Edit Notification", MessageBoxButtons.OK);
                    return; // Request they select a rule for the tower
                }

                root = sel.Text;
                rule = sel.FirstNode.Text;
            }
            else
            {
                if (sel.Parent.Parent != null)
                    sel = sel.Parent;

                rule = sel.Text;
                root = sel.Parent.Text;
            }

            Notification not = new Notification();
            not.myData = this;
            not.SetupData("Edit a Notification", root, rule);

            not.ShowDialog();
            PopulateNotificationList();
        }

        private void tsm_RemoveNotify_Click(object sender, EventArgs e)
        {
            TreeNode sel;
            ArrayList tLst = new ArrayList();
            string root, rule, line;

            sel = tv_Notifications.SelectedNode;
            if (sel == null)
                return;

            if (sel.Parent == null)
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("Are you sure you want to Delete All Tower Rules for :" + sel.Text, "Delete All Rules", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    root = sel.Text;
                    foreach (PosNotify pn in PlugInData.NL.NotifyList)
                    {
                        if (pn.Tower != root)
                            tLst.Add(pn);
                    }
                    PlugInData.NL.NotifyList = new ArrayList(tLst);
                }
                else
                    return;
            }
            else
            {
                if (sel.Parent.Parent != null)
                    sel = sel.Parent;

                rule = sel.Text;
                root = sel.Parent.Text;

                foreach (PosNotify pn in PlugInData.NL.NotifyList)
                {
                    line = pn.Type + " [ Start at " + pn.InitQty + " " + pn.Initial + " | Every " + pn.FreqQty + " " + pn.Frequency + " after.]";
                    if ((pn.Tower == root) && (line == rule))
                    {
                        DialogResult dr = MessageBox.Show("Are you sure you want to Delete the Rule for :" + root + "\n Rule: " + rule, "Delete Rule", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            PlugInData.NL.NotifyList.Remove(pn);
                        }
                        break;
                    }
                }
            }

            PlugInData.NL.SaveNotificationList();
            PopulateNotificationList();
        }

        private void tsm_EditPlayer_Click(object sender, EventArgs e)
        {
            TreeNode plyr;
            AddPlayer apf = new AddPlayer();
            apf.myData = this;

            plyr = tv_Players.SelectedNode;
            if (plyr != null)
            {
                if (plyr.Parent != null)
                    plyr = plyr.Parent;

                apf.SetupData("Edit Player", plyr.Text);

                apf.ShowDialog();
                PopulatePlayerList();
            }
        }

        private void tsm_RemovePlayer_Click(object sender, EventArgs e)
        {
            TreeNode plyr;
            bool delYes = false;

            foreach (Player p in PlugInData.PL.Players)
            {
                plyr = tv_Players.SelectedNode;
                if (plyr != null)
                {
                    if (plyr.Parent != null)
                        plyr = plyr.Parent;

                    if (p.Name == plyr.Text)
                    {
                        // Query user to save any changes
                        DialogResult dr = MessageBox.Show("Are you sure you want to Delete :" + p.Name, "Delete Player", MessageBoxButtons.YesNo);

                        if (dr == DialogResult.Yes)
                        {
                            PlugInData.PL.Players.Remove(p);
                            delYes = true;
                            break;
                        }
                    }
                }
            }
            if (delYes)
            {
                PlugInData.PL.SavePlayerList();
                PopulatePlayerList();
            }
        }

        private void bgw_ManualSend_DoWork(object sender, DoWorkEventArgs e)
        {
            TreeNode sel;
            string root, rule, line;

            sel = tv_Notifications.SelectedNode;
            if (sel == null)
                return;

            if (sel.Parent == null)
            {
                if (sel.Nodes.Count > 1)
                {
                    DialogResult dr = MessageBox.Show("You Must Select the Rule you wish to Test, Not the Tower.", "Send Notification", MessageBoxButtons.OK);
                    return; // Request they select a rule for the tower
                }

                root = sel.Text;
                rule = sel.FirstNode.Text;
            }
            else
            {
                if (sel.Parent.Parent != null)
                    sel = sel.Parent;

                rule = sel.Text;
                root = sel.Parent.Text;
            }

            foreach (PosNotify pn in PlugInData.NL.NotifyList)
            {
                line = pn.Type + " [ Start at " + pn.InitQty + " " + pn.Initial + " | Every " + pn.FreqQty + " " + pn.Frequency + " after.]";
                if ((pn.Tower == root) && (rule == line))
                {
                    if (PlugInData.PDL.Designs.ContainsKey(root))
                    {
                        PlugInData.NL.SendNotification(pn, PlugInData.PDL.Designs[root]);
                    }
                    break;
                }
            }
        }

        #endregion


        #region Module API Link Handling

        private void AddTowerToModLinkMenu(string twr, string crp, string mn, string syst)
        {
            DevComponents.AdvTree.Node corp = null, towr;
            DevComponents.AdvTree.Node systM = null;
            string twrNm;

            if (crp.Length < 1)  // If no corp name, just leave here
                return;

            foreach (DevComponents.AdvTree.Node cE in ct_MonTwrForLink.Nodes)
            {
                if (cE.Text.Equals(crp))
                {
                    corp = cE;

                    foreach (DevComponents.AdvTree.Node sE in cE.Nodes)
                    {
                        if (sE.Text.Equals(syst))
                        {
                            systM = sE;
                            break;
                        }
                    }
                    break;
                }
            }

            if (corp == null)
            {
                // Corp does not exist, so add it
                corp = new DevComponents.AdvTree.Node();
                corp.Text = crp;
                corp.Selectable = false;
                corp.Expanded = true;
                ct_MonTwrForLink.Nodes.Add(corp);
            }

            if (systM == null)
            {
                // Corp does not exist, so add it
                systM = new DevComponents.AdvTree.Node();
                systM.Text = syst;
                systM.Selectable = false;
                systM.Expanded = false;
                corp.Nodes.Add(systM);
            }

            twrNm = twr + " [" + mn + "]";
            towr = new DevComponents.AdvTree.Node();
            towr.Text = twrNm;
            // Tower does not exist - add it
            systM.Nodes.Add(towr);
        }

        public void ClearModuleLinkAndRedraw(long modID, string twrName)
        {
            POS st;

            if (!PlugInData.ModuleLinks.ContainsKey(modID))
                return;

            if (!PlugInData.PDL.Designs.ContainsKey(twrName))
                return;

            st = PlugInData.PDL.Designs[twrName];

            foreach (Module m in st.Modules)
            {
                if (m.modID == modID)
                {
                    m.modID = 0;
                    break;
                }
            }
            PlugInData.ModuleLinks.Remove(modID);
            // Update Link module color backgrounds appropriately
            PlugInData.SaveModuleLinkListToDisk();
            PlugInData.PDL.SaveDesignListing();

            UpdateModuleLinks();
        }

        public void SetModuleLinkAndRedraw()
        {
            POS st;
            string tnm;
            
            // Create a module link with the desired information
            if (!PlugInData.ModuleLinks.ContainsKey(PlugInData.LinkInProgress.modID))
            {
                PlugInData.ModuleLinks.Add(PlugInData.LinkInProgress.modID, PlugInData.LinkInProgress);

                tnm = ct_MonTwrForLink.Text;
                tnm = tnm.Substring(0, tnm.IndexOf("[") - 1);
                st = PlugInData.PDL.Designs[tnm];

                foreach (Module m in st.Modules)
                {
                    if (PlugInData.LinkInProgress.modLoc.Equals(m.Location))
                    {
                        // Found correct module, set moduleID
                        m.modID = PlugInData.LinkInProgress.modID;
                    }
                }

                PlugInData.LinkInProgress = new ModuleLink();
            }
            
            PlugInData.SaveModuleLinkListToDisk();
            PlugInData.PDL.UpdateTowerSiloValuesForAPI();   // New link, so update the Data !
            PlugInData.PDL.SaveDesignListing();

            // Update Link module color backgrounds appropriately
            UpdateModuleLinks();
        }

        private void UpdateModuleLinks()
        {
            long tModID = 0;
            int colCt = 0;
            int rowCt = 0;
            POS st;
            string tnm, twrN;
            LinkModule LM;

            if (ct_MonTwrForLink.SelectedNode == null)
                return;

            twrN = ct_MonTwrForLink.Text;
            tnm = twrN.Substring(0, twrN.IndexOf("[") - 1);
            st = PlugInData.PDL.Designs[tnm];

            gp_TowerSelectList.Controls.Clear();
            foreach (Module m in st.Modules)
            {
                switch (m.typeID)
                {
                    case 16216: // Mobile Labs
                    case 28351: //
                    case 32245: //
                    case 14343: // Silo Types
                    case 17982: //
                    case 25270: //
                    case 25271: //
                    case 25280: //
                    case 25821: //
                    case 30655: // Silo Types
                    case 27897: // JB
                    case 17621: // CHA
                    case 12237: // SMA
                    case 24646: // CSMA
                        LM = new LinkModule();
                        if (m.modID == 0)
                        {
                            tModID++;
                        }
                        
                        if (m.State.Equals("Online"))
                            LM.SetLinkModuleInfo(m.Name, m.modID, m.typeID, tnm, st.corpID, st.CorpName, 0, false, m.Location, m.selMineral.name, "");
                        else
                            LM.SetLinkModuleInfo(m.Name, m.modID, m.typeID, tnm, st.corpID, st.CorpName, 0, false, m.Location, m.State, "");
                        LM.Location = new Point(colCt * (LM.Width + 1), rowCt * (LM.Height + 1));
                        colCt++;
                        if (colCt >= Math.Floor((float)(gp_TowerSelectList.Width / (LM.Width+1))))
                        {
                            rowCt++;
                            colCt = 0;
                        }
                        gp_TowerSelectList.Controls.Add(LM);
                        break;
                    default:
                        break;
                }
            }

            colCt = 0;
            rowCt = 0;
            gp_SystemModules.Controls.Clear();
            if (PlugInData.CpML.ContainsKey(st.CorpName))
            {
                foreach (APIModule apm in PlugInData.CpML[st.CorpName].Values)
                {

                    if (PlugInData.SystemIDToStr.ContainsKey(apm.systemID) && (PlugInData.SystemIDToStr[apm.systemID].Equals(st.System)))
                    {
                        switch (apm.typeID)
                        {
                            case 16216: // Mobile Labs
                            case 28351: //
                            case 32245: //
                            case 14343: // Silo Types
                            case 17982: //
                            case 25270: //
                            case 25271: //
                            case 25280: //
                            case 25821: //
                            case 30655: // Silo Types
                            case 27897: // JB
                            case 17621: // CHA
                            case 12237: // SMA
                            case 24646: // CSMA
                                LM = new LinkModule();

                                LM.SetLinkModuleInfo(apm.name, apm.modID, apm.typeID, tnm, st.corpID, st.CorpName, apm.systemID, true, "", "", apm.updateTime);
                                LM.Location = new Point(colCt * (LM.Width + 1), rowCt * (LM.Height + 1));
                                colCt++;
                                if (colCt >= Math.Floor((float)(gp_SystemModules.Width / (LM.Width + 1))))
                                {
                                    rowCt++;
                                    colCt = 0;
                                }
                                gp_SystemModules.Controls.Add(LM);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            foreach (ModuleLink ML in PlugInData.ModuleLinks.Values)
            {
                if (ct_MonTwrForLink.Text.Contains(ML.twrName))
                {
                    // Update Tower side
                    foreach (LinkModule LMd in gp_TowerSelectList.Controls)
                    {
                        if (ML.modID == LMd.modID)
                            LMd.UpdateLinkColor(ML.LinkColor, "");
                    }
                }
                // Update System side
                foreach (LinkModule LMd in gp_SystemModules.Controls)
                {
                    if (ML.modID == LMd.modID)
                        LMd.UpdateLinkColor(ML.LinkColor, ML.twrName);
                }
            }
        }

        private void ct_MonTwrForLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateModuleLinks();
        }

        private void gp_SystemModules_Resize(object sender, EventArgs e)
        {
            ct_MonTwrForLink_SelectedIndexChanged(sender, e);
        }

        #endregion


        #region Stored Fuel Display

        private long IsItemTowerFuelType(long ID)
        {
            switch (ID)
            {
                case 44:    // Enr Uranium
                    return 1;
                case 3683:  // Oxygen
                    return 2;
                case 3689:  // Mech Parts
                    return 3;
                case 9832:  // Coolant
                    return 4;
                case 9848:  // Robotics
                    return 5;
                case 16272: // Heavy Water
                    return 10;
                case 16273: // Liquid Ozone
                    return 11;
                case 24592: // Charter
                case 24593: // Charter
                case 24594: // Charter
                case 24595: // Charter
                case 24596: // Charter
                case 24597: // Charter
                    return 13;
                case 17888: // Nitrogen Isotopes
                    return 6;
                case 16274: // Helium Isotopes
                    return 7;
                case 17889: // Hydrogen Isotopes
                    return 8;
                case 17887: // Oxygen Isotopes
                    return 9;
                case 16275: // Strontium
                    return 12;
                default:
                    return 0;
            }
        }

        private bool IsItemTowerModType(long ID)
        {
            if (PlugInData.ML.Modules.ContainsKey(ID))
                return true;

            return false;
        }

        public void PopulateStoredFuelDisplay()
        {
            bool hasFI = false, hasFIM = false, locE = false;
            long itmT;
            DevComponents.AdvTree.Node Owner, LocN, ModN;
            DevComponents.AdvTree.Cell Qtys;

            long[] FuelTotals, LocFT, ModFT;

            if (PlugInData.bgw_APIUpdate.IsBusy)
                return;

            at_StoredFuel.Nodes.Clear();
            tp_ModsAndFuel.Visible = true;
            LocFT = new long[14];

            foreach (var ch in PlugInData.ChML)
            {
                // This is character name <-- Primary Node
                if (ch.Value.Values.Count > 0)
                {
                    Owner = new DevComponents.AdvTree.Node(ch.Key);
                    hasFIM = false;
                    FuelTotals = new long[14];

                    // APIModule is a location, ship, etc - not an actual data item
                    foreach (APIModule APM in ch.Value.Values)
                    {
                        LocN = new DevComponents.AdvTree.Node();
                        locE = false;
                        foreach (DevComponents.AdvTree.Node LC in Owner.Nodes)
                        {
                            if (LC.Text.Equals(APM.locName))
                            {
                                locE = true;
                                LocN = LC;
                                break;
                            }
                        }

                        if (!locE)
                        {
                            LocN.Text = APM.locName;
                            LocFT = new long[14];
                        }
                        else
                        {
                            for (int x = 1; x < 15; x++)
                            {

                                LocFT[x - 1] = Convert.ToInt64(Double.Parse(LocN.Cells[x].Text));
                            }
                        }

                        hasFI = false;
                        ModFT = new long[14];
                        ModN = new DevComponents.AdvTree.Node(APM.name);

                        if (APM.Items.Count > 0)
                        {
                            hasFI = PopulateModuleItemNodes(APM.Items, ModFT, ModN);
                        }
                        else
                        {
                            itmT = IsItemTowerFuelType(APM.typeID);

                            if (itmT > 0)
                            {
                                hasFI = true;
                                 ModFT[itmT] += APM.Qty;
                            }
                        }

                        if (hasFI)
                        {
                            hasFIM = true;
                            for (int x = 0; x < 14; x++)
                            {
                                Qtys = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", ModFT[x]));
                                LocFT[x] += ModFT[x];
                                ModN.Cells.Add(Qtys);
                            }
                            LocN.Nodes.Add(ModN);
                            if (locE)
                            {
                                for (int x = 1; x < 15; x++)
                                {
                                    LocN.Cells[x].Text = String.Format("{0:#,0.#}", LocFT[x-1]);
                                    FuelTotals[x-1] += ModFT[x-1];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < 14; x++)
                                {
                                    Qtys = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", LocFT[x]));
                                    FuelTotals[x] += LocFT[x];
                                    LocN.Cells.Add(Qtys);
                                }
                                Owner.Nodes.Add(LocN);
                            }
                        }                          
                    }

                    if (hasFIM)
                    {
                        for (int x = 0; x < 14; x++)
                        {
                            Qtys = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", FuelTotals[x]));
                            Owner.Cells.Add(Qtys);
                        }
                        at_StoredFuel.Nodes.Add(Owner);
                    }
                }
            }

            foreach (var ch in PlugInData.CpML)
            {
                // This is character name <-- Primary Node
                if (ch.Value.Values.Count > 0)
                {
                    Owner = new DevComponents.AdvTree.Node(ch.Key);
                    hasFIM = false;
                    FuelTotals = new long[14];

                    // APIModule is a location, ship, etc - not an actual data item
                    foreach (APIModule APM in ch.Value.Values)
                    {
                        LocN = new DevComponents.AdvTree.Node();
                        locE = false;
                        foreach (DevComponents.AdvTree.Node LC in Owner.Nodes)
                        {
                            if (LC.Text.Equals(APM.locName))
                            {
                                locE = true;
                                LocN = LC;
                                break;
                            }
                        }

                        if (!locE)
                        {
                            LocN.Text = APM.locName;
                            LocFT = new long[14];
                        }
                        else
                        {
                            for (int x = 1; x < 15; x++)
                            {
                                LocFT[x - 1] = Convert.ToInt64(Double.Parse(LocN.Cells[x].Text));
                            }
                        }

                        hasFI = false;
                        ModFT = new long[14];
                        ModN = new DevComponents.AdvTree.Node(APM.name);

                        if (APM.name.Contains("Hangar"))
                        {
                        }
                        if (APM.Items.Count > 0)
                        {
                            hasFI = PopulateModuleItemNodes(APM.Items, ModFT, ModN);
                        }
                        else
                        {
                            itmT = IsItemTowerFuelType(APM.typeID);

                            if (itmT > 0)
                            {
                                hasFI = true;
                                ModFT[itmT] += APM.Qty;
                            }
                        }

                        if (hasFI)
                        {
                            hasFIM = true;
                            for (int x = 0; x < 14; x++)
                            {
                                Qtys = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", ModFT[x]));
                                LocFT[x] += ModFT[x];
                                ModN.Cells.Add(Qtys);
                            }
                            LocN.Nodes.Add(ModN);
                            if (locE)
                            {
                                for (int x = 1; x < 15; x++)
                                {
                                    LocN.Cells[x].Text = String.Format("{0:#,0.#}", LocFT[x - 1]);
                                    FuelTotals[x - 1] += ModFT[x - 1];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < 14; x++)
                                {
                                    Qtys = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", LocFT[x]));
                                    FuelTotals[x] += LocFT[x];
                                    LocN.Cells.Add(Qtys);
                                }
                                Owner.Nodes.Add(LocN);
                            }
                        }
                    }

                    if (hasFIM)
                    {
                        for (int x = 0; x < 14; x++)
                        {
                            Qtys = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", FuelTotals[x]));
                            Owner.Cells.Add(Qtys);
                        }
                        at_StoredFuel.Nodes.Add(Owner);
                    }
                }
            }
        }

        public bool PopulateModuleItemNodes(SortedList<long, PlugInData.ModuleItem> MIs, long[] FT, DevComponents.AdvTree.Node RN)
        {
            long itmT;
            bool hasFI = false;
            bool sNodeFI = false;
            bool hasAnyFI = false;
            DevComponents.AdvTree.Node ModN;
            DevComponents.AdvTree.Cell Qtys;
            long[] FTotes;

            foreach (PlugInData.ModuleItem MI in MIs.Values)
            {
                FTotes = new long[14];
                hasFI = false;
                sNodeFI = false;
                ModN = new DevComponents.AdvTree.Node(MI.name);

                if ((MI.SubItems != null) && (MI.SubItems.Count > 0))
                {
                    // I have a container to parse, make recursive call here
                    sNodeFI = PopulateModuleItemNodes(MI.SubItems, FTotes, ModN);
                }
                else
                {
                    // Items to look at and total up                                   
                    itmT = IsItemTowerFuelType(MI.typeID);
                    if (itmT > 0)
                    {
                        hasFI = true;
                        FTotes[itmT] += MI.qty;
                    }
                }

                if (hasFI || sNodeFI)
                {
                    hasAnyFI = true;

                    for (int x = 0; x < 14; x++)
                    {
                        Qtys = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", FTotes[x]));
                        FT[x] += FTotes[x];
                        ModN.Cells.Add(Qtys);
                    }
                    RN.Nodes.Add(ModN);
                }
            }
            return hasAnyFI;
        }

        private void cbx_FuelInStations_CheckedChanged(object sender, EventArgs e)
        {
            PopulateStoredFuelDisplay();
        }

        private void cbx_FuelInShips_CheckedChanged(object sender, EventArgs e)
        {
            PopulateStoredFuelDisplay();
        }

        private void cbx_FuelInSpace_CheckedChanged(object sender, EventArgs e)
        {
            PopulateStoredFuelDisplay();
        }

        private void rb_LocationSort_CheckedChanged(object sender, EventArgs e)
        {
            PopulateStoredFuelDisplay();
        }

        private void b_RefreshAPI_Click(object sender, EventArgs e)
        {
            PopulateStoredFuelDisplay();
        }

        //public void PopulateStoredModsDisplay()
        //{
        //    bool fiSh, fiSt, fiSp;
        //    bool locS;
        //    long itmID, itmT;
        //    string loc;
        //    DevComponents.AdvTree.Node Owner, SubN, LocN;
        //    DevComponents.AdvTree.Cell Qtys;

        //    SortedList<string, SortedList<string, long>> ModQty = new SortedList<string, SortedList<string, long>>();
        //    SortedList<string, long> LModQty = new SortedList<string, long>();
        //    SortedList<string, DevComponents.AdvTree.Node> Locns = new SortedList<string, DevComponents.AdvTree.Node>();
        //    APIModule APIM;

        //    if (PlugInData.bgw_APIUpdate.IsBusy)
        //        return;

        //    tp_ModsAndFuel.Visible = true;

        //    fiSh = cbx_FuelInShips.Checked;
        //    fiSt = cbx_FuelInStations.Checked;
        //    fiSp = cbx_FuelInSpace.Checked;

        //    locS = rb_LocationSort.Checked;

        //    // Build sorted data listing
        //    at_TowerMods.Nodes.Clear();
        //    if (locS)
        //    {
        //    }
        //    else
        //    {
        //        // Default case - only active for the moment - No re-sort required
        //        foreach (var ch in PlugInData.ChML)
        //        {
        //            Owner = new DevComponents.AdvTree.Node(ch.Key);
        //            Locns.Clear();
        //            ModQty.Clear();

        //            foreach (var itm in ch.Value)
        //            {
        //                // Is the item itelf a Module or is it a Container
        //                itmID = itm.Key;
        //                APIM = (APIModule)itm.Value;

        //                if (APIM.Items.Count > 0)
        //                {
        //                    // Container -- Need a CAN Parser, would appear under locations
        //                    // Will show if inside of a can or a ship's hold
        //                    foreach (var l in APIM.Items)
        //                    {
        //                        if ((l.Value.SubItems != null) && (l.Value.SubItems.Count > 0))
        //                        {
        //                            // Container Inside of another Container (umm... CHA, Corp Hanger, Can inside of Ship, Can in Hanger, etc.)
        //                        }
        //                        else
        //                        {
        //                            // Items inside of container only                                   
        //                            if (IsItemTowerModType(l.Value.typeID))
        //                            {
        //                                // We have a module inside of this Can
        //                                loc = APIM.locName;
        //                                if (Locns.ContainsKey(loc))
        //                                {
        //                                    LocN = Locns[loc];
        //                                }
        //                                else
        //                                {
        //                                    LocN = new DevComponents.AdvTree.Node(loc);
        //                                    Locns.Add(loc, LocN);
        //                                }

        //                                if (ModQty.ContainsKey(loc) && ModQty[loc].ContainsKey(APIM.name))
        //                                {
        //                                    ModQty[loc][APIM.name] += l.Value.qty;
        //                                }
        //                                else if (ModQty.ContainsKey(loc) && !ModQty[loc].ContainsKey(APIM.name))
        //                                {
        //                                    ModQty[loc].Add(APIM.name, l.Value.qty);
        //                                }
        //                                else
        //                                {
        //                                    ModQty.Add(loc, new SortedList<string, long>());
        //                                    ModQty[loc].Add(APIM.name, l.Value.qty);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    // Item
        //                    if (IsItemTowerModType(APIM.typeID))
        //                    {
        //                        loc = APIM.locName;
        //                        if (Locns.ContainsKey(loc))
        //                        {
        //                            LocN = Locns[loc];
        //                        }
        //                        else
        //                        {
        //                            LocN = new DevComponents.AdvTree.Node(loc);
        //                            Locns.Add(loc, LocN);
        //                        }

        //                        if (ModQty.ContainsKey(loc) && ModQty[loc].ContainsKey(APIM.name))
        //                        {
        //                            ModQty[loc][APIM.name] += APIM.Qty;
        //                        }
        //                        else if (ModQty.ContainsKey(loc) && !ModQty[loc].ContainsKey(APIM.name))
        //                        {
        //                            ModQty[loc].Add(APIM.name, APIM.Qty);
        //                        }
        //                        else
        //                        {
        //                            ModQty.Add(loc, new SortedList<string, long>());
        //                            ModQty[loc].Add(APIM.name, APIM.Qty);
        //                        }
        //                    }
        //                }
        //            }

        //            foreach (var v in fuelI)
        //            {
        //                LocN = Locns[v.Key];
        //                stot = new long[14];
        //                foreach (var f in v.Value)
        //                {
        //                    ltot = f.Value;
        //                    SubN = new DevComponents.AdvTree.Node(PlugInData.ItemIDToName[f.Key]);

        //                    for (int x = 1; x < 14; x++)
        //                    {
        //                        if (ltot[x] > 0)
        //                            Qtys = new DevComponents.AdvTree.Cell(ltot[x].ToString());
        //                        else
        //                            Qtys = new DevComponents.AdvTree.Cell("");

        //                        tots[x] += ltot[x];
        //                        stot[x] += ltot[x];
        //                        SubN.Cells.Add(Qtys);
        //                    }

        //                    LocN.Nodes.Add(SubN);
        //                }
        //                for (int y = 1; y < 14; y++)
        //                {
        //                    if (stot[y] > 0)
        //                        Qtys = new DevComponents.AdvTree.Cell(stot[y].ToString());
        //                    else
        //                        Qtys = new DevComponents.AdvTree.Cell("");

        //                    LocN.Cells.Add(Qtys);
        //                }

        //                Owner.Nodes.Add(LocN);
        //            }

        //            for (int y = 1; y < 14; y++)
        //            {
        //                if (tots[y] > 0)
        //                    Qtys = new DevComponents.AdvTree.Cell(tots[y].ToString());
        //                else
        //                    Qtys = new DevComponents.AdvTree.Cell("");

        //                Owner.Cells.Add(Qtys);
        //            }

        //            at_StoredFuel.Nodes.Add(Owner);
        //        }

        //        foreach (var ch in PlugInData.CML)
        //        {

        //            Owner = new DevComponents.AdvTree.Node(PlugInData.CorpIDToName[ch.Key]);
        //            Locns.Clear();
        //            fuelI.Clear();

        //            foreach (var itm in ch.Value)
        //            {
        //                // Is the item itelf a Fuel Item or is it a container
        //                itmID = itm.Key;
        //                APIM = (APIModule)itm.Value;

        //                if (APIM.Items.Count > 0)
        //                {
        //                    // Container -- Need a CAN Parser, would appear under locations
        //                    // Will show if inside of a can or a ship's hold
        //                    foreach (var l in APIM.Items)
        //                    {
        //                        if ((l.Value.SubItems != null) && (l.Value.SubItems.Count > 0))
        //                        {
        //                            // Container Inside of another Container (umm... CHA, Corp Hanger, Can inside of Ship, Can in Hanger, etc.)
        //                        }
        //                        else
        //                        {
        //                            // Items inside of container only                                   
        //                            itmT = IsItemTowerFuelType(l.Value.typeID);
        //                            if (itmT > 0)
        //                            {
        //                                loc = APIM.locName;
        //                                if (Locns.ContainsKey(loc))
        //                                {
        //                                    LocN = Locns[loc];
        //                                }
        //                                else
        //                                {
        //                                    LocN = new DevComponents.AdvTree.Node(loc);
        //                                    Locns.Add(loc, LocN);
        //                                }

        //                                if (fuelI.ContainsKey(loc) && fuelI[loc].ContainsKey(APIM.modID))
        //                                {
        //                                    tots = fuelI[loc][APIM.modID];
        //                                }
        //                                else if (fuelI.ContainsKey(loc) && !fuelI[loc].ContainsKey(APIM.modID))
        //                                {
        //                                    tots = new long[14];
        //                                    fuelI[loc].Add(APIM.modID, tots);
        //                                }
        //                                else
        //                                {
        //                                    tots = new long[14];
        //                                    fuelI.Add(loc, new SortedList<long, int[]>());
        //                                    fuelI[loc].Add(APIM.modID, tots);
        //                                }

        //                                tots[itmT] += l.Value.qty;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    // Item
        //                    itmT = IsItemTowerFuelType(APIM.typeID);
        //                    if (itmT > 0)
        //                    {
        //                        loc = APIM.locName;
        //                        if (Locns.ContainsKey(loc))
        //                        {
        //                            LocN = Locns[loc];
        //                        }
        //                        else
        //                        {
        //                            LocN = new DevComponents.AdvTree.Node(loc);
        //                            Locns.Add(loc, LocN);
        //                        }

        //                        if (fuelI.ContainsKey(loc) && fuelI[loc].ContainsKey(APIM.modID))
        //                        {
        //                            tots = fuelI[loc][APIM.modID];
        //                        }
        //                        else if (fuelI.ContainsKey(loc) && !fuelI[loc].ContainsKey(APIM.modID))
        //                        {
        //                            tots = new long[14];
        //                            fuelI[loc].Add(APIM.modID, tots);
        //                        }
        //                        else
        //                        {
        //                            tots = new long[14];
        //                            fuelI.Add(loc, new SortedList<long, int[]>());
        //                            fuelI[loc].Add(APIM.modID, tots);
        //                        }

        //                        tots[itmT] += APIM.Qty;
        //                    }
        //                }
        //            }

        //            tots = new long[14];
        //            foreach (var v in fuelI)
        //            {
        //                LocN = Locns[v.Key];
        //                stot = new long[14];
        //                foreach (var f in v.Value)
        //                {
        //                    ltot = f.Value;
        //                    SubN = new DevComponents.AdvTree.Node(PlugInData.ItemIDToName[f.Key]);

        //                    for (int x = 1; x < 14; x++)
        //                    {
        //                        if (ltot[x] > 0)
        //                            Qtys = new DevComponents.AdvTree.Cell(ltot[x].ToString());
        //                        else
        //                            Qtys = new DevComponents.AdvTree.Cell("");

        //                        tots[x] += ltot[x];
        //                        stot[x] += ltot[x];
        //                        SubN.Cells.Add(Qtys);
        //                    }

        //                    LocN.Nodes.Add(SubN);
        //                }
        //                for (int y = 1; y < 14; y++)
        //                {
        //                    if (stot[y] > 0)
        //                        Qtys = new DevComponents.AdvTree.Cell(stot[y].ToString());
        //                    else
        //                        Qtys = new DevComponents.AdvTree.Cell("");

        //                    LocN.Cells.Add(Qtys);
        //                }

        //                Owner.Nodes.Add(LocN);
        //            }

        //            for (int y = 1; y < 14; y++)
        //            {
        //                if (tots[y] > 0)
        //                    Qtys = new DevComponents.AdvTree.Cell(tots[y].ToString());
        //                else
        //                    Qtys = new DevComponents.AdvTree.Cell("");

        //                Owner.Cells.Add(Qtys);
        //            }

        //            at_StoredFuel.Nodes.Add(Owner);
        //        }
        //    }
        //}


        #endregion

        
        #region IHUB Handlers

        public void PopulateIHUBDisplay()
        {
            string owner;
            int rInd;
            SortedList<string, SystemIHub> ownHubs;

            dgv_iHubs.Rows.Clear();
            owner = cb_iHubOwner.SelectedItem.ToString();

            if (!PlugInData.iHubs.ContainsKey(owner))
                return;

            ownHubs = PlugInData.iHubs[owner];

            foreach (var v in ownHubs)
            {
                rInd = dgv_iHubs.Rows.Add();
                dgv_iHubs.Rows[rInd].Cells[0].Value = v.Key.ToString();

                foreach (InfrastructureUG IUG in v.Value.OreUG.Values)
                {
                    if (IUG.cell > 0)
                    {
                        dgv_iHubs.Rows[rInd].Cells[IUG.cell].Value = IUG.colNm;
                        if (IUG.online)
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.LimeGreen;
                        else
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.Salmon;
                    }
                }
                foreach (InfrastructureUG IUG in v.Value.SrvUG.Values)
                {
                    if (IUG.cell > 0)
                    {
                        dgv_iHubs.Rows[rInd].Cells[IUG.cell].Value = IUG.colNm;
                        if (IUG.online)
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.LimeGreen;
                        else
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.Salmon;
                    }
                }
                foreach (InfrastructureUG IUG in v.Value.EntUG.Values)
                {
                    if (IUG.cell > 0)
                    {
                        dgv_iHubs.Rows[rInd].Cells[IUG.cell].Value = IUG.colNm;
                        if (IUG.online)
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.LimeGreen;
                        else
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.Salmon;
                    }
                }
                foreach (InfrastructureUG IUG in v.Value.PrtUG.Values)
                {
                    if (IUG.cell > 0)
                    {
                        dgv_iHubs.Rows[rInd].Cells[IUG.cell].Value = IUG.colNm;
                        if (IUG.online)
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.LimeGreen;
                        else
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.Salmon;
                    }
                }
                foreach (InfrastructureUG IUG in v.Value.FlxUG.Values)
                {
                    if (IUG.cell > 0)
                    {
                        dgv_iHubs.Rows[rInd].Cells[IUG.cell].Value = IUG.colNm;
                        if (IUG.online)
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.LimeGreen;
                        else
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.Salmon;
                    }
                }
                foreach (InfrastructureUG IUG in v.Value.StratUG.Values)
                {
                    if (IUG.cell > 0)
                    {
                        dgv_iHubs.Rows[rInd].Cells[IUG.cell].Value = IUG.colNm;
                        if (IUG.online)
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.LimeGreen;
                        else
                            dgv_iHubs.Rows[rInd].Cells[IUG.cell].Style.BackColor = Color.Salmon;
                    }
                }
            }
        }

        public void PopulateIHUBOwners()
        {
            cb_iHubOwner.Items.Clear();
            cb_iHubOwner.Items.AddRange(PlugInData.iHubs.Keys.ToArray());
            if (cb_iHubOwner.Items.Count > 0)
                cb_iHubOwner.SelectedIndex = 0;
        }

        private void cb_iHubOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateIHUBDisplay();
        }

        private void cms_IHubs_Opening(object sender, CancelEventArgs e)
        {
            if (dgv_iHubs.SelectedCells[0].ColumnIndex == 0)
            {
                cms_IHubs.Items[0].Enabled = false;
                cms_IHubs.Items[1].Enabled = false;
                cms_IHubs.Items[2].Enabled = false;
                cms_IHubs.Items[3].Enabled = false;
                cms_IHubs.Items[4].Enabled = true;
                cms_IHubs.Items[5].Enabled = true;
                cms_IHubs.Items[6].Enabled = true;
            }
            else
            {
                cms_IHubs.Items[0].Enabled = true;
                cms_IHubs.Items[1].Enabled = true;
                cms_IHubs.Items[2].Enabled = true;
                cms_IHubs.Items[3].Enabled = true;
                cms_IHubs.Items[4].Enabled = false;
                cms_IHubs.Items[5].Enabled = false;
                cms_IHubs.Items[6].Enabled = false;
            }
        }

        private void tmsi_MarkIUGOffline_Click(object sender, EventArgs e)
        {
            // Ore = 1-5
            // Survay = 6-10
            // Entrapment = 11-15
            // Pirate = 16-20
            // Quantumn = 21-25
            // Bridge = 26
            // CynoGen = 27
            // CynoJam = 28
            // SC Construct = 29
            int row, col;
            string SelSystem, SelIUG = "Unknown", colNm = "";
            row = dgv_iHubs.SelectedCells[0].RowIndex;
            col = dgv_iHubs.SelectedCells[0].ColumnIndex;

            SelSystem = dgv_iHubs[0, row].Value.ToString();

            if (!PlugInData.iHubs.ContainsKey(cb_iHubOwner.Text))
                return;
            if (!PlugInData.iHubs[cb_iHubOwner.Text].ContainsKey(SelSystem))
                return;

            if (col < 6)
            {
                SelIUG = "Ore Prospecting Array " + col;
                colNm = "Ore " + col;
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].OreUG.ContainsKey(SelIUG))
                {
                     PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].OreUG[SelIUG].online = false;
                }
            }
            else if (col < 11)
            {
                SelIUG = "Survey Networks " + col;
                colNm = "Surv " + (col - 5);
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].SrvUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].SrvUG[SelIUG].online = false;
                }
            }
            else if (col < 16)
            {
                SelIUG = "Entrapment Array " + col;
                colNm = "Entp " + (col - 10);
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].EntUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].EntUG[SelIUG].online = false;
                }
            }
            else if (col < 21)
            {
                SelIUG = "Pirate Detection Array " + col;
                colNm = "Pirt " + (col - 15);
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].PrtUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].PrtUG[SelIUG].online = false;
                }
            }
            else if (col < 26)
            {
                SelIUG = "Quantum Flux Generator " + col;
                colNm = "Q.Flx " + (col - 20);
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].FlxUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].FlxUG[SelIUG].online = false;
                }
            }
            else if (col == 26)
            {
                SelIUG = "Advanced Logistics Network";
                colNm = "Bridge";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG[SelIUG].online = false;
                }
            }
            else if (col == 27)
            {
                SelIUG = "Cynosural Navigation";
                colNm = "CynoGen";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG[SelIUG].online = false;
                }
            }
            else if (col == 28)
            {
                SelIUG = "Cynosural Suppresion";
                colNm = "Cyno-Jam";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG[SelIUG].online = false;
                }
            }
            else if (col == 29)
            {
                SelIUG = "Supercapital Construction Facilities";
                colNm = "S-Cap";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG[SelIUG].online = false;
                }
            }
            PopulateIHUBDisplay();
            PlugInData.SaveIHubListing();
        }

        private void tmsi_AddIUG_Click(object sender, EventArgs e)
        {
            // Ore = 1-5
            // Survay = 6-10
            // Entrapment = 11-15
            // Pirate = 16-20
            // Quantumn = 21-25
            // Bridge = 26
            // CynoGen = 27
            // CynoJam = 28
            // SC Construct = 29
            int row, col;
            string SelSystem, SelIUG = "Unknown", colNm = "";
            InfrastructureUG IUG;

            row = dgv_iHubs.SelectedCells[0].RowIndex;
            col = dgv_iHubs.SelectedCells[0].ColumnIndex;
            SelSystem = dgv_iHubs[0, row].Value.ToString();

            if (col < 1)    // col 0 == System, we do not want that to do anything here
                return;
            if (!PlugInData.iHubs.ContainsKey(cb_iHubOwner.Text))
                return;
            if (!PlugInData.iHubs[cb_iHubOwner.Text].ContainsKey(SelSystem))
                return;

            if (col < 6)
            {
                SelIUG = "Ore Prospecting Array " + col;
                colNm = "Ore " + col;
                if (!PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].OreUG.ContainsKey(SelIUG))
                {
                    IUG = new InfrastructureUG();
                    IUG.name = SelIUG;
                    IUG.colNm = colNm;
                    IUG.cell = col;
                    IUG.online = true;
                    IUG.ID = -1;
                    IUG.typeID = -1;
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].OreUG.Add(IUG.name, IUG);
                }
            }
            else if (col < 11)
            {
                SelIUG = "Survey Networks " + (col - 5);
                colNm = "Surv " + (col  - 5);
                if (!PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].SrvUG.ContainsKey(SelIUG))
                {
                    IUG = new InfrastructureUG();
                    IUG.name = SelIUG;
                    IUG.colNm = colNm;
                    IUG.cell = col;
                    IUG.online = true;
                    IUG.ID = -1;
                    IUG.typeID = -1;
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].SrvUG.Add(IUG.name, IUG);
                }
            }
            else if (col < 16)
            {
                SelIUG = "Entrapment Array " + (col - 10);
                colNm = "Entp " + (col - 10);
                if (!PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].EntUG.ContainsKey(SelIUG))
                {
                    IUG = new InfrastructureUG();
                    IUG.name = SelIUG;
                    IUG.colNm = colNm;
                    IUG.cell = col;
                    IUG.online = true;
                    IUG.ID = -1;
                    IUG.typeID = -1;
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].EntUG.Add(IUG.name, IUG);
                }
            }
            else if (col < 21)
            {
                SelIUG = "Pirate Detection Array " + (col - 15);
                colNm = "Pirt " + (col - 15);
                if (!PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].PrtUG.ContainsKey(SelIUG))
                {
                    IUG = new InfrastructureUG();
                    IUG.name = SelIUG;
                    IUG.colNm = colNm;
                    IUG.cell = col;
                    IUG.online = true;
                    IUG.ID = -1;
                    IUG.typeID = -1;
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].PrtUG.Add(IUG.name, IUG);
                }
            }
            else if (col < 26)
            {
                SelIUG = "Quantum Flux Generator " + (col - 20);
                colNm = "Q.Flx " + (col - 20);
                if (!PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].FlxUG.ContainsKey(SelIUG))
                {
                    IUG = new InfrastructureUG();
                    IUG.name = SelIUG;
                    IUG.colNm = colNm;
                    IUG.cell = col;
                    IUG.online = true;
                    IUG.ID = -1;
                    IUG.typeID = -1;
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].FlxUG.Add(IUG.name, IUG);
                }
            }
            else if (col == 26)
            {
                SelIUG = "Advanced Logistics Network";
                colNm = "Bridge";
                if (!PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    IUG = new InfrastructureUG();
                    IUG.name = SelIUG;
                    IUG.colNm = colNm;
                    IUG.cell = col;
                    IUG.online = true;
                    IUG.ID = -1;
                    IUG.typeID = -1;
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.Add(IUG.name, IUG);
                }
            }
            else if (col == 27)
            {
                SelIUG = "Cynosural Navigation";
                colNm = "CynoGen";
                if (!PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    IUG = new InfrastructureUG();
                    IUG.name = SelIUG;
                    IUG.colNm = colNm;
                    IUG.cell = col;
                    IUG.online = true;
                    IUG.ID = -1;
                    IUG.typeID = -1;
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.Add(IUG.name, IUG);
                }
            }
            else if (col == 28)
            {
                SelIUG = "Cynosural Suppresion";
                colNm = "Cyno-Jam";
                if (!PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    IUG = new InfrastructureUG();
                    IUG.name = SelIUG;
                    IUG.colNm = colNm;
                    IUG.cell = col;
                    IUG.online = true;
                    IUG.ID = -1;
                    IUG.typeID = -1;
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.Add(IUG.name, IUG);
                }
            }
            else if (col == 29)
            {
                SelIUG = "Supercapital Construction Facilities";
                colNm = "S-Cap";
                if (!PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    IUG = new InfrastructureUG();
                    IUG.name = SelIUG;
                    IUG.colNm = colNm;
                    IUG.cell = col;
                    IUG.online = true;
                    IUG.ID = -1;
                    IUG.typeID = -1;
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.Add(IUG.name, IUG);
                }
            }
            PopulateIHUBDisplay();
            PlugInData.SaveIHubListing();
        }

        private void tmsi_RemoveIUG_Click(object sender, EventArgs e)
        {
            // Ore = 1-5
            // Survay = 6-10
            // Entrapment = 11-15
            // Pirate = 16-20
            // Quantumn = 21-25
            // Bridge = 26
            // CynoGen = 27
            // CynoJam = 28
            // SC Construct = 29
            int row, col;
            string SelSystem, SelIUG = "Unknown";

            row = dgv_iHubs.SelectedCells[0].RowIndex;
            col = dgv_iHubs.SelectedCells[0].ColumnIndex;
            SelSystem = dgv_iHubs[0, row].Value.ToString();

            if (!PlugInData.iHubs.ContainsKey(cb_iHubOwner.Text))
                return;
            if (!PlugInData.iHubs[cb_iHubOwner.Text].ContainsKey(SelSystem))
                return;

            if (col < 6)
            {
                SelIUG = "Ore Prospecting Array " + col;
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].OreUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].OreUG.Remove(SelIUG);
                }
            }
            else if (col < 11)
            {
                SelIUG = "Survey Networks " + col;
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].SrvUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].SrvUG.Remove(SelIUG);
                }
            }
            else if (col < 16)
            {
                SelIUG = "Entrapment Array " + col;
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].EntUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].EntUG.Remove(SelIUG);
                }
            }
            else if (col < 21)
            {
                SelIUG = "Pirate Detection Array " + col;
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].PrtUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].PrtUG.Remove(SelIUG);
                }
            }
            else if (col < 26)
            {
                SelIUG = "Quantum Flux Generator " + col;
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].FlxUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].FlxUG.Remove(SelIUG);
                }
            }
            else if (col == 26)
            {
                SelIUG = "Advanced Logistics Network";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.Remove(SelIUG);
                }
            }
            else if (col == 27)
            {
                SelIUG = "Cynosural Navigation";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.Remove(SelIUG);
                }
            }
            else if (col == 28)
            {
                SelIUG = "Cynosural Suppresion";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.Remove(SelIUG);
                }
            }
            else if (col == 29)
            {
                SelIUG = "Supercapital Construction Facilities";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.Remove(SelIUG);
                }
            }
            PopulateIHUBDisplay();
            PlugInData.SaveIHubListing();
        }

        private void tmsi_MarkHUBOffline_Click(object sender, EventArgs e)
        {

        }

        private void tsmi_MarkHUBOnline_Click(object sender, EventArgs e)
        {

        }

        private void tsmi_MarkIUGOnline_Click(object sender, EventArgs e)
        {
            // Ore = 1-5
            // Survay = 6-10
            // Entrapment = 11-15
            // Pirate = 16-20
            // Quantumn = 21-25
            // Bridge = 26
            // CynoGen = 27
            // CynoJam = 28
            // SC Construct = 29
            int row, col;
            string SelSystem, SelIUG = "Unknown", colNm = "";
            row = dgv_iHubs.SelectedCells[0].RowIndex;
            col = dgv_iHubs.SelectedCells[0].ColumnIndex;

            SelSystem = dgv_iHubs[0, row].Value.ToString();

            if (!PlugInData.iHubs.ContainsKey(cb_iHubOwner.Text))
                return;
            if (!PlugInData.iHubs[cb_iHubOwner.Text].ContainsKey(SelSystem))
                return;

            if (col < 6)
            {
                SelIUG = "Ore Prospecting Array " + col;
                colNm = "Ore " + col;
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].OreUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].OreUG[SelIUG].online = true;
                }
            }
            else if (col < 11)
            {
                SelIUG = "Survey Networks " + col;
                colNm = "Surv " + (col - 5);
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].SrvUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].SrvUG[SelIUG].online = true;
                }
            }
            else if (col < 16)
            {
                SelIUG = "Entrapment Array " + col;
                colNm = "Entp " + (col - 10);
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].EntUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].EntUG[SelIUG].online = true;
                }
            }
            else if (col < 21)
            {
                SelIUG = "Pirate Detection Array " + col;
                colNm = "Pirt " + (col - 15);
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].PrtUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].PrtUG[SelIUG].online = true;
                }
            }
            else if (col < 26)
            {
                SelIUG = "Quantum Flux Generator " + col;
                colNm = "Q.Flx " + (col - 20);
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].FlxUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].FlxUG[SelIUG].online = true;
                }
            }
            else if (col == 26)
            {
                SelIUG = "Advanced Logistics Network";
                colNm = "Bridge";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG[SelIUG].online = true;
                }
            }
            else if (col == 27)
            {
                SelIUG = "Cynosural Navigation";
                colNm = "CynoGen";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG[SelIUG].online = true;
                }
            }
            else if (col == 28)
            {
                SelIUG = "Cynosural Suppresion";
                colNm = "Cyno-Jam";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG[SelIUG].online = true;
                }
            }
            else if (col == 29)
            {
                SelIUG = "Supercapital Construction Facilities";
                colNm = "S-Cap";
                if (PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG.ContainsKey(SelIUG))
                {
                    PlugInData.iHubs[cb_iHubOwner.Text][SelSystem].StratUG[SelIUG].online = true;
                }
            }
            PopulateIHUBDisplay();
            PlugInData.SaveIHubListing();
        }

        private void tmsi_RemoveHUB_Click(object sender, EventArgs e)
        {

        }

        private void b_AddIHUB_Click(object sender, EventArgs e)
        {

        }
        
        #endregion


        #region IGB Security Configuration

        private void clb_POSSecList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string opt, mod, itm;
            string[] split;

            if (SecAddLoad)
                return;

            itm = clb_POSSecList.SelectedItem.ToString();

            split = itm.Split(' ');
            opt = split[1];
            mod = split[2].Replace("[", "");
            mod = mod.Replace("]", "");

            foreach (IGBSecurity igb in PlugInData.POSSecList)
            {
                if ((igb.Option.Equals(opt)) && igb.Modifier.Equals(mod))
                {
                    if (e.NewValue == CheckState.Checked)
                        igb.Active = true;
                    else
                        igb.Active = false;

                    break;
                }
            }
            PlugInData.SaveSecurityListing();
        }

        private void clb_IHubSecList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string opt, mod, itm;
            string[] split;

            if (SecAddLoad)
                return;

            itm = clb_IHubSecList.SelectedItem.ToString();

            split = itm.Split(' ');
            opt = split[1];
            mod = split[2].Replace("[", "");
            mod = mod.Replace("]", "");

            foreach (IGBSecurity igb in PlugInData.IHubSecList)
            {
                if ((igb.Option.Equals(opt)) && igb.Modifier.Equals(mod))
                {
                    if (e.NewValue == CheckState.Checked)
                        igb.Active = true;
                    else
                        igb.Active = false;

                    break;
                }
            }
            PlugInData.SaveSecurityListing();
        }

        private void tmsi_RPosSec_Click(object sender, EventArgs e)
        {
            string opt, mod, itm;
            string[] split;

            if (clb_POSSecList.SelectedItem == null)
                return;

            itm = clb_POSSecList.SelectedItem.ToString();

            split = itm.Split(' ');
            opt = split[1];
            mod = itm.Substring(itm.IndexOf("[")+1);
            mod = mod.Replace("]", "");

            foreach (IGBSecurity igb in PlugInData.POSSecList)
            {
                if ((igb.Option.Equals(opt)) && igb.Modifier.Equals(mod))
                {
                    PlugInData.POSSecList.Remove(igb);
                    break;
                }
            }
            SecAddLoad = true;
            PopulateIGBPosSecurityDisplay();
            SecAddLoad = false;
            PlugInData.SaveSecurityListing();
        }

        private void tsmi_RHubSec_Click(object sender, EventArgs e)
        {
            string opt, mod, itm;
            string[] split;

            if (clb_IHubSecList.SelectedItem == null)
                return;

            itm = clb_IHubSecList.SelectedItem.ToString();

            split = itm.Split(' ');
            opt = split[1];
            mod = split[2].Replace("[", "");
            mod = mod.Replace("]", "");

            foreach (IGBSecurity igb in PlugInData.IHubSecList)
            {
                if ((igb.Option.Equals(opt)) && igb.Modifier.Equals(mod))
                {
                    PlugInData.IHubSecList.Remove(igb);
                    break;
                }
            }
            SecAddLoad = true;
            PopulateIGBHubSecurityDisplay();
            SecAddLoad = false;
            PlugInData.SaveSecurityListing();
        }

        private void b_AddPOSSec_Click(object sender, EventArgs e)
        {
            string option, mod;
            bool andOR;

            option = cb_PosSecOption.Text;
            mod = cb_PosSecMod.Text;
            andOR = rb_psAND.Checked;

            PlugInData.POSSecList.Add(new IGBSecurity(option, mod, andOR, true));
            SecAddLoad = true;
            PopulateIGBPosSecurityDisplay();
            SecAddLoad = false;
            PlugInData.SaveSecurityListing();
        }

        private void b_AddIHubSec_Click(object sender, EventArgs e)
        {
            string option, mod;
            bool andOR;

            option = cb_HubSecOption.Text;
            mod = cb_IHubSecMod.Text;
            andOR = rb_ihAND.Checked;

            PlugInData.IHubSecList.Add(new IGBSecurity(option, mod, andOR, true));
            SecAddLoad = true;
            PopulateIGBHubSecurityDisplay();
            SecAddLoad = false;
            PlugInData.SaveSecurityListing();
        }

        private void cb_PosSecOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_PosSecOption.Text.Equals("Player"))
            {
                cb_PosSecMod.Items.Clear();
                foreach (Player p in PlugInData.SecUsers.Values)
                    cb_PosSecMod.Items.Add(p.Name + " [ " + p.Ally + " ]");
                cb_PosSecMod.Show();
                lb_psMT.Show();
            }
            else if (cb_PosSecOption.Text.Equals("Role"))
            {
                cb_PosSecMod.Items.Clear();
                cb_PosSecMod.Items.Add("Director");
                cb_PosSecMod.Items.Add("Factory Manager");
                cb_PosSecMod.Items.Add("Station Manager");
                cb_PosSecMod.Items.Add("Equipment Config");
                cb_PosSecMod.Items.Add("Starbase Config");
                cb_PosSecMod.Show();
                lb_psMT.Show();
            }
            else if (cb_PosSecOption.Text.Equals("Anyone"))
            {
                cb_PosSecMod.Items.Clear();
                cb_PosSecMod.Hide();
                lb_psMT.Hide();
            }
            else if (cb_PosSecOption.Text.Equals("Alliance"))
            {
                cb_PosSecMod.Items.Clear();
                foreach (Alliance_Data ad in PlugInData.AL.alliances.Values)
                    if (!cb_PosSecMod.Items.Contains(ad.name))
                        cb_PosSecMod.Items.Add(ad.name);
                cb_PosSecMod.Show();
                lb_psMT.Show();
            }
            else if (cb_PosSecOption.Text.Equals("Corporation"))
            {
                cb_PosSecMod.Items.Clear();
                foreach (Player p in PlugInData.SecUsers.Values)
                {
                    if (!cb_PosSecMod.Items.Contains(p.Corp))
                        cb_PosSecMod.Items.Add(p.Corp);
                }
                cb_PosSecMod.Show();
                lb_psMT.Show();
            }
        }

        private void cb_HubSecOption_SelectedIndexChanged(object sender, EventArgs e)
        {         
            if (cb_HubSecOption.Text.Equals("Player"))
            {
                cb_IHubSecMod.Items.Clear();
                foreach (Player p in PlugInData.SecUsers.Values)
                    cb_IHubSecMod.Items.Add(p.Name + " [ " + p.Ally + " ]");
                cb_IHubSecMod.Show();
                lb_psMT.Show();
            }
            else if (cb_HubSecOption.Text.Equals("Role"))
            {
                cb_IHubSecMod.Items.Clear();
                cb_IHubSecMod.Items.Add("Director");
                cb_IHubSecMod.Items.Add("Factory Manager");
                cb_IHubSecMod.Items.Add("Station Manager");
                cb_IHubSecMod.Items.Add("Equipment Config");
                cb_IHubSecMod.Items.Add("Starbase Config");
                cb_IHubSecMod.Show();
                lb_psMT.Show();
            }
            else if (cb_HubSecOption.Text.Equals("Anyone"))
            {
                cb_IHubSecMod.Items.Clear();
                cb_IHubSecMod.Hide();
                lb_psMT.Hide();
            }
            else if (cb_HubSecOption.Text.Equals("Alliance"))
            {
                cb_IHubSecMod.Items.Clear();
                foreach (Alliance_Data ad in PlugInData.AL.alliances.Values)
                    if (!cb_IHubSecMod.Items.Contains(ad.name))
                        cb_IHubSecMod.Items.Add(ad.name);
                cb_IHubSecMod.Show();
                lb_psMT.Show();
            }
            else if (cb_HubSecOption.Text.Equals("Corporation"))
            {
                cb_IHubSecMod.Items.Clear();
                foreach (Player p in PlugInData.SecUsers.Values)
                {
                    if (!cb_IHubSecMod.Items.Contains(p.Corp))
                        cb_IHubSecMod.Items.Add(p.Corp);
                }
                cb_IHubSecMod.Show();
                lb_psMT.Show();
            }
        }

        private void PopulateIGBSecurityDisplay()
        {
            SecAddLoad = true;
            PopulateIGBPosSecurityDisplay();
            PopulateIGBHubSecurityDisplay();
            SecAddLoad = false;
        }

        private void PopulateIGBPosSecurityDisplay()
        {
            string secItem, andOR;

            clb_POSSecList.Items.Clear();

            foreach (IGBSecurity igb in PlugInData.POSSecList)
            {
                if (igb.AND)
                    andOR = "{AND} ";
                else
                    andOR = "{OR} ";

                secItem = andOR + igb.Option + " [" + igb.Modifier + "]";

                clb_POSSecList.Items.Add(secItem, igb.Active);
            }
        }

        private void PopulateIGBHubSecurityDisplay()
        {
            string secItem, andOR;

            clb_IHubSecList.Items.Clear();

            foreach (IGBSecurity igb in PlugInData.IHubSecList)
            {
                if (igb.AND)
                    andOR = "{AND} ";
                else
                    andOR = "{OR} ";

                secItem = andOR + igb.Option + " [" + igb.Modifier + "]";

                clb_IHubSecList.Items.Add(secItem, igb.Active);
            }
        }

        #endregion


        #region Fuel Price Handler

        private void UpdateItemPrice(string id, double arg)
        {
            EveHQ.Core.frmModifyPrice newPrice = new EveHQ.Core.frmModifyPrice(id, arg);
            newPrice.ShowDialog();

            CalculateAndDisplayDesignFuelData();
            PopulateTowerFillDG();
            UpdateSelectedTowerList();
        }

        private void pb_mEU_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                UpdateItemPrice(PlugInData.BFStats.EnrUran.itemID, 0);
        }

        private void pb_mOxy_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                UpdateItemPrice(PlugInData.BFStats.Oxygen.itemID, 0);
        }

        private void pb_mMP_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                UpdateItemPrice(PlugInData.BFStats.MechPart.itemID, 0);
        }

        private void pb_mCoolant_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                UpdateItemPrice(PlugInData.BFStats.Coolant.itemID, 0);
        }

        private void pb_MRobotics_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                UpdateItemPrice(PlugInData.BFStats.Robotics.itemID, 0);
        }

        private void pb_dIso_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (l_IsotopeType.Text.Equals("O2"))
                    UpdateItemPrice(PlugInData.BFStats.N2Iso.itemID, 0);
                else if (l_IsotopeType.Text.Equals("H2"))
                    UpdateItemPrice(PlugInData.BFStats.H2Iso.itemID, 0);
                else if (l_IsotopeType.Text.Equals("N2"))
                    UpdateItemPrice(PlugInData.BFStats.N2Iso.itemID, 0);
                else if (l_IsotopeType.Text.Equals("He"))
                    UpdateItemPrice(PlugInData.BFStats.HeIso.itemID, 0);
            }
        }

        private void pb_MIso_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (l_M_IsoType.Text.Equals("O2"))
                    UpdateItemPrice(PlugInData.BFStats.N2Iso.itemID, 0);
                else if (l_M_IsoType.Text.Equals("H2"))
                    UpdateItemPrice(PlugInData.BFStats.H2Iso.itemID, 0);
                else if (l_M_IsoType.Text.Equals("N2"))
                    UpdateItemPrice(PlugInData.BFStats.N2Iso.itemID, 0);
                else if (l_M_IsoType.Text.Equals("He"))
                    UpdateItemPrice(PlugInData.BFStats.HeIso.itemID, 0);
            }
        }

        private void pb_mHW_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                UpdateItemPrice(PlugInData.BFStats.HvyWater.itemID, 0);
        }

        private void pb_mLO_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                UpdateItemPrice(PlugInData.BFStats.LiqOzone.itemID, 0);
        }

        private void pb_mCharter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                UpdateItemPrice(PlugInData.BFStats.Charters.itemID, 0);
        }

        private void pb_mStront_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                UpdateItemPrice(PlugInData.BFStats.Strontium.itemID, 0);
        }

        private void dg_MonitoredTowers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string name, cltxt;
            DataGridView dgv;

            if (e.Button == MouseButtons.Right)
            {
                dgv = (DataGridView)sender;
                name = dgv.Columns[dgv.CurrentCell.ColumnIndex].HeaderText;

                if (name.Equals("Enr Uranium"))
                    UpdateItemPrice(PlugInData.BFStats.EnrUran.itemID, 0);
                else if (name.Equals("Oxygen"))
                    UpdateItemPrice(PlugInData.BFStats.Oxygen.itemID, 0);
                else if (name.Equals("Mech Parts"))
                    UpdateItemPrice(PlugInData.BFStats.MechPart.itemID, 0);
                else if (name.Equals("Coolant"))
                    UpdateItemPrice(PlugInData.BFStats.Coolant.itemID, 0);
                else if (name.Equals("Robotics"))
                    UpdateItemPrice(PlugInData.BFStats.Robotics.itemID, 0);
                else if (name.Equals("Isotopes"))
                {
                    cltxt = dgv.CurrentCell.Value.ToString();

                    if (cltxt.Contains("O2"))
                        UpdateItemPrice(PlugInData.BFStats.O2Iso.itemID, 0);
                    else if (cltxt.Contains("N2"))
                        UpdateItemPrice(PlugInData.BFStats.N2Iso.itemID, 0);
                    else if (cltxt.Contains("H2"))
                        UpdateItemPrice(PlugInData.BFStats.H2Iso.itemID, 0);
                    else if (cltxt.Contains("He"))
                        UpdateItemPrice(PlugInData.BFStats.HeIso.itemID, 0);
                }
                else if (name.Equals("Hvy Water"))
                    UpdateItemPrice(PlugInData.BFStats.HvyWater.itemID, 0);
                else if (name.Equals("Liq Ozone"))
                    UpdateItemPrice(PlugInData.BFStats.LiqOzone.itemID, 0);
                else if (name.Equals("Charters"))
                    UpdateItemPrice(PlugInData.BFStats.Charters.itemID, 0);
                else if (name.Equals("Strontium"))
                    UpdateItemPrice(PlugInData.BFStats.Strontium.itemID, 0);
            }
        }

        private void dg_TowerFuelList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string name, cltxt;
            DataGridView dgv;

            if (e.Button == MouseButtons.Right)
            {
                dgv = (DataGridView)sender;
                name = dgv.Columns[dgv.CurrentCell.ColumnIndex].HeaderText;

                if (name.Equals("Enr. Ur."))
                    UpdateItemPrice(PlugInData.BFStats.EnrUran.itemID, 0);
                else if (name.Equals("Oxygen"))
                    UpdateItemPrice(PlugInData.BFStats.Oxygen.itemID, 0);
                else if (name.Equals("Mech Pts"))
                    UpdateItemPrice(PlugInData.BFStats.MechPart.itemID, 0);
                else if (name.Equals("Coolant"))
                    UpdateItemPrice(PlugInData.BFStats.Coolant.itemID, 0);
                else if (name.Equals("Robotics"))
                    UpdateItemPrice(PlugInData.BFStats.Robotics.itemID, 0);
                else if (name.Equals("Isotopes"))
                {
                    cltxt = dgv.CurrentCell.Value.ToString();

                    if (cltxt.Contains("O2"))
                        UpdateItemPrice(PlugInData.BFStats.O2Iso.itemID, 0);
                    else if (cltxt.Contains("N2"))
                        UpdateItemPrice(PlugInData.BFStats.N2Iso.itemID, 0);
                    else if (cltxt.Contains("H2"))
                        UpdateItemPrice(PlugInData.BFStats.H2Iso.itemID, 0);
                    else if (cltxt.Contains("He"))
                        UpdateItemPrice(PlugInData.BFStats.HeIso.itemID, 0);
                }
                else if (name.Equals("Hvy. Water"))
                    UpdateItemPrice(PlugInData.BFStats.HvyWater.itemID, 0);
                else if (name.Equals("Liq Ozone"))
                    UpdateItemPrice(PlugInData.BFStats.LiqOzone.itemID, 0);
                else if (name.Equals("Charters"))
                    UpdateItemPrice(PlugInData.BFStats.Charters.itemID, 0);
                else if (name.Equals("Strontium"))
                    UpdateItemPrice(PlugInData.BFStats.Strontium.itemID, 0);
            }
        }

        private void dg_SelectedFuel_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string name;
            DataGridView dgv;

            if (e.Button == MouseButtons.Right)
            {
                dgv = (DataGridView)sender;
                name = dgv[0, dgv.CurrentCell.RowIndex].Value.ToString();

                if (name.Equals("Enriched Uranium"))
                    UpdateItemPrice(PlugInData.BFStats.EnrUran.itemID, 0);
                else if (name.Equals("Oxygen"))
                    UpdateItemPrice(PlugInData.BFStats.Oxygen.itemID, 0);
                else if (name.Equals("Mechanical Parts"))
                    UpdateItemPrice(PlugInData.BFStats.MechPart.itemID, 0);
                else if (name.Equals("Coolant"))
                    UpdateItemPrice(PlugInData.BFStats.Coolant.itemID, 0);
                else if (name.Equals("Robotics"))
                    UpdateItemPrice(PlugInData.BFStats.Robotics.itemID, 0);
                else if (name.Contains("Oxygen Isotope"))
                    UpdateItemPrice(PlugInData.BFStats.O2Iso.itemID, 0);
                else if (name.Contains("Nitrogen Isotope"))
                    UpdateItemPrice(PlugInData.BFStats.N2Iso.itemID, 0);
                else if (name.Contains("Hydrogen Isotope"))
                    UpdateItemPrice(PlugInData.BFStats.H2Iso.itemID, 0);
                else if (name.Contains("Helium Isotope"))
                    UpdateItemPrice(PlugInData.BFStats.HeIso.itemID, 0);
                else if (name.Equals("Heavy Water"))
                    UpdateItemPrice(PlugInData.BFStats.HvyWater.itemID, 0);
                else if (name.Equals("Liquid Ozone"))
                    UpdateItemPrice(PlugInData.BFStats.LiqOzone.itemID, 0);
                else if (name.Equals("Charters"))
                    UpdateItemPrice(PlugInData.BFStats.Charters.itemID, 0);
                else if (name.Equals("Strontium Clathrates"))
                    UpdateItemPrice(PlugInData.BFStats.Strontium.itemID, 0);
            }
        }

        private void dg_TotalFuel_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string name;
            DataGridView dgv;

            if (e.Button == MouseButtons.Right)
            {
                dgv = (DataGridView)sender;
                name = dgv[0, dgv.CurrentCell.RowIndex].Value.ToString();

                if (name.Equals("Enriched Uranium"))
                    UpdateItemPrice(PlugInData.BFStats.EnrUran.itemID, 0);
                else if (name.Equals("Oxygen"))
                    UpdateItemPrice(PlugInData.BFStats.Oxygen.itemID, 0);
                else if (name.Equals("Mechanical Parts"))
                    UpdateItemPrice(PlugInData.BFStats.MechPart.itemID, 0);
                else if (name.Equals("Coolant"))
                    UpdateItemPrice(PlugInData.BFStats.Coolant.itemID, 0);
                else if (name.Equals("Robotics"))
                    UpdateItemPrice(PlugInData.BFStats.Robotics.itemID, 0);
                else if (name.Contains("Oxygen Isotope"))
                    UpdateItemPrice(PlugInData.BFStats.O2Iso.itemID, 0);
                else if (name.Contains("Nitrogen Isotope"))
                    UpdateItemPrice(PlugInData.BFStats.N2Iso.itemID, 0);
                else if (name.Contains("Hydrogen Isotope"))
                    UpdateItemPrice(PlugInData.BFStats.H2Iso.itemID, 0);
                else if (name.Contains("Helium Isotope"))
                    UpdateItemPrice(PlugInData.BFStats.HeIso.itemID, 0);
                else if (name.Equals("Heavy Water"))
                    UpdateItemPrice(PlugInData.BFStats.HvyWater.itemID, 0);
                else if (name.Equals("Liquid Ozone"))
                    UpdateItemPrice(PlugInData.BFStats.LiqOzone.itemID, 0);
                else if (name.Equals("Charters"))
                    UpdateItemPrice(PlugInData.BFStats.Charters.itemID, 0);
                else if (name.Equals("Strontium Clathrates"))
                    UpdateItemPrice(PlugInData.BFStats.Strontium.itemID, 0);
            }
        }




        #endregion


        #region Tower Fuel Update - Seperate Window

        private void tsmi_UpdateTowerFuel_Click(object sender, EventArgs e)
        {
            POS pl;
            UpdateTowerFuel UTF;

            if (PlugInData.PDL.Designs.ContainsKey(selName))
            {
                pl = PlugInData.PDL.Designs[selName];

                UTF = new UpdateTowerFuel(pl);
                DialogResult DR = UTF.ShowDialog();

                if (DR != DialogResult.Cancel)
                {
                    PlugInData.PDL.CalculatePOSFuelRunTimes(PlugInData.API_D, PlugInData.Config.data.FuelCosts);
                    PlugInData.PDL.SaveDesignListing();
                    SetChangedStatus(false, "");
                    BuildPOSListForMonitoring();
                    PopulateMonitoredPoSDisplay();
                    UpdateTowerMonitorDisplay();
                }
            }

        }

        #endregion

    }

}
