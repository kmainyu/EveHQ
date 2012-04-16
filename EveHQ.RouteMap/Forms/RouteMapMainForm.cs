// ========================================================================
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
using ZedGraph;

namespace EveHQ.RouteMap
{
    public partial class RouteMapMainForm : DevComponents.DotNetBar.Office2007Form
    {
        public static int numBusy;
        public bool MapDataChanged = false;
        public static double ShipJumpRange;
        public static int ShipFuelPerLY;
        public string RouteStart, RouteEnd;
        public ArrayList AvoidList;
        public ArrayList WaypointList;
        public ArrayList CynoRouteRslt;
        public List<Vertex> CurrentRoute;
        public List<Vertex> CurrentGateRoute;
        public static ManualResetEvent doneEvent;
        EveHQ.Core.Pilot EvePilot = null;
        Ship JumpShip = null;
        IntPtr handle;
        public Route NewRoute;
        private bool AvWpChanged = false;
        private bool Reflecting = false;
        public int ActiveRoute = 0;
        public int SelCelID = 0;
        public double CurShipJumpRange = 0;
        private SolarSystem smSelSys, SDsSys, anSysSel;
        private SystemCelestials SDsCel = null;
        private SortedList<string, AgentMishTypePerc> MishTypes;
        private const float AU = 149597870691; // In meters
        private DataView SortDV;
        private static SortedList<int, int> SCols = null;
        JumpBridge selBridge = null;
        private bool Loading = false;
        private bool ChkUnchk = false;
        private bool MapToggle = false;
        private bool SDSysChange = false;
        public string ActiveSearch = "";
        private ComboBox combo;
        private int SelActID = 0;

        private static readonly string[] gas = new[] { "Atmospheric Gases", "Evaporite Deposits", "Hydrocarbons", "Silicates" };
        private static readonly string[] r08 = new[] { "Cobalt", "Scandium", "Titanium", "Tungsten" };
        private static readonly string[] r16 = new[] { "Cadmium", "Vanadium", "Chromium", "Platinum" };
        private static readonly string[] r32 = new[] { "Caesium", "Technetium", "Hafnium", "Mercury" };
        private static readonly string[] r64 = new[] { "Promethium", "Dysprosium", "Neodymium", "Thulium" };

        #region Main Form Start and Misc

        public RouteMapMainForm()
        {
            int dim;
            bool st;

            DateTime startT, endT;
            TimeSpan runT;
            startT = DateTime.Now;

            InitializeComponent();
            handle = this.Handle;
            SDsCel = new SystemCelestials();

            SetupPilotAndShipListings();
            PopulateJBView(null);
            PopMishTypes();

            //tlp_ActivityMonitorSelects.
            st = (bool)PlugInData.Config.CFlags["Right Panel Expanded"];
            dim = (int)PlugInData.Config.Sizes["Right Panel Size"];
            gp_RightPanel.Width = dim;
            ep_RightPanel.Expanded = st;

            st = (bool)PlugInData.Config.CFlags["Bottom Panel Expanded"];
            dim = (int)PlugInData.Config.Sizes["Bottom Panel Size"];
            ep_SearchResult.Height = dim;
            esp_SearchResult.Expanded = st;

            cb_SystemSelect.Items.AddRange(PlugInData.SystemList.ToArray());
            cb_FromSystem.Items.AddRange(PlugInData.SOVSystemList.ToArray());
            cb_ToSystem.Items.AddRange(PlugInData.SOVSystemList.ToArray());
            cb_CynoSystemName.Items.AddRange(PlugInData.SOVSystemList.ToArray());
            cb_SDSystemName.Items.AddRange(PlugInData.SystemList.ToArray());
            cb_ConfigRegionSelect.Items.AddRange(PlugInData.RegionList.ToArray());

            cb_Alliance.Items.Add("None");
            cb_JBAlliance.Items.AddRange(PlugInData.AllianceList.ToArray());
            cb_Alliance.Items.AddRange(PlugInData.AllianceList.ToArray());

            cb_Corporation.Items.Add("Unknown");
            cb_Corporation.Items.AddRange(PlugInData.Misc.PlayerCorpList.ToArray());

            cb_TowerType.Items.AddRange(PlugInData.Misc.TowerTypes.ToArray());

            cb_MoonGoo1.Items.Add("Nothing");
            cb_MoonGoo2.Items.Add("Nothing");
            cb_MoonGoo3.Items.Add("Nothing");
            cb_MoonGoo4.Items.Add("Nothing");
            cb_MoonGoo5.Items.Add("Nothing");
            cb_MoonGoo6.Items.Add("Nothing");
            cb_MoonGoo7.Items.Add("Nothing");
            cb_MoonGoo8.Items.Add("Nothing");
            cb_MoonGoo1.Items.AddRange(PlugInData.Misc.GooTypes.ToArray());
            cb_MoonGoo2.Items.AddRange(PlugInData.Misc.GooTypes.ToArray());
            cb_MoonGoo3.Items.AddRange(PlugInData.Misc.GooTypes.ToArray());
            cb_MoonGoo4.Items.AddRange(PlugInData.Misc.GooTypes.ToArray());
            cb_MoonGoo5.Items.AddRange(PlugInData.Misc.GooTypes.ToArray());
            cb_MoonGoo6.Items.AddRange(PlugInData.Misc.GooTypes.ToArray());
            cb_MoonGoo7.Items.AddRange(PlugInData.Misc.GooTypes.ToArray());
            cb_MoonGoo8.Items.AddRange(PlugInData.Misc.GooTypes.ToArray());

            cb_MapDetails.SelectedIndex = PlugInData.Config.Settings["Detail Text"];
            //tscb_ZoomPercent.SelectedIndex = 0;
            WaypointList = new ArrayList();
            AvoidList = new ArrayList();
            RouteStart = "";
            RouteEnd = "";

            cbx_WHType.Items.AddRange(PlugInData.Misc.WHTypes.ToArray());

            SetupSystemMoonGooColumns();
            SetupSystemCelestialDGColumns();
            SetupSystemPlanetDGColumns();
            SetupSystemStaticWH();
            SetupPossibleWormholes();
 
            PopulateCynoList();

            if (!PlugInData.Config.MapColors.ContainsKey("Bridge Range Highlight"))
            {
                PlugInData.Config.MapColors.Add("Bridge Range Highlight", Color.Lime);
            }
            if (!PlugInData.Config.MapColors.ContainsKey("Ship Jump Range Highlight"))
            {
                PlugInData.Config.MapColors.Add("Ship Jump Range Highlight", Color.LightBlue);
            }

            PopulateRegionView();

            MainMapView.SetupMapView(PlugInData.SystemList, PlugInData.GalMap, PlugInData.Config, MiniMap, this);
            //    MainMapView.SetMapScale = sp;
            MainMapView.Invalidate();
            MiniMap.InitializeSmallMap(this);
            SetupInitialConfigData();
            cb_SSSearchFor.SelectedIndex = 0;
            tc_MapSelect.SelectedTab = tp_Galaxy;
            //tc_MapSelect.Refresh();

            Assembly asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream("EveHQ.RouteMap.RouteMap_help_info.rtf");
            rtb_HelpInfo.LoadFile(stream, RichTextBoxStreamType.RichText);
            
            SaveConfigToDisk();

            ActMon_1.InitializeMonitor();
            ActMon_2.InitializeMonitor();
            ActMon_3.InitializeMonitor();
            ActMon_4.InitializeMonitor();

            SetMapMode(false);

            endT = DateTime.Now;
            runT = endT.Subtract(startT);
        }

        private void PopMishTypes()
        {
            MishTypes = new SortedList<string, AgentMishTypePerc>();

            MishTypes.Add("Distribution", new AgentMishTypePerc(true, false, false));
            MishTypes.Add("Mining", new AgentMishTypePerc(false, true, false));
            MishTypes.Add("R&D", new AgentMishTypePerc(false, false, false));
            MishTypes.Add("Security", new AgentMishTypePerc(false, false, true));
        }

        private void PopulateRegionView()
        {
            DevComponents.AdvTree.Node Regn, Cnst;

            at_Regions.Nodes.Clear();
            foreach (var r in PlugInData.RCSelList)
            {
                Regn = new DevComponents.AdvTree.Node(r.Key);
                foreach (var c in r.Value.Constellations)
                {
                    Cnst = new DevComponents.AdvTree.Node(c.Key);
                    Cnst.CheckBoxVisible = true;
                    Cnst.Checked = c.Value.Selected;
                    Cnst.Name = r.Value.Name + "|" + c.Value.Name;
                    Regn.Nodes.Add(Cnst);
                }
                Regn.CheckBoxVisible = true;
                Regn.Expanded = false;
                Regn.Checked = r.Value.Selected;
                Regn.Name = r.Value.Name;
                Regn.Nodes.Sort();
                at_Regions.Nodes.Add(Regn);
            }
            at_Regions.Nodes.Sort();
            MainMapView.MapViewSettingChanged();
        }

        private void RouteMapMainForm_Leave(object sender, EventArgs e)
        {
            //CheckForSystemDetailChangesAndSave();
            //if (MapDataChanged)
            //    SaveUpdatedGalaxyMap();
        }

        private void RouteMapMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CheckForSystemDetailChangesAndSave();
            if (MapDataChanged)
                SaveUpdatedGalaxyMap();
        }

        private delegate void IncrementProgressBar();

        private void tc_RouteSeach_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            //tc_RouteSeach.Refresh();
        }

        private void tc_Celestials_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            //tc_Celestials.Refresh();
        }

        #endregion

        #region System_Selection

        public void MapViewShowSystemInformation()
        {
            tc_RouteSeach.SelectedTab = tp_Info;
        }

        public void NewSystemSelected(SolarSystem sSys)
        {
            ArrayList s_jk = new ArrayList();

            tb_SystemSovHolder.Text = "";
            tb_SystemKills.Text = "";
            if (sSys.Name == "")
                return;

            tb_SystemName.Text = sSys.Name;
            tb_RegionName.Text = PlugInData.GalMap.GalData.Regions[sSys.RegionID].Name;
            tb_ConstellationName.Text = PlugInData.GalMap.GalData.Regions[sSys.RegionID].Constellations[sSys.ConstID].Name;
            tb_SystemSecurity.Text = Math.Round(sSys.Security, 2).ToString();
            if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(sSys.ID))
            {
                int alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[sSys.ID].allianceID;
                if (alID != 0)
                    tb_SystemSovHolder.Text = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name + " [" + PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker + "]";
            }

            s_jk = PlugInData.JKList.GetLatestJumpKillForSID(sSys.ID);

            tb_SystemKills.Text = s_jk[1] + " S, " + s_jk[2] + " P, " + s_jk[3] + " NPC";
            tb_SystemJumps.Text = s_jk[0].ToString();

            ComputeAndDisplayConstellationValues(sSys.RegionID, sSys.ConstID);
            BuildAndDisplayOtherSystemData(PlugInData.GalMap.GalData.Regions[sSys.RegionID].Constellations[sSys.ConstID].Systems[sSys.ID], tv_SystemDetails, true);

            ShowSystemDetails(sSys);
        }

        public void ComputeAndDisplayConstellationValues(int rID, int cID)
        {
            ArrayList s_jk = new ArrayList();
            int sk = 0, pk = 0, nk = 0, sj = 0;

            if (PlugInData.GalMap.GalData.Regions[rID].Constellations.ContainsKey(cID))
            {
                foreach (var ss in PlugInData.GalMap.GalData.Regions[rID].Constellations[cID].Systems)
                {
                    s_jk = PlugInData.JKList.GetLatestJumpKillForSID(ss.Value.ID);
                    sk += Convert.ToInt32(s_jk[1]);
                    pk += Convert.ToInt32(s_jk[2]);
                    nk += Convert.ToInt32(s_jk[3]);
                    sj += Convert.ToInt32(s_jk[0]);

                }

                tb_ConstellationKills.Text = sk + " Sh, " + pk + " Pd, " + nk + " NPCs";
                tb_ConstellationJumps.Text = sj.ToString();
            }
        }

        public void BuildAndDisplayOtherSystemData(SolarSystem ss, TreeView tv, bool doJump)
        {
            TreeNode station, planet, moon, belts, iceBelts, agents;
            TreeNode stnSrvc, cStn, Plnt, Mn, Agt;
            int numAgent = 0;
            ArrayList jumpToFrom, jumpJFFrom;
            int dg_ind;
            string sysNm;
            string[] ores;
            SystemCelestials SysDat = new SystemCelestials();

            tv.Nodes.Clear();

            SysDat.GetSystemCelestial(ss);

            if (ss.Stations.Count > 0)
            {
                station = tv.Nodes.Add("Stations (" + ss.Stations.Count + ")");
                foreach (var stn in ss.Stations)
                {
                    stnSrvc = station.Nodes.Add(stn.Value.Name);

                    if (stn.Value.Agents.Count > 0)
                    {
                        agents = stnSrvc.Nodes.Add("Agents (" + stn.Value.Agents.Count + ")");
                        foreach (Agent ag in stn.Value.Agents)
                        {
                            if (!ag.ResearchType.Equals("None"))
                            {
                                Agt = agents.Nodes.Add(ag.Name + " (r)");
                                Agt.Nodes.Add("Research: " + ag.ResearchType);
                            }
                            else
                                Agt = agents.Nodes.Add(ag.Name);
                            Agt.Nodes.Add("Level: " + ag.Level);
                            Agt.Nodes.Add("Quality: " + ag.Quality);
                            Agt.Nodes.Add("Type: " + ag.Type);
                            Agt.Nodes.Add("Division: " + ag.Divis);
                            if (ag.Locate)
                                Agt.Nodes.Add("Locator Agent");

                            numAgent++;
                        }
                    }

                    if (stn.Value.Refine > 0)
                        stnSrvc.Nodes.Add("Refining (" + (stn.Value.Refine * 100) + "%)");
                    foreach (string s in stn.Value.Services)
                        stnSrvc.Nodes.Add(s);


                }
            }

            if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(ss.ID))
            {
                cStn = tv.Nodes.Add("Conq. Stations (" + PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[ss.ID].ConqStations.Count + ")");
                foreach (var cqs in PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[ss.ID].ConqStations)
                {
                    if (cqs.Value.SSysID == ss.ID)
                    {
                        stnSrvc = cStn.Nodes.Add(cqs.Value.Name);
                        foreach (string s in cqs.Value.Services)
                            stnSrvc.Nodes.Add(s);
                    }
                }
            }

            planet = tv.Nodes.Add("Planets (" + ss.Planets + ")");
            foreach (var plt in SysDat.Planets.Values)
            {
                Plnt = planet.Nodes.Add(plt.Name);
                if (plt.IceFields > 0)
                    Plnt.Nodes.Add("Ice Fields: " + plt.IceFields);

                if (plt.Belts > 0)
                    Plnt.Nodes.Add("Ore Fields: " + plt.Belts);
            }

            if (ss.Moons > 0)
            {
                moon = tv.Nodes.Add("Moons (" + ss.Moons + ")");
                foreach (Moon mns in SysDat.Moons.Values)
                {
                    Mn = moon.Nodes.Add(mns.Name);
                }
            }

            if (ss.OreBelts > 0)
            {
                belts = tv.Nodes.Add("Belts (" + ss.OreBelts + ")");

                ores = PlugInData.GalMap.GetOre(ss.SecClass).Split(',');

                foreach (string s in ores)
                    belts.Nodes.Add(s);
            }
            if (ss.IceBelts > 0)
            {
                iceBelts = tv.Nodes.Add("Ice Fields (" + ss.IceBelts + ")");

                ores = PlugInData.GalMap.GetIce(ss.RegionID, ss.Security).Split(',');

                foreach (string s in ores)
                    iceBelts.Nodes.Add(s);
            }

            if (doJump)
            {
                jumpToFrom = PlugInData.GalMap.GetSystemsCanJumpTo(ss, ShipJumpRange);
                jumpJFFrom = PlugInData.GalMap.GetSystemsJFCanJumpFrom(ss, ShipJumpRange);
                dgv_JumpTo.Rows.Clear();
                foreach (JumpDistance jd in jumpToFrom)
                {
                    dg_ind = dgv_JumpTo.Rows.Add();

                    sysNm = jd.DestSystem.Name;
                    if (jd.DestSystem.Stations.Count > 0)
                        sysNm += " (s)";
                    else if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(jd.DestSystem.ID))
                        sysNm += " (cs)";

                    dgv_JumpTo[0, dg_ind].Value = sysNm;
                    dgv_JumpTo[1, dg_ind].Value = Math.Round(jd.Distance, 3);
                    dgv_JumpTo[2, dg_ind].Value = Math.Round(jd.DestSystem.Security, 2);
                    dgv_JumpTo[2, dg_ind].Style.BackColor = Color.FromArgb(0xFF, jd.DestSystem.GetSystemColor());
                }

                dgv_JumpFrom.Rows.Clear();
                foreach (var cs in PlugInData.CapShips.Ships)
                {
                    if (cs.Value.Name.Equals(cb_ShipSelect.Text))
                    {
                        if (cs.Value.CanGate)
                        {
                            // Jump Freighter or Black Ops
                            foreach (JumpDistance jd in jumpJFFrom)
                            {
                                dg_ind = dgv_JumpFrom.Rows.Add();

                                sysNm = jd.DestSystem.Name;
                                if (jd.DestSystem.Stations.Count > 0)
                                    sysNm += " (s)";
                                else if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(jd.DestSystem.ID))
                                    sysNm += " (cs)";

                                dgv_JumpFrom[0, dg_ind].Value = sysNm;
                                dgv_JumpFrom[1, dg_ind].Value = Math.Round(jd.Distance, 3);
                                dgv_JumpFrom[2, dg_ind].Value = Math.Round(jd.DestSystem.Security, 2);
                                dgv_JumpFrom[2, dg_ind].Style.BackColor = Color.FromArgb(0xFF, jd.DestSystem.GetSystemColor());
                            }
                            break;
                        }
                        else
                        {
                            // Other Type
                            foreach (JumpDistance jd in jumpToFrom)
                            {
                                dg_ind = dgv_JumpFrom.Rows.Add();

                                sysNm = jd.DestSystem.Name;
                                if (jd.DestSystem.Stations.Count > 0)
                                    sysNm += " (s)";
                                else if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(jd.DestSystem.ID))
                                    sysNm += " (cs)";

                                dgv_JumpFrom[0, dg_ind].Value = sysNm;
                                dgv_JumpFrom[1, dg_ind].Value = Math.Round(jd.Distance, 3);
                                dgv_JumpFrom[2, dg_ind].Value = Math.Round(jd.DestSystem.Security, 2);
                                dgv_JumpFrom[2, dg_ind].Style.BackColor = Color.FromArgb(0xFF, jd.DestSystem.GetSystemColor());
                            }
                            break;
                        }
                    }
                }
            }

            if (numAgent > 0)
            {
                agents = tv.Nodes.Add("Agents (" + numAgent + ")");
                foreach (var stn in ss.Stations)
                {
                    foreach (Agent ag in stn.Value.Agents)
                    {
                        if (!ag.ResearchType.Equals("None"))
                        {
                            Agt = agents.Nodes.Add(ag.Name + " (r)");
                            Agt.Nodes.Add("Research: " + ag.ResearchType);
                        }
                        else
                            Agt = agents.Nodes.Add(ag.Name);

                        Agt.Nodes.Add("Station: " + stn.Value.Name);
                        Agt.Nodes.Add("Level: " + ag.Level);
                        Agt.Nodes.Add("Quality: " + ag.Quality);
                        Agt.Nodes.Add("Type: " + ag.Type);
                        Agt.Nodes.Add("Division: " + ag.Divis);
                        if (ag.Locate)
                            Agt.Nodes.Add("Locator Agent");
                    }
                }
            }
        }

        #endregion

        #region Map Settings

        public void MinimapMovedMap(Point newP)
        {
            MainMapView.ScrollToPosition(newP);
        }

        private void cb_MapDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlugInData.Config.Settings["Detail Text"] = cb_MapDetails.SelectedIndex;
            MainMapView.UpdateConfig(PlugInData.Config);
            PlugInData.SaveConfigToDisk();
        }

        private void b_FindSystemR_Click(object sender, EventArgs e)
        {
            string sysName = cb_SystemSelect.Text;
            string whPoss;
            long whidn;

            // If the system to find is a wormhole, and we are not in the wormhole map - then, um.... time to do something
            if (sysName[0].Equals('J'))
            {
                whPoss = sysName.Replace("J", "");
                if (long.TryParse(whPoss, out whidn))
                {
                    // This is a wormhole - need to do something
                    if (!PlugInData.WHMapSelected)
                        SetMapMode(true);
                }
                else
                {
                    if (PlugInData.WHMapSelected)
                        SetMapMode(false);
                }
            }
            else
            {
                if (PlugInData.WHMapSelected)
                    SetMapMode(false);
            }

            MainMapView.SetMapScale = PlugInData.Config.Zooms["Zoom On Search"];
            MainMapView.SearchSelectedSystem(sysName);

        }

        private void tc_MapSelect_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            string sysName = cb_SystemSelect.Text;
            //tc_MapSelect.Refresh();

            if (tc_MapSelect.SelectedTab == tp_AddSysDetails)
            {
                ColorizeMGDG();
            }
        }
        
        #endregion

        #region Config_Tab

        public void SetupInitialConfigData()
        {
            PlugInData.InitialSetupConfig = true;

            sw_RegionNm.Value = PlugInData.Config.CFlags["Show Region Names"];
            sw_ConstNm.Value = PlugInData.Config.CFlags["Show Const Names"];
            sw_SystemNm.Value = PlugInData.Config.CFlags["Show System Names"];
            sw_ShowDetail.Value = PlugInData.Config.CFlags["Show Detail Text"];
            sw_ShowJB.Value = PlugInData.Config.CFlags["Show Jump Bridges"];
            sw_ShowCyno.Value = PlugInData.Config.CFlags["Show Cyno Beacons"];
            sw_ShowSov.Value = PlugInData.Config.CFlags["Show Sovreignty"];
            sw_BridgeDetail.Value = PlugInData.Config.CFlags["Show Bridge Details"];
            sw_RedoJump.Value = PlugInData.Config.CFlags["Recompute Jump Route"];
            sw_RedoGate.Value = PlugInData.Config.CFlags["Recompute Gate Route"];
            sw_SearchResultHL.Value = PlugInData.Config.CFlags["Show Search Results"];
            sw_BridgeHL.Value = PlugInData.Config.CFlags["Show Bridge Ranges"];
            sw_JumpFromHL.Value = PlugInData.Config.CFlags["Show Jump From Ranges"];
            sw_JumpToHL.Value = PlugInData.Config.CFlags["Show Jump To Ranges"];

            cb_MapItems.Items.Clear();
            foreach (var mc in PlugInData.Config.MapColors)
                cb_MapItems.Items.Add(mc.Key);

            cb_MapItems.SelectedIndex = 0;

            nud_GateAvoid.Value = (decimal)PlugInData.Config.Weights["Gate Avoid"];
            nud_GateDefault.Value = (decimal)PlugInData.Config.Weights["Gate Default"];
            nud_GateJB.Value = (decimal)PlugInData.Config.Weights["Gate Bridge"];
            nud_JumpAvoid.Value = (decimal)PlugInData.Config.Weights["Jump Avoid"];
            nud_JumpCyno.Value = (decimal)PlugInData.Config.Weights["Jump Cyno"];
            nud_JumpDefault.Value = (decimal)PlugInData.Config.Weights["Jump Default"];
            nud_JumpJB.Value = (decimal)PlugInData.Config.Weights["Jump Bridge"];
            nud_JumpStations.Value = (decimal)PlugInData.Config.Weights["Jump Station"];
            nud_SafeTower.Value = (decimal)PlugInData.Config.Weights["Jump Tower"];

            PlugInData.Config.Zooms["Zoom On Search"] = Math.Min(Math.Max(PlugInData.Config.Zooms["Zoom On Search"] * 100, (float)nud_SearchZoom.Minimum), (float)nud_SearchZoom.Maximum) / 100;
            nud_SearchZoom.Value = (decimal)(Convert.ToDouble(PlugInData.Config.Zooms["Zoom On Search"]) * 100);

            PlugInData.InitialSetupConfig = false;


        }

        public void SaveSettings()
        {
            PlugInData.Config.Weights["Gate Avoid"] = Convert.ToInt32(nud_GateAvoid.Value);
            PlugInData.Config.Weights["Gate Default"] = Convert.ToInt32(nud_GateDefault.Value);
            PlugInData.Config.Weights["Gate Bridge"] = Convert.ToInt32(nud_GateJB.Value);
            PlugInData.Config.Weights["Jump Avoid"] = Convert.ToInt32(nud_JumpAvoid.Value);
            PlugInData.Config.Weights["Jump Cyno"] = Convert.ToInt32(nud_JumpCyno.Value);
            PlugInData.Config.Weights["Jump Default"] = Convert.ToInt32(nud_JumpDefault.Value);
            PlugInData.Config.Weights["Jump Bridge"] = Convert.ToInt32(nud_JumpJB.Value);
            PlugInData.Config.Weights["Jump Station"] = Convert.ToInt32(nud_JumpStations.Value);
            PlugInData.Config.Weights["Jump Tower"] = Convert.ToInt32(nud_SafeTower.Value);

            PlugInData.Config.Zooms["Zoom On Search"] = (float)(nud_SearchZoom.Value / 100);

            SaveConfigToDisk();
        }

        public void SaveConfigToDisk()
        {
            string RMapBase_Path, RMapManage_Path, RMapData_Path, fname;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                RMapBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                RMapBase_Path = Application.StartupPath;
            }
            RMapManage_Path = Path.Combine(RMapBase_Path, "RouteMap");
            RMapData_Path = Path.Combine(RMapManage_Path, "Data");

            if (!Directory.Exists(RMapManage_Path))
                Directory.CreateDirectory(RMapManage_Path);

            if (!Directory.Exists(RMapData_Path))
                Directory.CreateDirectory(RMapData_Path);

            fname = Path.Combine(RMapData_Path, "Configuration.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, PlugInData.Config);
            pStream.Close();
        }

        private void cb_ConfigRegionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var rgn in PlugInData.GalMap.GalData.Regions)
            {
                if (rgn.Value.Name.Equals(cb_ConfigRegionSelect.Text))
                    pb_RegionColor.BackColor = rgn.Value.RColor;
            }
        }

        private void cb_MapItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MapItems.Text.Equals("Normal Gate"))
            {
                sl_Alpha.Show();
                sl_Alpha.Value = PlugInData.Config.Settings["Normal Gate Alpha"];
            }
            else if (cb_MapItems.Text.Equals("Region Gate"))
            {
                sl_Alpha.Show();
                sl_Alpha.Value = PlugInData.Config.Settings["Region Gate Alpha"];
            }
            else if (cb_MapItems.Text.Equals("Constellation Gate"))
            {
                sl_Alpha.Show();
                sl_Alpha.Value = PlugInData.Config.Settings["Const Gate Alpha"];
            }
            else
            {
                sl_Alpha.Hide();
            }

            pb_MapItemColor.BackColor = PlugInData.Config.MapColors[cb_MapItems.Text];
        }

        private void sl_Alpha_ValueChanged(object sender, EventArgs e)
        {
            Color bCol = PlugInData.Config.MapColors[cb_MapItems.Text];

            if (cb_MapItems.Text.Equals("Normal Gate"))
            {
                PlugInData.Config.Settings["Normal Gate Alpha"] = sl_Alpha.Value;
                bCol = Color.FromArgb(PlugInData.Config.Settings["Normal Gate Alpha"], PlugInData.Config.MapColors[cb_MapItems.Text]);
            }
            else if (cb_MapItems.Text.Equals("Region Gate"))
            {
                PlugInData.Config.Settings["Region Gate Alpha"] = sl_Alpha.Value;
                bCol = Color.FromArgb(PlugInData.Config.Settings["Region Gate Alpha"], PlugInData.Config.MapColors[cb_MapItems.Text]);
            }
            else if (cb_MapItems.Text.Equals("Constellation Gate"))
            {
                PlugInData.Config.Settings["Const Gate Alpha"] = sl_Alpha.Value;
                bCol = Color.FromArgb(PlugInData.Config.Settings["Const Gate Alpha"], PlugInData.Config.MapColors[cb_MapItems.Text]);
            }
            pb_MapItemColor.BackColor = bCol;
        }

        private void pb_RegionColor_Click(object sender, EventArgs e)
        {
            Color cc, nc;

            cc = pb_RegionColor.BackColor;

            DialogResult dr = cd_PickColor.ShowDialog();

            nc = cd_PickColor.Color;
            pb_RegionColor.BackColor = nc;

            // Set given region Name, Color to set color
            foreach (var rgn in PlugInData.GalMap.GalData.Regions)
            {
                if (rgn.Value.Name.Equals(cb_ConfigRegionSelect.Text))
                {
                    rgn.Value.RColor = pb_RegionColor.BackColor;
                    foreach (var cns in rgn.Value.Constellations)
                        cns.Value.CColor = rgn.Value.RColor;
                    break;
                }
            }
        }

        private void pb_MapItemColor_Click(object sender, EventArgs e)
        {
            Color cc, nc;

            cc = pb_MapItemColor.BackColor;

            DialogResult dr = cd_PickColor.ShowDialog();

            if (cb_MapItems.Text.Equals("Normal Gate"))
            {
                nc = Color.FromArgb(PlugInData.Config.Settings["Normal Gate Alpha"], cd_PickColor.Color);
            }
            else if (cb_MapItems.Text.Equals("Region Gate"))
            {
                nc = Color.FromArgb(PlugInData.Config.Settings["Region Gate Alpha"], cd_PickColor.Color);
            }
            else if (cb_MapItems.Text.Equals("Constellation Gate"))
            {
                nc = Color.FromArgb(PlugInData.Config.Settings["Const Gate Alpha"], cd_PickColor.Color);
            }
            else
            {
                nc = cd_PickColor.Color;
            }
            pb_MapItemColor.BackColor = nc;
            PlugInData.Config.MapColors[cb_MapItems.Text] = cd_PickColor.Color;
        }

        public void SaveUpdatedGalaxyMap()
        {
            this.Cursor = Cursors.WaitCursor;
            //PlugInData.GalMap.SaveGalaxy(PlugInData.LastCacheRefresh);
            PlugInData.SaveSystemCoordsToDisk();
            MapDataChanged = false;
            this.Cursor = Cursors.Default;
        }

        private void b_ApplyConfig_Click(object sender, EventArgs e)
        {
            // Apply new settings
            // Update and Invalidate MapView Somehow
            this.Cursor = Cursors.WaitCursor;
            SaveSettings();
            PlugInData.GalMap.SaveGalaxy(PlugInData.LastCacheRefresh);
            MainMapView.UpdateConfig(PlugInData.Config);
            this.Cursor = Cursors.Default;
        }

        #endregion

        #region Pilot, Ship and Skill Selection

        private void SetupPilotAndShipListings()
        {
            SortedList<string, DevComponents.AdvTree.Node> CatNode = new SortedList<string, DevComponents.AdvTree.Node>();
            DevComponents.AdvTree.Node groupNM, amar, mini, cald, galn, ore, shipN;
            bool cDef, aDef, mDef, gDef;

            Loading = true;

            amar = new DevComponents.AdvTree.Node();
            cald = new DevComponents.AdvTree.Node();
            galn = new DevComponents.AdvTree.Node();
            mini = new DevComponents.AdvTree.Node();

            cb_Pilot.Items.Clear();
            foreach (EveHQ.Core.Pilot pilt in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                if (pilt.Active)
                    cb_Pilot.Items.Add(pilt.Name);
            }
            cb_ShipSelect.Nodes.Clear();
            foreach (Ship cs in PlugInData.CapShips.Ships.Values)
            {
                if (!CatNode.ContainsKey(cs.Category))
                {
                    groupNM = new DevComponents.AdvTree.Node();
                    groupNM.Text = cs.Category;
                    groupNM.Selectable = false;
                    CatNode.Add(cs.Category, groupNM);
                    cb_ShipSelect.Nodes.Add(groupNM);
                }
            }

            foreach (var nd in CatNode)
            {
                cDef = false;
                aDef = false;
                mDef = false;
                gDef = false;
                foreach (Ship cs in PlugInData.CapShips.Ships.Values)
                {
                    if (cs.Category.Equals(nd.Key))
                    {
                        groupNM = nd.Value;
                        switch (cs.groupID)
                        {
                            case 463:
                            case 543:
                            case 883:
                            case 941:
                                ore = new DevComponents.AdvTree.Node();
                                ore.Text = cs.Name;
                                groupNM.Nodes.Add(ore);
                                break;
                            default:
                                if (cs.raceID == 1)
                                {
                                    if (!cDef)
                                    {
                                        cald = new DevComponents.AdvTree.Node();
                                        cald.Text = "Caldari";
                                        cald.Selectable = false;
                                        groupNM.Nodes.Add(cald);
                                        cDef = true;
                                    }
                                    shipN = new DevComponents.AdvTree.Node();
                                    shipN.Text = cs.Name;
                                    cald.Nodes.Add(shipN);
                                }
                                else if (cs.raceID == 2)
                                {
                                    if (!mDef)
                                    {
                                        mini = new DevComponents.AdvTree.Node();
                                        mini.Text = "Minmatar";
                                        mini.Selectable = false;
                                        groupNM.Nodes.Add(mini);
                                        mDef = true;
                                    }
                                    shipN = new DevComponents.AdvTree.Node();
                                    shipN.Text = cs.Name;
                                    mini.Nodes.Add(shipN);
                                }
                                else if (cs.raceID == 4)
                                {
                                    if (!aDef)
                                    {
                                        amar = new DevComponents.AdvTree.Node();
                                        amar.Text = "Amarr";
                                        amar.Selectable = false;
                                        groupNM.Nodes.Add(amar);
                                        aDef = true;
                                    }
                                    shipN = new DevComponents.AdvTree.Node();
                                    shipN.Text = cs.Name;
                                    amar.Nodes.Add(shipN);
                                }
                                else if (cs.raceID == 8)
                                {
                                    if (!gDef)
                                    {
                                        galn = new DevComponents.AdvTree.Node();
                                        galn.Text = "Gallente";
                                        galn.Selectable = false;
                                        groupNM.Nodes.Add(galn);
                                        gDef = true;
                                    }
                                    shipN = new DevComponents.AdvTree.Node();
                                    shipN.Text = cs.Name;
                                    galn.Nodes.Add(shipN);
                                }
                                break;
                        }
                    }
                }
            }

            if (cb_Pilot.Items.Count > 0)
            {
                if (PlugInData.Config.SelPilot != null)
                    cb_Pilot.SelectedIndex = cb_Pilot.Items.IndexOf(PlugInData.Config.SelPilot.Name);
                else if (cb_Pilot.Items.Count > 0)
                    cb_Pilot.SelectedIndex = 0;
            }

            if (cb_ShipSelect.Nodes.Count > 0)
            {
                if (PlugInData.Config.SelShip != null)
                {
                    foreach (DevComponents.AdvTree.Node n in cb_ShipSelect.Nodes)
                    {
                        if (n.HasChildNodes)
                        {
                            foreach (DevComponents.AdvTree.Node cn in n.Nodes)
                            {
                                if (cn.HasChildNodes)
                                {
                                    foreach (DevComponents.AdvTree.Node ccn in cn.Nodes)
                                    {
                                        if (ccn.Text.Equals(PlugInData.Config.SelShip.Name))
                                        {
                                            cb_ShipSelect.SelectedNode = ccn;
                                        }
                                    }
                                }
                                else
                                {
                                    if (cn.Text.Equals(PlugInData.Config.SelShip.Name))
                                    {
                                        cb_ShipSelect.SelectedNode = cn;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Loading = false;
        }

        private void cb_Pilot_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool found = false;

            if (cb_Pilot.Text == "")
                return;

            foreach (EveHQ.Core.Pilot pilt in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                if (pilt.Active)
                {
                    if (pilt.Name.Equals(cb_Pilot.Text))
                    {
                        PlugInData.Config.SelPilot = pilt;
                        EvePilot = pilt;
                    }
                }
            }

            if (PlugInData.Config.SelPilot != null)
            {
                cb_JDC.SelectedIndex = Convert.ToInt32(PlugInData.Config.SelPilot.KeySkills[(int)EveHQ.Core.Pilot.KeySkill.JumpDriveCalibration]);
                cb_JFC.SelectedIndex = Convert.ToInt32(PlugInData.Config.SelPilot.KeySkills[(int)EveHQ.Core.Pilot.KeySkill.JumpFuelConservation]);
                cb_JF.SelectedIndex = Convert.ToInt32(PlugInData.Config.SelPilot.KeySkills[(int)EveHQ.Core.Pilot.KeySkill.JumpFreighters]);
                chk_OverrideJDC.Checked = false;
                chk_OverrideJF.Checked = false;
                chk_OverrideJFC.Checked = false;
                cb_JDC.Enabled = false;
                cb_JFC.Enabled = false;
                cb_JF.Enabled = false;

                if (!Loading)
                {
                    if (!PlugInData.Misc.PilotShipSave.ContainsKey(cb_Pilot.Text))
                    {
                        PlugInData.Misc.PilotShipSave.Add(cb_Pilot.Text, new Ship());

                        foreach (var cs in PlugInData.CapShips.Ships)
                        {
                            if (cs.Value.Name.Equals(cb_ShipSelect.Text))
                            {
                                PlugInData.Misc.PilotShipSave[cb_Pilot.Text] = cs.Value;
                                break;
                            }
                        }

                        PlugInData.Misc.SavePSMisc();
                    }
                    else
                    {
                        // Populate the ship CB
                        PlugInData.Config.SelShip = new Ship(PlugInData.Misc.PilotShipSave[cb_Pilot.Text]);
                        Loading = true;
                        foreach (DevComponents.AdvTree.Node n in cb_ShipSelect.Nodes)
                        {
                            if (n.HasChildNodes)
                            {
                                foreach (DevComponents.AdvTree.Node cn in n.Nodes)
                                {
                                    if (cn.HasChildNodes)
                                    {
                                        foreach (DevComponents.AdvTree.Node ccn in cn.Nodes)
                                        {
                                            if (ccn.Text.Equals(PlugInData.Config.SelShip.Name))
                                            {
                                                cb_ShipSelect.SelectedNode = ccn;
                                                found = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cn.Text.Equals(PlugInData.Config.SelShip.Name))
                                        {
                                            cb_ShipSelect.SelectedNode = cn;
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                                break;
                        }
                        Loading = false;
                    }
                }
            }
            else
            {
                cb_JDC.SelectedIndex = 0;
                cb_JFC.SelectedIndex = 0;
                cb_JF.SelectedIndex = 0;
                chk_OverrideJDC.Checked = false;
                chk_OverrideJF.Checked = false;
                chk_OverrideJFC.Checked = false;
                cb_JDC.Enabled = false;
                cb_JFC.Enabled = false;
                cb_JF.Enabled = false;
            }

            PlugInData.SaveConfigToDisk();
            CalculateJumpRange();

            if ((tb_SystemName.Text.Length > 1) && (!tb_SystemName.Text.Equals("System Name")))
                MainMapView.SearchSelectedSystem(tb_SystemName.Text);

        }

        private void cb_ShipSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool found = false;

            foreach (var cs in PlugInData.CapShips.Ships)
            {
                if (cs.Value.Name.Equals(cb_ShipSelect.Text))
                {
                    found = true;
                    ShipJumpRange = cs.Value.JumpDistance;
                    PlugInData.Config.SelShip = cs.Value;
                    JumpShip = cs.Value;

                    if (!Loading)
                    {
                        if (PlugInData.Misc.PilotShipSave.ContainsKey(cb_Pilot.Text))
                            PlugInData.Misc.PilotShipSave[cb_Pilot.Text] = new Ship(cs.Value);
                        else
                        {
                            if (cb_Pilot.Text != "")
                                PlugInData.Misc.PilotShipSave.Add(cb_Pilot.Text, new Ship(cs.Value));
                        }
                        PlugInData.Misc.SavePSMisc();
                    }
                    break;
                }
            }

            if (!found && !Loading)
            {
                ShipJumpRange = 0;
                PlugInData.Config.SelShip = null;
                JumpShip = null;
            }

            PlugInData.SaveConfigToDisk();
            CalculateJumpRange();

            if ((tb_SystemName.Text.Length > 1) && (!tb_SystemName.Text.Equals("System Name")))
                MainMapView.SearchSelectedSystem(tb_SystemName.Text);
        }

        private void cb_JDC_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sysName = tb_SystemName.Text;

            CalculateJumpRange();

            if ((sysName.Length > 0) && (!sysName.Equals("System Name")))
                MainMapView.SearchSelectedSystem(sysName);
        }

        private void cb_JFC_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateJumpRange();
        }

        private void cb_JF_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateJumpRange();
        }

        private void chk_OverrideJDC_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_OverrideJDC.Checked)
            {
                cb_JDC.Enabled = false;
                cb_JDC.SelectedIndex = Convert.ToInt32(PlugInData.Config.SelPilot.KeySkills[(int)EveHQ.Core.Pilot.KeySkill.JumpDriveCalibration]);
            }
            else
                cb_JDC.Enabled = true;
        }

        private void chk_OverrideJFC_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_OverrideJFC.Checked)
            {
                cb_JFC.Enabled = false;
                cb_JFC.SelectedIndex = Convert.ToInt32(PlugInData.Config.SelPilot.KeySkills[(int)EveHQ.Core.Pilot.KeySkill.JumpFuelConservation]);
            }
            else
                cb_JFC.Enabled = true;
        }

        private void chk_OverrideJF_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_OverrideJF.Checked)
            {
                cb_JF.Enabled = false;
                cb_JF.SelectedIndex = Convert.ToInt32(PlugInData.Config.SelPilot.KeySkills[(int)EveHQ.Core.Pilot.KeySkill.JumpFreighters]);
            }
            else
                cb_JF.Enabled = true;
        }

        private void CalculateJumpRange()
        {
            int JDC, JFC, JF;
            int fType;

            JDC = cb_JDC.SelectedIndex;
            JFC = cb_JFC.SelectedIndex;
            JF = cb_JF.SelectedIndex;

            foreach (var cs in PlugInData.CapShips.Ships)
            {
                if (cs.Value.Name.Equals(cb_ShipSelect.Text))
                {
                    ShipJumpRange = cs.Value.JumpDistance * (1 + (JDC * 0.25));
                    fType = cs.Value.fuelID;

                    if (cs.Value.CanGate)
                    {
                        //JF
                        ShipFuelPerLY = Convert.ToInt32(cs.Value.FuelConsumption * (1 - (JFC * 0.1)));
                        ShipFuelPerLY = Convert.ToInt32(ShipFuelPerLY * (1 - (JF * 0.1)));
                    }
                    else
                    {
                        ShipFuelPerLY = Convert.ToInt32(cs.Value.FuelConsumption * (1 - (JFC * 0.1)));
                    }
                    break;
                }
            }

            CurShipJumpRange = Math.Round(ShipJumpRange, 2);
            tb_ShipJumpRange.Text = CurShipJumpRange.ToString() + " ly";
            tb_FuelPerLY.Text = ShipFuelPerLY + " ly";
        }

        #endregion

        #region Route Selection

        private void b_ClearWaypoints_Click(object sender, EventArgs e)
        {
            WaypointList.Clear();
            WaypointListUpdated();
        }

        private void b_ClearAvoids_Click(object sender, EventArgs e)
        {
            AvoidList.Clear();
            AvoidListUpdated();
        }

        private void b_SetWaypoint_Click(object sender, EventArgs e)
        {
            if (WaypointList == null)
                WaypointList = new ArrayList();

            if (cb_SystemSelect.Text != "")
            {
                WaypointList.Add(cb_SystemSelect.Text);
            }

            WaypointListUpdated();
        }

        public void MapViewSelectWaypoint(SolarSystem sSys)
        {
            if (!WaypointList.Contains(sSys.Name))
                WaypointList.Add(sSys.Name);

            WaypointListUpdated();
        }

        private void b_SetEnd_Click(object sender, EventArgs e)
        {
            if (cb_SystemSelect.Text != "")
                RouteEnd = cb_SystemSelect.Text;
            else
                RouteEnd = "";

            tb_DestSystem.Text = RouteEnd;
        }

        public void MapViewSelectEnd(SolarSystem sSys)
        {
            RouteEnd = sSys.Name;
            tb_DestSystem.Text = RouteEnd;
        }

        private void b_SetAvoid_Click(object sender, EventArgs e)
        {
            if (AvoidList == null)
                AvoidList = new ArrayList();

            if (cb_SystemSelect.Text != "")
            {
                if (!AvoidList.Contains(cb_SystemSelect.Text))
                    AvoidList.Add(cb_SystemSelect.Text);
            }

            AvoidListUpdated();
        }

        public void MapViewSelectAvoid(SolarSystem sSys)
        {
            if (!AvoidList.Contains(sSys.Name))
                AvoidList.Add(sSys.Name);

            AvoidListUpdated();
        }
        
        private void b_SetStart_Click(object sender, EventArgs e)
        {
            if (cb_SystemSelect.Text != "")
                RouteStart = cb_SystemSelect.Text;
            else
                RouteStart = "";

            tb_StartSystem.Text = RouteStart;
            MainMapView.SetSelectedStartSystem(RouteStart);
        }

        public void MapViewSetStart(SolarSystem sSys)
        {
            RouteStart = sSys.Name;
            tb_StartSystem.Text = RouteStart;
        }

        public void RouteMapCenterSystem(SolarSystem sSys)
        {
            MainMapView.SetMapScale = PlugInData.Config.Zooms["Zoom On Search"];
            MainMapView.SearchSelectedSystem(sSys.Name);
        }

        public void FindAlternateTravelNode(SolarSystem cNode)
        {
            ArrayList fNodeJL, dNodeJL, aNodeJL;
            SolarSystem fNode = new SolarSystem();
            SolarSystem dNode = new SolarSystem();
            JumpBridge JB;
            aNodeJL = new ArrayList();
            AlternateNode an;
            int alID;
            ArrayList s_jk = new ArrayList();
            bool CCS = false;

            if ((cNode.ID == CurrentRoute[0].SolarSystem.ID) || (cNode.ID == CurrentRoute[CurrentRoute.Count - 1].SolarSystem.ID))
            {
                // You cannot select new start or destination in this manner - POP-UP, create new route instead
                return;
            }

            anSysSel = cNode;   // Preserve the reference for replacement if selected

            for (int cs = 1; cs < CurrentRoute.Count; cs++)
            {
                if (CurrentRoute[cs].SolarSystem.ID == cNode.ID)
                {
                    fNode = CurrentRoute[cs - 1].SolarSystem;
                    dNode = CurrentRoute[cs + 1].SolarSystem;
                    break;
                }
            }

            fNodeJL = PlugInData.GalMap.GetSystemsCanJumpTo(fNode, ShipJumpRange);
            dNodeJL = PlugInData.GalMap.GetSystemsCanJumpTo(dNode, ShipJumpRange);

            foreach (JumpDistance jf in fNodeJL)
            {
                foreach (JumpDistance jd in dNodeJL)
                {
                    if ((jf.DestSystem.ID == jd.DestSystem.ID) && (jf.DestSystem.ID != cNode.ID))
                    {
                        an = new AlternateNode();
                        an.from = fNode;
                        an.curr = jf.DestSystem;
                        an.to = dNode;
                        an.sec = Math.Round(jf.DestSystem.Security, 2);
                        an.distFrom = jf.Distance;
                        an.distTo = jd.Distance;

                        // Jump in to Alternate
                        an.JumpTyp = PlugInData.JumpType.Cyno;
                        an.JumpTT = "Cyno Alt - Cyno";
                        an.ISOFrom = Convert.ToInt32(Ship.GetFuel(an.distFrom, NewRoute, 0));
                        an.cargoF = Math.Round((an.ISOFrom * 0.15), 2);

                        JB = PlugInData.DoesBridgeLinkExist (an.curr.ID, an.from.ID);

                        DataRow[] dr = PlugInData.SysD.SystemTable.Select("SID=" + an.curr.ID + " AND Safe=True");
                        if (dr.Length > 0)
                            CCS = Convert.ToBoolean(dr[0]["Safe"]);
                        else
                            CCS = false;

                        if (PlugInData.CynoGenJamList.ContainsKey(an.curr.ID) && !PlugInData.CynoGenJamList[an.curr.ID].IsJammer)
                        {
                            an.JumpTyp = PlugInData.JumpType.Beacon;
                            an.JumpTT = "Cyno Generator to Jump To";
                            an.ISOFrom = Convert.ToInt32(Ship.GetFuel(an.distFrom, NewRoute, 0));
                            an.cargoF = Math.Round((an.ISOFrom * 0.15), 2);
                        }
                        else if (PlugInData.CynoGenJamList.ContainsKey(an.curr.ID) && PlugInData.CynoGenJamList[an.curr.ID].IsJammer)
                        {
                            an.JumpTyp = PlugInData.JumpType.Undefined;
                            an.JumpTT = "Cyno Jammer in System - No way to Jump!";
                            an.ISOFrom = Convert.ToInt32(Ship.GetFuel(an.distFrom, NewRoute, 0));
                            an.cargoF = Math.Round((an.ISOFrom * 0.15), 2);
                        }
                        else if (CCS)
                        {
                            an.JumpTyp = PlugInData.JumpType.CynoSafe;
                            an.JumpTT = "Cyno Safe Spot in System";
                            an.ISOFrom = Convert.ToInt32(Ship.GetFuel(an.distFrom, NewRoute, 0));
                            an.cargoF = Math.Round((an.ISOFrom * 0.15), 2);
                        }
                        else if (JB != null)
                        {
                            an.JumpTyp = PlugInData.JumpType.Bridge;
                            an.Bridge = JB;
                            an.JumpTT = "Jump Bridge";
                            an.LOFrom = Convert.ToInt32(Ship.GetFuel(an.distFrom, NewRoute, 1));
                            an.cargoF = Math.Round((an.LOFrom * 0.4), 2);
                        }

                        // Jump Out of Alternate
                        an.JumpTypTo = PlugInData.JumpType.Cyno;
                        an.JumpToTT = "Cyno Alt - Cyno";
                        an.ISOTo = Convert.ToInt32(Ship.GetFuel(an.distTo, NewRoute, 0));
                        an.cargoT = Math.Round((an.ISOTo * 0.15), 2);

                        JB = PlugInData.DoesBridgeLinkExist(an.curr.ID, an.to.ID);

                        dr = PlugInData.SysD.SystemTable.Select("SID=" + an.to.ID + " AND Safe=True");
                        if (dr.Length > 0)
                            CCS = Convert.ToBoolean(dr[0]["Safe"]);
                        else
                            CCS = false;

                         if (PlugInData.CynoGenJamList.ContainsKey(an.to.ID) && !PlugInData.CynoGenJamList[an.to.ID].IsJammer)
                        {
                            an.JumpTypTo = PlugInData.JumpType.Beacon;
                            an.JumpToTT = "Cyno Generator to Jump To";
                            an.ISOTo = Convert.ToInt32(Ship.GetFuel(an.distTo, NewRoute, 0));
                            an.cargoT = Math.Round((an.ISOTo * 0.15), 2);
                        }
                        else if (PlugInData.CynoGenJamList.ContainsKey(an.to.ID) && PlugInData.CynoGenJamList[an.to.ID].IsJammer)
                        {
                            an.JumpTypTo = PlugInData.JumpType.Undefined;
                            an.JumpToTT = "Cyno Jammer in System - No way to Jump!";
                            an.ISOTo = Convert.ToInt32(Ship.GetFuel(an.distTo, NewRoute, 0));
                            an.cargoT = Math.Round((an.ISOTo * 0.15), 2);
                        }
                        else if (CCS)
                        {
                            an.JumpTypTo = PlugInData.JumpType.CynoSafe;
                            an.JumpToTT = "Cyno Safe Spot in System";
                            an.ISOTo = Convert.ToInt32(Ship.GetFuel(an.distTo, NewRoute, 0));
                            an.cargoT = Math.Round((an.ISOTo * 0.15), 2);
                        }
                        else if (JB != null)
                        {
                            an.JumpTypTo = PlugInData.JumpType.Bridge;
                            an.Bridge = JB;
                            an.JumpToTT = "Jump Bridge";
                            an.LOTo = Convert.ToInt32(Ship.GetFuel(an.distTo, NewRoute, 1));
                            an.cargoT = Math.Round((an.LOTo * 0.4), 2);
                        }

                        an.IsoType = JumpShip.GetIsoType();
                        an.constellation = PlugInData.GalMap.GalData.Regions[jf.DestSystem.RegionID].Constellations[jf.DestSystem.ConstID].Name;   // Constellation
                        an.region = PlugInData.GalMap.GalData.Regions[jf.DestSystem.RegionID].Name;         // Region

                        if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(jf.DestSystem.ID))
                        {
                            alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[jf.DestSystem.ID].allianceID;
                            if (PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances.ContainsKey(alID))
                            {
                                an.sov = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name;
                                an.sTic = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker;
                            }
                        }

                        s_jk = PlugInData.JKList.GetLatestJumpKillForSID(jf.DestSystem.ID);

                        an.Kills = s_jk[1] + " Ships, " + s_jk[2] + " Pods, " + s_jk[3] + " NPCs";
                        an.Jumps = s_jk[0].ToString();

                        if (an.curr.Stations.Count > 0)
                        {
                            an.Station = true;
                            an.StationTT = "One or More Stations are in the System!";
                        }
                        else if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(an.curr.ID))
                        {
                            an.Station = true;
                            an.StationTT = PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[an.curr.ID].ConqStations.ElementAt(0).Value.Name;
                        }

                        if (an.curr.Stations.Count > 0)
                        {
                            an.Station = true;
                            an.StationTT = "One or More Stations are in the System!";
                        }
                        else if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(an.curr.ID))
                        {
                            an.Station = true;
                            an.StationTT = PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[an.curr.ID].ConqStations.ElementAt(0).Value.Name;
                        }

                        aNodeJL.Add(an);
                    }
                }
            }

            // Display a list of alternate nodes to user to select the system
            SelAltNode SAN = new SelAltNode(aNodeJL);
            SAN.ShowDialog();
        }

        public void AlternateRouteNodeSelected(AlternateNode node)
        {
            SolarSystem fNode = new SolarSystem();
            SolarSystem dNode = new SolarSystem();
            Vertex nv = new Vertex();

            // A new Node point was selected, re-do the CurrentRoute to match the new path
            nv.SolarSystem = node.curr;
            nv.FuelCost = Convert.ToInt32(node.ISOFrom);
            nv.LOCost = Convert.ToInt32(node.LOFrom);
           
            for (int cs = 1; cs < CurrentRoute.Count; cs++)
            {
                if (CurrentRoute[cs].SolarSystem.ID == anSysSel.ID)
                {
                    nv.Path = CurrentRoute[cs - 1].SolarSystem;
                    nv.JumpTyp = node.JumpTyp;
                    CurrentRoute[cs + 1].Path = node.to;
                    CurrentRoute[cs + 1].FuelCost = Convert.ToInt32(node.ISOTo);
                    CurrentRoute[cs + 1].LOCost = Convert.ToInt32(node.LOTo);
                    CurrentRoute[cs + 1].JumpTyp = node.JumpTypTo;
                    CurrentRoute[cs + 1].LYTraveled = (float)node.distTo;
                    nv.LYTraveled = (float)node.distFrom;
                    CurrentRoute[cs] = nv;
                    break;
                }
            }
            BuildAndDisplayCynoRoute();
        }

        #endregion

        #region Jump_Gate_Route_Computations

        private void b_ComputeRoute_Click(object sender, EventArgs e)
        {           
            ActiveRoute = 1;
            BuildAndCalcRoute(true);
        }

        private void b_ComputeGateRoute_Click(object sender, EventArgs e)
        {
            ActiveRoute = 2;
            BuildAndCalcRoute(false);
        }

        private void BuildAndCalcRoute(bool jump)
        {
            double minSec, maxSec;

            if ( (!PlugInData.SystemList.Contains(RouteStart)) ||
                 (!PlugInData.SystemList.Contains(RouteEnd)) )
            {
                if (AvWpChanged)
                    return;

                // Both starting and ending system is needed to compute a route
                MessageBox.Show("You must have both a starting and destination system selected to generate a route.", "Input Missing");
                return;
            }

            SolarSystem sSys = PlugInData.GalMap.GetSystemByName(RouteStart);
            SolarSystem dSys = PlugInData.GalMap.GetSystemByName(RouteEnd);

            if ((dSys.Security > 0.45) && jump)
            {
                if (AvWpChanged)
                    return;

                MessageBox.Show("Sorry, but you cannot jump into High-Sec via a Cyno.", "Invalid Destination");
                return;
            }

            if (JumpShip == null)
            {
                if (jump)
                {
                    MessageBox.Show("No ship type selected, using the Carrier 'Archon'.", "No Ship Type");
                    JumpShip = PlugInData.CapShips.Ships["Archon"];
                }
                else
                {
                    MessageBox.Show("No ship type selected, using the Frigate 'Heron'.", "No Ship Type");
                    JumpShip = PlugInData.CapShips.Ships["Heron"];
                }
            }

            if ((ShipJumpRange <= 0) && jump)
            {
                MessageBox.Show("Sorry, but you Must select a Jump Capable ship to generate a Cyno Route.", "Invalid Ship Type");
                return;
            }

            if (!JumpShip.CanGate && !jump)
            {
                // Ship needs to be able to gate
                MessageBox.Show("Calculation taking place, but current ship cannot use Gates.\nBridge fuel calculations will be off..", "Invalid Ship Type");
                //return;
            }

            if (!jump)
            {
                minSec = Convert.ToDouble(nud_MinSecLev.Value);
                maxSec = Convert.ToDouble(nud_MaxSecLev.Value);
            }
            else
            {
                maxSec = 0.45;
                minSec = -1.0;
            }

            NewRoute = new Route(sSys, dSys, AvoidList, WaypointList, EvePilot, JumpShip, minSec, maxSec);

            NewRoute.ShipJumpRange = ShipJumpRange;
            NewRoute.ShipFuelPerLY = ShipFuelPerLY;

            if (jump)
            {
                NewRoute.PreferStation = cb_StationOnly.Checked;
                NewRoute.UseCynoBeacon = cbx_UseCynoGens.Checked;
                NewRoute.UseJumpBridge = cbx_UseJumpBridge.Checked;
                NewRoute.UseCynoSafeTwr = cbx_JRSafeTower.Checked;
                pbar_RouteCalc.Value = 0;

                pbar_RouteCalc.Maximum = PlugInData.JUMP_TARGET_SYSTEMS * (1 + WaypointList.Count);
            }
            else
            {
                NewRoute.PreferStation = cb_GRStation.Checked;
                NewRoute.UseCynoBeacon = cb_GRCyno.Checked;
                NewRoute.UseJumpBridge = cb_GRJB.Checked;
                NewRoute.UseCynoSafeTwr = false;
                pbar_GRSearch.Value = 0;
                if (WaypointList.Count > 0)
                    pbar_GRSearch.Maximum = PlugInData.JUMP_TARGET_SYSTEMS * (1 + WaypointList.Count);
                else
                    pbar_GRSearch.Maximum = PlugInData.JUMP_TARGET_SYSTEMS;
            }

            // Disable button to prevent double clicks
            if (jump)
            {
                b_ComputeRoute.Enabled = false;
                bgw_CynoRouteCalc.RunWorkerAsync();
            }
            else
            {
                b_ComputeGateRoute.Enabled = false;
                bgw_GateRouteCalc.RunWorkerAsync();
            }
        }

        private void bgw_CynoRouteCalc_DoWork(object sender, DoWorkEventArgs e)
        {
            CynoRoute CynoRouteFinder = new CynoRoute();
            CynoRouteFinder.SystemProcessed += bgw_CynoRouteCalc_ProgressChanged;

            CurrentRoute = CynoRouteFinder.GetRoute(NewRoute);
        }

        private void bgw_CynoRouteCalc_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            IncrementProgressBar a = Pbar_Increment;
            pbar_RouteCalc.BeginInvoke(a);
        }

        private void Pbar_Increment()
        {
            pbar_RouteCalc.Value += 1;
        }

        private void Pbar_GRIncrement()
        {
            pbar_GRSearch.Value += 1;
        }

        private void BuildAndDisplayCynoRoute()
        {
            RouteNode CurNode;
            RouteSystem RS;
            int alID, num;
            string isoType, safeTT;
            SystemCelestials SC = new SystemCelestials();
            double totalM3 = 0, totalDist = 0;
            double totalISO = 0, totalLO = 0;
            ArrayList s_jk = new ArrayList();
            bool hSafe = false;

            isoType = JumpShip.GetIsoType();

            gp_JumpRoute.Controls.Clear();
            num = 0;
            // Populate Computed Route Display
            lbx_JumpRouteLength.Text = "Route Length is " + (CurrentRoute.Count - 1) + " Jumps.";
            for (int cs = 0; cs < CurrentRoute.Count; cs++)
            {
                CurNode = new RouteNode();
                CurNode.JumpNum = cs;
                CurNode.from = CurrentRoute[cs].SolarSystem;

                if (CurrentRoute[cs].SolarSystem.Stations.Count > 0)
                {
                    CurNode.Station = true;
                    CurNode.StationTT = "One or More Stations are in the System!";
                }
                else if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(CurrentRoute[cs].SolarSystem.ID))
                {
                    CurNode.Station = true;
                    CurNode.StationTT = PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[CurrentRoute[cs].SolarSystem.ID].ConqStations.ElementAt(0).Value.Name;
                }

                CurNode.sec = Math.Round(CurrentRoute[cs].SolarSystem.Security, 2);
                if (cs > 0)
                {
                    if (CurrentRoute[cs].LYTraveled > ShipJumpRange)
                    {
                        CurNode.distance = Math.Round(CurrentRoute[cs].LYTraveled - CurrentRoute[cs - 1].LYTraveled, 2);
                        totalDist += Math.Round(CurrentRoute[cs].LYTraveled - CurrentRoute[cs - 1].LYTraveled, 2);
                    }
                    else
                    {
                        CurNode.distance = Math.Round(CurrentRoute[cs].LYTraveled, 2);
                        totalDist += Math.Round(CurrentRoute[cs].LYTraveled, 2);
                    }
                }
                else
                    CurNode.distance = 0;

                if ((CurrentRoute[cs].JumpTyp == PlugInData.JumpType.Bridge) && (cs > 0))
                {
                    CurNode.LO = CurrentRoute[cs].LOCost;
                    totalLO += CurrentRoute[cs].LOCost;
                    CurNode.ISO = 0;
                }
                else if (cs > 0)
                {
                    CurNode.ISO = CurrentRoute[cs].FuelCost;
                    CurNode.IsoType = isoType;
                    totalISO += CurrentRoute[cs].FuelCost;
                    CurNode.LO = 0;
                }
                else
                {
                    CurNode.ISO = 0;
                    CurNode.LO = 0;
                    CurNode.IsoType = isoType;
                }

                CurNode.cargo = Math.Round(((CurNode.ISO * 0.15) + (CurNode.LO * 0.4)), 2);
                totalM3 += Math.Round(((CurNode.ISO * 0.15) + (CurNode.LO * 0.4)), 2);

                CurNode.JumpTyp = CurrentRoute[cs].JumpTyp;

                if (CurrentRoute[cs].JumpTyp == PlugInData.JumpType.Bridge)
                {
                    CurNode.JumpTT = "(JB) " + CurrentRoute[cs].Bridge.FromMoon + " [PW: " + CurrentRoute[cs].Bridge.Password + " ]";
                    CurNode.JumpTT += "\n(JB) " + CurrentRoute[cs].Bridge.ToMoon + " [PW: " + CurrentRoute[cs].Bridge.Password + " ]";
                }
                else if (CurrentRoute[cs].JumpTyp == PlugInData.JumpType.Cyno)
                    CurNode.JumpTT = "Cyno Alt - Cyno";
                else if (CurrentRoute[cs].JumpTyp == PlugInData.JumpType.Beacon)
                    CurNode.JumpTT = "Cyno Generator - Beacon";
                else if (CurrentRoute[cs].JumpTyp == PlugInData.JumpType.Gate)
                    CurNode.JumpTT = "Gate Jump";
                else if (CurrentRoute[cs].JumpTyp == PlugInData.JumpType.CynoSafe)
                {
                    CurNode.JumpTT = "Cyno Alt - Cyno";

                    DataRow[] dr = PlugInData.SysD.SystemTable.Select("SID=" + CurrentRoute[cs].SolarSystem.ID + " AND Safe=True");
                    if (dr.Length > 0)
                        hSafe = Convert.ToBoolean(dr[0]["Safe"]);
                    else
                        hSafe = false;

                    if (hSafe)
                    {
                        SC.GetSystemCelestial(CurrentRoute[cs].SolarSystem);

                        safeTT = dr[0]["Moon"].ToString() + " [pw: " + dr[0]["Password"].ToString() + "]";

                        CurNode.StationTT = "POS Safe Spot for Cyno at: " + safeTT;
                        CurNode.CynoSafeMoon = dr[0]["Moon"].ToString();
                        break;
                    }
                }
                else if (CurrentRoute[cs].JumpTyp == PlugInData.JumpType.Undefined)
                    CurNode.JumpTT = "Um - with current settings you cannot Get here !";

                CurNode.constellation = PlugInData.GalMap.GalData.Regions[CurrentRoute[cs].SolarSystem.RegionID].Constellations[CurrentRoute[cs].SolarSystem.ConstID].Name;   // Constellation
                CurNode.region = PlugInData.GalMap.GalData.Regions[CurrentRoute[cs].SolarSystem.RegionID].Name;         // Region

                if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(CurrentRoute[cs].SolarSystem.ID))
                {
                    alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[CurrentRoute[cs].SolarSystem.ID].allianceID;
                    if (PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances.ContainsKey(alID))
                    {
                        CurNode.sov = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name;
                        CurNode.sTic = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker;
                    }
                }

                s_jk = PlugInData.JKList.GetLatestJumpKillForSID(CurrentRoute[cs].SolarSystem.ID);

                CurNode.Kills = s_jk[1] + " Ships, " + s_jk[2] + " Pods, " + s_jk[3] + " NPCs";
                CurNode.Jumps = s_jk[0].ToString();

                if ((cs + 1) == CurrentRoute.Count)
                    CurNode.Dest = true;

                RS = new RouteSystem(CurNode, this);
                RS.Location = new Point(0, (num * 67));

                gp_JumpRoute.Controls.Add(RS);
                num++;
            }
            CurNode = new RouteNode();
            CurNode.JumpNum = -1;
            CurNode.ISO = totalISO;
            CurNode.distance = totalDist;
            CurNode.LO = totalLO;
            CurNode.cargo = totalM3;
            CurNode.Jumps = (CurrentRoute.Count - 1).ToString();
            CurNode.IsoType = isoType;
            RS = new RouteSystem(CurNode, this);
            RS.Location = new Point(0, (num * 67));
            gp_JumpRoute.Controls.Add(RS);

            b_ComputeRoute.Enabled = true;
            pbar_RouteCalc.Value = 0;
            MainMapView.Route = CurrentRoute;
            MainMapView.RouteChanged = true;
            MainMapView.UpdateMapForChange();
        }

        private void bgw_CynoRouteCalc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BuildAndDisplayCynoRoute();
        }

        private void bgw_GateRouteCalc_DoWork(object sender, DoWorkEventArgs e)
        {
            GateRoute GateRouteFinder = new GateRoute();
            GateRouteFinder.SystemProcessed += bgw_GateRouteCalc_ProgressChanged;

            CurrentGateRoute = GateRouteFinder.GetRoute(NewRoute);
        }

        private void bgw_GateRouteCalc_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            IncrementProgressBar a = Pbar_GRIncrement;
            pbar_GRSearch.BeginInvoke(a);
        }

        private void bgw_GateRouteCalc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RouteNode CurNode;
            RouteSystem RS;
            int alID, num;
            double totalM3 = 0;
            double totalLO = 0;
            ArrayList s_jk = new ArrayList();

            gp_GateRoute.Controls.Clear();
            num = 0;
            // Populate Computed Route Display
            lbx_RouteLength.Text = "Route Length is " + (CurrentGateRoute.Count - 1) + " Gates.";
            for (int cs = 0; cs < CurrentGateRoute.Count; cs++)
            {
                CurNode = new RouteNode();
                CurNode.AltNode = false;
                CurNode.JumpNum = cs;
                CurNode.from = CurrentGateRoute[cs].SolarSystem;

                if (CurrentGateRoute[cs].SolarSystem.Stations.Count > 0)
                {
                    CurNode.Station = true;
                    CurNode.StationTT = "One or More Stations are in the System!";
                }
                else if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(CurrentGateRoute[cs].SolarSystem.ID))
                {
                    CurNode.Station = true;
                    CurNode.StationTT = PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[CurrentGateRoute[cs].SolarSystem.ID].ConqStations.ElementAt(0).Value.Name;
                }

                CurNode.sec = Math.Round(CurrentGateRoute[cs].SolarSystem.Security, 2);
                CurNode.distance = -1;

                CurNode.ISO = -1;
                
                CurNode.LO = CurrentGateRoute[cs].LOCost;                                     // Fuel LO
                totalLO += CurrentGateRoute[cs].LOCost;

                CurNode.cargo = CurNode.LO * 0.4;
                totalM3 += CurNode.cargo;

                CurNode.JumpTyp = CurrentGateRoute[cs].JumpTyp;
                if (CurrentGateRoute[cs].JumpTyp == PlugInData.JumpType.Bridge)
                {
                    CurNode.JumpTT = "(JB) " + CurrentGateRoute[cs].Bridge.FromMoon + " [PW: " + CurrentGateRoute[cs].Bridge.Password + " ]";
                    CurNode.JumpTT += "\n(JB) " + CurrentGateRoute[cs].Bridge.ToMoon + " [PW: " + CurrentGateRoute[cs].Bridge.Password + " ]";
                }
                else if (CurrentGateRoute[cs].JumpTyp == PlugInData.JumpType.Cyno)
                    CurNode.JumpTT = "Cyno Alt - Cyno";
                else if (CurrentGateRoute[cs].JumpTyp == PlugInData.JumpType.Beacon)
                    CurNode.JumpTT = "Cyno Generator - Beacon";
                else if (CurrentGateRoute[cs].JumpTyp == PlugInData.JumpType.Gate)
                    CurNode.JumpTT = "Gate Jump";
                else if (CurrentGateRoute[cs].JumpTyp == PlugInData.JumpType.Undefined)
                    CurNode.JumpTT = "Um - with current settings you cannot Get here !";

                CurNode.constellation = PlugInData.GalMap.GalData.Regions[CurrentGateRoute[cs].SolarSystem.RegionID].Constellations[CurrentGateRoute[cs].SolarSystem.ConstID].Name;   // Constellation
                CurNode.region = PlugInData.GalMap.GalData.Regions[CurrentGateRoute[cs].SolarSystem.RegionID].Name;         // Region

                if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(CurrentGateRoute[cs].SolarSystem.ID))
                {
                    alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[CurrentGateRoute[cs].SolarSystem.ID].allianceID;
                    if (PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances.ContainsKey(alID))
                    {
                        CurNode.sov = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name;
                        CurNode.sTic = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker;
                    }
                }

                s_jk = PlugInData.JKList.GetLatestJumpKillForSID(CurrentGateRoute[cs].SolarSystem.ID);

                CurNode.Kills = s_jk[1] + " Ships, " + s_jk[2] + " Pods, " + s_jk[3] + " NPCs";
                CurNode.Jumps = s_jk[0].ToString();

                RS = new RouteSystem(CurNode, this);
                RS.Location = new Point(0, (num * 67));

                gp_GateRoute.Controls.Add(RS);
                num++;
            }
            CurNode = new RouteNode();
            CurNode.JumpNum = -1;
            CurNode.ISO = -1;
            CurNode.distance = -1;
            CurNode.LO = totalLO;
            CurNode.cargo = totalM3;
            CurNode.Jumps = (CurrentGateRoute.Count - 1).ToString();
            RS = new RouteSystem(CurNode, this);
            RS.Location = new Point(0, (num * 67));
            gp_GateRoute.Controls.Add(RS);

            b_ComputeGateRoute.Enabled = true;
            pbar_GRSearch.Value = 0;
            MainMapView.Route = CurrentGateRoute;
            MainMapView.RouteChanged = true;
            MainMapView.UpdateMapForChange();
        }

        #endregion

        #region Avoid List and Waypoint List

        private void AvoidListUpdated()
        {
            lb_AvoidList.Items.Clear();
            lb_AvoidList.Items.AddRange(AvoidList.ToArray());
             
            if (PlugInData.Config.CFlags["Recompute Gate Route"] && ActiveRoute == 2)
            {
                AvWpChanged = true;
                BuildAndCalcRoute(false);
                AvWpChanged = false;
            }
            else if (PlugInData.Config.CFlags["Recompute Jump Route"] && ActiveRoute == 1)
            {
                AvWpChanged = true; 
                BuildAndCalcRoute(true);
                AvWpChanged = false;
            }
        }

        private void WaypointListUpdated()
        {
            lb_WaypointList.Items.Clear();
            lb_WaypointList.Items.AddRange(WaypointList.ToArray());

            if (PlugInData.Config.CFlags["Recompute Gate Route"] && ActiveRoute == 2)
            {
                AvWpChanged = true;
                BuildAndCalcRoute(false);
                AvWpChanged = false;
            }
            else if (PlugInData.Config.CFlags["Recompute Jump Route"] && ActiveRoute == 1)
            {
                AvWpChanged = true;
                BuildAndCalcRoute(true);
                AvWpChanged = false;
            }
        }

        private void tsmi_JT_Zoon_Click(object sender, EventArgs e)
        {
            string name;
            int sInd;

            if (dgv_JumpTo.SelectedRows.Count != 1)
                return;

            if (dgv_JumpTo.SelectedRows[0].Cells[0].Value == null)
                return;

            name = dgv_JumpTo.SelectedRows[0].Cells[0].Value.ToString();
            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            MainMapView.SetMapScale = 0.9f;
            MainMapView.SearchSelectedSystem(name);
        }

        private void tsmi_JT_WP_Click(object sender, EventArgs e)
        {
            string name;
            int sInd;

            if (WaypointList == null)
                WaypointList = new ArrayList();

            if (dgv_JumpTo.SelectedRows.Count != 1)
                return;

            if (dgv_JumpTo.SelectedRows[0].Cells[0].Value == null)
                return;

            name = dgv_JumpTo.SelectedRows[0].Cells[0].Value.ToString();
            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            if (!WaypointList.Contains(name))
                WaypointList.Add(name);

            WaypointListUpdated();
        }

        private void tsmi_JT_Avoid_Click(object sender, EventArgs e)
        {
            string name;
            int sInd;

            if (AvoidList == null)
                AvoidList = new ArrayList();

            if (dgv_JumpTo.SelectedRows.Count != 1)
                return;

            if (dgv_JumpTo.SelectedRows[0].Cells[0].Value == null)
                return;

            name = dgv_JumpTo.SelectedRows[0].Cells[0].Value.ToString();
            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            if (!AvoidList.Contains(name))
                AvoidList.Add(name);

            AvoidListUpdated();
        }

        private void tmsi_JF_Zoom_Click(object sender, EventArgs e)
        {
            string name;
            int sInd;

            if (dgv_JumpFrom.SelectedRows.Count != 1)
                return;

            if (dgv_JumpFrom.SelectedRows[0].Cells[0].Value == null)
                return;

            name = dgv_JumpFrom.SelectedRows[0].Cells[0].Value.ToString();
            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            MainMapView.SetMapScale = 0.9f;
            MainMapView.SearchSelectedSystem(name);
        }

        private void tsmi_JF_WP_Click(object sender, EventArgs e)
        {
            string name;
            int sInd;

            if (WaypointList == null)
                WaypointList = new ArrayList();

            if (dgv_JumpFrom.SelectedRows.Count != 1)
                return;

            if (dgv_JumpFrom.SelectedRows[0].Cells[0].Value == null)
                return;

            name = dgv_JumpFrom.SelectedRows[0].Cells[0].Value.ToString();
            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            if (!WaypointList.Contains(name))
                WaypointList.Add(name);

            WaypointListUpdated();
        }

        private void tsmi_JF_Avoid_Click(object sender, EventArgs e)
        {
            string name;
            int sInd;

            if (AvoidList == null)
                AvoidList = new ArrayList();

            if (dgv_JumpFrom.SelectedRows.Count != 1)
                return;

            if (dgv_JumpFrom.SelectedRows[0].Cells[0].Value == null)
                return;

            name = dgv_JumpFrom.SelectedRows[0].Cells[0].Value.ToString();
            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            if (!AvoidList.Contains(name))
                AvoidList.Add(name);

            AvoidListUpdated();
        }

         private void tsmi_RemoveAvoid_Click(object sender, EventArgs e)
        {
            AvoidList.Remove(lb_AvoidList.SelectedItem);

            AvoidListUpdated();
        }

        private void tsmi_ClearWaypointList_Click(object sender, EventArgs e)
        {
            if (WaypointList == null)
                WaypointList = new ArrayList();
            WaypointList.Clear();
            WaypointListUpdated();
        }

        private void tsmi_ClearAvoidList_Click(object sender, EventArgs e)
        {
            if (AvoidList == null)
                AvoidList = new ArrayList();
            AvoidList.Clear();
            AvoidListUpdated();
        }

        private void tsmi_MoveSystemUp_Click(object sender, EventArgs e)
        {
            int index;
            string selItem;

            index = lb_WaypointList.SelectedIndex;

            if (index < 1)
                return;

            selItem = lb_WaypointList.SelectedItem.ToString();

            WaypointList.Remove(lb_WaypointList.SelectedItem);
            WaypointList.Insert(index - 1, selItem);

            WaypointListUpdated();
        }

        private void tsmi_MoveSystemDown_Click(object sender, EventArgs e)
        {
            int index;
            string selItem;

            index = lb_WaypointList.SelectedIndex;

            if ((index >= (WaypointList.Count - 1)) || (index < 0))
                return;

            selItem = lb_WaypointList.SelectedItem.ToString();

            WaypointList.Remove(lb_WaypointList.SelectedItem);
            WaypointList.Insert(index + 1, selItem);

            WaypointListUpdated();
        }

        private void tsmi_RemoveWaypoint_Click(object sender, EventArgs e)
        {
            WaypointList.Remove(lb_WaypointList.SelectedItem);

            WaypointListUpdated();
        }

        private void tsmi_SRJumpToSystem_Click(object sender, EventArgs e)
        {
            string name = "", search;
            int sInd;

            if (dgv_SearchResult.SelectedRows.Count != 1)
                return;

            // Seach Type Selection
            search = cb_SSSearchFor.Text;

            // Set up visible fields and options
            switch (search)
            {
                case "Agents":
                    name = dgv_SearchResult.SelectedRows[0].Cells[14].Value.ToString();
                    break;
                case "Ore Type(s)":
                    name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                case "Systems":
                    name = dgv_SearchResult.SelectedRows[0].Cells[2].Value.ToString();
                    break;
                case "Ice Type(s)":
                    name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                case "Stations":
                    name = dgv_SearchResult.SelectedRows[0].Cells[7].Value.ToString();
                    break;
                case "Planets":
                    name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                default:
                    return;
            }

            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            tb_DestSystem.Text = name;
            tb_StartSystem.Text = cb_SystemSelect.Text;
            RouteStart = cb_SystemSelect.Text;
            RouteEnd = name;

            ActiveRoute = 1;
            BuildAndCalcRoute(true);

            tc_RouteSeach.SelectedTab = tp_JumpRoute;
        }

        private void tsmi_SRGateToSystem_Click(object sender, EventArgs e)
        {
            string name = "";
            int sInd;
            string search;

            if (dgv_SearchResult.SelectedRows.Count != 1)
                return;

            // Seach Type Selection
            search = cb_SSSearchFor.Text;

            // Set up visible fields and options
            switch (search)
            {
                case "Agents":
                        name = dgv_SearchResult.SelectedRows[0].Cells[14].Value.ToString();
                    break;
                case "Ore Type(s)":
                        name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                case "Systems":
                        name = dgv_SearchResult.SelectedRows[0].Cells[2].Value.ToString();
                    break;
                case "Ice Type(s)":
                        name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                case "Stations":
                        name = dgv_SearchResult.SelectedRows[0].Cells[7].Value.ToString();
                    break;
                case "Planets":
                        name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                default:
                    return;
            }
         
            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            tb_DestSystem.Text = name;
            tb_StartSystem.Text = cb_SystemSelect.Text;
            RouteStart = cb_SystemSelect.Text;
            RouteEnd = name;

            ActiveRoute = 2;
            BuildAndCalcRoute(false);

            tc_RouteSeach.SelectedTab = tp_GateRoute;
        }

        private void setSystemAsWaypointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "", search;
            int sInd;

            if (dgv_SearchResult.SelectedRows.Count != 1)
                return;

            // Seach Type Selection
            search = cb_SSSearchFor.Text;

            // Set up visible fields and options
            switch (search)
            {
                case "Agents":
                    name = dgv_SearchResult.SelectedRows[0].Cells[14].Value.ToString();
                    break;
                case "Ore Type(s)":
                    name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                case "Systems":
                    name = dgv_SearchResult.SelectedRows[0].Cells[2].Value.ToString();
                    break;
                case "Ice Type(s)":
                    name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                case "Stations":
                    name = dgv_SearchResult.SelectedRows[0].Cells[7].Value.ToString();
                    break;
                case "Planets":
                    name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                default:
                    return;
            }

            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            if (!WaypointList.Contains(name))
                WaypointList.Add(name);

            WaypointListUpdated();
        }

        private void tsmi_SRFindSystem_Click(object sender, EventArgs e)
        {
            string name = "", search;
            int sInd;

            if (dgv_SearchResult.SelectedRows.Count != 1)
                return;

            // Seach Type Selection
            search = cb_SSSearchFor.Text;

            // Set up visible fields and options
            switch (search)
            {
                case "Agents":
                    name = dgv_SearchResult.SelectedRows[0].Cells[14].Value.ToString();
                    break;
                case "Ore Type(s)":
                    name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                case "Systems":
                    name = dgv_SearchResult.SelectedRows[0].Cells[2].Value.ToString();
                    break;
                case "Ice Type(s)":
                    name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                case "Stations":
                    name = dgv_SearchResult.SelectedRows[0].Cells[7].Value.ToString();
                    break;
                case "Planets":
                    name = dgv_SearchResult.SelectedRows[0].Cells[4].Value.ToString();
                    break;
                default:
                    return;
            }        
            
            if (name.Contains("("))
            {
                sInd = name.IndexOf('(');
                name = name.Substring(0, (sInd - 1));
            }
            if (name.Contains("{"))
            {
                sInd = name.IndexOf('{');
                name = name.Substring(0, (sInd - 1));
            }

            MainMapView.SetMapScale = PlugInData.Config.Zooms["Zoom On Search"];
            MainMapView.SearchSelectedSystem(name);
        }

        #endregion

        #region Jump Bridge Handler

        private void cb_FromSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            SolarSystem sel;
            SystemCelestials SysDat = new SystemCelestials();
            // Determine From Moons avaialable and populate moons
            sel = PlugInData.GalMap.GetSystemByName(cb_FromSystem.Text);
            SysDat.GetSystemCelestial(sel);

            if (sel != null && sel.Name != "")
            {
                cb_FromMoon.Items.Clear();
                foreach (Moon m in SysDat.Moons.Values)
                {
                    cb_FromMoon.Items.Add(m.Name);
                }
            }
        }

        private void cb_ToSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            SolarSystem sel;
            SystemCelestials SysDat = new SystemCelestials();
            // Determine From Moons avaialable and populate moons
            sel = PlugInData.GalMap.GetSystemByName(cb_ToSystem.Text);
            SysDat.GetSystemCelestial(sel);

            if (sel != null && sel.Name != "")
            {
                cb_ToMoon.Items.Clear();
                foreach (Moon m in SysDat.Moons.Values)
                {
                    cb_ToMoon.Items.Add(m.Name);
                }
            }
        }

        private void b_ClearJBList_Click(object sender, EventArgs e)
        {
           DialogResult dr = MessageBox.Show("Are you sure that you want to clear all of the entered Jump Bridges?", "Clear ALL Jump Bridges", MessageBoxButtons.YesNo);

           if (dr == DialogResult.Yes)
           {
               PlugInData.JumpBridgeList.Clear();
               PlugInData.SaveJBCynoToDisk();
               PopulateJBView(null);
               MainMapView.UpdateMapForChange();
               MainMapView.UpdateMapForChange();
           }
        }

        private void b_AddJumpBridge_Click(object sender, EventArgs e)
        {
            string fromS, fromM, toS, toM;
            string ally, pWord;
            bool Online;
            SolarSystem From, To;
            JumpBridge newJB;

            fromS = cb_FromSystem.Text;
            toS = cb_ToSystem.Text;
            fromM = cb_FromMoon.Text;
            toM = cb_ToMoon.Text;
            ally = cb_JBAlliance.Text;
            pWord = tb_JBPassword.Text;
            Online = rb_JBOnline.Checked;

            if ((fromS == "") || (fromM == "") || (toS == "") || (toM == "") || (ally == "") || (pWord == ""))
            {
                MessageBox.Show("You need to select From System, From Moon, To System, To Moon, Alliance, and Password to add a Jump Bridge.", "Input Missing");
                return;
            }

            From = PlugInData.GalMap.GetSystemByName(fromS);
            To = PlugInData.GalMap.GetSystemByName(toS);

            if (From.GetJumpDistance(To) > 5.0)
            {
                MessageBox.Show("You cannot have a bridge Link farther than 5LY.", "Invalid JB Link");
                return;
            }

            if (!cbx_PreserveData.Checked)
            {
                cb_FromSystem.Text = "";
                cb_FromMoon.Text = "";
                cb_ToSystem.Text = "";
                cb_ToMoon.Text = "";
            }

            if (selBridge != null)
            {
                newJB = new JumpBridge(From, To, fromM, toM, pWord, ally, PlugInData.Config.MapColors["Gate Route"], From.GetJumpDistance(To), Online);
                UpdateJumpBridge(selBridge, newJB);
                PopulateJBView(null);
            }
            else
            {

                if (PlugInData.JumpBridgeList.ContainsKey(From.ID))
                {
                    newJB = new JumpBridge(From, To, fromM, toM, pWord, ally, PlugInData.Config.MapColors["Gate Route"], From.GetJumpDistance(To), Online);
                    PlugInData.JumpBridgeList[From.ID].Add(fromM, newJB);
                    PopulateJBView(newJB);
                }
                else if (PlugInData.JumpBridgeList.ContainsKey(To.ID))
                {
                    newJB = new JumpBridge(From, To, fromM, toM, pWord, ally, PlugInData.Config.MapColors["Gate Route"], From.GetJumpDistance(To), Online);
                    PlugInData.JumpBridgeList[To.ID].Add(fromM, newJB);
                    PopulateJBView(null);
                }
                else
                {
                    newJB = new JumpBridge(From, To, fromM, toM, pWord, ally, PlugInData.Config.MapColors["Gate Route"], From.GetJumpDistance(To), Online);
                    PlugInData.JumpBridgeList.Add(From.ID, new SortedList<string, JumpBridge>());
                    PlugInData.JumpBridgeList[From.ID].Add(fromM, newJB);
                    PopulateJBView(newJB);
                }
            }

            PlugInData.SaveJBCynoToDisk();
            MainMapView.UpdateMapForChange();
            UpdateSystemCelestialsForInformation("JB", newJB);
        }

        private void UpdateSystemCelestialsForInformation(string type, JumpBridge njb)
        {
            int selID;
            SystemCelestials SC = new SystemCelestials();

            switch (type)
            {
                case "JB":
                    // Get celestials for From System
                    SC.GetSystemCelestial(njb.From);
                    selID = SC.GetMoonIDForName(njb.FromMoon);
                    if (SC.Moons.ContainsKey(selID))
                    {
                        
                    }
                    break;
                default:
                    break;
            }
        }

        private void tmsi_JBRemove_Click(object sender, EventArgs e)
        {
            if (dgv_Bridges.CurrentRow == null)
                return;

            if (selBridge != null)
            {
                RemoveJumpBridge(selBridge);
            }
        }

        public void RemoveJumpBridge(JumpBridge j)
        {
            if (PlugInData.JumpBridgeList.ContainsKey(j.From.ID))
            {
                if (PlugInData.JumpBridgeList[j.From.ID].ContainsKey(j.FromMoon))
                {
                    PlugInData.JumpBridgeList[j.From.ID].Remove(j.FromMoon);

                    PlugInData.SaveJBCynoToDisk();
                    PopulateJBView(null);
                    MainMapView.UpdateMapForChange();

                    return;
                }
                else if (PlugInData.JumpBridgeList[j.From.ID].ContainsKey(j.ToMoon))
                {
                    PlugInData.JumpBridgeList[j.From.ID].Remove(j.ToMoon);

                    PlugInData.SaveJBCynoToDisk();
                    PopulateJBView(null);
                    MainMapView.UpdateMapForChange();

                    return;
                }
            }
            
            if (PlugInData.JumpBridgeList.ContainsKey(j.To.ID))
            {
                if (PlugInData.JumpBridgeList[j.To.ID].ContainsKey(j.FromMoon))
                {
                    PlugInData.JumpBridgeList[j.To.ID].Remove(j.FromMoon);

                    PlugInData.SaveJBCynoToDisk();
                    PopulateJBView(null);
                    MainMapView.UpdateMapForChange();

                    return;
                }
                else if (PlugInData.JumpBridgeList[j.To.ID].ContainsKey(j.ToMoon))
                {
                    PlugInData.JumpBridgeList[j.To.ID].Remove(j.ToMoon);

                    PlugInData.SaveJBCynoToDisk();
                    PopulateJBView(null);
                    MainMapView.UpdateMapForChange();

                    return;
                }
            }
        }

        public void UpdateJumpBridge(JumpBridge j, JumpBridge nj)
        {
            if (PlugInData.JumpBridgeList.ContainsKey(j.From.ID))
            {
                if (PlugInData.JumpBridgeList[j.From.ID].ContainsKey(j.FromMoon))
                {
                    PlugInData.JumpBridgeList[j.From.ID].Remove(j.FromMoon);
                    PlugInData.JumpBridgeList[j.From.ID].Add(nj.FromMoon, nj);

                    PlugInData.SaveJBCynoToDisk();
                    MainMapView.UpdateMapForChange();

                    return;
                }
                else if (PlugInData.JumpBridgeList[j.From.ID].ContainsKey(j.ToMoon))
                {
                    PlugInData.JumpBridgeList[j.From.ID].Remove(j.ToMoon);
                    PlugInData.JumpBridgeList[j.From.ID].Add(nj.FromMoon, nj);

                    PlugInData.SaveJBCynoToDisk();
                    MainMapView.UpdateMapForChange();

                    return;
                }
            }
            
            if (PlugInData.JumpBridgeList.ContainsKey(j.To.ID))
            {
                if (PlugInData.JumpBridgeList[j.To.ID].ContainsKey(j.FromMoon))
                {
                    PlugInData.JumpBridgeList[j.To.ID].Remove(j.FromMoon);
                    PlugInData.JumpBridgeList[j.To.ID].Add(nj.FromMoon, nj);

                    PlugInData.SaveJBCynoToDisk();
                    MainMapView.UpdateMapForChange();

                    return;
                }
                else if (PlugInData.JumpBridgeList[j.To.ID].ContainsKey(j.ToMoon))
                {
                    PlugInData.JumpBridgeList[j.To.ID].Remove(j.ToMoon);
                    PlugInData.JumpBridgeList[j.To.ID].Add(nj.FromMoon, nj);

                    PlugInData.SaveJBCynoToDisk();
                    MainMapView.UpdateMapForChange();

                    return;
                }
            }
        }

        private void dgv_Bridges_SelectionChanged(object sender, EventArgs e)
        {
            long fID, tID;
            string fromM, toM;

            if (dgv_Bridges.CurrentRow == null)
                return;

            selBridge = null;

            fID = Convert.ToInt64(dgv_Bridges.CurrentRow.Cells[6].Value);
            tID = Convert.ToInt64(dgv_Bridges.CurrentRow.Cells[7].Value);
            if ((dgv_Bridges.Rows.Count <= 0) || (dgv_Bridges.CurrentRow.Cells[0].Value == null))
                return;

            fromM = dgv_Bridges.CurrentRow.Cells[0].Value.ToString();
            toM = dgv_Bridges.CurrentRow.Cells[1].Value.ToString();

            if (PlugInData.JumpBridgeList.ContainsKey(fID))
            {
                if (PlugInData.JumpBridgeList[fID].ContainsKey(fromM))
                {
                    selBridge = PlugInData.JumpBridgeList[fID][fromM];
                    cb_FromSystem.Text = selBridge.From.Name;
                    cb_FromMoon.Text = selBridge.FromMoon;
                    cb_ToMoon.Text = selBridge.ToMoon;
                    cb_ToSystem.Text = selBridge.To.Name;
                    tb_JBPassword.Text = selBridge.Password;
                    cb_JBAlliance.Text = selBridge.AllianceName;
                    if (selBridge.Online)
                        rb_JBOnline.Checked = true;
                    else
                        rb_JBOffline.Checked = true;

                    return;
                }
                else if (PlugInData.JumpBridgeList[fID].ContainsKey(toM))
                {
                    selBridge = PlugInData.JumpBridgeList[fID][toM];
                    cb_FromSystem.Text = selBridge.From.Name;
                    cb_FromMoon.Text = selBridge.FromMoon;
                    cb_ToMoon.Text = selBridge.ToMoon;
                    cb_ToSystem.Text = selBridge.To.Name;
                    tb_JBPassword.Text = selBridge.Password;
                    cb_JBAlliance.Text = selBridge.AllianceName;
                    if (selBridge.Online)
                        rb_JBOnline.Checked = true;
                    else
                        rb_JBOffline.Checked = true;

                    return;
                }
            }
            
            if (PlugInData.JumpBridgeList.ContainsKey(tID))
            {
                if (PlugInData.JumpBridgeList[tID].ContainsKey(fromM))
                {
                    selBridge = PlugInData.JumpBridgeList[tID][fromM];
                    cb_FromSystem.Text = selBridge.From.Name;
                    cb_FromMoon.Text = selBridge.FromMoon;
                    cb_ToMoon.Text = selBridge.ToMoon;
                    cb_ToSystem.Text = selBridge.To.Name;
                    tb_JBPassword.Text = selBridge.Password;
                    cb_JBAlliance.Text = selBridge.AllianceName;
                    if (selBridge.Online)
                        rb_JBOnline.Checked = true;
                    else
                        rb_JBOffline.Checked = true;

                    return;
                }
                if (PlugInData.JumpBridgeList[tID].ContainsKey(toM))
                {
                    selBridge = PlugInData.JumpBridgeList[tID][toM];
                    cb_FromSystem.Text = selBridge.From.Name;
                    cb_FromMoon.Text = selBridge.FromMoon;
                    cb_ToMoon.Text = selBridge.ToMoon;
                    cb_ToSystem.Text = selBridge.To.Name;
                    tb_JBPassword.Text = selBridge.Password;
                    cb_JBAlliance.Text = selBridge.AllianceName;
                    if (selBridge.Online)
                        rb_JBOnline.Checked = true;
                    else
                        rb_JBOffline.Checked = true;

                    return;
                }
            }
        }

        private void PopulateJBView(JumpBridge njb)
        {
            int num = 0;
            selBridge = null;

            if (njb != null)
            {
                num = dgv_Bridges.Rows.Add();
                dgv_Bridges[0, num].Value = njb.FromMoon;
                dgv_Bridges[1, num].Value = njb.ToMoon;
                dgv_Bridges[2, num].Value = Math.Round(njb.Distance, 2) + " ly";
                dgv_Bridges[3, num].Value = njb.Password;
                dgv_Bridges[4, num].Value = njb.AllianceName;
                if (njb.Online)
                    dgv_Bridges[5, num].Value = "Online";
                else
                    dgv_Bridges[5, num].Value = "Offline";
                dgv_Bridges[6, num].Value = njb.From.ID;
                dgv_Bridges[7, num].Value = njb.To.ID;
                dgv_Bridges.ClearSelection();
            }
            else
            {
                dgv_Bridges.Rows.Clear();
                foreach (var sjb in PlugInData.JumpBridgeList.Values)
                {
                    foreach (JumpBridge jb in sjb.Values)
                    {
                        num = dgv_Bridges.Rows.Add();
                        dgv_Bridges[0, num].Value = jb.FromMoon;
                        dgv_Bridges[1, num].Value = jb.ToMoon;
                        dgv_Bridges[2, num].Value = Math.Round(jb.Distance, 2) + " ly";
                        dgv_Bridges[3, num].Value = jb.Password;
                        dgv_Bridges[4, num].Value = jb.AllianceName;
                        if (jb.Online)
                            dgv_Bridges[5, num].Value = "Online";
                        else
                            dgv_Bridges[5, num].Value = "Offline";
                        dgv_Bridges[6, num].Value = jb.From.ID;
                        dgv_Bridges[7, num].Value = jb.To.ID;
                    }
                }
            }
        }

        private void b_ExportJB_Click(object sender, EventArgs e)
        {
            ExportCynoGenJB();
        }

        private void b_ImportJB_Click(object sender, EventArgs e)
        {
            ImportCynoGenJB();
        }

        private void b_ImportCyno_Click(object sender, EventArgs e)
        {
            ImportCynoGenJB();
        }

        private void b_ExportCyno_Click(object sender, EventArgs e)
        {
            ExportCynoGenJB();
        }

        private void bi_DotLanImport_Click(object sender, EventArgs e)
        {
            string[] jbs;
            string[] jb;
            string[] pm;
            string pmStr;

            SolarSystem From, To;
            JumpBridge newJB;

            // Text Import for Jump Bridge List from DotLan.net
            JB_Import ID;
            ID = new JB_Import("DotLan JB Import");

            ID.ShowDialog();

            if (PlugInData.ImportText.Length < 1)
                return;

            jbs = PlugInData.ImportText.Split('\n');

            foreach (string s in jbs)
            {
                newJB = new JumpBridge();

                jb = s.Split(' ');
                From = PlugInData.GalMap.GetSystemByName(jb[0]);
                To = PlugInData.GalMap.GetSystemByName(jb[5]);

                pm = jb[1].Split('-');
                pmStr = jb[0] + " ";
                pmStr += PlugInData.ToRoman(Convert.ToInt32(pm[0])) + " - Moon " + pm[1];

                newJB.From = From;
                newJB.To = To;
                newJB.FromMoon = pmStr;

                pm = jb[6].Split('-');
                pmStr = jb[5] + " ";
                pmStr += PlugInData.ToRoman(Convert.ToInt32(pm[0])) + " - Moon " + pm[1];

                newJB.ToMoon = pmStr;

                newJB.Password = jb[7];
                newJB.Online = true;

                newJB.Distance = From.GetJumpDistance(To);

                int alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[From.ID].allianceID;
                if (alID != 0)
                    newJB.AllianceName = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name;
                else
                {
                    alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[To.ID].allianceID;
                    if (alID != 0)
                        newJB.AllianceName = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name;
                }

                if (PlugInData.JumpBridgeList.ContainsKey(From.ID))
                {
                    if (!PlugInData.JumpBridgeList[From.ID].ContainsKey(newJB.FromMoon))
                        PlugInData.JumpBridgeList[From.ID].Add(newJB.FromMoon, newJB);
                }
                else
                {
                    PlugInData.JumpBridgeList.Add(From.ID, new SortedList<string, JumpBridge>());
                    PlugInData.JumpBridgeList[From.ID].Add(newJB.FromMoon, newJB);
                }
            }

            PlugInData.SaveJBCynoToDisk();
            PopulateJBView(null);
            MainMapView.UpdateMapForChange();
        }

        private void bi_ExportDotLanJB_Click(object sender, EventArgs e)
        {
            // Text Export for Jump Bridge & Cyno List *.csv
            string line, export;

            if (PlugInData.JumpBridgeList.Count <= 0)
            {
                MessageBox.Show("No Jump Bridges Exist.", "Input Missing");
                return;
            }

            export = "";
            foreach (var sjb in PlugInData.JumpBridgeList.Values)
            {
                foreach (JumpBridge jb in sjb.Values)
                {
                    line = jb.From.Name + " ";
                    line += PlugInData.GetPlanetMoonNumbersFromName(jb.FromMoon) + " --> ";
                    line += jb.To.Name + " ";
                    line += PlugInData.GetPlanetMoonNumbersFromName(jb.ToMoon) + " ";
                    line += jb.Password;

                    if (export.Length > 0)
                        export += "\n";

                    export += line;
                }
            }
            Clipboard.SetText(export);
        }

        private void ImportCynoGenJB()
        {
            // Text Import for Jump Bridge & Cyno List *.csv
            string line;
            string[] data;
            StreamReader SR;
            string fname;
            SolarSystem From, To;
            int fID, tID;
            JumpBridge newJB;
            CynoGenJam cg;
            bool ol = false;

            ofd_ImportFile.InitialDirectory = PlugInData.RMapExport_Path;
            ofd_ImportFile.Filter = "JB_Cyno (*.csv)|*.csv";
            ofd_ImportFile.FilterIndex = 0;
            ofd_ImportFile.ShowDialog();

            fname = ofd_ImportFile.FileName;

            if (File.Exists(fname))
                SR = File.OpenText(fname);
            else
                return;

            while ((line = SR.ReadLine()) != null)
            {
                data = line.Split(',');
                if (data[0].Equals("Bridge"))
                {
                    // Importing a Jump Bridge
                    fID = Convert.ToInt32(data[1]);
                    tID = Convert.ToInt32(data[2]);
                    From = PlugInData.GalMap.GetSystemByID(fID);
                    To = PlugInData.GalMap.GetSystemByID(tID);

                    if (data[7].Equals("true"))
                        ol = true;

                    if (PlugInData.JumpBridgeList.ContainsKey(fID))
                    {
                        if (!PlugInData.JumpBridgeList[From.ID].ContainsKey(data[4]))
                        {
                            newJB = new JumpBridge(To, From, data[4], data[3], data[5], data[6], PlugInData.Config.MapColors["Gate Route"], To.GetJumpDistance(From), ol);
                            PlugInData.JumpBridgeList[From.ID].Add(data[4], newJB);
                        }
                    }
                    else
                    {
                        newJB = new JumpBridge(To, From, data[4], data[3], data[5], data[6], PlugInData.Config.MapColors["Gate Route"], To.GetJumpDistance(From), ol);
                        PlugInData.JumpBridgeList.Add(From.ID, new SortedList<string, JumpBridge>());
                        PlugInData.JumpBridgeList[From.ID].Add(data[4], newJB);
                    }

                }
                else
                {
                    fID = Convert.ToInt32(data[1]);
                    From = PlugInData.GalMap.GetSystemByID(fID);

                    // Importing a Cyno Gen or Cyno Jammer
                    if (data[0].Contains("Gen"))
                        cg = new CynoGenJam(From, data[2], false, data[3]);
                    else
                        cg = new CynoGenJam(From, data[2], true, data[3]);

                    if (!PlugInData.CynoGenJamList.ContainsKey(fID))
                        PlugInData.CynoGenJamList.Add(fID, cg);
                }
            }

            SR.Close();
            PlugInData.SaveJBCynoToDisk();
            PopulateJBView(null);
            PopulateCynoList();
            MainMapView.UpdateMapForChange();
        }

        private void ExportCynoGenJB()
        {
            // Text Export for Jump Bridge & Cyno List *.csv
            string line;
            StreamWriter SW;
            string fname;

            if ((PlugInData.JumpBridgeList.Count <= 0) && (PlugInData.CynoGenJamList.Count <= 0))
            {
                MessageBox.Show("No Jump Bridges, Cyno Generators or Cyno Jammers Exist.", "Input Missing");
                return;
            }

            sfd_ExportSelect.InitialDirectory = PlugInData.RMapExport_Path;
            sfd_ExportSelect.Filter = "JB_Cyno (*.csv)|*.csv";
            sfd_ExportSelect.FilterIndex = 0;
            sfd_ExportSelect.ShowDialog();

            fname = sfd_ExportSelect.FileName;

            SW = File.CreateText(fname);

            foreach (var sjb in PlugInData.JumpBridgeList.Values)
            {
                foreach (JumpBridge jb in sjb.Values)
                {
                    line = "Bridge,";
                    line += jb.From.ID + ",";
                    line += jb.To.ID + ",";
                    line += jb.FromMoon + ",";
                    line += jb.ToMoon + ",";
                    line += jb.Password + ",";
                    line += jb.AllianceName + ",";
                    line += jb.Distance + ",";
                    line += jb.CorpName + ",";

                    if (jb.Online)
                        line += "true";
                    else
                        line += "false";

                    SW.WriteLine(line);
                }
            }

            foreach (CynoGenJam cgj in PlugInData.CynoGenJamList.Values)
            {
                if (cgj.IsJammer)
                    line = "Cyno Jam,";
                else
                    line = "Cyno Gen,";

                line += cgj.systemID + ",";
                line += cgj.moon + ",";
                line += cgj.Note;

                SW.WriteLine(line);
            }

            SW.Close();
        }

        private void cb_CynoSystemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SolarSystem sel;
            SystemCelestials SysDat = new SystemCelestials();
            // Determine From Moons avaialable and populate moons
            sel = PlugInData.GalMap.GetSystemByName(cb_CynoSystemName.Text);
            SysDat.GetSystemCelestial(sel);

            if (sel != null && sel.Name != "")
            {
                cb_CynoSystemMoon.Items.Clear();
                cb_CynoSystemMoon.Items.Add("Unknown");
                foreach (Moon m in SysDat.Moons.Values)
                {
                    cb_CynoSystemMoon.Items.Add(m.Name);
                }
                cb_CynoSystemMoon.SelectedIndex = 0;
            }
        }

        private void b_AddCyno_Click(object sender, EventArgs e)
        {
            string sName, moon;
            SolarSystem sel;
            CynoGenJam cgj;

            sName = cb_CynoSystemName.Text;
            moon = cb_CynoSystemMoon.Text;

            if ((sName == "") || (moon == ""))
            {
                MessageBox.Show("You need to select System and Moon to add a Cyno.", "Input Missing");
                return;
            }

            sel = PlugInData.GalMap.GetSystemByName(cb_CynoSystemName.Text);

            cgj = new CynoGenJam(sel, moon, rb_CynoJam.Checked, "");

            if (!PlugInData.CynoGenJamList.ContainsKey(sel.ID))
                PlugInData.CynoGenJamList.Add(sel.ID, cgj);
            PlugInData.SaveJBCynoToDisk();
            PopulateCynoList();
            MainMapView.UpdateMapForChange();
        }

        public void UpdateCynoGen(CynoGenJam c)
        {
            if (PlugInData.CynoGenJamList.ContainsKey(c.systemID))
            {
                PlugInData.CynoGenJamList[c.systemID] = c;
                PopulateCynoList();
                PlugInData.SaveJBCynoToDisk();
                MainMapView.UpdateMapForChange();
            }
        }

        private void b_ClearAllCyno_Click(object sender, EventArgs e)
        {
            PlugInData.CynoGenJamList.Clear();
            PlugInData.SaveJBCynoToDisk();
            PopulateCynoList();
            MainMapView.UpdateMapForChange();
        }

        private void PopulateCynoList()
        {
            CynoGJ C;
            int num = 0, xPt = 0;
            int nWid = 1, widC = 0;
            double wid;

            wid = gp_Cynos.Width;
            nWid = Convert.ToInt32(Math.Floor(wid / 410));

            if (nWid < 1)
                nWid = 1;

            gp_Cynos.Controls.Clear();
            // Populate Computed Route Display
            foreach (CynoGenJam cgj in PlugInData.CynoGenJamList.Values)
            {
                C = new CynoGJ(this, cgj);

                xPt = widC * 410;
                widC++;
                C.Location = new Point(xPt, (num * 44));
                gp_Cynos.Controls.Add(C);

                if (widC >= nWid)
                {
                    xPt = 0;
                    widC = 0;
                    num++;
                }
            }
        }

        public void RemoveCynoGen(CynoGenJam c)
        {
            if (PlugInData.CynoGenJamList.ContainsKey(c.systemID))
            {
                PlugInData.CynoGenJamList.Remove(c.systemID);
            }
            PlugInData.SaveJBCynoToDisk();
            PopulateCynoList();
        }

        private void gp_Cynos_SizeChanged(object sender, EventArgs e)
        {
            PopulateCynoList();
        }

        #endregion

        #region System Search Handler

        private void cb_SSSearchFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string search;

            // Seach Type Selection
            search = cb_SSSearchFor.Text;
            
            // Set up visible fields and options
            switch (search)
            {
                case "Agents":
                    lb_Select1.Text = "Agent Faction:";
                    lb_Select1.Visible = true;
                    cb_Select1.Visible = true;
                    cb_Select1.Items.Clear();
                    cb_Select1.Items.Add("All");
                    cb_Select1.Items.AddRange(PlugInData.FactionList.ToArray());
                    cb_Select1.Text = "All";

                    lb_Select2.Text = "System Faction:";
                    lb_Select2.Visible = true;
                    cb_Select2.Visible = true;
                    cb_Select2.Items.Clear();
                    cb_Select2.Items.Add("All");
                    cb_Select2.Items.AddRange(PlugInData.FactionList.ToArray());
                    cb_Select2.Text = "All";

                    lb_Select3.Text = "Corporation:";
                    lb_Select3.Visible = true;
                    cb_Select3.Visible = true;
                    cb_Select3.Items.Clear();
                    cb_Select3.Items.Add("All");
                    cb_Select3.Items.AddRange(PlugInData.CorpList.ToArray());
                    cb_Select3.Text = "All";

                    lb_Select4.Text = "Division:";
                    lb_Select4.Visible = true;
                    cb_Select4.Visible = true;
                    cb_Select4.Items.Clear();
                    cb_Select4.Items.Add("All");
                    cb_Select4.Items.Add("Distribution");
                    cb_Select4.Items.Add("Mining");
                    cb_Select4.Items.Add("R&D");
                    cb_Select4.Items.Add("Security");
//                    cb_Select4.Items.AddRange(PlugInData.DivisionList.ToArray());
                    cb_Select4.Text = "All";

                    lb_Select5.Text = "Agent Type:";
                    lb_Select5.Visible = true;
                    cb_Select5.Visible = true;
                    cb_Select5.Items.Clear();
                    cb_Select5.Items.Add("All");
                    cb_Select5.Items.AddRange(PlugInData.GalMap.GalData.AgentTypes.ToArray());
                    cb_Select5.Text = "All";

                    lb_Select6.Text = "Research Type:";
                    lb_Select6.Visible = true;
                    cb_Select6.Visible = true;
                    cb_Select6.Items.Clear();
                    cb_Select6.Items.Add("All");
                    cb_Select6.Items.AddRange(PlugInData.GalMap.GalData.ResearchTypes.ToArray());
                    cb_Select6.Text = "All";

                    lb_Select7.Text = "Mission Type (>90% or Max %):";
                    cb_Select7.Items.Clear();
                    cb_Select7.Items.Add("All");
                    cb_Select7.Items.Add("Courier");
                    cb_Select7.Items.Add("Combat");
                    cb_Select7.Items.Add("Mining");
                    cb_Select7.Items.Add("Trade");
                    cb_Select7.Text = "All";
                    lb_Select7.Visible = false;
                    cb_Select7.Visible = false;

                    nud_TypePercent.Visible = false;
                    nud_TypePercent.Value = 90;

                    clb_SearchSelections.Items.Clear();
                    clb_SearchSelections.Items.Add("Agent Quality < 0", true);
                    clb_SearchSelections.Items.Add("Agent Quality > 0", true);
                    clb_SearchSelections.Items.Add("Agents Pilot Can Use", false);

                    clb_SearchSelections.Items.Add("Level 1 Agents", false);
                    clb_SearchSelections.Items.Add("Level 2 Agents", false);
                    clb_SearchSelections.Items.Add("Level 3 Agents", false);
                    clb_SearchSelections.Items.Add("Level 4 Agents", false);
                    clb_SearchSelections.Items.Add("Level 5 Agents", false);

                    clb_SearchSelections.Items.Add("Locator Agents", false);

                    cbx_SSSEmpire.Checked = true;
                    cbx_SSSLow.Checked = false;
                    cbx_SSSZero.Checked = false;

                    rb_Galaxy.Checked = true;
                    break;
                case "Ore Type(s)":
                    lb_Select1.Visible = false;
                    cb_Select1.Visible = false;
                    lb_Select2.Visible = false;
                    cb_Select2.Visible = false;
                    lb_Select3.Visible = false;
                    cb_Select3.Visible = false;
                    lb_Select4.Visible = false;
                    cb_Select4.Visible = false;
                    lb_Select5.Visible = false;
                    cb_Select5.Visible = false;
                    lb_Select6.Visible = false;
                    cb_Select6.Visible = false;
                    lb_Select7.Visible = false;
                    cb_Select7.Visible = false;
                    nud_TypePercent.Visible = false;

                    clb_SearchSelections.Items.Clear();
                    clb_SearchSelections.Items.AddRange(PlugInData.Misc.OreTypes.ToArray());

                    cbx_SSSEmpire.Checked = true;
                    cbx_SSSLow.Checked = false;
                    cbx_SSSZero.Checked = false;

                    rb_Galaxy.Checked = true;
                    break;
                case "Systems":
                    lb_Select1.Visible = false;
                    cb_Select1.Visible = false;
                    lb_Select2.Visible = false;
                    cb_Select2.Visible = false;
                    lb_Select3.Visible = false;
                    cb_Select3.Visible = false;
                    lb_Select4.Visible = false;
                    cb_Select4.Visible = false;
                    lb_Select5.Visible = false;
                    cb_Select5.Visible = false;
                    lb_Select6.Visible = false;
                    cb_Select6.Visible = false;
                    lb_Select7.Visible = false;
                    cb_Select7.Visible = false;
                    nud_TypePercent.Visible = false;

                    clb_SearchSelections.Items.Clear();

                    cbx_SSSEmpire.Checked = true;
                    cbx_SSSLow.Checked = false;
                    cbx_SSSZero.Checked = false;

                    rb_Galaxy.Checked = true;
                    break;
                case "Ice Type(s)":
                    lb_Select1.Visible = false;
                    cb_Select1.Visible = false;
                    lb_Select2.Visible = false;
                    cb_Select2.Visible = false;
                    lb_Select3.Visible = false;
                    cb_Select3.Visible = false;
                    lb_Select4.Visible = false;
                    cb_Select4.Visible = false;
                    lb_Select5.Visible = false;
                    cb_Select5.Visible = false;
                    lb_Select6.Visible = false;
                    cb_Select6.Visible = false;
                    lb_Select7.Visible = false;
                    cb_Select7.Visible = false;
                    nud_TypePercent.Visible = false;

                    clb_SearchSelections.Items.Clear();
                    clb_SearchSelections.Items.AddRange(PlugInData.Misc.IceTypes.ToArray());

                    cbx_SSSEmpire.Checked = true;
                    cbx_SSSLow.Checked = false;
                    cbx_SSSZero.Checked = false;

                    rb_Galaxy.Checked = true;
                    break;
                case "Stations":
                    lb_Select1.Text = "Station NPC Corp:";
                    lb_Select1.Visible = true;
                    cb_Select1.Visible = true;
                    cb_Select1.Items.Clear();
                    cb_Select1.Items.Add("All");
                    cb_Select1.Items.AddRange(PlugInData.CorpList.ToArray());
                    cb_Select1.Text = "All";

                    lb_Select2.Text = "Station Player Corp:";
                    lb_Select2.Visible = true;
                    cb_Select2.Visible = true;
                    cb_Select2.Items.Clear();
                    cb_Select2.Items.Add("All");
                    cb_Select2.Items.AddRange(PlugInData.CCorpList.ToArray());
                    cb_Select2.Text = "All";
                    lb_Select3.Visible = false;
                    cb_Select3.Visible = false;
                    lb_Select4.Visible = false;
                    cb_Select4.Visible = false;
                    lb_Select5.Visible = false;
                    cb_Select5.Visible = false;
                    lb_Select6.Visible = false;
                    cb_Select6.Visible = false;
                    lb_Select7.Visible = false;
                    cb_Select7.Visible = false;
                    nud_TypePercent.Visible = false;

                    clb_SearchSelections.Items.Clear();
                    clb_SearchSelections.Items.Add("Refining", false);
                    clb_SearchSelections.Items.Add("Reprocessing", false);
                    clb_SearchSelections.Items.Add("Cloning", false);
                    clb_SearchSelections.Items.Add("Fitting", false);
                    clb_SearchSelections.Items.Add("Factory", false);
                    clb_SearchSelections.Items.Add("Laboratory", false);
                    clb_SearchSelections.Items.Add("Loyalty Point Store", false);
                    clb_SearchSelections.Items.Add("Insurance", false);

                    cbx_SSSEmpire.Checked = true;
                    cbx_SSSLow.Checked = false;
                    cbx_SSSZero.Checked = false;

                    rb_Galaxy.Checked = true;
                    break;
                case "Planets":
                    lb_Select1.Text = "Sov Holder (Alliance)";
                    lb_Select1.Visible = true;
                    cb_Select1.Visible = true;
                    cb_Select1.Items.Clear();
                    cb_Select1.Items.Add("All");
                    cb_Select1.Items.AddRange(PlugInData.AllianceList.ToArray());
                    cb_Select1.Text = "All";

                    lb_Select2.Visible = false;
                    cb_Select2.Visible = false;
                    lb_Select3.Visible = false;
                    cb_Select3.Visible = false;
                    lb_Select4.Visible = false;
                    cb_Select4.Visible = false;
                    lb_Select5.Visible = false;
                    cb_Select5.Visible = false;
                    lb_Select6.Visible = false;
                    cb_Select6.Visible = false;
                    lb_Select7.Visible = false;
                    cb_Select7.Visible = false;
                    nud_TypePercent.Visible = false;

                    clb_SearchSelections.Items.Clear();
                    clb_SearchSelections.Items.Add("Barren");
                    clb_SearchSelections.Items.Add("Gas");
                    clb_SearchSelections.Items.Add("Ice");
                    clb_SearchSelections.Items.Add("Lava");
                    clb_SearchSelections.Items.Add("Oceanic");
                    clb_SearchSelections.Items.Add("Plasma");
                    clb_SearchSelections.Items.Add("Storm");
                    clb_SearchSelections.Items.Add("Temperate");

                    cbx_SSSEmpire.Checked = true;
                    cbx_SSSLow.Checked = false;
                    cbx_SSSZero.Checked = false;

                    rb_Galaxy.Checked = true;
                    break;
                default:
                    break;
            }
        }

        private void b_DoSearch_Click(object sender, EventArgs e)
        {
            // Actual Search Start
            string search;

            // Seach Type Selection
            search = cb_SSSearchFor.Text;

            // Set up visible fields and options
            switch (search)
            {
                case "Agents":
                    PerformAgentSearch();
                    break;
                case "Ore Type(s)":
                    PerformIceOreSearch(true);
                    break;
                case "Ice Type(s)":
                    PerformIceOreSearch(false);
                    break;
                case "Stations":
                    PerformStationSearch(false);
                    break;
                case "Planets":
                    PerformPlanetSearch();
                    break;
                case "Systems":
                    PerformSystemSearch();
                    break;
                default:
                    break;
            }

            MainMapView.UpdateMapForChange();
        }

        private void PerformPlanetSearch()
        {
            Dictionary<SolarSystem, Vertex> SystemTable;
            SystemCelestials SC = new SystemCelestials();
            List<Vertex> SystemRoute = new List<Vertex>();
            GateRoute SystemTableFinder = new GateRoute();
            Route GRoute;
            int range, alID;
            bool emp, low, zero;
            bool UsePlanet = false;
            ArrayList SearchItems = new ArrayList();
            string sysName, plnt, pType, planetList, SCorp, sovHolder = "";

            if (rb_Galaxy.Checked)
                range = 0;
            else if (rb_Region.Checked)
                range = 1;
            else if (rb_Const.Checked)
                range = 2;
            else if (rb_JumpRange.Checked)
                range = 4;
            else
                range = 3;

            emp = cbx_SSSEmpire.Checked;
            low = cbx_SSSLow.Checked;
            zero = cbx_SSSZero.Checked;
            SCorp = cb_Select1.Text;

            sysName = cb_SystemSelect.Text;
            if (sysName.Length < 4)
            {
                // Both starting and ending system is needed to compute a route
                MessageBox.Show("You must have a Search System Selected.", "Input Missing");
                return;
            }

            esp_SearchResult.Expanded = true;
            foreach (int checkedItem in clb_SearchSelections.CheckedIndices)
                SearchItems.Add(checkedItem.ToString());

            SolarSystem sSys = PlugInData.GalMap.GetSystemByName(sysName);

            GRoute = new Route(sSys, sSys, AvoidList, WaypointList, EvePilot, JumpShip, -1, 1);

            GRoute.ShipJumpRange = ShipJumpRange;
            GRoute.ShipFuelPerLY = ShipFuelPerLY;
            GRoute.UseJumpBridge = cbx_UseJumpBridge.Checked;

            //// Disable button to prevent double clicks
            b_DoSearch.Enabled = false;
            SystemTable = SystemTableFinder.GetRouteTable(GRoute);
            b_DoSearch.Enabled = true;
            ds_SearchResult.Tables["Planets"].Rows.Clear();

            foreach (Region r in PlugInData.GalMap.GalData.Regions.Values)
            {
                if ((range == 1) && (sSys.RegionID != r.ID))
                    continue;

                foreach (Constellation c in r.Constellations.Values)
                {
                    if ((range == 2) && (sSys.ConstID != c.ID))
                        continue;
                
                    foreach (SolarSystem s in c.Systems.Values)
                    {
                        UsePlanet = true;

                        if ((range == 3) && (sSys.ID != s.ID))
                        {
                            UsePlanet = false;
                            continue;
                        }

                        if (!emp && (s.Security > 0.45))
                            UsePlanet = false;
                        if (!low && ((s.Security < 0.45) && (s.Security > 0)))
                            UsePlanet = false;
                        if (!zero && (s.Security <= 0))
                            UsePlanet = false;

                        if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(s.ID))
                        {
                            alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[s.ID].allianceID;
                            if (alID != 0)
                                sovHolder = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name;
                            else
                                sovHolder = "";
                        }

                        if (!SCorp.Equals("All"))
                            if (!SCorp.Equals(sovHolder))
                                UsePlanet = false;

                        if (UsePlanet)
                        {
                            SC.GetSystemCelestial(s);

                            planetList = "";
                            foreach (Planet p in SC.Planets.Values)
                            {
                                plnt = p.GetPlanetTypeByGraphicID();
                                if (!planetList.Contains(plnt))
                                {
                                    if (planetList.Length < 1)
                                        planetList += plnt;
                                    else
                                        planetList += ", " + plnt;
                                }
                            }

                            foreach (string ort in SearchItems)
                            {
                                pType = clb_SearchSelections.Items[Convert.ToInt32(ort)].ToString();
                                if (!planetList.Contains(pType))
                                {
                                    UsePlanet = false;
                                    break;
                                }
                            }

                            if (!UsePlanet)
                                continue;

                            SystemRoute.Clear();
                            Vertex backTrack = SystemTable[s].Copy();
                            while (backTrack.SolarSystem != sSys)
                            {
                                //adding items to the front since were working backwards
                                SystemRoute.Insert(0, backTrack);
                                backTrack = SystemTable[backTrack.Path].Copy();
                            }

                            // If using a defined jump range, then filter here as well
                            if ((range == 4) && (SystemRoute.Count > (Convert.ToInt32(nud_SelectedRange.Value))))
                                continue;
                        
                            ds_SearchResult.Tables["Planets"].Rows.Add(SystemRoute.Count, Math.Round(s.Security, 2), r.Name, c.Name, s.Name, planetList, sovHolder);
                        }
                    }
                }
            }
            ActiveSearch = "Planets";
            dgv_SearchResult.DataSource = null;
            SortDV = ds_SearchResult.Tables["Planets"].DefaultView;
            for (int x = 0; x < dgv_SearchResult.Columns.Count; x++)
                dgv_SearchResult.Columns[x].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_SearchResult.DataSource = SortDV;
            SCols = new SortedList<int, int>();
        }

        private void PerformSystemSearch()
        {
            Dictionary<SolarSystem, Vertex> SystemTable;
            List<Vertex> SystemRoute = new List<Vertex>();
            GateRoute SystemTableFinder = new GateRoute();
            Route GRoute;
            int range, alID;
            bool emp, low, zero;
            bool UseOre = false;
            bool HasStation = false;
            ArrayList SearchItems = new ArrayList();
            string sysName, sovHolder = "";

            if (rb_Galaxy.Checked)
                range = 0;
            else if (rb_Region.Checked)
                range = 1;
            else if (rb_Const.Checked)
                range = 2;
            else if (rb_JumpRange.Checked)
                range = 4;
            else
                range = 3;

            emp = cbx_SSSEmpire.Checked;
            low = cbx_SSSLow.Checked;
            zero = cbx_SSSZero.Checked;

            sysName = cb_SystemSelect.Text;
            if (sysName.Length < 4)
            {
                // Both starting and ending system is needed to compute a route
                MessageBox.Show("You must have a Search System Selected.", "Input Missing");
                return;
            }

            esp_SearchResult.Expanded = true;
            foreach (int checkedItem in clb_SearchSelections.CheckedIndices)
                SearchItems.Add(checkedItem.ToString());

            SolarSystem sSys = PlugInData.GalMap.GetSystemByName(sysName);

            GRoute = new Route(sSys, sSys, AvoidList, WaypointList, EvePilot, JumpShip, -1, 1);

            GRoute.ShipJumpRange = ShipJumpRange;
            GRoute.ShipFuelPerLY = ShipFuelPerLY;
            GRoute.UseJumpBridge = cbx_UseJumpBridge.Checked;

            //// Disable button to prevent double clicks
            b_DoSearch.Enabled = false;
            SystemTable = SystemTableFinder.GetRouteTable(GRoute);
            b_DoSearch.Enabled = true;
            ds_SearchResult.Tables["Systems"].Rows.Clear();

            foreach (Region r in PlugInData.GalMap.GalData.Regions.Values)
            {
                foreach (Constellation c in r.Constellations.Values)
                {
                    foreach (SolarSystem s in c.Systems.Values)
                    {
                        UseOre = true;

                        switch (range)
                        {
                            case 0:         // Galaxy
                                break;
                            case 1:         // Region
                                if (sSys.RegionID != r.ID)
                                    UseOre = false;
                                break;
                            case 2:         // Constellation
                                if (sSys.ConstID != c.ID)
                                    UseOre = false;
                                break;
                            case 3:         // System
                                if (sSys.ID != s.ID)
                                    UseOre = false;
                                break;
                            default:        // DuH what?
                                break;
                        }

                        if (!emp && (s.Security > 0.45))
                            UseOre = false;
                        if (!low && ((s.Security < 0.45) && (s.Security > 0)))
                            UseOre = false;
                        if (!zero && (s.Security <= 0))
                            UseOre = false;
                        if (PlugInData.WHRegions.Contains(s.RegionID) || PlugInData.WHRegions.Contains(sSys.RegionID))
                            UseOre = false;
                       
                        if (UseOre)
                        {
                            SystemRoute.Clear();
                            Vertex backTrack = SystemTable[s].Copy();
                            while (backTrack.SolarSystem != sSys)
                            {
                                //adding items to the front since were working backwards
                                SystemRoute.Insert(0, backTrack);
                                backTrack = SystemTable[backTrack.Path].Copy();
                            }

                            // If using a defined jump range, then filter here as well
                            if ((range == 4) && (SystemRoute.Count > (Convert.ToInt32(nud_SelectedRange.Value))))
                                continue;

                            if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(s.ID))
                            {
                                alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[s.ID].allianceID;
                                if (alID != 0)
                                    sovHolder = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name + " [" + PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker + "]";
                                else
                                    sovHolder = "";
                            }

                            if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(s.ID))
                                HasStation = true;
                            else if (s.Stations.Count > 0)
                                HasStation = true;
                            else
                                HasStation = false;

                            ds_SearchResult.Tables["Systems"].Rows.Add(SystemRoute.Count, Math.Round(s.Security, 2), s.Name, r.Name, c.Name, HasStation, sovHolder);
                        }
                    }
                }
            }
            ActiveSearch = "Systems";
            dgv_SearchResult.DataSource = null;
            SortDV = ds_SearchResult.Tables["Systems"].DefaultView;
            for (int x = 0; x < dgv_SearchResult.Columns.Count; x++)
                dgv_SearchResult.Columns[x].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_SearchResult.DataSource = SortDV;
            SCols = new SortedList<int, int>();
        }

        private void PerformIceOreSearch(bool ore)
        {
            Dictionary<SolarSystem, Vertex> SystemTable;
            List<Vertex> SystemRoute = new List<Vertex>();
            GateRoute SystemTableFinder = new GateRoute();
            Route GRoute;
            int range, alID;
            bool emp, low, zero;
            bool UseOre = false;
            ArrayList SearchItems = new ArrayList();
            string sysName, oreTyp, oreList, sovHolder = "";

            if (rb_Galaxy.Checked)
                range = 0;
            else if (rb_Region.Checked)
                range = 1;
            else if (rb_Const.Checked)
                range = 2;
            else if (rb_JumpRange.Checked)
                range = 4;
            else
                range = 3;

            emp = cbx_SSSEmpire.Checked;
            low = cbx_SSSLow.Checked;
            zero = cbx_SSSZero.Checked;

            sysName = cb_SystemSelect.Text;
            if (sysName.Length < 4)
            {
                // Both starting and ending system is needed to compute a route
                MessageBox.Show("You must have a Search System Selected.", "Input Missing");
                return;
            }

            esp_SearchResult.Expanded = true;
            foreach (int checkedItem in clb_SearchSelections.CheckedIndices)
                SearchItems.Add(checkedItem.ToString());

            SolarSystem sSys = PlugInData.GalMap.GetSystemByName(sysName);

            GRoute = new Route(sSys, sSys, AvoidList, WaypointList, EvePilot, JumpShip, -1, 1);

            GRoute.ShipJumpRange = ShipJumpRange;
            GRoute.ShipFuelPerLY = ShipFuelPerLY;
            GRoute.UseJumpBridge = cbx_UseJumpBridge.Checked;

            //// Disable button to prevent double clicks
            b_DoSearch.Enabled = false;
            SystemTable = SystemTableFinder.GetRouteTable(GRoute);
            b_DoSearch.Enabled = true;
            ds_SearchResult.Tables["Ores"].Rows.Clear();

            foreach (Region r in PlugInData.GalMap.GalData.Regions.Values)
            {
                foreach (Constellation c in r.Constellations.Values)
                {
                    foreach (SolarSystem s in c.Systems.Values)
                    {
                        UseOre = true;

                        switch (range)
                        {
                            case 0:         // Galaxy
                                break;
                            case 1:         // Region
                                if (sSys.RegionID != r.ID)
                                    UseOre = false;
                                break;
                            case 2:         // Constellation
                                if (sSys.ConstID != c.ID)
                                    UseOre = false;
                                break;
                            case 3:         // System
                                if (sSys.ID != s.ID)
                                    UseOre = false;
                                break;
                            default:        // DuH what?
                                break;
                        }

                        if (!emp && (s.Security > 0.45))
                            UseOre = false;
                        if (!low && ((s.Security < 0.45) && (s.Security > 0)))
                            UseOre = false;
                        if (!zero && (s.Security <= 0))
                            UseOre = false;

                        if (ore)
                        {
                            oreList = PlugInData.GalMap.GetOre(s.SecClass);
                            if (s.OreBelts <= 0)
                                UseOre = false;
                        }
                        else
                        {
                            oreList = PlugInData.GalMap.GetIce(r.ID, s.Security);
                            if (s.IceBelts <= 0)
                                UseOre = false;
                        }

                        foreach (string ort in SearchItems)
                        {
                            oreTyp = clb_SearchSelections.Items[Convert.ToInt32(ort)].ToString();
                            if (!oreList.Contains(oreTyp))
                            {
                                UseOre = false;
                                break;
                            }
                        }

                        if (UseOre)
                        {
                            SystemRoute.Clear();
                            Vertex backTrack = SystemTable[s].Copy();
                            while (backTrack.SolarSystem != sSys)
                            {
                                //adding items to the front since were working backwards
                                SystemRoute.Insert(0, backTrack);
                                backTrack = SystemTable[backTrack.Path].Copy();
                            }

                            // If using a defined jump range, then filter here as well
                            if ((range == 4) && (SystemRoute.Count > (Convert.ToInt32(nud_SelectedRange.Value))))
                                continue;
                            
                            if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(s.ID))
                            {
                                alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[s.ID].allianceID;
                                if (alID != 0)
                                    sovHolder = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name + " [" + PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker + "]";
                                else
                                    sovHolder = "";
                            }

                            ds_SearchResult.Tables["Ores"].Rows.Add(SystemRoute.Count, Math.Round(s.Security, 2), r.Name, c.Name, s.Name, oreList, sovHolder);
                        }
                    }
                }
            }
            ActiveSearch = "Ores";
            dgv_SearchResult.DataSource = null;
            SortDV = ds_SearchResult.Tables["Ores"].DefaultView;
            for (int x = 0; x < dgv_SearchResult.Columns.Count; x++)
                dgv_SearchResult.Columns[x].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_SearchResult.DataSource = SortDV;
            SCols = new SortedList<int, int>();
        }

        private void PerformAgentSearch()
        {
            Dictionary<SolarSystem, Vertex> SystemTable;
            List<Vertex> SystemRoute = new List<Vertex>();
            GateRoute SystemTableFinder = new GateRoute();
            Route GRoute;
            string AFaction, SFaction, ACorp, ADiv, AType, AResrch, corp, fact, pilotName = ""; //MishType, 
            double effQual = 0, effStd = 0, sysQual = 0, agtEffStd = 0, piltStd = 0, factStd = 0;
            //double typPerc = 0;
            double AgtQual = 20.0;
            int range;
            bool emp, low, zero, lq, hq, loc;
            bool plt = false;
            bool[] levs = new bool[6];
            bool noLevsSelected = true;
            EveHQ.Core.PilotSkill NegSkill;
            AgentMishTypePerc amt = null;
                  
            ArrayList SearchItems = new ArrayList();
            double rS = 0;
            int dist;
            string sysName;

            AFaction = cb_Select1.Text;
            SFaction = cb_Select2.Text;
            ACorp = cb_Select3.Text;
            ADiv = cb_Select4.Text;
            AType = cb_Select5.Text;
            AResrch = cb_Select6.Text;
            //MishType = cb_Select7.Text;
            //typPerc = Convert.ToDouble(nud_TypePercent.Value);

            //if (MishType.Contains("Mining"))
            //{
            //    if (typPerc > 27.5)
            //        typPerc = 25;
            //}
            //else if (MishType.Contains("Trade"))
            //{
            //    if (typPerc > 5)
            //        typPerc = 5;
            //}

            if (rb_Galaxy.Checked)
                range = 0;
            else if (rb_Region.Checked)
                range = 1;
            else if (rb_Const.Checked)
                range = 2;
            else if (rb_JumpRange.Checked)
                range = 4;
            else
                range = 3;

            emp = cbx_SSSEmpire.Checked;
            low = cbx_SSSLow.Checked;
            zero = cbx_SSSZero.Checked;

            sysName = cb_SystemSelect.Text;
            if (sysName.Length < 4)
            {
                // Both starting and ending system is needed to compute a route
                MessageBox.Show("You must have a Search System Selected.", "Input Missing");
                return;
            }

            esp_SearchResult.Expanded = true;
            foreach (int checkedItem in clb_SearchSelections.CheckedIndices)
                SearchItems.Add(checkedItem.ToString());

            levs[0] = true;
            lq = false;
            hq = false;
            loc = false;

            foreach (string astr in SearchItems)
            {
                if (Convert.ToInt32(astr) == 0)
                    lq = true;
                else if (Convert.ToInt32(astr) == 1)
                    hq = true;
                else if (Convert.ToInt32(astr) == 8)
                    loc = true;
                else if (Convert.ToInt32(astr) == 2)
                    plt = true;
                else
                {
                    levs[Convert.ToInt32(astr) - 2] = true;
                    noLevsSelected = false;
                }
            }

            pilotName = cb_Pilot.Text;
            if (pilotName.Length < 1)
            {
                MessageBox.Show("You must have a Pilot Selected to Check Pilot Standings, Skipping for Now.", "Input Missing");
                plt = false;
            }
            else
                EveHQ.Core.Standings.GetStandings(pilotName);

            SolarSystem sSys = PlugInData.GalMap.GetSystemByName(sysName);

            GRoute = new Route(sSys, sSys, AvoidList, WaypointList, EvePilot, JumpShip, -1, 1);

            GRoute.ShipJumpRange = ShipJumpRange;
            GRoute.ShipFuelPerLY = ShipFuelPerLY;
            GRoute.UseJumpBridge = cbx_UseJumpBridge.Checked;

            //// Disable button to prevent double clicks
            b_DoSearch.Enabled = false;
            SystemTable = SystemTableFinder.GetRouteTable(GRoute);
            b_DoSearch.Enabled = true;
            ds_SearchResult.Tables["Agents"].Rows.Clear();

            foreach (Region r in PlugInData.GalMap.GalData.Regions.Values)
            {
                foreach (Constellation c in r.Constellations.Values)
                {
                    foreach (SolarSystem s in c.Systems.Values)
                    {
                        foreach (Station st in s.Stations.Values)
                        {
                            foreach (Agent a in st.Agents)
                            {
                                switch (range)
                                {
                                    case 0:         // Galaxy
                                        break;
                                    case 1:         // Region
                                        if (sSys.RegionID != r.ID)
                                            continue;
                                        break;
                                    case 2:         // Constellation
                                        if (sSys.ConstID != c.ID)
                                            continue;
                                        break;
                                    case 3:         // System
                                        if (sSys.ID != s.ID)
                                            continue;
                                        break;
                                    default:        // DuH what?
                                        break;
                                }
                                if (!AFaction.Equals("All"))
                                    if (PlugInData.Misc.Corporations.ContainsKey(st.Faction))
                                        if (PlugInData.Misc.Factions.ContainsKey(PlugInData.Misc.Corporations[st.Faction].facID))
                                            if (!AFaction.Equals(PlugInData.Misc.Factions[PlugInData.Misc.Corporations[st.Faction].facID]))
                                                continue;

                                if (!SFaction.Equals("All"))
                                    if (PlugInData.Misc.Corporations.ContainsKey(st.Faction))
                                        if (PlugInData.Misc.Factions.ContainsKey(PlugInData.Misc.Corporations[st.Faction].facID))
                                            if (!SFaction.Equals(PlugInData.Misc.Factions[PlugInData.Misc.Corporations[st.Faction].facID]))
                                                continue;

                                if (!ACorp.Equals("All"))
                                    if (PlugInData.Misc.Corporations.ContainsKey(st.corpID))
                                        if (!ACorp.Equals(PlugInData.Misc.Corporations[st.corpID].name))
                                            continue;

                                if (!ADiv.Equals("All"))
                                    if (!ADiv.Equals(a.Divis))
                                        continue;

                                if (!AType.Equals("All"))
                                    if (!AType.Equals(a.Type))
                                        continue;

                                if (!AResrch.Equals("All"))
                                    if (!a.ResearchType.Contains(AResrch))
                                        continue;

                                if (MishTypes.ContainsKey(a.Divis))
                                {
                                    amt = MishTypes[a.Divis];
                                }

                                if (!emp && (s.Security > 0.45))
                                    continue;
                                if (!low && ((s.Security < 0.45) && (s.Security > 0)))
                                    continue;
                                if (!zero && (s.Security <= 0))
                                    continue;

                                if (!levs[a.Level] && !noLevsSelected)
                                    continue;
                                if (!hq && (a.Quality >= 0))
                                    continue;
                                if (!lq && (a.Quality <= 0))
                                    continue;

                                if (pilotName.Length > 0)
                                {
                                    effStd = EveHQ.Core.Standings.GetStanding(pilotName, a.corpID.ToString(), true); // Corp
                                    agtEffStd = EveHQ.Core.Standings.GetStanding(pilotName, a.ID.ToString(), true);  // Agent
                                    factStd = EveHQ.Core.Standings.GetStanding(pilotName, PlugInData.Misc.Corporations[st.Faction].facID.ToString(), true);  // Faction

                                    piltStd = Math.Max(effStd, agtEffStd);
                                    piltStd = Math.Max(effStd, factStd);

                                    //effQual = a.Quality + agtEffStd;
                                    effQual = AgtQual + agtEffStd;
                                    if (EvePilot.PilotSkills.Contains("Negotiation"))
                                    {
                                        NegSkill = (EveHQ.Core.PilotSkill)EvePilot.PilotSkills["Negotiation"];
                                        effQual += (5 * NegSkill.Level);
                                    }
                                }

                                
                                if (s.Security >= 0)
                                {
                                    sysQual = effQual + ((1-s.Security) * 100);
                                }
                                else if (s.Security < 0)
                                {
                                    sysQual = effQual + ((s.Security * -1) * 100) + 100;
                                }
                                sysQual = Math.Round(sysQual, 2);
                                effQual = Math.Round(effQual, 2);
                                effStd = Math.Round(effStd, 2);
                                piltStd = Math.Round(piltStd, 2);

                                if (loc && (!a.Locate))
                                    continue;

                                //rS = Convert.ToDouble(a.Quality) / 20;
                                rS = -1;     // Since now Agent Quality for det usage is always -20 (for now)
                                rS = (((a.Level - 1) * 2) + rS);

                                if (plt && (piltStd < rS))
                                    continue;

                                SystemRoute.Clear();
                                Vertex backTrack = SystemTable[s].Copy();
                                while (backTrack.SolarSystem != sSys)
                                {
                                    //adding items to the front since were working backwards
                                    SystemRoute.Insert(0, backTrack);
                                    backTrack = SystemTable[backTrack.Path].Copy();
                                }
                                dist = SystemRoute.Count;

                                // If using a defined jump range, then filter here as well
                                if ((range == 4) && (dist > (Convert.ToInt32(nud_SelectedRange.Value))))
                                    continue;

                                if (PlugInData.Misc.Corporations.ContainsKey(a.corpID))
                                {
                                    corp = PlugInData.Misc.Corporations[a.corpID].name;
                                    if (PlugInData.Misc.Factions.ContainsKey(PlugInData.Misc.Corporations[st.Faction].facID))
                                        fact = PlugInData.Misc.Factions[PlugInData.Misc.Corporations[st.Faction].facID];
                                    else
                                        fact = "Unknown";
                                }
                                else
                                {
                                    corp = "Unknown";
                                    fact = "Unknown";
                                }

                                if (amt != null)
                                {
                                    // Note: a.Quality replaced with a 20 for now due to new system
                                    if (a.Locate)
                                        ds_SearchResult.Tables["Agents"].Rows.Add(a.Name, corp, fact, a.Divis, a.Level, dist, AgtQual, effQual, sysQual, rS, piltStd, Math.Round(s.Security, 2), r.Name, c.Name, s.Name, st.Name, a.Type + " [L]", a.ResearchType, amt.Distribution, amt.Mining, amt.Security);
                                    else
                                        ds_SearchResult.Tables["Agents"].Rows.Add(a.Name, corp, fact, a.Divis, a.Level, dist, AgtQual, effQual, sysQual, rS, piltStd, Math.Round(s.Security, 2), r.Name, c.Name, s.Name, st.Name, a.Type, a.ResearchType, amt.Distribution, amt.Mining, amt.Security);
                                }
                                else
                                {
                                    // Note: a.Quality replaced with a 20 for now due to new system
                                    if (a.Locate)
                                        ds_SearchResult.Tables["Agents"].Rows.Add(a.Name, corp, fact, a.Divis, a.Level, dist, AgtQual, effQual, sysQual, rS, piltStd, Math.Round(s.Security, 2), r.Name, c.Name, s.Name, st.Name, a.Type + " [L]", a.ResearchType, false, false, false);
                                    else
                                        ds_SearchResult.Tables["Agents"].Rows.Add(a.Name, corp, fact, a.Divis, a.Level, dist, AgtQual, effQual, sysQual, rS, piltStd, Math.Round(s.Security, 2), r.Name, c.Name, s.Name, st.Name, a.Type, a.ResearchType, false, false, false);
                                }
                            }
                        }
                    }
                }
            }

            // Now I have my agents, add them to the Data Grid display
            ActiveSearch = "Agents";
            dgv_SearchResult.DataSource = null;
            SortDV = ds_SearchResult.Tables["Agents"].DefaultView;
            dgv_SearchResult.DataSource = SortDV;
            for (int x = 0; x < dgv_SearchResult.Columns.Count; x++)
            {
                dgv_SearchResult.Columns[x].SortMode = DataGridViewColumnSortMode.Programmatic;

                if (x == 3)
                {
                    dgv_SearchResult.Columns[x].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (((x >= 4) && (x <= 11)) || ((x >= 18) && (x <= 21)))
                {
                    dgv_SearchResult.Columns[x].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            dgv_SearchResult.Columns[0].Frozen = true;
            SCols = new SortedList<int, int>();
        }

        private void PerformStationSearch(bool ConqSt)
        {
            Dictionary<SolarSystem, Vertex> SystemTable;
            List<Vertex> SystemRoute = new List<Vertex>();
            GateRoute SystemTableFinder = new GateRoute();
            ArrayList SearchItems = new ArrayList();
            Route GRoute;
            string SCorp, SPCorp, corp, fact, sovHolder = "";
            bool SEmp, SLow, SZero;
            bool UseStation = false;
            SolarSystem sSys;
            int range, alID;
            string sysName;

            SCorp = cb_Select1.Text;
            SPCorp = cb_Select2.Text;

            if (rb_Galaxy.Checked)
                range = 0;
            else if (rb_Region.Checked)
                range = 1;
            else if (rb_Const.Checked)
                range = 2;
            else if (rb_JumpRange.Checked)
                range = 4;
            else
                range = 3;

            SEmp = cbx_SSSEmpire.Checked;
            SLow = cbx_SSSLow.Checked;
            SZero = cbx_SSSZero.Checked;

            sysName = cb_SystemSelect.Text;
            if (sysName.Length < 4)
            {
                MessageBox.Show("You must have a Search System Selected.", "Input Missing");
                return;
            }

            esp_SearchResult.Expanded = true;
            foreach (string checkedItem in clb_SearchSelections.CheckedItems)
                SearchItems.Add(checkedItem);

            sSys = PlugInData.GalMap.GetSystemByName(sysName);

            GRoute = new Route(sSys, sSys, AvoidList, WaypointList, EvePilot, JumpShip, -1, 1);

            GRoute.ShipJumpRange = ShipJumpRange;
            GRoute.ShipFuelPerLY = ShipFuelPerLY;
            GRoute.UseJumpBridge = cbx_UseJumpBridge.Checked;

            b_DoSearch.Enabled = false;
            SystemTable = SystemTableFinder.GetRouteTable(GRoute);
            b_DoSearch.Enabled = true;
            ds_SearchResult.Tables["Stations"].Rows.Clear();

            foreach (Region r in PlugInData.GalMap.GalData.Regions.Values)
            {
                foreach (Constellation c in r.Constellations.Values)
                {
                    foreach (SolarSystem s in c.Systems.Values)
                    {
                        if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(s.ID))
                        {
                            //if (ConqSt)
                            //{
                                foreach (ConqStation cst in PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[s.ID].ConqStations.Values)
                                {
                                    UseStation = true;
                                    switch (range)
                                    {
                                        case 0:         // Galaxy
                                            break;
                                        case 1:         // Region
                                            if (sSys.RegionID != r.ID)
                                                UseStation = false;
                                            break;
                                        case 2:         // Constellation
                                            if (sSys.ConstID != c.ID)
                                                UseStation = false;
                                            break;
                                        case 3:         // System
                                            if (sSys.ID != s.ID)
                                                UseStation = false;
                                            break;
                                        default:        // DuH what?
                                            break;
                                    }

                                    if (!SPCorp.Equals("All")) // Conq Stations have a Player Corp Name
                                        if (!SPCorp.Equals(cst.CorpName))
                                            UseStation = false; 

                                    foreach (string srvc in SearchItems)
                                    {
                                        // Conquerable Stations do Not have a defined / known list of services
                                        if (!cst.Services.Contains(srvc))
                                        {
                                            UseStation = false;
                                            break;
                                        }
                                    }

                                    if (!SEmp && (s.Security >= 0.45))
                                        UseStation = false;

                                    if (!SLow && ((s.Security > 0) && (s.Security < 0.45)))
                                        UseStation = false;

                                    if (!SZero && (s.Security <= 0))
                                        UseStation = false;

                                    if (UseStation)
                                    {
                                        SystemRoute.Clear();
                                        Vertex backTrack = SystemTable[s].Copy();
                                        while (backTrack.SolarSystem != sSys)
                                        {
                                            //adding items to the front since were working backwards
                                            SystemRoute.Insert(0, backTrack);
                                            backTrack = SystemTable[backTrack.Path].Copy();
                                        }

                                        // If using a defined jump range, then filter here as well
                                        if ((range == 4) && (SystemRoute.Count > (Convert.ToInt32(nud_SelectedRange.Value))))
                                            continue;

                                        corp = cst.CorpName;
                                        fact = "Conq. Station";

                                        if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(s.ID))
                                        {
                                            alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[s.ID].allianceID;
                                            if (alID != 0)
                                                sovHolder = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name + " [" + PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker + "]";
                                            else
                                                sovHolder = "";
                                        }
                                        ds_SearchResult.Tables["Stations"].Rows.Add(cst.Name, corp, fact, SystemRoute.Count, Math.Round(s.Security, 2), r.Name, c.Name, s.Name, sovHolder);
                                    }
                                }
                            //}
                        }
                        else
                        {
                            foreach (Station st in s.Stations.Values)
                            {
                                UseStation = true;
                                switch (range)
                                {
                                    case 0:         // Galaxy
                                        break;
                                    case 1:         // Region
                                        if (sSys.RegionID != r.ID)
                                            UseStation = false;
                                        break;
                                    case 2:         // Constellation
                                        if (sSys.ConstID != c.ID)
                                            UseStation = false;
                                        break;
                                    case 3:         // System
                                        if (sSys.ID != s.ID)
                                            UseStation = false;
                                        break;
                                    default:        // DuH what?
                                        break;
                                }

                                if (!SCorp.Equals("All"))
                                {
                                    if (!PlugInData.CorpIDList.ContainsKey(st.corpID))
                                        UseStation = false;
                                    else if (!SCorp.Equals(PlugInData.CorpIDList[st.corpID]))
                                        UseStation = false;
                                }

                                foreach (string srvc in SearchItems)
                                {
                                    if (!DoesStationHaveService(st, srvc))
                                    {
                                        UseStation = false;
                                        break;
                                    }
                                }

                                if (!SEmp && (s.Security >= 0.45))
                                    UseStation = false;

                                if (!SLow && ((s.Security > 0) && (s.Security < 0.45)))
                                    UseStation = false;

                                if (!SZero && (s.Security <= 0))
                                    UseStation = false;

                                if (UseStation)
                                {
                                    SystemRoute.Clear();
                                    Vertex backTrack = SystemTable[s].Copy();
                                    while (backTrack.SolarSystem != sSys)
                                    {
                                        //adding items to the front since were working backwards
                                        SystemRoute.Insert(0, backTrack);
                                        backTrack = SystemTable[backTrack.Path].Copy();
                                    }

                                    // If using a defined jump range, then filter here as well
                                    if ((range == 4) && (SystemRoute.Count > (Convert.ToInt32(nud_SelectedRange.Value))))
                                        continue;

                                    corp = PlugInData.Misc.Corporations[st.corpID].name;
                                    if (PlugInData.Misc.Factions.ContainsKey(st.Faction))
                                        fact = PlugInData.Misc.Factions[st.Faction];
                                    else
                                        fact = "NPC Station";

                                    if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(s.ID))
                                    {
                                        alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[s.ID].allianceID;
                                        if (alID != 0)
                                            sovHolder = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name + " [" + PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker + "]";
                                        else
                                            sovHolder = "";
                                    }
                                    ds_SearchResult.Tables["Stations"].Rows.Add(st.Name, corp, fact, SystemRoute.Count, Math.Round(s.Security, 2), r.Name, c.Name, s.Name, sovHolder);
                                }
                            }
                        }
                    }
                }
            }

            dgv_SearchResult.DataSource = null;
            ActiveSearch = "Stations";

            SortDV = ds_SearchResult.Tables["Stations"].DefaultView;
            dgv_SearchResult.DataSource = SortDV;
            for (int x = 0; x < dgv_SearchResult.Columns.Count; x++)
                dgv_SearchResult.Columns[x].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_SearchResult.Columns[0].Frozen = true;
            SCols = new SortedList<int, int>();
        }

        public bool DoesStationHaveService(Station s, string serv)
        {
            foreach (string srv in s.Services)
            {
                if (srv.Contains(serv))
                    return true;
            }
            return false;
        }

        private void dgv_SearchResult_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv;
            bool multi = false;

            dgv = (DataGridView)sender;

            if (Control.ModifierKeys == Keys.Control)
                multi = true;

            SortSearchResultDGVByColumns(dgv, e.ColumnIndex, multi);
        }

        private void SortSearchResultDGVByColumns(DataGridView dgv, int colIndex, bool multi)
        {
            // SCols[colIndex]
            // 1 = ASC Sort
            // 2 = DESC Sort
            string SortString = "", colName = "", sType = "ASC";
            int SCount = 0;
            BindingSource BS = new BindingSource();

            SCount = SCols.Count;

            if (!multi && (SCount > 1))
                SCols.Clear();

            if (!multi || SCount < 1)
            {
                // Come in here if: not mutli-sort select (Control Key) OR we do not have any columns selected
                if (SCols.ContainsKey(colIndex))
                {
                    if ((SCount == 1) && (SCols[colIndex] == 1))
                        SCols[colIndex] = 2;
                    else if ((SCount == 1) && (SCols[colIndex] == 2))
                        SCols[colIndex] = 1;
                }
                else
                {
                    // Sorting by a new column, ASC
                    SCols.Clear();
                    SCols.Add(colIndex, 1);
                }
            }
            else
            {
                // Multiple sort folks - lets do the deed
                if (SCols.ContainsKey(colIndex))
                {
                    if (SCols[colIndex] == 1)
                        SCols[colIndex] = 2;
                    else if (SCols[colIndex] == 2)
                        SCols[colIndex] = 1;
                }
                else
                {
                    // Sorting by a new column, ASC
                    SCols.Add(colIndex, 1);
                }
            }

            int count = 0;
            foreach (var col in SCols)
            {
                colName = SortDV.Table.Columns[col.Key].ColumnName;
                if (col.Value == 1)
                {
                    sType = "ASC";
                }
                else
                {
                    sType = "DESC";
                }

                if (count > 0)
                    SortString += ", ";

                SortString += colName + " " + sType;

                count++;
            }

            SortDV.Sort = SortString;

            foreach (var col in SCols)
            {
                if (col.Value == 1)
                {
                    dgv_SearchResult.Columns[col.Key].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv_SearchResult.Columns[col.Key].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            //            dgv_SearchResult.DataSource = SortDV;
        }

        #endregion

        #region Route Exporting and Importing

        private void b_ExportJR_Click(object sender, EventArgs e)
        {
            // Text Export Cyno/Jump Route
            string line;
            StreamWriter SW;
            string fname;

            if (CurrentRoute.Count <= 0)
            {
                MessageBox.Show("No Route - You need to create / Search a route before you can Export.", "Input Missing");
                return;
            }

            sfd_ExportSelect.InitialDirectory = PlugInData.RMapData_Path;
            sfd_ExportSelect.Filter = "Jump Route (*.jrt)|*.jrt";
            sfd_ExportSelect.FilterIndex = 0;
            DialogResult DR = sfd_ExportSelect.ShowDialog();
            if (DR == DialogResult.Cancel)
                return;

            fname = sfd_ExportSelect.FileName;

            SW = File.CreateText(fname);

            line = "START:" + NewRoute.Start.Name;
            SW.WriteLine(line);

            foreach (Vertex v in CurrentRoute)
            {
                if (v.SolarSystem.Name.Equals(NewRoute.Start.Name) || v.SolarSystem.Name.Equals(NewRoute.Dest.Name))
                    continue;

                line = "JUMP:" + v.SolarSystem.Name + " By: " + v.JumpTyp.ToString();
                SW.WriteLine(line);
            }
            
            line = "END:" + NewRoute.Dest.Name;
            SW.WriteLine(line);
           
            foreach (var s in NewRoute.WayPoints)
            {
                if ((!s.Name.Equals(NewRoute.Start.Name)) && (!s.Name.Equals(NewRoute.Dest.Name)))
                {
                    line = "WP:" + s.Name;
                    SW.WriteLine(line);
                }
            }
            foreach (var s in NewRoute.AvoidSystems.Keys)
            {
                line = "AV:" + s.Name;
                SW.WriteLine(line);
            }

            if (NewRoute.UseCynoBeacon)
                line = "CYNO:True";
            else
                line = "CYNO:False";
            SW.WriteLine(line);

            if (NewRoute.UseJumpBridge)
                line = "USEJB:True";
            else
                line = "USEJB:False";
            SW.WriteLine(line);

            if (NewRoute.PreferStation)
                line = "PSTN:True";
            else
                line = "PSTN:False";
            SW.WriteLine(line);

            if (NewRoute.UseCynoSafeTwr)
                line = "CSAF:True";
            else
                line = "CSAF:False";
            SW.WriteLine(line);

            SW.Close();
        }

        private void b_ImportJR_Click(object sender, EventArgs e)
        {
            // Text Import for Jump Bridge & Cyno List *.csv
            string line;
            StreamReader SR;
            string fname;
            string[] kv;

             ofd_ImportFile.InitialDirectory = PlugInData.RMapData_Path;
            ofd_ImportFile.Filter = "Jump Route (*.jrt)|*.jrt";
            ofd_ImportFile.FilterIndex = 0;
            
            DialogResult DR = ofd_ImportFile.ShowDialog();
            if (DR == DialogResult.Cancel)
                return;

            fname = ofd_ImportFile.FileName;

            SR = File.OpenText(fname);
            AvoidList.Clear();
            WaypointList.Clear();
            tb_StartSystem.Text = "";
            tb_DestSystem.Text = "";
            nud_MinSecLev.Value = -1;
            nud_MaxSecLev.Value = 1;
            
            while ((line = SR.ReadLine()) != null)
            {
                // Read in each line, and the pull in the individual data pieces
                kv = line.Split(':');
                switch (kv[0])
                {
                    case "START":
                        tb_StartSystem.Text = kv[1];
                        RouteStart = kv[1];
                        break;
                    case "END":
                        tb_DestSystem.Text = kv[1];
                        RouteEnd = kv[1];
                        break;
                    case "WP":
                        WaypointList.Add(kv[1]);
                        break;
                    case "AV":
                        AvoidList.Add(kv[1]);
                        break;
                    case "CYNO":
                        if (kv[1].Equals("True"))
                            cbx_UseCynoGens.Checked = true;
                        else
                            cbx_UseCynoGens.Checked = false;
                        break;
                    case "CSAF":
                        if (kv[1].Equals("True"))
                            cbx_JRSafeTower.Checked = true;
                        else
                            cbx_JRSafeTower.Checked = false;
                        break;
                    case "USEJB":
                        if (kv[1].Equals("True"))
                            cbx_UseJumpBridge.Checked = true;
                        else
                            cbx_UseJumpBridge.Checked = false;
                        break;
                    case "PSTN":
                        if (kv[1].Equals("True"))
                            cb_StationOnly.Checked = true;
                        else
                            cb_StationOnly.Checked = false;
                        break;
                    default:
                        break;
                }

            }

            SR.Close();

            WaypointListUpdated();
            AvoidListUpdated();
            ActiveRoute = 1;
            BuildAndCalcRoute(true);
        }

        private void b_ImportGR_Click(object sender, EventArgs e)
        {
            // Text Import for Jump Bridge & Cyno List *.csv
            string line;
            StreamReader SR;
            string RMapBase_Path, RMapManage_Path, RMapData_Path, fname;
            string[] kv;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                RMapBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                RMapBase_Path = Application.StartupPath;
            }
            RMapManage_Path = Path.Combine(RMapBase_Path, "RouteMap");
            RMapData_Path = Path.Combine(RMapManage_Path, "Routes");

            if (!Directory.Exists(RMapManage_Path))
                Directory.CreateDirectory(RMapManage_Path);

            if (!Directory.Exists(RMapData_Path))
                Directory.CreateDirectory(RMapData_Path);


            ofd_ImportFile.InitialDirectory = RMapData_Path;
            ofd_ImportFile.Filter = "Jump Route (*.jrt)|*.jrt";
            ofd_ImportFile.FilterIndex = 0;
            DialogResult DR = ofd_ImportFile.ShowDialog();

            if (DR == DialogResult.Cancel)
                return;

            fname = ofd_ImportFile.FileName;

            SR = File.OpenText(fname);
            AvoidList.Clear();
            WaypointList.Clear();
            tb_StartSystem.Text = "";
            tb_DestSystem.Text = "";
            nud_MinSecLev.Value = -1;
            nud_MaxSecLev.Value = 1;

            while ((line = SR.ReadLine()) != null)
            {
                // Read in each line, and the pull in the individual data pieces
                kv = line.Split(':');
                switch (kv[0])
                {
                    case "START":
                        tb_StartSystem.Text = kv[1];
                        RouteStart = kv[1];
                        break;
                    case "END":
                        tb_DestSystem.Text = kv[1];
                        RouteEnd = kv[1];
                        break;
                    case "WP":
                        WaypointList.Add(kv[1]);
                        break;
                    case "AV":
                        AvoidList.Add(kv[1]);
                        break;
                    case "CYNO":
                        if (kv[1].Equals("True"))
                            cbx_UseCynoGens.Checked = true;
                        else
                            cbx_UseCynoGens.Checked = false;
                        break;
                    case "CSAF":
                        if (kv[1].Equals("True"))
                            cbx_JRSafeTower.Checked = true;
                        else
                            cbx_JRSafeTower.Checked = false;
                        break;
                    case "USEJB":
                        if (kv[1].Equals("True"))
                            cbx_UseJumpBridge.Checked = true;
                        else
                            cbx_UseJumpBridge.Checked = false;
                        break;
                    case "PSTN":
                        if (kv[1].Equals("True"))
                            cb_StationOnly.Checked = true;
                        else
                            cb_StationOnly.Checked = false;
                        break;
                    case "MINS":
                        nud_MinSecLev.Value = Convert.ToDecimal(kv[1]);
                        break;
                    case "MAXS":
                        nud_MaxSecLev.Value = Convert.ToDecimal(kv[1]);
                        break;
                    default:
                        break;
                }

            }

            SR.Close();

            WaypointListUpdated();
            AvoidListUpdated();
            ActiveRoute = 2;
            BuildAndCalcRoute(false);
        }

        private void b_ExportGR_Click(object sender, EventArgs e)
        {
            // Text Export Cyno/Jump Route
            string line;
            StreamWriter SW;
            string RMapBase_Path, RMapManage_Path, RMapData_Path, fname;

            if (CurrentGateRoute.Count <= 0)
            {
                MessageBox.Show("No Route - You need to create / Search a route before you can Export.", "Input Missing");
                return;
            }

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                RMapBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                RMapBase_Path = Application.StartupPath;
            }
            RMapManage_Path = Path.Combine(RMapBase_Path, "RouteMap");
            RMapData_Path = Path.Combine(RMapManage_Path, "Routes");

            if (!Directory.Exists(RMapManage_Path))
                Directory.CreateDirectory(RMapManage_Path);

            if (!Directory.Exists(RMapData_Path))
                Directory.CreateDirectory(RMapData_Path);

            sfd_ExportSelect.InitialDirectory = RMapData_Path;
            sfd_ExportSelect.Filter = "Gate Route (*.grt)|*.grt";
            sfd_ExportSelect.FilterIndex = 0;
            DialogResult DR = sfd_ExportSelect.ShowDialog();

            if (DR == DialogResult.Cancel)
                return;

            fname = sfd_ExportSelect.FileName;

            SW = File.CreateText(fname);

            line = "START:" + NewRoute.Start.Name;
            SW.WriteLine(line);

            foreach (Vertex v in CurrentGateRoute)
            {
                if (v.SolarSystem.Name.Equals(NewRoute.Start.Name) || v.SolarSystem.Name.Equals(NewRoute.Dest.Name))
                    continue;

                line = "GATE:" + v.SolarSystem.Name;
                SW.WriteLine(line);
            }

            line = "END:" + NewRoute.Dest.Name;
            SW.WriteLine(line);
            foreach (var s in NewRoute.WayPoints)
            {
                if ((!s.Name.Equals(NewRoute.Start.Name)) && (!s.Name.Equals(NewRoute.Dest.Name)))
                {
                    line = "WP:" + s.Name;
                    SW.WriteLine(line);
                }
            }
            foreach (var s in NewRoute.AvoidSystems.Keys)
            {
                line = "AV:" + s.Name;
                SW.WriteLine(line);
            }

            if (NewRoute.UseCynoBeacon)
                line = "CYNO:True";
            else
                line = "CYNO:False";
            SW.WriteLine(line);

            if (NewRoute.UseJumpBridge)
                line = "USEJB:True";
            else
                line = "USEJB:False";
            SW.WriteLine(line);

            if (NewRoute.PreferStation)
                line = "PSTN:True";
            else
                line = "PSTN:False";
            SW.WriteLine(line);

            if (NewRoute.UseCynoSafeTwr)
                line = "CSAF:True";
            else
                line = "CSAF:False";
            SW.WriteLine(line);

            line = "MINS:" + NewRoute.minSec;
            SW.WriteLine(line);
            line = "MAXS:" + NewRoute.maxSec;
            SW.WriteLine(line);

            SW.Close();
        }

        #endregion

        #region Map Display Selection

        private void tp_SolarSystem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region SystemDetailHandler

        public void ShowPlanetDetails(Planet p, SolarSystem s)
        {
            DataRow PD;
            smSelSys = s;

            DataRow[] pr = PlugInData.SysD.PlanetTable.Select("SID=" + s.ID + " AND PID=" + p.ID);

            if (pr.Length > 0)
                PD = pr[0];
            else
                PD = PlugInData.SysD.PlanetTable.NewPlanetTableRow();

            gp_POS.Hide();
            gp_MoonGoo.Hide();
            sl_StnLoc.Hide();
            ct_StationLoc.Hide();
            gp_Strategic.Show();
            tc_RouteSeach.SelectedTab = tp_SystemInfo;

            sl_Celestial.Text = p.Name;

            scbx_IHub.Checked = Convert.ToBoolean(PD["HasIHUB"]);
            scbx_TCU.Checked = Convert.ToBoolean(PD["HasTCU"]);

            SelCelID = p.ID;
        }

        public void ShowGateDetails(StarGate sg, SolarSystem s)
        {
            gp_POS.Hide();
            gp_MoonGoo.Hide();
            sl_StnLoc.Hide();
            ct_StationLoc.Hide();
            gp_Strategic.Hide();
            tc_RouteSeach.SelectedTab = tp_SystemInfo;

            sl_Celestial.Text = sg.destSys;

            SelCelID = sg.ID;
        }

        public void ShowMoonDetails(Moon m, SolarSystem s)
        {
            DataRow MD;
            smSelSys = s;

            DataRow[] mr = PlugInData.SysD.MoonTable.Select("SID=" + s.ID + " AND MID=" + m.ID);

            if (mr.Length > 0)
                MD = mr[0];
            else
                MD = PlugInData.SysD.MoonTable.NewMoonTableRow();

            gp_Strategic.Show();
            gp_MoonGoo.Show();
            gp_POS.Show();
            sl_StnLoc.Hide();
            ct_StationLoc.Hide();
            tc_RouteSeach.SelectedTab = tp_SystemInfo;

            sl_Celestial.Text = m.Name;
            scbx_IHub.Checked = false;

            scbx_TCU.Checked = Convert.ToBoolean(MD["HasTCU"]);
            tb_TowerName.Text = MD["TName"].ToString();
            cb_Corporation.Text = MD["CName"].ToString();
            cb_Alliance.Text = MD["AName"].ToString();
            cb_TowerType.Text = MD["TType"].ToString();
            tb_SysTowerPW.Text = MD["TPassword"].ToString();
            scbx_CynoSafeSpot.Checked = Convert.ToBoolean(MD["Safe"]);

            cbx_CynoType.Text = MD["HasTCU"].ToString();
            scbx_HasJumpBridge.Checked = Convert.ToBoolean(MD["Bridge"]);
            scbx_HasHangerArray.Checked = Convert.ToBoolean(MD["CHA"]);
            scbx_HasShipArray.Checked = Convert.ToBoolean(MD["SMA"]);
            scbx_HasCapArray.Checked = Convert.ToBoolean(MD["CSMA"]);
            scbx_HasCapAssembly.Checked = Convert.ToBoolean(MD["CAsm"]);
            scbx_IsMining.Checked = Convert.ToBoolean(MD["Mining"]);
            cbx_TowerDef.Text = MD["HasTCU"].ToString();

            cb_MoonGoo1.Text = MD["Goo1"].ToString();
            cb_MoonGoo2.Text = MD["Goo2"].ToString();
            cb_MoonGoo3.Text = MD["Goo3"].ToString();
            cb_MoonGoo4.Text = MD["Goo4"].ToString();
            cb_MoonGoo5.Text = MD["Goo5"].ToString();
            cb_MoonGoo6.Text = MD["Goo6"].ToString();
            cb_MoonGoo7.Text = MD["Goo7"].ToString();
            cb_MoonGoo8.Text = MD["Goo8"].ToString();

            SelCelID = m.ID;
        }

        public void ShowStationDetails(Station st, SolarSystem s)
        {
            smSelSys = s;
            
            gp_Strategic.Hide();
            gp_POS.Hide();
            gp_MoonGoo.Hide();
            sl_StnLoc.Hide();
            ct_StationLoc.Hide();
            tc_RouteSeach.SelectedTab = tp_SystemInfo;

            sl_Celestial.Text = st.Name;

            SelCelID = st.ID;
        }

        public void ShowConqStationDetails(ConqStation cs, SolarSystem s)
        {
            string selName = "Unknown";
            smSelSys = s;
            DataRow[] pr, mr;

            pr = PlugInData.SysD.PlanetTable.Select("SID=" + s.ID + " AND STID>0");
            mr = PlugInData.SysD.MoonTable.Select("SID=" + s.ID + " AND STID>0");
            if (pr.Length > 0)
            {
                selName = pr[0]["PName"].ToString();
            }
            else if (mr.Length > 0)
            {
                selName = mr[0]["MName"].ToString();
            }
            else
            {
                selName = "Unknown";
            }

            gp_Strategic.Hide();
            gp_POS.Hide();
            gp_MoonGoo.Hide();
            sl_StnLoc.Show();
            ct_StationLoc.Show();
            tc_RouteSeach.SelectedTab = tp_SystemInfo;
            sl_Celestial.Text = cs.Name;

            if (ct_StationLoc.Nodes.Count > 0)
            {
                foreach (DevComponents.AdvTree.Node n in ct_StationLoc.Nodes)
                {
                    if (n.Text.Equals(selName))
                        ct_StationLoc.SelectedNode = n;
                    else if (n.HasChildNodes)
                    {
                        foreach (DevComponents.AdvTree.Node cn in n.Nodes)
                        {
                            if (cn.Text.Equals(selName))
                            {
                                ct_StationLoc.SelectedNode = cn;
                            }
                        }
                    }
                }
            }

            SelCelID = cs.ID;
        }

        public void BuildCelestialListForCS(DevComponents.DotNetBar.Controls.ComboTree ct, SystemCelestials sc)
        {
            DevComponents.AdvTree.Node pNode, moon;

            sc.CelestToID = new SortedList<string, int>();
            ct.Nodes.Clear();
            pNode = new DevComponents.AdvTree.Node();
            pNode.Text = "Unknown";
            ct.Nodes.Add(pNode);

            foreach (Planet p in sc.Planets.Values)
            {
                pNode = new DevComponents.AdvTree.Node();
                pNode.Text = p.Name;

                if (!ct.Nodes.Contains(pNode))
                {
                    ct.Nodes.Add(pNode);
                    sc.CelestToID.Add(p.Name, p.ID);
                }

                foreach (Moon m in sc.Moons.Values)
                {
                    if (p.CelIndex == m.CelIndex)
                    {
                        moon = new DevComponents.AdvTree.Node();
                        moon.Text = m.Name;

                        if (!pNode.Nodes.Contains(moon))
                        {
                            pNode.Nodes.Add(moon);
                            sc.CelestToID.Add(m.Name, m.ID);
                        }
                    }
                }
            }
        }

        public void ShowDistanceValues(string src, string dest, double dist, int udType)
        {
            switch (udType)
            {
                case 1:     // Source
                    sl_DistFrom.Text = src;
                    sl_DistTo.Text = "";
                    sl_Distance.Text = "";
                    break;
                case 2:     // Dest & Distance
                    sl_DistTo.Text = dest;

                    if ((dist / PlugInData.AU) > 0.25)
                        sl_Distance.Text = String.Format("{0:#,0.##}", Math.Round((dist / PlugInData.AU), 2)) + " Au";
                    else
                        sl_Distance.Text = String.Format("{0:#,0.##}",  Math.Round((dist / 1000), 2)) + " km";
                    break;
                default:    // Ooops
                    break;
            }
        }

        public void ShowSystemDetails(SolarSystem s)
        {
            TreeNode Planets, PTyp, Moons, Belts, Ice, Station, Gate, stnSrvc, agents, Agt, JBH, JB, Blt, IceB, Anom, AnSn;
            TreeNode WHExpCt, StaticWH;
            ArrayList bridges;
            float sysRad;
            string pType, whp;
            string[] ores;
            ArrayList s_jk = new ArrayList();
            DataView SWH;
            bool whSys = false;
            long whidn;

            sl_Name.Text = s.Name;
            sysRad = ((float)s.Radius / AU) * 2;
            sl_SystemSize.Text = Math.Round(sysRad, 2) + " Au";
            lb_SystemSize.Text = Math.Round(sysRad, 2) + " Au";

            ti_Celestials.Text = s.Name + " Cel.";

            SystemCelestials SC = new SystemCelestials();
            SC.GetSystemCelestial(s);

            tv_SystemCelestials.Nodes.Clear();

            tv_SystemCelestials.Nodes.Add("System: " + s.Name);
            tv_SystemCelestials.Nodes.Add("Region: " + PlugInData.GalMap.GalData.Regions[s.RegionID].Name);
            tv_SystemCelestials.Nodes.Add("Constellation: " + PlugInData.GalMap.GalData.Regions[s.RegionID].Constellations[s.ConstID].Name);
            tv_SystemCelestials.Nodes.Add("Security: " + Math.Round(s.Security, 2).ToString());

            if (s.Name[0].Equals('J'))
            {
                whp = s.Name.Replace("J", "");
                if (long.TryParse(whp, out whidn))
                {
                    whSys = true;
                }
            }
            if (whSys)
            {
                // Add in wormhole class
                if (PlugInData.Misc.WHRegnToClass.ContainsKey(s.RegionID))
                {
                    tv_SystemCelestials.Nodes.Add("WH Class: " + PlugInData.Misc.WHRegnToClass[s.RegionID]);
                }

                if (!SC.Anomaly.Equals(""))
                {
                    Anom = tv_SystemCelestials.Nodes.Add("Anomaly: " + SC.Anomaly);
                    foreach (string an in SC.AnomEffects)
                    {
                        AnSn = Anom.Nodes.Add(an);
                        if (an.Contains("Penalty"))
                            AnSn.ForeColor = Color.Red;
                        else if (an.Contains("[ -"))
                            AnSn.ForeColor = Color.Red;
                        else
                            AnSn.ForeColor = Color.Green;
                    }
                }

                if (PlugInData.WHMapSelected)
                {
                    SWH = PlugInData.SysD.StaticWormholes.AsDataView();
                    SWH.RowFilter = "SID=" + s.ID;

                    dgv_SystemStaticWH.DataSource = SWH;

                    if (SWH.Count > 0)
                    {
                        StaticWH = tv_SystemCelestials.Nodes.Add("Static WH (" + SWH.Count + ")");

                        for (int x = 0; x < SWH.Count; x++)
                        {
                            string disp;

                            disp = SWH[0]["WH_Type"].ToString() + " ==> " + SWH[0]["StaticToClass"].ToString();

                            StaticWH.Nodes.Add(disp);
                        }
                    }
                }

                // Add in Wormhole Exploration Sites
                if (PlugInData.Misc.WHRegnToClass.ContainsKey(s.RegionID))
                {
                    foreach (var c in PlugInData.Misc.WHExpByClass)
                    {
                        if (c.Key.Equals(Convert.ToInt32(PlugInData.Misc.WHRegnToClass[s.RegionID])) || (c.Key.Equals(0)))
                        {
                            foreach (var ct in c.Value)
                            {
                                WHExpCt = tv_SystemCelestials.Nodes.Add(ct.Key);
                                foreach (var st in ct.Value)
                                    WHExpCt.Nodes.Add(st.ToString());
                            }
                        }
                    }
                }
            }

            if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(s.ID))
            {
                int alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[s.ID].allianceID;
                if (alID != 0)
                    tv_SystemCelestials.Nodes.Add("SovHld: " + PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name + " [" + PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker + "]");
            }


            s_jk = PlugInData.JKList.GetLatestJumpKillForSID(s.ID);
            tv_SystemCelestials.Nodes.Add("Kills : " + s_jk[1] + " S, " + s_jk[2] + " P, " + s_jk[3] + " NPC");
            tv_SystemCelestials.Nodes.Add("Jumps : " + s_jk[0].ToString());
            tv_SystemCelestials.Nodes.Add("Size  : " + Math.Round(sysRad, 2) + " Au");

            PopulateJumpKillsGraph(s.ID, s.Name);

            if (SC.Planets.Count <= 0)
                return;

            Planets = tv_SystemCelestials.Nodes.Add("Planets (" + SC.Planets.Count + ")");
            foreach (Planet p in SC.Planets.Values)
            {
                
                pType = p.Name + "{ " + p.GetPlanetTypeByGraphicID() + " }";
                PTyp = Planets.Nodes.Add(pType);
                PTyp.Nodes.Add("Radius: " + String.Format("{0:#,0.##}", (p.radius / 1000)) + "km");
            }
            Planets.Expand();

            if (SC.Moons.Count > 0)
            {
                Moons = tv_SystemCelestials.Nodes.Add("Moons (" + SC.Moons.Count + ")");
                foreach (Moon m in SC.Moons.Values)
                {
                    Moons.Nodes.Add(m.Name);
                }
            }

            if (SC.OreBelts.Count > 0)
            {
                Belts = tv_SystemCelestials.Nodes.Add("Ore Belts (" + SC.OreBelts.Count + ")");

                foreach (Belt b in SC.OreBelts.Values)
                { 
                    Belts.Nodes.Add(b.Name);
                }
            }

            if (SC.IceBelts.Count > 0)
            {
                Ice = tv_SystemCelestials.Nodes.Add("Ice Fields (" + SC.IceBelts.Count + ")");
                
                foreach (Belt b in SC.IceBelts.Values)
                {
                    Ice.Nodes.Add(b.Name);
                }
            }

            Gate = tv_SystemCelestials.Nodes.Add("Gates (" + SC.Gates.Count + ")");
            foreach (StarGate sg in SC.Gates.Values)
                Gate.Nodes.Add(sg.destSys);
            Gate.Expand();
            //tv_SystemCelestials.ExpandAll();

            if (s.Stations.Count > 0)
            {
                Station = tv_SystemCelestials.Nodes.Add("Stations (" + s.Stations.Count + ")");
                foreach (var stn in s.Stations)
                {
                    stnSrvc = Station.Nodes.Add(stn.Value.Name);

                    if (stn.Value.Agents.Count > 0)
                    {
                        agents = stnSrvc.Nodes.Add("Agents (" + stn.Value.Agents.Count + ")");
                        foreach (Agent ag in stn.Value.Agents)
                        {
                            Agt = agents.Nodes.Add(ag.Name);
                            Agt.Nodes.Add("Level: " + ag.Level);
                            Agt.Nodes.Add("Quality: " + ag.Quality);
                            Agt.Nodes.Add("Type: " + ag.Type);
                            Agt.Nodes.Add("Division: " + ag.Divis);
                            if (ag.Locate)
                                Agt.Nodes.Add("Locator Agent");
                        }
                    }

                    if (stn.Value.Refine > 0)
                        stnSrvc.Nodes.Add("Refining (" + (stn.Value.Refine * 100) + "%)");
                    foreach (string str in stn.Value.Services)
                        stnSrvc.Nodes.Add(str);


                }
            }

            if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(s.ID))
            {
                Station = tv_SystemCelestials.Nodes.Add("Conq. Stations (" + PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[s.ID].ConqStations.Count + ")");
                foreach (var cqs in PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[s.ID].ConqStations)
                {
                    if (cqs.Value.SSysID == s.ID)
                    {
                        stnSrvc = Station.Nodes.Add(cqs.Value.Name);
                        foreach (string ssr in cqs.Value.Services)
                            stnSrvc.Nodes.Add(ssr);
                    }
                }
            }

            bridges = GetBridgesInSystem(s.ID);
            if (bridges.Count > 0)
            {
                JBH = tv_SystemCelestials.Nodes.Add("Jump Bridges");
                foreach (JumpBridge bdg in bridges)
                {
                    if (bdg.From.ID == s.ID)
                    {
                        JB = JBH.Nodes.Add(bdg.FromMoon);
                        JB.Nodes.Add(bdg.ToMoon);
                        JB.Nodes.Add("PW: " + bdg.Password);
                    }
                    else if (bdg.To.ID == s.ID)
                    {
                        JB = JBH.Nodes.Add(bdg.ToMoon);
                        JB.Nodes.Add(bdg.FromMoon);
                        JB.Nodes.Add("PW: " + bdg.Password);
                    }
                }

                //JBH.ExpandAll();
            }

            if (SC.OreBelts.Count > 0)
            {
                ores = PlugInData.GalMap.GetOre(s.SecClass).Split(',');
                Blt = tv_SystemCelestials.Nodes.Add("Ore Types (" + ores.Length + ")");

                foreach (string str in ores)
                    Blt.Nodes.Add(str);

                Blt.Expand();
            }

            if (SC.IceBelts.Count > 0)
            {
                ores = PlugInData.GalMap.GetIce(s.RegionID, s.Security).Split(',');
                IceB = tv_SystemCelestials.Nodes.Add("Ice Types (" + ores.Length + ")");

                foreach (string str in ores)
                    IceB.Nodes.Add(str);

                IceB.Expand();
            }

            tv_SystemCelestials.SelectedNode = Planets;
            // 1. Build display for all system moons and moon goo
            // 2. Build display of entered towers, IHub, TCU
            ShowSystemAddViewDetails(s);
            //ShowSystemCelestialList(s);
        }

        public void JKDataUpdate()
        {
            SolarSystem ss;

            if (SelActID != 0)
            {
                ss = PlugInData.GalMap.GetSystemByID(SelActID);
                ShowSystemDetails(ss);
            }

            ActMon_1.UpdateJKGraph();
            ActMon_2.UpdateJKGraph();
            ActMon_3.UpdateJKGraph();
            ActMon_4.UpdateJKGraph();
        }

        private void t_APICheck_Tick(object sender, EventArgs e)
        {
            JKDataUpdate();
        }

        public void PopulateJumpKillsGraph(int sID, string sysName)
        {
            PointPairList jd, sd, pd, fd;
            LineItem myCurve;
            DateTime timStamp, jTS = new DateTime(0001, 1, 1), kTS = new DateTime(0001, 1, 1);
            string lastAPI = "Unknown";
            int ov = 2;

            // get a reference to the GraphPane
            GraphPane myPane = zg_JumpKills.GraphPane;

            SelActID = sID;

            myPane.CurveList.Clear();
            // Set the Titles

            myPane.Title.Text = "System Activity for " + sysName;
            myPane.XAxis.Type = AxisType.Date;
            myPane.XAxis.Scale.MajorUnit = DateUnit.Hour;
            myPane.XAxis.Scale.Format = "T";
            myPane.YAxis.Title.Text = "Quantity";
            //myPane.YAxis.Type = AxisType.Linear;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.DarkGray;
            myPane.YAxis.MajorGrid.Color = Color.DarkGray;
            myPane.YAxis.Type = AxisType.Log;
            myPane.Fill = new Fill(Color.Black);
            myPane.Chart.Fill = new Fill(Color.Black);
            myPane.Border.Color = Color.White;
            myPane.Legend.Fill = new Fill(Color.Black);
            myPane.Legend.FontSpec.FontColor = Color.DodgerBlue;
            myPane.Chart.Border.Color = Color.Transparent;
            myPane.Title.FontSpec.FontColor = Color.DodgerBlue;
            myPane.XAxis.Title.FontSpec.FontColor = Color.DodgerBlue;
            myPane.YAxis.Title.FontSpec.FontColor = Color.DodgerBlue;
            myPane.XAxis.Scale.FontSpec.FontColor = Color.DodgerBlue;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.DodgerBlue;
            myPane.XAxis.Color = Color.DodgerBlue;
            myPane.YAxis.Color = Color.DodgerBlue;

            jd = new PointPairList();
            sd = new PointPairList();
            pd = new PointPairList();
            fd = new PointPairList();

            foreach (var skTS in PlugInData.JKList.Jumps)
            {
                timStamp = skTS.Key;
                if (skTS.Value.ContainsKey(sID))
                {
                    jd.Add(timStamp.ToOADate(), skTS.Value[sID] + ov);
                }
                else
                {
                    jd.Add(timStamp.ToOADate(), ov);
                }

                jTS = timStamp;
            }

            foreach (var skTS in PlugInData.JKList.Kills)
            {
                timStamp = skTS.Key;
                if (skTS.Value.ContainsKey(sID))
                {
                    sd.Add(timStamp.ToOADate(), Convert.ToInt32(skTS.Value[sID][0]) + ov);
                    pd.Add(timStamp.ToOADate(), Convert.ToInt32(skTS.Value[sID][1]) + ov);
                    fd.Add(timStamp.ToOADate(), Convert.ToInt32(skTS.Value[sID][2]) + ov);
                }
                else
                {
                    sd.Add(timStamp.ToOADate(), ov);
                    pd.Add(timStamp.ToOADate(), ov);
                    fd.Add(timStamp.ToOADate(), ov);
                }

                kTS = timStamp;
            }

            myCurve = myPane.AddCurve("System Jumps", jd, Color.Yellow, SymbolType.None);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.1f;
            myCurve.Line.IsOptimizedDraw = true;

            myCurve = myPane.AddCurve("Ship Kills", sd, Color.Violet, SymbolType.None);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.1f;
            myCurve.Line.IsOptimizedDraw = true;

            myCurve = myPane.AddCurve("Pod Kills", pd, Color.Red, SymbolType.None);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.1f;
            myCurve.Line.IsOptimizedDraw = true;

            myCurve = myPane.AddCurve("NPC Kills", fd, Color.PaleGreen, SymbolType.None);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.1f;
            myCurve.Line.IsOptimizedDraw = true;

            if (jTS.CompareTo(kTS) > 0)
                lastAPI = "J - " + jTS.ToString();
            else
                lastAPI = "K - " + kTS.ToString();

            myPane.XAxis.Title.Text = "Time in Hours (Last API - " + lastAPI + ")";
                       
            zg_JumpKills.AxisChange();
            zg_JumpKills.Refresh();
        }

        private string zg_JumpKills_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            string timeStp = "", Jumps = "", SKills = "", PKills = "", NKills = "", retString = "Unknown";
            DateTime jTm = new DateTime(0001, 1, 1);
            DateTime kTm = new DateTime(0001, 1, 1);

            // New Jump / Kills data has been processed
            // Need to update displays if a system is selected!
            if (!SelActID.Equals(0))
            {
                if (PlugInData.JKList.Jumps.Count > iPt)
                {
                    jTm = PlugInData.JKList.Jumps.ElementAt(iPt).Key;
                    if (PlugInData.JKList.Jumps.ElementAt(iPt).Value.ContainsKey(SelActID))
                    {
                        Jumps = "Jumps: " + PlugInData.JKList.Jumps.ElementAt(iPt).Value[SelActID];
                    }
                    else
                        Jumps = "Jumps: 0";
                }
                if (PlugInData.JKList.Kills.Count > iPt)
                {
                    kTm = PlugInData.JKList.Kills.ElementAt(iPt).Key;
                    if (PlugInData.JKList.Kills.ElementAt(iPt).Value.ContainsKey(SelActID))
                    {
                        SKills = "Ship Kills: " + PlugInData.JKList.Kills.ElementAt(iPt).Value[SelActID][0];
                        PKills = "Pod Kills: " + PlugInData.JKList.Kills.ElementAt(iPt).Value[SelActID][1];
                        NKills = "NPC Kills: " + PlugInData.JKList.Kills.ElementAt(iPt).Value[SelActID][2];
                    }
                    else
                    {
                        SKills = "Ship Kills: 0";
                        PKills = "Pod Kills: 0";
                        NKills = "NPC Kills: 0";
                    }
                }
            }

            if (jTm.CompareTo(kTm) > 0)
                timeStp = "J - " + jTm.ToString();
            else
                timeStp = "K - " + kTm.ToString();

            retString = timeStp + "\n";
            if (!Jumps.Equals(""))
                retString += Jumps + "\n";
            
            if (!SKills.Equals(""))
            {
                retString += SKills + "\n";
                retString += PKills + "\n";
                retString += NKills + "\n";
            }

            return retString;
        }

        private ArrayList GetBridgesInSystem(int ID)
        {
            ArrayList retVal = new ArrayList();

            foreach (var jbl in PlugInData.JumpBridgeList.Values)
            {
                foreach (var jb in jbl.Values)
                {
                    if (jb.From.ID == ID)
                    {
                        retVal.Add(jb);
                    }

                    if (jb.To.ID == ID)
                    {
                        retVal.Add(jb);
                    }
                }
            }

            return retVal;
        }

        private void tv_SystemCelestials_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode sNode;

            //if (tc_MapSelect.SelectedTab.Equals(tp_SolarSystem))
            //{
                // When we are in solar system map, automatically select, center and zoom on the
                // Celestial object that was just selected.
                sNode = tv_SystemCelestials.SelectedNode;

                // We only want to do this for child nodes / ie: Actual objects to look at
                if (sNode.Nodes.Count > 0)
                    return;

                if (sNode.Parent == null)
                    return;

                // Belts are not currently displayed on the system level map
                if (sNode.Parent.Text.Equals("Ore Belts"))
                    return;

                // Belts are not currently displayed on the system level map
                if (sNode.Parent.Text.Equals("Ice Fields"))
                    return;

            //}
        }

        #endregion

        #region Rebuild Galaxy Data

        private void b_RebuildGalaxyData_Click(object sender, EventArgs e)
        {

           DialogResult dr = MessageBox.Show("Rebuilding the Galaxy data will reset all system, constellation, and region colors! \n" + 
                                             "In addition this will take some time to do, do you really want to do this?", "Rebuild Galaxy", MessageBoxButtons.YesNo);

           if (dr == DialogResult.Yes)
           {
               Cursor = Cursors.WaitCursor;

               PlugInData.RebuildGalaxyData();

               Cursor = Cursors.Arrow;

               // Re-draw the Maps !
               MainMapView.UpdateGalaxy(PlugInData.GalMap);
           }

        }

        #endregion

        #region MoonGoo Selection Reflecting

        private void cb_MoonGoo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MoonGoo1.Text.Equals("Nothing") && !Reflecting)
            {
                Reflecting = true;
                cb_MoonGoo2.Text = "Nothing";
                cb_MoonGoo3.Text = "Nothing";
                cb_MoonGoo4.Text = "Nothing";
                cb_MoonGoo5.Text = "Nothing";
                cb_MoonGoo6.Text = "Nothing";
                cb_MoonGoo7.Text = "Nothing";
                cb_MoonGoo8.Text = "Nothing";
                Reflecting = false;
            }
        }

        private void cb_MoonGoo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MoonGoo2.Text.Equals("Nothing") && !Reflecting)
            {
                Reflecting = true;
                cb_MoonGoo3.Text = "Nothing";
                cb_MoonGoo4.Text = "Nothing";
                cb_MoonGoo5.Text = "Nothing";
                cb_MoonGoo6.Text = "Nothing";
                cb_MoonGoo7.Text = "Nothing";
                cb_MoonGoo8.Text = "Nothing";
                Reflecting = false;
            }
        }

        private void cb_MoonGoo3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MoonGoo3.Text.Equals("Nothing") && !Reflecting)
            {
                Reflecting = true;
                cb_MoonGoo4.Text = "Nothing";
                cb_MoonGoo5.Text = "Nothing";
                cb_MoonGoo6.Text = "Nothing";
                cb_MoonGoo7.Text = "Nothing";
                cb_MoonGoo8.Text = "Nothing";
                Reflecting = false;
            }
        }

        private void cb_MoonGoo4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MoonGoo4.Text.Equals("Nothing") && !Reflecting)
            {
                Reflecting = true;
                cb_MoonGoo5.Text = "Nothing";
                cb_MoonGoo6.Text = "Nothing";
                cb_MoonGoo7.Text = "Nothing";
                cb_MoonGoo8.Text = "Nothing";
                Reflecting = false;
            }
        }

        private void cb_MoonGoo5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MoonGoo5.Text.Equals("Nothing") && !Reflecting)
            {
                Reflecting = true;
                cb_MoonGoo6.Text = "Nothing";
                cb_MoonGoo7.Text = "Nothing";
                cb_MoonGoo8.Text = "Nothing";
                Reflecting = false;
            }
        }

        private void cb_MoonGoo6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MoonGoo6.Text.Equals("Nothing") && !Reflecting)
            {
                Reflecting = true;
                cb_MoonGoo7.Text = "Nothing";
                cb_MoonGoo8.Text = "Nothing";
                Reflecting = false;
            }
        }

        private void cb_MoonGoo7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MoonGoo7.Text.Equals("Nothing") && !Reflecting)
            {
                Reflecting = true;
                cb_MoonGoo8.Text = "Nothing";
                Reflecting = false;
            }
        }

        private void cb_MoonGoo8_SelectedIndexChanged(object sender, EventArgs e)
        {
            // No point
        }

        private void dgv_MoonGoo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0))
                return;

            if (dgv_MoonGoo[e.ColumnIndex, e.RowIndex].EditType != null)
            {
                if (dgv_MoonGoo[e.ColumnIndex, e.RowIndex].EditType.ToString().Equals("System.Windows.Forms.DataGridViewComboBoxEditingControl"))
                {
                    if (dgv_MoonGoo.EditingControl == null)
                        SendKeys.Send("{F4}");
                }
            }
        }

        private void dgv_SystemCelest_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0))
                return;

            if (dgv_SystemCelest[e.ColumnIndex, e.RowIndex].EditType != null)
            {
                if (dgv_SystemCelest[e.ColumnIndex, e.RowIndex].EditType.ToString().Equals("System.Windows.Forms.DataGridViewComboBoxEditingControl"))
                {
                    if (dgv_SystemCelest.EditingControl == null)
                        SendKeys.Send("{F4}");
                }
            }
        }

        private void dgv_Planets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0))
                return;

            if (dgv_SystemCelest[e.ColumnIndex, e.RowIndex].EditType != null)
            {
                if (dgv_SystemCelest[e.ColumnIndex, e.RowIndex].EditType.ToString().Equals("System.Windows.Forms.DataGridViewComboBoxEditingControl"))
                {
                    if (dgv_SystemCelest.EditingControl == null)
                        SendKeys.Send("{F4}");
                }
            }
        }



        #endregion

        #region View and Edit System Stuff (Goo, Towers)

        public void SetupSystemMoonGooColumns()
        {
            DataGridViewComboBoxColumn dgCB;
            DataGridViewTextBoxColumn dgTB;

            // Add Goo Columns to MoonGoo DataGridView Display
            for (int x = 1; x < 9; x++)
            {
                dgCB = new DataGridViewComboBoxColumn();
                dgCB.Name = "Goo" + x;
                dgCB.HeaderText = "Moon Goo #" + x;
                dgCB.Items.Add("Unknown");
                dgCB.Items.Add("Nothing");
                dgCB.Sorted = true;
                dgCB.Items.AddRange(PlugInData.Misc.GooTypes.ToArray());
                dgCB.AutoComplete = true;
                dgCB.MinimumWidth = 100;
                dgCB.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                dgCB.DataPropertyName = "Goo" + x;

                dgv_MoonGoo.Columns.Add(dgCB);
            }
            // Tower Type
            dgTB = new DataGridViewTextBoxColumn();
            dgTB.Name = "TTyp";
            dgTB.HeaderText = "TTYP";
            dgTB.Visible = false;
            dgTB.DataPropertyName = "TType";
            dgv_MoonGoo.Columns.Add(dgTB);

            // Tower Name for Tooltip
            dgTB = new DataGridViewTextBoxColumn();
            dgTB.Name = "TNam";
            dgTB.HeaderText = "TNM";
            dgTB.Visible = false;
            dgTB.DataPropertyName = "TName";
            dgv_MoonGoo.Columns.Add(dgTB);
        }

        public void SetupSystemCelestialDGColumns()
        {
            DataGridViewComboBoxColumn dgCB;
            DataGridViewCheckBoxColumn dgCX;
            DataGridViewTextBoxColumn dgTB;

            // Tower Name
            dgTB = new DataGridViewTextBoxColumn();
            dgTB.Name = "TowerName";
            dgTB.HeaderText = "Tower Name";
            dgTB.Visible = true;
            dgTB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgTB.DataPropertyName = "TName";
            dgv_SystemCelest.Columns.Add(dgTB);
            // Corporation
            dgTB = new DataGridViewTextBoxColumn();
            dgTB.Name = "TowerCorp";
            dgTB.HeaderText = "Corporation";
            dgTB.Visible = true;
            dgTB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgTB.DataPropertyName = "CName";
            dgv_SystemCelest.Columns.Add(dgTB);
            // Alliance
            dgCB = new DataGridViewComboBoxColumn();
            dgCB.Name = "LocationAlliance";
            dgCB.HeaderText = "Alliance";
            dgCB.Items.Add("Unknown");
            dgCB.Sorted = true;
            dgCB.Items.AddRange(PlugInData.AllianceList.ToArray());
            dgCB.AutoComplete = true;
            dgCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgCB.DropDownWidth = 250;
            dgCB.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            dgCB.DataPropertyName = "AName";
            dgv_SystemCelest.Columns.Add(dgCB);
            // Tower Type
            dgCB = new DataGridViewComboBoxColumn();
            dgCB.Name = "TowerType";
            dgCB.HeaderText = "Tower Type";
            dgCB.Items.Add("NA");
            dgCB.Items.Add("Unknown");
            dgCB.Sorted = true;
            dgCB.Items.AddRange(PlugInData.Misc.TowerTypes.ToArray());
            dgCB.AutoComplete = true;
            dgCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgCB.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            dgCB.DataPropertyName = "TType";
            dgv_SystemCelest.Columns.Add(dgCB);
            // Password
            dgTB = new DataGridViewTextBoxColumn();
            dgTB.Name = "TowerPassword";
            dgTB.HeaderText = "Password";
            dgTB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgTB.DataPropertyName = "TPassword";
            dgv_SystemCelest.Columns.Add(dgTB);
            // Defense
            dgCB = new DataGridViewComboBoxColumn();
            dgCB.Name = "TowerDefense";
            dgCB.HeaderText = "Defense";
            dgCB.Items.Add("NA");
            dgCB.Items.Add("Unknown");
            dgCB.Items.Add("None");
            dgCB.Items.Add("Defended");
            dgCB.Items.Add("Death Star");
            dgCB.Items.Add("Dik-Star");
            dgCB.Sorted = true;
            dgCB.AutoComplete = true;
            dgCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgCB.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            dgCB.DropDownWidth = 80;
            dgCB.DataPropertyName = "TDefense";
            dgv_SystemCelest.Columns.Add(dgCB);
            // TCU
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "SystemTCU";
            dgCX.HeaderText = "TCU";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "HasTCU";
            dgv_SystemCelest.Columns.Add(dgCX);
            // Cyno
            dgCB = new DataGridViewComboBoxColumn();
            dgCB.Name = "SystemCyno";
            dgCB.HeaderText = "Cyno";
            dgCB.Items.Add("NA");
            dgCB.Items.Add("None");
            dgCB.Items.Add("Jammer");
            dgCB.Items.Add("Generator");
            dgCB.Sorted = true;
            dgCB.AutoComplete = true;
            dgCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgCB.DropDownWidth = 80;
            dgCB.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            dgCB.DataPropertyName = "Cyno";
            dgv_SystemCelest.Columns.Add(dgCB);
            // Jump Bridge
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "MoonJB";
            dgCX.HeaderText = "JB";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "Bridge";
            dgv_SystemCelest.Columns.Add(dgCX);
            // CHA
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "MoonCHA";
            dgCX.HeaderText = "CHA";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "CHA";
            dgv_SystemCelest.Columns.Add(dgCX);
            // SMA
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "MoonSMA";
            dgCX.HeaderText = "SMA";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "SMA";
            dgv_SystemCelest.Columns.Add(dgCX);
            // C-SMA
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "MoonCSMA";
            dgCX.HeaderText = "C-SMA";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "CSMA";
            dgv_SystemCelest.Columns.Add(dgCX);
            // C-Cnst
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "MoonCCnst";
            dgCX.HeaderText = "CSCA";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "CAsm";
            dgv_SystemCelest.Columns.Add(dgCX);
            // Mining
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "MoonMining";
            dgCX.HeaderText = "Mining";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "Mining";
            dgv_SystemCelest.Columns.Add(dgCX);
            // Safe Tower
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "MoonSafe";
            dgCX.HeaderText = "Safe";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "Safe";
            dgv_SystemCelest.Columns.Add(dgCX);
            // Moon ID
            dgTB = new DataGridViewTextBoxColumn();
            dgTB.Name = "MID";
            dgTB.HeaderText = "MID";
            dgTB.DataPropertyName = "MID";
            dgTB.Visible = false;
            dgv_SystemCelest.Columns.Add(dgTB);
        }

        public void SetupSystemPlanetDGColumns()
        {
            DataGridViewComboBoxColumn dgCB;
            DataGridViewCheckBoxColumn dgCX;
            DataGridViewTextBoxColumn dgTB;

            // Corporation
            dgTB = new DataGridViewTextBoxColumn();
            dgTB.Name = "TowerCorp";
            dgTB.HeaderText = "Corporation";
            dgTB.Visible = true;
            dgTB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgTB.DataPropertyName = "CName";
            dgv_Planets.Columns.Add(dgTB);
            // Alliance
            dgCB = new DataGridViewComboBoxColumn();
            dgCB.Name = "LocationAlliance";
            dgCB.HeaderText = "Alliance";
            dgCB.Items.Add("Unknown");
            dgCB.Sorted = true;
            dgCB.Items.AddRange(PlugInData.AllianceList.ToArray());
            dgCB.AutoComplete = true;
            dgCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgCB.DropDownWidth = 250;
            dgCB.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            dgCB.DataPropertyName = "AName";
            dgv_Planets.Columns.Add(dgCB);
            // I-Hub
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "SystemIHUB";
            dgCX.HeaderText = "I-Hub";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "HasIHUB";
            dgv_Planets.Columns.Add(dgCX);
            // TCU
            dgCX = new DataGridViewCheckBoxColumn();
            dgCX.Name = "SystemTCU";
            dgCX.HeaderText = "TCU";
            dgCX.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgCX.Width = 40;
            dgCX.DataPropertyName = "HasTCU";
            dgv_Planets.Columns.Add(dgCX);
        }

        public void AddMissingSystemDetailElements(SolarSystem s)
        {
            SystemCelestials SC = new SystemCelestials();
            DataRow[] pr, mr, sr;
            ds_SystemDetails.MoonTableRow nmr;
            ds_SystemDetails.PlanetTableRow npr;
            ds_SystemDetails.SystemTableRow nsr;

            SC.GetSystemCelestial(s);

            sr = PlugInData.SysD.SystemTable.Select("SID=" + s.ID);

            if (sr.Length < 1)
            {
                nsr = PlugInData.SysD.SystemTable.NewSystemTableRow();

                nsr.SID = s.ID;
                nsr.CID = s.ConstID;
                nsr.RID = s.RegionID;

                PlugInData.SysD.SystemTable.AddSystemTableRow(nsr);
            }

            foreach (Planet p in SC.Planets.Values)
            {
                pr = PlugInData.SysD.PlanetTable.Select("PID=" + p.ID);

                if (pr.Length < 1)
                {
                    // Does not exist, so add default values
                    npr = PlugInData.SysD.PlanetTable.NewPlanetTableRow();

                    npr.PID = p.ID;
                    npr.SID = s.ID;
                    npr.PName = p.Name;

                    foreach (Station st in s.Stations.Values)
                    {
                        if (st.Name.Contains(p.Name) && (!st.Name.Contains("Moon")))
                        {
                            npr.STID = st.ID;
                        }
                    }

                    PlugInData.SysD.PlanetTable.AddPlanetTableRow(npr);
                }
            }

            foreach (Moon m in SC.Moons.Values)
            {
                mr = PlugInData.SysD.MoonTable.Select("MID=" + m.ID);
                if (mr.Length < 1)
                {
                    // Does not exist, so add default values
                    nmr = PlugInData.SysD.MoonTable.NewMoonTableRow();

                    nmr.MID = m.ID;
                    nmr.SID = s.ID;
                    nmr.MName = m.Name;

                    foreach (Planet p in SC.Planets.Values)
                    {
                        if (m.CelIndex == p.CelIndex)
                        {
                            nmr.PID = p.ID;
                            break;
                        }
                    }

                    foreach (Station st in s.Stations.Values)
                    {
                        if (st.Name.Contains(m.Name))
                        {
                            nmr.STID = st.ID;
                        }
                    }

                    PlugInData.SysD.MoonTable.AddMoonTableRow(nmr);
                }
            }
        }

        public void ShowSystemAddViewDetails(SolarSystem s)
        {
            DataView MV, PV, SV;

            cb_SDSystemName.Text = s.Name;
            if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(s.ID))
            {
                int alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[s.ID].allianceID;
                if (alID != 0)
                    lb_SDSovHolder.Text = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name + " [" + PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker + "]";
            }

            // Disable column Auto-Generate. I want to do it manually.
            dgv_MoonGoo.AutoGenerateColumns = false;
            dgv_SystemCelest.AutoGenerateColumns = false;
            dgv_Planets.AutoGenerateColumns = false;
         
            // Add any missing system elements to the listings
            AddMissingSystemDetailElements(s);

            MV = PlugInData.SysD.MoonTable.AsDataView();
            MV.RowFilter = "SID=" + s.ID;

            PV = PlugInData.SysD.PlanetTable.AsDataView();
            PV.RowFilter = "SID=" + s.ID;

            SV = PlugInData.SysD.MoonTable.AsDataView();
            SV.RowFilter = "SID=" + s.ID;

            dgv_MoonGoo.DataSource = MV;
            dgv_SystemCelest.DataSource = SV;
            dgv_Planets.DataSource = PV;

            ColorizeMGDG();
        }

        private void tc_EditType_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            if (tc_EditType.SelectedTab == tp_MoonGoo)
                ColorizeMGDG();
        }

        private void ColorizeMGDG()
        {
            foreach (DataGridViewRow dr in dgv_MoonGoo.Rows)
            {
                for (int col = 1; col < 9; col++)
                {
                    if (dr.Cells[col].Value.Equals("Unknown"))
                        dr.Cells[col].Style.BackColor = Color.Gainsboro;
                    else
                        dr.Cells[col].Style.BackColor = GetMinColor(dr.Cells[col].Value.ToString());
                }
                if (!dr.Cells["TTyp"].Value.Equals(""))
                {
                    dr.Cells[0].Style.BackColor = Color.LightCoral;
                    dr.Cells[0].ToolTipText = dr.Cells["TNam"].Value.ToString();
                }
            }
            dgv_MoonGoo.Refresh();
        }

        private void dgv_MoonGoo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // If user just entered nothing for a column, set all following columns to Nothing.
            if ((e.ColumnIndex > 0) && (e.ColumnIndex < 9))
            {
                if (dgv_MoonGoo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals("Nothing"))
                {
                    dgv_MoonGoo.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                    for (int x = e.ColumnIndex + 1; x < 9; x++)
                    {
                        dgv_MoonGoo.Rows[e.RowIndex].Cells[x].Value = "Nothing";
                        dgv_MoonGoo.Rows[e.RowIndex].Cells[x].Style.BackColor = Color.White;
                    }
                }
                else
                {
                    if (dgv_MoonGoo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals("Unknown"))
                        dgv_MoonGoo.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gainsboro;
                    else
                        dgv_MoonGoo.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = GetMinColor(dgv_MoonGoo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    for (int x = e.ColumnIndex + 1; x < 9; x++)
                    {
                        if (dgv_MoonGoo.Rows[e.RowIndex].Cells[x].Value.Equals("Unknown"))
                        {
                            dgv_MoonGoo.Rows[e.RowIndex].Cells[x].Value = "Nothing";
                            dgv_MoonGoo.Rows[e.RowIndex].Cells[x].Style.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        private void cb_SDSystemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SDSysChange)
                return;

            CheckForSystemDetailChangesAndSave();
            
            SDsSys = PlugInData.GalMap.GetSystemByName(cb_SDSystemName.Text);

            if (SDsSys != null)
            {
                SDSysChange = true;
                SDsCel.GetSystemCelestial(SDsSys);
                ShowSystemAddViewDetails(SDsSys);
                SDSysChange = false;
            }
        }

        private void CheckForSystemDetailChangesAndSave()
        {
            PlugInData.SaveSystemDetailsToDisk();
        }

        private Color GetMinColor(string name)
        {
            if (gas.Contains(name))
                return Color.PaleGreen;

            if (r08.Contains(name))
                return Color.Yellow;

            if (r16.Contains(name))
                return Color.Gold;

            if (r32.Contains(name))
                return Color.DarkOrange;

            if (r64.Contains(name))
                return Color.Red;

            return Color.White;           
        }

        private void ImportGooCSV()
        {
            string line;
            string[] data;
            StreamReader SR;
            SolarSystem s;
            SystemCelestials SC = new SystemCelestials();
            string fname;
            int sID, mID, pID;
            ds_SystemDetails.SystemTableRow nsr;// = SysD.SystemTable.NewSystemTableRow();
            ds_SystemDetails.PlanetTableRow npr;
            ds_SystemDetails.MoonTableRow nmr;
            DataRow[] tdr;

            // Note: This is an absolute bare bones data import - nothing but actual Moon Goo types will be imported
            // This can and will take a bit of time to run, Set to Busy/Wait cursor
            this.Cursor = Cursors.WaitCursor;

            ofd_ImportFile.InitialDirectory = PlugInData.RMapExport_Path;
            ofd_ImportFile.Filter = "Goo (*.goo)|*.goo";
            ofd_ImportFile.FilterIndex = 0;
            DialogResult DR = ofd_ImportFile.ShowDialog();
            if (DR == DialogResult.Cancel)
                return;
            fname = ofd_ImportFile.FileName;

            if (File.Exists(fname))
                SR = File.OpenText(fname);
            else
                return;

            while ((line = SR.ReadLine()) != null)
            {
                data = line.Split(',');
                // 0 = system ID
                // 1 = moon ID
                // 3 = planet ID
                // 3,4,5,6,7,8,9,10 = Goo Type

                sID = Convert.ToInt32(data[0]);
                pID = Convert.ToInt32(data[2]);
                mID = Convert.ToInt32(data[1]);
                s = PlugInData.GalMap.GetSystemByID(sID);
                SC.GetSystemCelestial(s);

                // Add system if Not already present
                tdr = PlugInData.SysD.SystemTable.Select("SID=" + sID);
                if (tdr.Length < 1)
                {
                    nsr = PlugInData.SysD.SystemTable.NewSystemTableRow();
                    nsr.SID = sID;
                    nsr.CID = s.ConstID;
                    nsr.RID = s.RegionID;

                    PlugInData.SysD.SystemTable.AddSystemTableRow(nsr);
                }
            
                // Add any planets that are not currently present
                tdr = PlugInData.SysD.PlanetTable.Select("SID=" + sID + " AND PID=" + pID);
                if (tdr.Length < 1)
                {
                    // Does not exist, so add default values
                    npr = PlugInData.SysD.PlanetTable.NewPlanetTableRow();

                    npr.PID = pID;
                    npr.SID = sID;
                    npr.PName = SC.Planets[pID].Name;

                    PlugInData.SysD.PlanetTable.AddPlanetTableRow(npr);
                }

                // Add moons and Goo (user overwrite for Goo if other than Unknown)
                tdr = PlugInData.SysD.MoonTable.Select("SID=" + sID + " AND MID=" + mID);
                if (tdr.Length < 1)
                {
                    // Moon does not exist, so add values
                    nmr = PlugInData.SysD.MoonTable.NewMoonTableRow();

                    nmr.SID = sID;
                    nmr.MID = mID;
                    nmr.PID = pID;
                    nmr.MName = SC.Moons[mID].Name;
                    nmr.Goo1 = data[3];
                    nmr.Goo2 = data[4];
                    nmr.Goo3 = data[5];
                    nmr.Goo4 = data[6];
                    nmr.Goo5 = data[7];
                    nmr.Goo6 = data[8];
                    nmr.Goo7 = data[9];
                    nmr.Goo8 = data[10];
                }
                else
                {
                    if (!data[3].Equals("Unknown"))
                        tdr[0]["Goo1"] = data[3];
                    if (!data[4].Equals("Unknown"))
                        tdr[0]["Goo2"] = data[4];
                    if (!data[5].Equals("Unknown"))
                        tdr[0]["Goo3"] = data[5];
                    if (!data[6].Equals("Unknown"))
                        tdr[0]["Goo4"] = data[6];
                    if (!data[7].Equals("Unknown"))
                        tdr[0]["Goo5"] = data[7];
                    if (!data[8].Equals("Unknown"))
                        tdr[0]["Goo6"] = data[8];
                    if (!data[9].Equals("Unknown"))
                        tdr[0]["Goo7"] = data[9];
                    if (!data[10].Equals("Unknown"))
                        tdr[0]["Goo8"] = data[10];
                }

            }

            SR.Close();

            // Done, set back
            CheckForSystemDetailChangesAndSave();

            this.Cursor = Cursors.Default;
        }

        private void ExportGooCSV(int sysID)
        {
            StreamWriter SW;
            DataRow[] mr;
            string fname;
            string dataLine;

            // This will take a bit of time to run, Set to Busy/Wait cursor
            this.Cursor = Cursors.WaitCursor;

            sfd_ExportSelect.InitialDirectory = PlugInData.RMapExport_Path;
            sfd_ExportSelect.Filter = "Goo (*.goo)|*.goo";
            sfd_ExportSelect.FilterIndex = 0;
            DialogResult DR = sfd_ExportSelect.ShowDialog();
            if (DR == DialogResult.Cancel)
                return;

            fname = sfd_ExportSelect.FileName;

            SW = File.CreateText(fname);

            if (sysID > 0)
            {
                mr = PlugInData.SysD.MoonTable.Select("SID=" + sysID);

                foreach (DataRow dr in mr)
                {
                    // Goo Entries Exist for this System
                    dataLine = sysID.ToString();
                    dataLine += "," + dr["PID"].ToString();
                    dataLine += "," + dr["MID"].ToString();
                    dataLine += "," + dr["Goo1"].ToString();
                    dataLine += "," + dr["Goo2"].ToString();
                    dataLine += "," + dr["Goo3"].ToString();
                    dataLine += "," + dr["Goo4"].ToString();
                    dataLine += "," + dr["Goo5"].ToString();
                    dataLine += "," + dr["Goo6"].ToString();
                    dataLine += "," + dr["Goo7"].ToString();
                    dataLine += "," + dr["Goo8"].ToString();

                    SW.WriteLine(dataLine);
                }
            }
            else
            {
                foreach (DataRow dsr in PlugInData.SysD.SystemTable)
                {
                    mr = PlugInData.SysD.MoonTable.Select("SID=" + dsr["SID"]);

                    foreach (DataRow dr in mr)
                    {
                        // Goo Entries Exist for this System
                        dataLine = dr["SID"].ToString();
                        dataLine += "," + dr["PID"].ToString();
                        dataLine += "," + dr["MID"].ToString();
                        dataLine += "," + dr["Goo1"].ToString();
                        dataLine += "," + dr["Goo2"].ToString();
                        dataLine += "," + dr["Goo3"].ToString();
                        dataLine += "," + dr["Goo4"].ToString();
                        dataLine += "," + dr["Goo5"].ToString();
                        dataLine += "," + dr["Goo6"].ToString();
                        dataLine += "," + dr["Goo7"].ToString();
                        dataLine += "," + dr["Goo8"].ToString();

                        SW.WriteLine(dataLine);
                    }
                }
            }

            SW.Close();

            // Done, set back
            this.Cursor = Cursors.Default;
        }

        private void ImportGooXML()
        {
            string fname;
            ds_SystemDetails impDS = new ds_SystemDetails();

            sfd_ExportSelect.InitialDirectory = PlugInData.RMapExport_Path;
            sfd_ExportSelect.Filter = "xml (*.xml)|*.xml";
            sfd_ExportSelect.FilterIndex = 0;
            DialogResult DR = sfd_ExportSelect.ShowDialog();
            if (DR == DialogResult.Cancel)
                return;

            fname = sfd_ExportSelect.FileName;

            if (File.Exists(fname))
            {
                impDS.ReadXml(fname);
            }

            PlugInData.SysD.Merge(impDS);
            CheckForSystemDetailChangesAndSave();
        }

        private void ExportSystemXML(SolarSystem s)
        {
            string fname;
            DataView subSet = new DataView();
            DataRow[] sr, pr, mr;
            ds_SystemDetails expDS = new ds_SystemDetails();

            if (s == null)
                return;

            sfd_ExportSelect.InitialDirectory = PlugInData.RMapExport_Path;
            sfd_ExportSelect.Filter = "xml (*.xml)|*.xml";
            sfd_ExportSelect.FilterIndex = 0;
            DialogResult DR = sfd_ExportSelect.ShowDialog();
            if (DR == DialogResult.Cancel)
                return;

            fname = sfd_ExportSelect.FileName;

            sr = PlugInData.SysD.SystemTable.Select("SID=" + s.ID);
            expDS.SystemTable.ImportRow(sr[0]);

            pr = PlugInData.SysD.PlanetTable.Select("SID=" + s.ID);
            foreach (DataRow dr in pr)
            {
                expDS.PlanetTable.ImportRow(dr);
            }

            mr = PlugInData.SysD.MoonTable.Select("SID=" + s.ID);
            foreach (DataRow dr in mr)
            {
                expDS.MoonTable.ImportRow(dr);
            }

            expDS.WriteXml(fname, XmlWriteMode.WriteSchema);
        }

        private void ExportAllXML()
        {
            string fname;
 
            sfd_ExportSelect.InitialDirectory = PlugInData.RMapExport_Path;
            sfd_ExportSelect.Filter = "xml (*.xml)|*.xml";
            sfd_ExportSelect.FilterIndex = 0;
            DialogResult DR = sfd_ExportSelect.ShowDialog();
            if (DR == DialogResult.Cancel)
                return;

            fname = sfd_ExportSelect.FileName;

            PlugInData.SysD.WriteXml(fname, XmlWriteMode.WriteSchema);
        }

        private void bbi_ExportCSV_Click(object sender, EventArgs e)
        {
            ExportGooCSV(SDsSys.ID);
        }

        private void bbi_XMLExport_Click(object sender, EventArgs e)
        {
            ExportSystemXML(SDsSys);
        }

        private void bbi_CSVA_Click(object sender, EventArgs e)
        {
            ExportGooCSV(0);
        }

        private void bbi_XMLAll_Click(object sender, EventArgs e)
        {
            ExportAllXML();
        }

        private void bi_ImportData_Click(object sender, EventArgs e)
        {
            ImportGooXML();
        }

        private void bi_ImportCSV_Click(object sender, EventArgs e)
        {
            ImportGooCSV();
        }

        private void tsmi_ClearTowerData_Click(object sender, EventArgs e)
        {
            int mID;
            DataRow[] dr;
            DataGridViewRow sr = dgv_SystemCelest.CurrentRow;

            mID = Convert.ToInt32(sr.Cells["MID"].Value);

            dr = PlugInData.SysD.MoonTable.Select("MID=" + mID);

            if (dr.Length > 0)
            {
                // Valid row, clear all tower related data
                dr[0]["TName"] = "";
                dr[0]["TType"] = "";
                dr[0]["TPassword"] = "";
                dr[0]["TDefense"] = "None";
                dr[0]["Cyno"] = "None";
                dr[0]["CName"] = "";
                dr[0]["AName"] = "";
                dr[0]["Bridge"] = false;
                dr[0]["CHA"] = false;
                dr[0]["SMA"] = false;
                dr[0]["CAsm"] = false;
                dr[0]["CSMA"] = false;
                dr[0]["Mining"] = false;
                dr[0]["Safe"] = false;
            }
        }

        #endregion

        #region Splitter Size and State Save

        private void ep_RightPanel_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            bool st;
            st = ep_RightPanel.Expanded;

            PlugInData.Config.CFlags["Right Panel Expanded"] = st;
            PlugInData.SaveConfigToDisk();
        }

        private void ep_RightPanel_SplitterMoved(object sender, SplitterEventArgs e)
        {
            int dim;
            dim = gp_RightPanel.Width;

            PlugInData.Config.Sizes["Right Panel Size"] = dim;
            PlugInData.SaveConfigToDisk();
        }

        private void esp_SearchResult_SplitterMoved(object sender, SplitterEventArgs e)
        {
            int dim;
            dim = ep_SearchResult.Height;

            PlugInData.Config.Sizes["Bottom Panel Size"] = dim;
            PlugInData.SaveConfigToDisk();
        }

        private void esp_SearchResult_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            bool st;
            st = esp_SearchResult.Expanded;

            PlugInData.Config.CFlags["Bottom Panel Expanded"] = st;
            PlugInData.SaveConfigToDisk();
        }

        private void es_EntryDetails_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //int dim;
            //dim = gp_EntryDetails.Width;

            //PlugInData.Config.Sizes["Left Panel Size"] = dim;
            //PlugInData.SaveConfigToDisk();
        }

        #endregion

        #region Import POSManager Towers

        // Since POSManager is - assuming enabled - always available for tower information, I want to automatically
        // Import any tower data into RouteMap for the known Towers in POS Manager
        private void ImportPOSManagerTowers()
        {
            //if (EveHQ.PosManager.PlugInData.PDL.Designs.Count > 0)
            //{
            //    foreach(EveHQ.PosManager.POS p in EveHQ.PosManager.PlugInData.PDL.Designs.Values)
            //    {
            //    }
            //}
        }

        #endregion

        #region Zooming To and Map Toggles

        private void at_Regions_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            string rName, cName, rcName;
            string[] split;

            if (ChkUnchk)
                return;

            rcName = e.Cell.Parent.Name;
            split = rcName.Split('|');

            rName = split[0];

            if (split.Length > 1)
            {
                // We have a constellation Node
                cName = split[1];

                PlugInData.RCSelList[rName].Constellations[cName].Selected = e.Cell.Checked;

                MainMapView.MapViewSettingChanged();
            }
            else
            {
                PlugInData.RCSelList[rName].Selected = e.Cell.Checked;

                foreach (CNode cn in PlugInData.RCSelList[rName].Constellations.Values)
                    cn.Selected = e.Cell.Checked;

                UpdateRegionView(rName);
            }

            PlugInData.SaveRegionConstSel();
        }

        private void UpdateRegionView(string rN)
        {
            string[] split;

            foreach (DevComponents.AdvTree.Node rn in at_Regions.Nodes)
            {
                if (rN.Length < 1)
                {
                    rn.Checked = PlugInData.RCSelList[rn.Name].Selected;

                    foreach (DevComponents.AdvTree.Node cn in rn.Nodes)
                    {
                        split = cn.Name.Split('|');
                        cn.Checked = PlugInData.RCSelList[rn.Name].Constellations[split[1]].Selected;
                    }
                }
                else if (rn.Name.Equals(rN))
                {
                    foreach (DevComponents.AdvTree.Node cn in rn.Nodes)
                    {
                        split = cn.Name.Split('|');
                        cn.Checked = PlugInData.RCSelList[rn.Name].Constellations[split[1]].Selected;
                    }
                    break;
                }
            }
            MainMapView.MapViewSettingChanged();
        }

        private void bi_CheckAllRC_Click(object sender, EventArgs e)
        {
            ChkUnchk = true;
            foreach (RNode rn in PlugInData.RCSelList.Values)
            {
                rn.Selected = true;
                foreach (CNode cn in rn.Constellations.Values)
                    cn.Selected = true;
            }
            PlugInData.SaveRegionConstSel();
            UpdateRegionView("");
            ChkUnchk = false;
        }

        private void bi_ClearAllRC_Click(object sender, EventArgs e)
        {
            ChkUnchk = true;
            foreach (RNode rn in PlugInData.RCSelList.Values)
            {
                rn.Selected = false;
                foreach (CNode cn in rn.Constellations.Values)
                    cn.Selected = false;
            }
            PlugInData.SaveRegionConstSel();
            UpdateRegionView("");
            ChkUnchk = false;
        }

        private void bi_PrintSelection_Click(object sender, EventArgs e)
        {
            MainMapView.PrintMapView();
        }

         private void bi_ShowSelection_Click(object sender, EventArgs e)
        {

        }

        private void cbx_SelGalaxy_CheckedChanged(object sender, EventArgs e)
        {
            MainMapView.MapViewSettingChanged();
        }

        private void tsmi_ShowLocation_Click(object sender, EventArgs e)
        {
            string rName, cName, rcName;
            string[] split;

            if (ChkUnchk)
                return;

            rcName = at_Regions.SelectedNode.Name;
            split = rcName.Split('|');

            rName = split[0];

            if (split.Length > 1)
            {
                // We have a constellation Node
                cName = split[1];
            }
            else
                cName = "";

            MainMapView.ZoomAndCenterOnRegionOrConstellation(rName, cName);
        }

        public void ToggleJBHL(bool local)
        {
            if (local)
                MainMapView.ToggleJBRangeHL();
            else
            {
                MapToggle = true;

                if (sw_BridgeHL.Value)
                    sw_BridgeHL.Value = false;
                else
                    sw_BridgeHL.Value = true;

                MapToggle = false;
            }
        }

        private void sw_BridgeHL_ValueChanged(object sender, EventArgs e)
        {
            if (MapToggle)
                return;

            PlugInData.Config.CFlags["Show Bridge Ranges"] = sw_BridgeHL.Value;
            PlugInData.SaveConfigToDisk();
            ToggleJBHL(true);
        }

        public void ToggleJumpFromHL(bool local)
        {
            if (local)
                MainMapView.ToggleShipJumpRangeHL();
            else
            {
                MapToggle = true;

                if (sw_JumpFromHL.Value)
                    sw_JumpFromHL.Value = false;
                else
                    sw_JumpFromHL.Value = true;

                MapToggle = false;
            }
        }

        private void sw_JumpFromHL_ValueChanged(object sender, EventArgs e)
        {
            if (MapToggle)
                return;

            PlugInData.Config.CFlags["Show Jump From Ranges"] = sw_JumpFromHL.Value;
            PlugInData.SaveConfigToDisk();
            ToggleJumpFromHL(true);
        }

        public void ToggleJumpToHL(bool local)
        {
            if (local)
                MainMapView.ToggleShipJumpToRangeHL();
            else
            {
                MapToggle = true;

                if (sw_JumpToHL.Value)
                    sw_JumpToHL.Value = false;
                else
                    sw_JumpToHL.Value = true;

                MapToggle = false;
            }
        }

        private void sw_JumpToHL_ValueChanged(object sender, EventArgs e)
        {
            if (MapToggle)
                return;

            PlugInData.Config.CFlags["Show Jump To Ranges"] = sw_JumpToHL.Value;
            PlugInData.SaveConfigToDisk();
            ToggleJumpToHL(true);
        }

        private void sw_SearchResultHL_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Show Search Ranges"] = sw_SearchResultHL.Value;
            PlugInData.SaveConfigToDisk();
            MainMapView.ToggleSearchResultHL();
        }

        private void sw_BridgeDetail_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Show Bridge Details"] = sw_BridgeDetail.Value;
            MainMapView.UpdateConfig(PlugInData.Config);
            PlugInData.SaveConfigToDisk();
        }

        private void sw_ShowSov_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Show Sovreignty"] = sw_ShowSov.Value;
            MainMapView.UpdateConfig(PlugInData.Config);
            PlugInData.SaveConfigToDisk();
        }

        private void sw_ShowCyno_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Show Cyno Beacons"] = sw_ShowCyno.Value;
            MainMapView.UpdateConfig(PlugInData.Config);
            PlugInData.SaveConfigToDisk();
        }

        private void sw_ShowJB_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Show Jump Bridges"] = sw_ShowJB.Value;
            MainMapView.UpdateConfig(PlugInData.Config);
            PlugInData.SaveConfigToDisk();
        }

        private void sw_ShowDetail_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Show Detail Text"] = sw_ShowDetail.Value;
            MainMapView.UpdateConfig(PlugInData.Config);
            PlugInData.SaveConfigToDisk();
        }

        private void sw_SystemNm_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Show System Names"] = sw_SystemNm.Value;
            MainMapView.UpdateConfig(PlugInData.Config);
            PlugInData.SaveConfigToDisk();
        }

        private void sw_ConstNm_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Show Const Names"] = sw_ConstNm.Value;
            MainMapView.UpdateConfig(PlugInData.Config);
            PlugInData.SaveConfigToDisk();
        }

        private void sw_RegionNm_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Show Region Names"] = sw_RegionNm.Value;
            MainMapView.UpdateConfig(PlugInData.Config);
            PlugInData.SaveConfigToDisk();
        }

        private void sw_RedoGate_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Recompute Gate Route"] = sw_RedoGate.Value;
            PlugInData.SaveConfigToDisk();
        }

        private void sw_RedoJump_ValueChanged(object sender, EventArgs e)
        {
            PlugInData.Config.CFlags["Recompute Jump Route"] = sw_RedoJump.Value;
            PlugInData.SaveConfigToDisk();
        }

        private void b_ZoomAndCenterRoute_Click(object sender, EventArgs e)
        {
            MainMapView.ZoomAndCenterOnRoute();
        }

        #endregion

        #region Wormhole Handlers

        public void SetupSystemStaticWH()
        {
            DataGridViewComboBoxColumn dgCB;
            DataGridViewTextBoxColumn dgTB;

            // Type
            dgCB = new DataGridViewComboBoxColumn();
            dgCB.Name = "WormholeType";
            dgCB.HeaderText = "Type";
            dgCB.Items.Add("Unknown");
            dgCB.Sorted = true;
            dgCB.Items.AddRange(PlugInData.Misc.WHTypes.ToArray());
            dgCB.AutoComplete = true;
            dgCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgCB.DropDownWidth = 60;
            dgCB.Width = 60;
            dgCB.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;           
            dgCB.DataPropertyName = "WH_Type";
            dgv_SystemStaticWH.Columns.Add(dgCB);
            // Target class
            dgTB = new DataGridViewTextBoxColumn();
            dgTB.Name = "TargetSystemClass";
            dgTB.HeaderText = "Target Class";
            dgTB.Visible = true;
            dgTB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgTB.DataPropertyName = "StaticToClass";
            dgTB.ReadOnly = true;
            dgTB.Width = 200;
            dgv_SystemStaticWH.Columns.Add(dgTB);
            // System ID
            dgTB = new DataGridViewTextBoxColumn();
            dgTB.Name = "SID";
            dgTB.HeaderText = "System ID";
            dgTB.Visible = false;
            dgTB.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgTB.DataPropertyName = "SID";
            dgv_SystemStaticWH.Columns.Add(dgTB);
        }
        
        private void SetupPossibleWormholes()
        {
            string whName, tClas = "", maxMass = "", jumpMass = "", massRegen = "", tTip;
            Color tColr = Color.Purple;
            int locX = 0, locY = 0;
            long hrsLife = 0, mrv = 0;
            long massMult = 0;
            System.Windows.Forms.Label whType;

            foreach (var wh in PlugInData.Misc.WH_Nm2Cls)
            {
                whName = wh.Key.ToString();

                if (PlugInData.Misc.WH_Classes.ContainsKey(PlugInData.Misc.WH_Nm2Cls[whName]))
                {
                    foreach (var whc in PlugInData.Misc.WH_Classes[PlugInData.Misc.WH_Nm2Cls[whName]])
                    {
                        switch (whc.Key)
                        {
                            case 1381:
                                switch (whc.Value)
                                {
                                    case 1:
                                        tClas = "Class " + whc.Value;
                                        tColr = Color.SlateBlue;
                                        break;
                                    case 2:
                                        tClas = "Class " + whc.Value;
                                        tColr = Color.MediumPurple;
                                        break;
                                    case 3:
                                        tClas = "Class " + whc.Value;
                                        tColr = Color.MediumOrchid;
                                        break;
                                    case 4:
                                        tClas = "Class " + whc.Value;
                                        tColr = Color.DarkViolet;
                                        break;
                                    case 5:
                                        tClas = "Class " + whc.Value;
                                        tColr = Color.Purple;
                                        break;
                                    case 6:
                                        tClas = "Class " + whc.Value;
                                        tColr = Color.Purple;
                                        break;
                                    case 7:
                                        tClas = "Hi-Sec";
                                        tColr = Color.LimeGreen;
                                        break;
                                    case 8:
                                        tClas = "Low-Sec";
                                        tColr = Color.Orange;
                                        break;
                                    default:
                                        tClas = "Null-Sec";
                                        tColr = Color.Red;
                                        break;
                                }
                                break;
                            case 1382:
                                hrsLife = whc.Value / 60;
                                break;
                            case 1383:
                                maxMass = String.Format("{0:#,0.##}", whc.Value) + " kg";
                                massMult = whc.Value;
                                break;
                            case 1384:
                                mrv = whc.Value;
                                massRegen = String.Format("{0:#,0.##}", whc.Value) + " kg";
                                break;
                            case 1385:
                                jumpMass = String.Format("{0:#,0.##}", whc.Value) + " kg";
                                break;
                            case 1457:
                                break;
                            default:
                                break;
                        }

                    }
                }
                else
                {
                    tClas = "Unknown";
                    hrsLife = 24;
                    maxMass = "To Wormhole";
                    massRegen = "Wormhole Junction";
                    jumpMass = "See Other Side";
                    tColr = Color.Purple;
                    mrv = 0;
                }

                if (mrv > 0)
                    tTip = "Ship: " + jumpMass + " | Max: " + maxMass + " | Regen: " + massRegen + " | Life: " + hrsLife + " Hours to " + tClas;
                else
                    tTip = "Ship: " + jumpMass + " | Max: " + maxMass + " | Life: " + hrsLife + " Hours to " + tClas;

                tTip += "\n------------------------------------------------------------\n";
                tTip += "Life Cycle Not Begun - Just forming " + hrsLife + " hours Left\n";
                tTip += "Probably won't Last Another Day - Greater than " + (hrsLife / 4) + " hours Left\n";
                tTip += "Reaching the End - Less than " + (hrsLife / 4) + " hours Left\n";
                tTip += "------------------------------------------------------------\n";
                tTip += "Not Significantly Disrupted - Over " + String.Format("{0:#,0.##}", (massMult * 0.45)) + " Mass Left\n";
                tTip += "Stability has benn Reduced - Less than " + String.Format("{0:#,0.##}", (massMult * 0.45)) + " Mass Left\n";
                tTip += "Mass Critically Disrupte - Less than " + String.Format("{0:#,0.##}", (massMult * 0.05)) + " Mass Left";

                whType = new System.Windows.Forms.Label();
                whType.Font = new System.Drawing.Font("Tahoma", 10, FontStyle.Bold);
                whType.Text = whName;
                whType.ForeColor = tColr;
                whType.BackColor = Color.Transparent;
                tt_MyTips.SetToolTip(whType, tTip);
                tt_MyTips.IsBalloon = true;
                whType.AutoSize = false;
                whType.Width = 50;
                whType.Height = 15;
                whType.TextAlign = ContentAlignment.MiddleCenter;
                whType.Location = new Point((10 + (50 * locX)), (5 + (16 * locY)));

                whType.Click += new EventHandler(WH_InfoText_OnClick);

                gp_WHPossible.Controls.Add(whType);

                locX++;
                if (locX > 7)
                {
                    locX = 0;
                    locY++;
                }
            }
        }

        private void WH_InfoText_OnClick(object sender, EventArgs e)
        {
            string infoText;
            System.Windows.Forms.Label lCont;

            lCont = (System.Windows.Forms.Label)sender;
            infoText = tt_MyTips.GetToolTip(lCont);

            MessageBox.Show(infoText, lCont.Text + " - Information", MessageBoxButtons.OK);
        }

        private void cbx_WHType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string whName, tClas;
            long hrsLife;

            whName = cbx_WHType.Text;

            if (PlugInData.Misc.WH_Nm2Cls.ContainsKey(whName))
            {
                if (PlugInData.Misc.WH_Classes.ContainsKey(PlugInData.Misc.WH_Nm2Cls[whName]))
                {
                    foreach (var whc in PlugInData.Misc.WH_Classes[PlugInData.Misc.WH_Nm2Cls[whName]])
                    {
                        switch (whc.Key)
                        {
                            case 1381:
                                if (whc.Value > 6)
                                {
                                    if (whc.Value.Equals(7))
                                        tClas = " Hi-Sec";
                                    else if (whc.Value.Equals(8))
                                        tClas = " Low-Sec";
                                    else
                                        tClas = " Null-Sec";
                                }
                                else
                                    tClas = " " + whc.Value;

                                lbx_WHClass.Text = tClas;
                                break;
                            case 1382:
                                hrsLife = whc.Value / 60;
                                lbx_WHLife.Text = hrsLife + " Hours";
                                break;
                            case 1383:
                                lbx_WHMaxMass.Text = String.Format("{0:#,0.##}", whc.Value) + " kg";
                                break;
                            case 1384:
                                lbx_MassRegen.Text = String.Format("{0:#,0.##}", whc.Value) + " kg";
                                break;
                            case 1385:
                                lbx_WHShipMass.Text = String.Format("{0:#,0.##}", whc.Value) + " kg";
                                break;
                            case 1457:
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    lbx_WHClass.Text = "Unknown";
                    lbx_WHLife.Text = "See Other Side";
                    lbx_WHMaxMass.Text = "";
                    lbx_MassRegen.Text = "";
                    lbx_WHShipMass.Text = "";
                }
            }
        }

        private void bi_GalaxyMap_Click(object sender, EventArgs e)
        {
            SetMapMode(false);
        }

        private void bi_WHMap_Click(object sender, EventArgs e)
        {
            SetMapMode(true);
        }

        private void SetMapMode(bool WHMode)
        {
            if (WHMode)
            {
                PlugInData.WHMapSelected = true;
                MiniMap.SmallMap_Refresh(WHMode);
                MainMapView.FlatDataChanged = true;
                MainMapView.InitializeMapVeiw();
                MainMapView.UpdateMapForChange();
                bi_MapModeSelect.Text = "Wormhole Map";
            }
            else
            {
                PlugInData.WHMapSelected = false;
                MiniMap.SmallMap_Refresh(WHMode);
                MainMapView.FlatDataChanged = true;
                MainMapView.InitializeMapVeiw();
                MainMapView.UpdateMapForChange();
                bi_MapModeSelect.Text = "Eve Galaxy Map";
            }
        }

        private void tsmi_RemoveWH_Click(object sender, EventArgs e)
        {
        }

        private void dgv_SystemStaticWH_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            string sysName = cb_SystemSelect.Text;
            SolarSystem ss;

            ss = PlugInData.GalMap.GetSystemByName(sysName);
            if (ss == null)
                return;

            e.Row.Cells["SID"].Value = ss.ID;
        }

        private void dgv_SystemStaticWH_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            combo = (ComboBox)e.Control;
            combo.SelectedIndexChanged += new EventHandler(combo_SelectedIndexChanged);
        }

        private void combo_SelectedIndexChanged(object senver, EventArgs e)
        {
            string whType = combo.Text;
            string tgtClas = "Unknown";

            if (PlugInData.Misc.WH_Nm2Cls.ContainsKey(whType))
            {
                tgtClas = PlugInData.Misc.GetTargetClassForTypeID(PlugInData.Misc.WH_Nm2Cls[whType]);
            }

            dgv_SystemStaticWH.Rows[dgv_SystemStaticWH.CurrentCell.RowIndex].Cells["TargetSystemClass"].Value = tgtClas;
        }

        
        #endregion

        private void b_ImportCorpsFromAlliances_Click(object sender, EventArgs e)
        {
            // Import corporations from the corp list file aquired at: http://www.eve-icsc.com/xml/corporationlist/
            //XmlDocument corpData;

            
            PlugInData.Misc.SaveMisc();
        }

        private void tsmi_AddCorpName_Click(object sender, EventArgs e)
        {

        }

 
    }
}
