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
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Timers;
using System.Linq;

namespace EveHQ.RouteMap
{
    #region Plug-in Interface Functions
    public partial class PlugInData : EveHQ.Core.IEveHQPlugIn
    {
        
        public enum JumpType
        {
            Cyno,
            Gate,
            Bridge,
            Beacon,
            CynoSafe,
            Undefined
        };

        public struct throttlePlayer
        {
            public long count;
            public DateTime last;

            public throttlePlayer(int ct)
            {
                count = ct;
                last = DateTime.Now;
            }

            public void IncCount()
            {
                count++;
                last = DateTime.Now;
            }
        };

        private const double LY_DIV = 9.4605284E+15 / 175;
        public static RouteMapMainForm RMMF;
        public static float AU = 149597870691; // In meters
        public static int JUMP_TARGET_SYSTEMS = 4219;
        public static double JUMP_BRIDGE_RANGE = 5.0;
        public static Dictionary<SolarSystem, List<SolarSystem>> UniverseGraph;
        public static SortedList<long, SystemCoordinates> SystemCoords;
        public static EveGalaxy GalMap;
        public static EveGalaxyAPI GalAPI;
        public static CapitalShips CapShips;
        public static MiscData Misc;
        public static ArrayList AllianceList;
        public static ArrayList SystemList;
        public static ArrayList LSSystemList;
        public static ArrayList SOVSystemList;
        public static ArrayList CorpList;
        public static ArrayList CCorpList;
        public static SortedList<long, string> CorpIDList;
        public static ArrayList FactionList;
        public static ArrayList DivisionList;
        public static ArrayList RegionList;
        public static SortedList<string, RNode> RCSelList = null;
        public static string RMapBase_Path;
        public static string RMapManage_Path;
        public static string RMapCache_Path;
        public static string RMapData_Path;
        public static string RMapExport_Path;
        public bool UseSerializableData = false;
        public static bool JKApiUpdated = false;
        public static string LastCacheRefresh = "1.99.12.999";
        public static ManualResetEvent doneEvent;
        public static ConfigData2 Config;
        public static Dictionary<long, CynoGenJam> CynoGenJamList;
        public static Dictionary<long, SortedList<string, JumpBridge>> JumpBridgeList;
        public static int numBusy;
        public static ds_SystemDetails SysD;
        public static bool WHMapSelected = false;
        public static API_Jump_Kill JKList;
        private System.Timers.Timer t_JKSystemUpdate;
        public static BackgroundWorker bgw_APIUpdate = new System.ComponentModel.BackgroundWorker();
        public static SortedList<string, throttlePlayer> ThrottleList;
        public static bool InitialSetupConfig = false;
        public static bool rebuild = false;
        public static string ImportText;

        //public static ds_CynoJB CynoJB;
        
        DateTime timeStart;
        Route GateRoute;
        Route JumpRoute;
        private List<Vertex> CurrentGateRoute;
        private List<Vertex> CurrentJumpRoute;

        public static readonly long[] SkipRegions = new long[]{
                                            10000004, 10000017, 10000019,
                                            11000001, 11000002, 11000003, 11000004,
                                            11000005, 11000006, 11000007, 11000008,
                                            11000009, 11000010, 11000011, 11000012,
                                            11000013, 11000014, 11000015, 11000016,
                                            11000017, 11000018, 11000019, 11000020,
                                            11000021, 11000022, 11000023, 11000024,
                                            11000025, 11000026, 11000027, 11000028,
                                            11000029, 11000030
                                        };

        public static readonly long[] SkipNonWHRegions = new long[]{
                                            10000004, 10000017, 10000019
                                        };

        public static readonly long[] WHRegions = new long[]{
                                            11000001, 11000002, 11000003, 11000004,
                                            11000005, 11000006, 11000007, 11000008,
                                            11000009, 11000010, 11000011, 11000012,
                                            11000013, 11000014, 11000015, 11000016,
                                            11000017, 11000018, 11000019, 11000020,
                                            11000021, 11000022, 11000023, 11000024,
                                            11000025, 11000026, 11000027, 11000028,
                                            11000029, 11000030
                                        };


        #region Start Up and Building

        public Boolean EveHQStartUp()
        {
            StreamReader sr;
            string cacheVers, iname;
            DateTime startT, endT;
            TimeSpan runT;
            string strSQL;
            DataSet regnData;
            bool indiv = false;
            bool buildGalMap = false;
            int rID;
           
            startT = DateTime.Now;

            GalMap = new EveGalaxy();
            GalAPI = new EveGalaxyAPI();
            CapShips = new CapitalShips();
            Misc = new MiscData();
            SysD = new ds_SystemDetails();
            //CynoJB = new ds_CynoJB();
            AllianceList = new ArrayList();
            SystemCoords = new SortedList<long, SystemCoordinates>();
            CynoGenJamList = new Dictionary<long, CynoGenJam>();
            JumpBridgeList = new Dictionary<long,SortedList<string,JumpBridge>>();
            JKList = new API_Jump_Kill();
            ThrottleList = new SortedList<string, throttlePlayer>();
            ImportText = "";

            // 
            // bgw_APIUpdate
            // 
            bgw_APIUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_APIUpdate_DoWork);
            bgw_APIUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_APIUpdate_RunWorkerCompleted);

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
            RMapCache_Path = Path.Combine(RMapManage_Path, "Cache");
            RMapExport_Path = Path.Combine(RMapManage_Path, "Exports");

            if (!Directory.Exists(RMapManage_Path))
                Directory.CreateDirectory(RMapManage_Path);

            if (!Directory.Exists(RMapData_Path))
                Directory.CreateDirectory(RMapData_Path);

            if (!Directory.Exists(RMapCache_Path))
                Directory.CreateDirectory(RMapCache_Path);

            if (!Directory.Exists(RMapExport_Path))
                Directory.CreateDirectory(RMapExport_Path);

            // Check for cache folder
            if (Directory.Exists(RMapCache_Path))
            {
                // Check for Last Version Text File
                if (File.Exists(Path.Combine(RMapCache_Path, "version.txt")))
                {
                    sr = new StreamReader(Path.Combine(RMapCache_Path, "version.txt"));
                    cacheVers = sr.ReadToEnd();
                    sr.Close();

                    if (IsUpdateAvailable(cacheVers, LastCacheRefresh))
                    {
                        Directory.Delete(RMapCache_Path, true);
                        UseSerializableData = false;
                    }
                    else
                        UseSerializableData = true;
                }
                else
                {
                    Directory.Delete(RMapCache_Path, true);
                    UseSerializableData = false;
                }
            }

            startT = DateTime.Now;
            LoadJKHist();

            // Load JumpKill History
            if (!bgw_APIUpdate.IsBusy)
            {
                bgw_APIUpdate.RunWorkerAsync();
            }

            if (!UseSerializableData)
            {
                // Load Galaxy Data - if it exists
                GalMap.LoadGalaxy();

                strSQL = "SELECT * FROM mapRegions;";
                regnData = EveHQ.Core.DataFunctions.GetData(strSQL);
                if (!regnData.Equals(System.DBNull.Value))
                {
                    if (regnData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in regnData.Tables[0].Rows)
                        {
                            rID = Convert.ToInt32(row.ItemArray[0]);

                            if (SkipRegions.Contains(rID))
                                row.Delete();
                        }
                        regnData.Tables[0].AcceptChanges();
                    }
                }

                if (!regnData.Equals(System.DBNull.Value))
                {
                    if (regnData.Tables[0].Rows.Count > 0)
                    {
                        if (GalMap.GalData.Regions.Count == 0)
                            numBusy = regnData.Tables[0].Rows.Count + 4;
                        else
                            numBusy = 3;

                        indiv = true;
                        doneEvent = new ManualResetEvent(false);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(GalAPI.EveGalaxyAPI_UpdateAPIData));
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CapShips.LoadShipListFromDB));
                        ThreadPool.QueueUserWorkItem(new WaitCallback(Misc.LoadAllMiscData));

                        if (GalMap.GalData.Regions.Count == 0)
                        {
                            buildGalMap = true;
                            ThreadPool.QueueUserWorkItem(new WaitCallback(GalMap.UpdateGalaxyData), 1);

                            foreach (DataRow row in regnData.Tables[0].Rows)
                            {
                                rID = Convert.ToInt32(row.ItemArray[0]);

                                ThreadPool.QueueUserWorkItem(new WaitCallback(GalMap.UpdateRegionData), rID);
                            }
                        }

                        doneEvent.WaitOne();
                    }
                }

                if (!indiv)
                {
                    // Load new Database
                    doneEvent = new ManualResetEvent(false);
                    if (GalMap.GalData.Regions.Count == 0)
                    {
                        numBusy = 4;
                        buildGalMap = true;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(GalMap.UpdateGalaxyData), 0);
                    }
                    else
                        numBusy = 3;

                    ThreadPool.QueueUserWorkItem(new WaitCallback(GalAPI.EveGalaxyAPI_UpdateAPIData));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CapShips.LoadShipListFromDB));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Misc.LoadAllMiscData));
                    doneEvent.WaitOne();
                }

                if (buildGalMap)
                {
                    // Build ConstJumpDriveAdj
                    GalMap.ComputeConstJumpDriveAdj();
                    // Build Universe Graph
                    CreateUniverseGraph();
                    // Save all my new Data
                    GalMap.SaveGalaxy(LastCacheRefresh);
                    // Save Universe Graph to Disk
                    SaveUniverseGraphToDisk();
                    // Load Misc Data Files
                    Misc.LoadMisc();
                }
                else
                {
                    // Update LastCacheRefresh Always here
                    GalMap.UpdateLastCacheRefresh(LastCacheRefresh);
                    // Load Universe Graph
                    LoadUniverseGraphFromDisk();
                    // Load System Coordinates
                    LoadSystemCoordsFromDisk();
                    // Load Ship Data
                    CapShips.LoadShipListFromDisk();
                    // Load Misc Data Files
                    Misc.LoadMisc();
                }
            }
            else
            {
                numBusy = 1;

                doneEvent = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(GalAPI.EveGalaxyAPI_UpdateAPIData));
                // Load Galaxy Data - if it exists
                GalMap.LoadGalaxy();
                // Load Universe Graph
                LoadUniverseGraphFromDisk();
                // Load System Coordinates
                LoadSystemCoordsFromDisk();
                // Load Ship Data
                CapShips.LoadShipListFromDisk();
                // Load Misc Data
                Misc.LoadMisc();

                doneEvent.WaitOne();
            }

            foreach (var A in PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances)
                AllianceList.Add(A.Value.name);

            // Add any known values to these lists - as they could have been removed, etc... (especially alliances)
            DataRow[] Selct = SysD.MoonTable.Select("CName<>''");
            foreach (DataRow cr in Selct)
            {
                iname = cr.ItemArray[6].ToString();
                if (!Misc.PlayerCorpList.Contains(iname))
                    Misc.PlayerCorpList.Add(iname);
            }

            Selct = SysD.MoonTable.Select("AName<>''");
            foreach (DataRow cr in Selct)
            {
                iname = cr.ItemArray[6].ToString();
                if (!AllianceList.Contains(iname))
                    AllianceList.Add(iname);
            }

            LoadSystemDetailsFromDisk();
            LoadRegionConstSel();
            BuildSystemListing();

            Config = new ConfigData2();
            LoadConfigFromDisk();
            LoadJBCynoFromDisk();

            // Timer Setup
            t_JKSystemUpdate = new System.Timers.Timer();
            t_JKSystemUpdate.Elapsed += new ElapsedEventHandler(UdateMonitorInformation);
            t_JKSystemUpdate.AutoReset = true;
            t_JKSystemUpdate.Interval = 300000; // check every 5 minutes
            t_JKSystemUpdate.Enabled = true;

            endT = DateTime.Now;
            runT = endT.Subtract(startT);
            return true;
        }

        private void UdateMonitorInformation(object sender, EventArgs e)
        {
            if (!bgw_APIUpdate.IsBusy)
            {
                bgw_APIUpdate.RunWorkerAsync();
            }
        }

        private void bgw_APIUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            // Load JumpKill History
            LoadJKHist();
            JKList.CheckAndReadInNewAPIData();
        }

        private void bgw_APIUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SaveJKHist();
         }

        #region Nested type: RomanDigit

        private enum RomanDigit
        {
            I = 1,
            V = 5,
            X = 10,
            L = 50,
            C = 100,
            D = 500,
            M = 1000
        }

        #endregion
        
        // Converts a Roman numerals value into an integer
        public static int RomanToNumber(string roman)
        {
            // Rule 7
            roman = roman.ToUpper().Trim();
            if (roman == "N")
                return 0;
            // Rule 4
            if (roman.Split('V').Length > 2 ||
                roman.Split('L').Length > 2 ||
                roman.Split('D').Length > 2)
                throw new ArgumentException("Rule 4");
            // Rule 1
            int count = 1;
            char last = 'Z';
            foreach (char numeral in roman)
            {
                // Valid character?
                // CCP probably using something dumb
                if ("IVXLCDM".IndexOf(numeral) == -1)
                    return 0;
                // Duplicate?
                if (numeral == last)
                {
                    count++;
                    if (count == 4)
                        throw new ArgumentException("Rule 1");
                }
                else
                {
                    count = 1;
                    last = numeral;
                }
            }
            // Create an ArrayList containing the values
            int ptr = 0;
            var values = new List<int>();
            int maxDigit = 1000;
            while (ptr < roman.Length)
            {
                // Base value of digit
                char numeral = roman[ptr];
                int digit = (int)Enum.Parse(typeof(RomanDigit), numeral.ToString());
                // Rule 3
                if (digit > maxDigit)
                    throw new ArgumentException("Rule 3");
                // Next digit
                int nextDigit;
                if (ptr < roman.Length - 1)
                {
                    char nextNumeral = roman[ptr + 1];
                    nextDigit = (int)Enum.Parse(typeof(RomanDigit), nextNumeral.ToString());
                    if (nextDigit > digit)
                    {
                        if ("IXC".IndexOf(numeral) == -1 ||
                            nextDigit > (digit * 10) ||
                            roman.Split(numeral).Length > 3)
                            throw new ArgumentException("Rule 3");
                        maxDigit = digit - 1;
                        digit = nextDigit - digit;
                        ptr++;
                    }
                }
                values.Add(digit);
                // Next digit
                ptr++;
            }
            // Rule 5
            for (int i = 0; i < values.Count - 1; i++)
            {
                if (values[i] < values[i + 1])
                    throw new ArgumentException("Rule 5");
            }
            // Rule 2
            int total = 0;
            foreach (int digit in values)
                total += digit;
            return total;
        }

        public static string ToRoman(int number)
        {
            if (-9999 >= number || number >= 9999)
            {
                throw new ArgumentOutOfRangeException("number");
            }

            if (number == 0)
            {
                return "NUL";
            }

            StringBuilder sb = new StringBuilder(10);

            if (number < 0)
            {
                sb.Append('-');
                number *= -1;
            }

            string[,] table = new string[,] { 
                    { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" }, 
                    { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" }, 
                    { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" },
                    { "", "M", "MM", "MMM", "M(V)", "(V)", "(V)M", "(V)MM", "(V)MMM", "M(X)" } 
                };

            for (int i = 1000, j = 3; i > 0; i /= 10, j--)
            {
                int digit = number / i;
                sb.Append(table[j, digit]);
                number -= digit * i;
            }

            return sb.ToString();
        }
        
        public static string GetPlanetMoonNumbersFromName(string name)
        {
            string[] pmdat;
            int pNum;

            pmdat = name.Split(' ');

            pNum = RomanToNumber(pmdat[1]);

            return pNum + "-" + pmdat[4];
        }

        public void BuildSystemListing()
        {
            RNode rn;
            CNode cn;
            SystemList = new ArrayList();
            LSSystemList = new ArrayList();
            SOVSystemList = new ArrayList();
            CorpList = new ArrayList();
            CCorpList = new ArrayList();
            FactionList = new ArrayList();
            DivisionList = new ArrayList();
            RegionList = new ArrayList();
            CorpIDList = new SortedList<long, string>();

            try
            {
                if (RCSelList == null)
                {
                    RCSelList = new SortedList<string, RNode>();
                    foreach (var rgn in GalMap.GalData.Regions)
                    {
                        if (!SkipRegions.Contains(rgn.Value.ID))
                            rn = new RNode(rgn.Value.ID, rgn.Value.Name, true);
                        else
                            rn = null;

                        foreach (var cs in rgn.Value.Constellations.Values)
                        {
                            if ((!SkipRegions.Contains(rgn.Value.ID)) && (rn != null))
                            {
                                cn = new CNode(cs.ID, rgn.Value.ID, cs.Name, true);
                                rn.Constellations.Add(cn.Name, cn);
                            }

                            foreach (var ss in cs.Systems)
                            {
                                SystemList.Add(ss.Value.Name);
                                if (ss.Value.Security <= 0)
                                {
                                    SOVSystemList.Add(ss.Value.Name);
                                }
                                if (ss.Value.Security < 0.45)
                                    LSSystemList.Add(ss.Value.Name);
                            }
                        }

                        if (rn != null)
                            RCSelList.Add(rn.Name, rn);
                    }
                    SaveRegionConstSel();
                }
                else
                {
                    foreach (var rgn in GalMap.GalData.Regions)
                    {
                        if (!WHRegions.Contains(rgn.Value.ID))
                            RegionList.Add(rgn.Value.Name);

                        foreach (var cs in rgn.Value.Constellations)
                        {
                            foreach (var ss in cs.Value.Systems)
                            {
                                SystemList.Add(ss.Value.Name);
                                if (ss.Value.Security <= 0)
                                {
                                    SOVSystemList.Add(ss.Value.Name);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Cannot create Region or Constellation Selection Listing", "Yell at Sherk!!!");
            }

            foreach (SolarSystem ss in GalAPI.Galaxy_API.ConqStationAPI.Systems.Values)
            {
                foreach (ConqStation cs in ss.ConqStations.Values)
                    if (!CCorpList.Contains(cs.CorpName))
                        CCorpList.Add(cs.CorpName);
            }

            foreach (string fact in Misc.Factions.Values)
                FactionList.Add(fact);

            foreach (var corp in Misc.Corporations.Values)
            {
                CorpIDList.Add(Convert.ToInt64(corp.ID), corp.name);
                CorpList.Add(corp.name);
            }

            foreach (string div in Misc.Divisions.Values)
                DivisionList.Add(div);

            SystemList.Sort();
            SOVSystemList.Sort();
            CorpList.Sort();
            CCorpList.Sort();
            FactionList.Sort();
            DivisionList.Sort();
            RegionList.Sort();
        }

        public static void RebuildGalaxyData()
        {
            string strSQL;
            DataSet regnData;
            bool indiv = false;
            
            int rID;

            rebuild = true;

            GalMap.GalData.Regions.Clear();
            GalMap.GalData.StarGates.Clear();
            GalMap.GalData.ResearchTypes = new ArrayList();
            GalMap.GalData.AgentTypes = new ArrayList();

            strSQL = "SELECT * FROM mapRegions;";
            regnData = EveHQ.Core.DataFunctions.GetData(strSQL);
            if (!regnData.Equals(System.DBNull.Value))
            {
                if (regnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in regnData.Tables[0].Rows)
                    {
                        rID = Convert.ToInt32(row.ItemArray[0]);

                        if (SkipNonWHRegions.Contains(rID))
                            row.Delete();
                    }
                    regnData.Tables[0].AcceptChanges();
                }
            }

            if (!regnData.Equals(System.DBNull.Value))
            {
                if (regnData.Tables[0].Rows.Count > 0)
                {
                    numBusy = regnData.Tables[0].Rows.Count + 1;

                    indiv = true;
                    doneEvent = new ManualResetEvent(false);

                    ThreadPool.QueueUserWorkItem(new WaitCallback(GalMap.UpdateGalaxyData), 1);

                    foreach (DataRow row in regnData.Tables[0].Rows)
                    {
                        rID = Convert.ToInt32(row.ItemArray[0]);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(GalMap.UpdateRegionData), rID);
                    }

                    doneEvent.WaitOne();
                }
            }

            if (!indiv)
            {
                // Load new Database
                doneEvent = new ManualResetEvent(false);
                numBusy = 1;

                ThreadPool.QueueUserWorkItem(new WaitCallback(GalMap.UpdateGalaxyData), 0);
                doneEvent.WaitOne();
            }

            // Build ConstJumpDriveAdj
            GalMap.ComputeConstJumpDriveAdj();

            // Build Universe Graph
            CreateUniverseGraph();

            // Save all my new Data
            GalMap.SaveGalaxy(LastCacheRefresh);

            // Save Universe Graph to Disk
            SaveUniverseGraphToDisk();

            // Rebuild System Coordinates (as appropriate)
            MakeSystemCoordsListing(true);

            rebuild = false;
        }

        private static void CreateUniverseGraph()
        {
            UniverseGraph = CynoRoute.BuildGraph();
        }

        public EveHQ.Core.PlugIn GetEveHQPlugInInfo()
        {
            EveHQ.Core.PlugIn EveHQPlugIn = new EveHQ.Core.PlugIn();

            EveHQPlugIn.Name = "EveHQ Route Map";
            EveHQPlugIn.Description = "Generate Maps, Routes, SoV, JB, Etc...";
            EveHQPlugIn.Author = "Jay Teague <aka: Sherksilver>";
            EveHQPlugIn.MainMenuText = "Map Manager";
            EveHQPlugIn.RunAtStartup = true;
            EveHQPlugIn.RunInIGB = true;
            EveHQPlugIn.MenuImage = Properties.Resources.plugin_icon;
            EveHQPlugIn.Version = Application.ProductVersion.ToString();

            return EveHQPlugIn;
        }

        #endregion

        #region In Game Browser

        public String IGBService(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            string cmd;
            
            timeStart = DateTime.Now;
            strHTML.Append(EveHQ.Core.IGB.IGBHTMLHeader(context, "RouteMap",0));
            strHTML.Append(RouteMapMenu(context));

            cmd = context.Request.Url.AbsolutePath.ToUpper();

            if (cmd.Equals("/EVEHQROUTEMAP") || cmd.Equals("/EVEHQROUTEMAP/"))
                strHTML.Append(MainPage(context));
            else if (cmd.Equals("/EVEHQROUTEMAP/GATEROUTE") || cmd.Equals("/EVEHQROUTEMAP/GATEROUTE/"))
                strHTML.Append(GateRoutePage(context));
            else if (cmd.Equals("/EVEHQROUTEMAP/JUMPROUTE") || cmd.Equals("/EVEHQROUTEMAP/JUMPROUTE/"))
                strHTML.Append(JumpRoutePage(context));
            else if (cmd.Contains("/EVEHQROUTEMAP/GR_PLOT"))
                strHTML.Append(PlotGateRoute(context));
            else if (cmd.Contains("/EVEHQROUTEMAP/JR_PLOT"))
                strHTML.Append(PlotJumpRoute(context));

            strHTML.Append(EveHQ.Core.IGB.IGBHTMLFooter(context));

            return strHTML.ToString();
        }

        public string PlotJumpRoute(System.Net.HttpListenerContext context)
        {
            string[] sp;
            SolarSystem sSys, dSys;
            Ship JumpShip;
            double minS, maxS;
            string isoType = "";
            bool useJB = false;
            bool useCG = false;
            bool useSS = false;
            bool useST = false;
            int num;
            int alID;
            double totalLO = 0, totalM3 = 0, totalISO = 0, totalDist = 0;
            RouteNode CurNode;
            //EveHQ.Core.Pilot EvePilot;
            StringBuilder strHTML = new StringBuilder();
            ArrayList s_jk = new ArrayList();

            sp = context.Request.Url.Query.Split('&');
            sp[0] = sp[0].Substring(sp[0].IndexOf('=') + 1);    // Ship
            sp[1] = sp[1].Substring(sp[1].IndexOf('=') + 1);    // Start System
            sp[2] = sp[2].Substring(sp[2].IndexOf('=') + 1);    // Dest System
            sp[3] = sp[3].Substring(sp[3].IndexOf('=') + 1);    // Min Sec Level
            sp[4] = sp[4].Substring(sp[4].IndexOf('=') + 1);    // Max Sec Level

            if (sp.Length > 5)
            {
                for (int x = 5; x < sp.Length; x++)
                {
                    if (sp[x].Contains("JB"))
                        useJB = true;
                    else if (sp[x].Contains("SS"))
                        useSS = true;
                    else if (sp[x].Contains("CG"))
                        useCG = true;
                    else if (sp[x].Contains("SF"))
                        useST = true;
                }
                sp[5] = sp[5].Substring(sp[5].IndexOf('=') + 1);    // Use JB (on = use, else do not)
                if (sp[5].Contains("on"))
                    useJB = true;
            }
            JumpShip = CapShips.Ships[sp[0]];
            sSys = GalMap.GetSystemByName(sp[1]);
            dSys = GalMap.GetSystemByName(sp[2]);
            minS = Convert.ToDouble(sp[3]);
            maxS = Convert.ToDouble(sp[4]);

            JumpRoute = new Route(sSys, dSys, new ArrayList(), new ArrayList(), new EveHQ.Core.Pilot(), JumpShip, minS, maxS);
            JumpRoute.ShipJumpRange = 11.25;
            JumpRoute.ShipFuelPerLY = 600;

            JumpRoute.PreferStation = useSS;
            JumpRoute.UseCynoBeacon = useCG;
            JumpRoute.UseJumpBridge = useJB;
            JumpRoute.UseCynoSafeTwr = useST;

            CynoRoute CynoRouteFinder = new CynoRoute();
            CurrentJumpRoute = CynoRouteFinder.GetIGBRoute(JumpRoute);

            // OK, I now have the route. Need to output it to the browser in a format similar to
            // what is used on the UI.
            num = 0;
            CurNode = new RouteNode();
            // Populate Computed Route Display
            for (int cs = 0; cs < CurrentJumpRoute.Count; cs++)
            {
                CurNode.JumpNum = cs;
                CurNode.from = CurrentJumpRoute[cs].SolarSystem;

                if (CurrentJumpRoute[cs].SolarSystem.Stations.Count > 0)
                    CurNode.Station = true;
                else if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(CurrentJumpRoute[cs].SolarSystem.ID))
                    CurNode.Station = true;
                else
                    CurNode.Station = false;

                CurNode.sec = Math.Round(CurrentJumpRoute[cs].SolarSystem.Security, 2);
                CurNode.distance = -1;

                isoType = JumpShip.GetIsoType();

                if (cs > 0)
                {
                    if (CurrentJumpRoute[cs].LYTraveled > JumpShip.JumpDistance)
                    {
                        CurNode.distance = Math.Round(CurrentJumpRoute[cs].LYTraveled - CurrentJumpRoute[cs - 1].LYTraveled, 2);
                        totalDist += Math.Round(CurrentJumpRoute[cs].LYTraveled - CurrentJumpRoute[cs - 1].LYTraveled, 2);
                    }
                    else
                    {
                        CurNode.distance = Math.Round(CurrentJumpRoute[cs].LYTraveled, 2);
                        totalDist += Math.Round(CurrentJumpRoute[cs].LYTraveled, 2);
                    }
                }
                else
                    CurNode.distance = 0;
                
                if ((CurrentJumpRoute[cs].JumpTyp == PlugInData.JumpType.Bridge) && (cs > 0))
                {
                    CurNode.LO = CurrentJumpRoute[cs].LOCost;
                    totalLO += CurrentJumpRoute[cs].LOCost;
                    CurNode.ISO = 0;
                }
                else if (cs > 0)
                {
                    CurNode.ISO = CurrentJumpRoute[cs].FuelCost;
                    CurNode.IsoType = isoType;
                    totalISO += CurrentJumpRoute[cs].FuelCost;
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

                CurNode.JumpTyp = CurrentJumpRoute[cs].JumpTyp;
                if (CurrentJumpRoute[cs].JumpTyp == PlugInData.JumpType.Bridge)
                {
                    CurNode.JumpTT = "(JB) " + CurrentJumpRoute[cs].Bridge.FromMoon + " [PW: " + CurrentJumpRoute[cs].Bridge.Password + " ]";
                    CurNode.JumpTT += "<br>(JB) " + CurrentJumpRoute[cs].Bridge.ToMoon + " [PW: " + CurrentJumpRoute[cs].Bridge.Password + " ]";
                }
                else if (CurrentJumpRoute[cs].JumpTyp == PlugInData.JumpType.Cyno)
                    CurNode.JumpTT = "Cyno";
                else if (CurrentJumpRoute[cs].JumpTyp == PlugInData.JumpType.Beacon)
                    CurNode.JumpTT = "Beacon";
                else if (CurrentJumpRoute[cs].JumpTyp == PlugInData.JumpType.Gate)
                    CurNode.JumpTT = "Gate";
                else if (CurrentJumpRoute[cs].JumpTyp == PlugInData.JumpType.Undefined)
                    CurNode.JumpTT = "Lost!";

                CurNode.constellation = PlugInData.GalMap.GalData.Regions[CurrentJumpRoute[cs].SolarSystem.RegionID].Constellations[CurrentJumpRoute[cs].SolarSystem.ConstID].Name;   // Constellation
                CurNode.region = PlugInData.GalMap.GalData.Regions[CurrentJumpRoute[cs].SolarSystem.RegionID].Name;         // Region

                if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(CurrentJumpRoute[cs].SolarSystem.ID))
                {
                    alID = PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[CurrentJumpRoute[cs].SolarSystem.ID].allianceID;
                    if (PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances.ContainsKey(alID))
                    {
                        CurNode.sov = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].name;
                        CurNode.sTic = PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[alID].ticker;
                    }
                }

                s_jk = PlugInData.JKList.GetLatestJumpKillForSID(CurrentJumpRoute[cs].SolarSystem.ID);
                CurNode.Kills = s_jk[1] + " Ships, " + s_jk[2] + " Pods, " + s_jk[3] + " NPCs";
                CurNode.Jumps = s_jk[0].ToString();

                strHTML.Append(PlotRouteNode(CurNode));

                num++;
            }
            CurNode.JumpNum = -1;
            CurNode.ISO = -1;
            CurNode.distance = -1;
            CurNode.LO = totalLO;
            CurNode.cargo = totalM3;
            CurNode.Jumps = (CurrentJumpRoute.Count - 1).ToString();

            strHTML.Insert(0, PlotRouteTotals(CurNode));
            return strHTML.ToString();
        }

        public string PlotGateRoute(System.Net.HttpListenerContext context)
        {
            string[] sp;
            SolarSystem sSys, dSys;
            Ship JumpShip;
            double minS, maxS;
            bool useJB = false;
            int num;
            int alID;
            double totalLO = 0, totalM3 = 0;
            RouteNode CurNode;
            //EveHQ.Core.Pilot EvePilot;
            StringBuilder strHTML = new StringBuilder();
            ArrayList s_jk = new ArrayList();

            sp = context.Request.Url.Query.Split('&');
            sp[0] = sp[0].Substring(sp[0].IndexOf('=') + 1);    // Ship
            sp[1] = sp[1].Substring(sp[1].IndexOf('=') + 1);    // Start System
            sp[2] = sp[2].Substring(sp[2].IndexOf('=') + 1);    // Dest System
            sp[3] = sp[3].Substring(sp[3].IndexOf('=') + 1);    // Min Sec Level
            sp[4] = sp[4].Substring(sp[4].IndexOf('=') + 1);    // Max Sec Level
            if (sp.Length > 5)
            {
                sp[5] = sp[5].Substring(sp[5].IndexOf('=') + 1);    // Use JB (on = use, else do not)
                if (sp[5].Contains("on"))
                    useJB = true;
           }
            JumpShip = CapShips.Ships[sp[0]];
            sSys = GalMap.GetSystemByName(sp[1]);
            dSys = GalMap.GetSystemByName(sp[2]);
            minS = Convert.ToDouble(sp[3]);
            maxS = Convert.ToDouble(sp[4]);

            GateRoute = new Route(sSys, dSys, new ArrayList(), new ArrayList(), new EveHQ.Core.Pilot(), JumpShip, minS, maxS);
            GateRoute.ShipJumpRange = 0;
            GateRoute.ShipFuelPerLY = 0;

            GateRoute.PreferStation = true;
            GateRoute.UseCynoBeacon = false;
            if (useJB)
                GateRoute.UseJumpBridge = true;
            else
                GateRoute.UseJumpBridge = false;
            GateRoute.UseCynoSafeTwr = false;

            GateRoute GateRouteFinder = new GateRoute();
            CurrentGateRoute = GateRouteFinder.GetIGBRoute(GateRoute);

            // OK, I now have the route. Need to output it to the browser in a format similar to
            // what is used on the UI.
            num = 0;
            CurNode = new RouteNode();
            // Populate Computed Route Display
            for (int cs = 0; cs < CurrentGateRoute.Count; cs++)
            {
                CurNode.JumpNum = cs;
                CurNode.from = CurrentGateRoute[cs].SolarSystem;

                if (CurrentGateRoute[cs].SolarSystem.Stations.Count > 0)
                    CurNode.Station = true;
                else if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(CurrentGateRoute[cs].SolarSystem.ID))
                    CurNode.Station = true;
                else
                    CurNode.Station = false;

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
                    CurNode.JumpTT += "<br>(JB) " + CurrentGateRoute[cs].Bridge.ToMoon + " [PW: " + CurrentGateRoute[cs].Bridge.Password + " ]";
                }
                else if (CurrentGateRoute[cs].JumpTyp == PlugInData.JumpType.Cyno)
                    CurNode.JumpTT = "Cyno";
                else if (CurrentGateRoute[cs].JumpTyp == PlugInData.JumpType.Beacon)
                    CurNode.JumpTT = "Beacon";
                else if (CurrentGateRoute[cs].JumpTyp == PlugInData.JumpType.Gate)
                    CurNode.JumpTT = "Gate";
                else if (CurrentGateRoute[cs].JumpTyp == PlugInData.JumpType.Undefined)
                    CurNode.JumpTT = "Lost!";

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

                strHTML.Append(PlotRouteNode(CurNode));

                num++;
            }
            CurNode.JumpNum = -1;
            CurNode.ISO = -1;
            CurNode.distance = -1;
            CurNode.LO = totalLO;
            CurNode.cargo = totalM3;
            CurNode.Jumps = (CurrentGateRoute.Count - 1).ToString();

            strHTML.Insert(0, PlotRouteTotals(CurNode));
            return strHTML.ToString();
        }

        public string PlotRouteTotals(RouteNode rn)
        {
            string retStr = "";

            // Put a blank line between each Node
            retStr += "<br>";
            retStr += "<table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"100%\" cellpadding=\"1\" cellspacing=\"1\">";
            retStr += "<tr>";
            retStr += "<td width=\"5%\"><b><font size=\"3\" color=\"blue\"></a></font></b></td>";
            retStr += "<td width=\"35%\"><b><font size=\"3\" color=\"green\"></a></font></b></td>";
            retStr += "<td width=\"40%\"><b><font size=\"3\" color=\"blue\"></a></font></b></td>";
            retStr += "<td width=\"15%\"><b><font size=\"3\" color=\"blue\"></a></font></b></td>";

            retStr += "<td width=\"5%\"><b><font size=\"3\" color=\"#FF0000\"></a></font></b></td>";
            retStr += "</tr>";

            retStr += "<tr><td colspan=2><b><font size=\"3\" color=\"green\">Total Jumps: " + rn.Jumps + "</a></font></b></td>";
            retStr += "<td><b><font size=\"3\" color=\"blue\">LO: " + rn.LO + ", Cargo: " + rn.cargo + " m3</a></font></b></td>";        
            retStr += "<td colspan=2><b><font size=\"3\" color=\"blue\"></a></font></b></td>";
            retStr += "</tr>";

            retStr += "<tr><td colspan = 2><b><font size=\"3\" color=\"blue\"></a></font></b></td>";
            retStr += "<td colspan=3><b><font size=\"3\" color=\"green\"></a></font></b></td>";

            retStr += "</tr></table>";

            return retStr;
        }

        public string PlotRouteNode(RouteNode rn)
        {
            string retStr = "";

            // Put a blank line between each Node
            retStr += "<br>";
            retStr += "<table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"100%\" cellpadding=\"1\" cellspacing=\"1\">";
            retStr += "<tr>";
            retStr += "<td width=\"5%\"><b><font size=\"3\" color=\"blue\">" + rn.JumpNum + "</a></font></b></td>";
            retStr += "<td width=\"35%\"><b><font size=\"3\" color=\"green\">" + rn.from.Name + "</a></font></b></td>";
            retStr += "<td width=\"40%\"><b><font size=\"3\" color=\"blue\">" + rn.region + ", " + rn.constellation + "</a></font></b></td>";
            retStr += "<td width=\"15%\"><b><font size=\"3\" color=\"blue\">" + rn.sTic + "</a></font></b></td>";

            if (rn.sec <= 0)
            {
                // red
                retStr += "<td width=\"5%\"><b><font size=\"3\" color=\"#FF0000\">" + rn.sec + "</a></font></b></td>";
            }
            else if ((rn.sec > 0) && (rn.sec <= 0.25))
            {
                // orange
                retStr += "<td width=\"5%\"><b><font size=\"3\" color=\"#F87217\">" + rn.sec + "</a></font></b></td>";
            }
            else if ((rn.sec > 0.25) && (rn.sec <= 0.44))
            {
                // dk yellow
                retStr += "<td width=\"5%\"><b><font size=\"3\" color=\"#FDD017\">" + rn.sec + "</a></font></b></td>";
            }
            else if ((rn.sec > 0.44) && (rn.sec <= 0.55))
            {
                // lt yellow
                retStr += "<td width=\"5%\"><b><font size=\"3\" color=\"#B1FB17\">" + rn.sec + "</a></font></b></td>";
            }
            else if ((rn.sec > 0.55) && (rn.sec <= 0.8))
            {
                // green
                retStr += "<td width=\"5%\"><b><font size=\"3\" color=\"#00FF00\">" + rn.sec + "</a></font></b></td>";
            }
            else if (rn.sec > 0.8)
            {
                // blue
                retStr += "<td width=\"5%\"><b><font size=\"3\" color=\"#00FFFF\">" + rn.sec + "</a></font></b></td>";
            }
            retStr += "</tr>";

            retStr += "<tr><td colspan=2><b><font size=\"3\" color=\"green\">" + rn.JumpTT + "</a></font></b></td>";

            if (rn.ISO > 0)
                retStr += "<td><b><font size=\"3\" color=\"blue\">Distance: " + rn.distance + " Ly | " + rn.IsoType + " Iso: " + rn.ISO + " | Cargo: " + rn.cargo + " m3</a></font></b></td>";
            else if (rn.LO > 0)
                retStr += "<td><b><font size=\"3\" color=\"blue\">LO: " + rn.LO + " | Cargo: " + rn.cargo + " m3</a></font></b></td>";
            else
                retStr += "<td><b><font size=\"3\" color=\"blue\"></a></font></b></td>";

            if (rn.Station)
                retStr += "<td colspan=2><b><font size=\"3\" color=\"green\">Station System</a></font></b></td>";
            else
                retStr += "<td colspan=2><b><font size=\"3\" color=\"blue\">No Station</a></font></b></td>";

            retStr += "</tr>";

            retStr += "<tr><td colspan = 2><b><font size=\"3\" color=\"blue\">Size: " + rn.from.GetSystemSize() + " Au</a></font></b></td>";
            retStr += "<td colspan=3><b><font size=\"3\" color=\"green\">Jumps: " + rn.Jumps + "   |    </font><font size=\"3\" color=\"red\">Kills: " + rn.Kills + "</a></font></b></td>";

            retStr += "</tr></table>";

            return retStr;
        }

        public string GateRoutePage(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();

            strHTML.Append("<p>Please select your Route parameters:<p>");
            strHTML.Append("<form method=\"GET\" action=\"/EveHQRouteMap/GR_PLOT\">");

            strHTML.Append("<p>Ship:&nbsp;&nbsp;&nbsp;<select name='SH' style='width: 200px;'>");
            foreach (Ship sh in CapShips.Ships.Values)
            {
                strHTML.Append("<option");
               strHTML.Append(">" + sh.Name + "</option>");
            }
            strHTML.Append("</select><br>");

            strHTML.Append("<p>Start System:&nbsp;&nbsp;&nbsp;<select name='ST' style='width: 200px;'>");
            foreach (string ss in SystemList)
            {
                strHTML.Append("<option");
                if (context.Request.Headers[9].Equals(ss))
                    strHTML.Append(" selected='selected'");

                strHTML.Append(">" + ss + "</option>");
            }
            strHTML.Append("</select><br>");

            strHTML.Append("<p>Dest. System:&nbsp;&nbsp;&nbsp;<select name='DS' style='width: 200px;'>");
            foreach (string ss in SystemList)
            {
                strHTML.Append("<option");
                strHTML.Append(">" + ss + "</option>");
            }
            strHTML.Append("</select><br><br>");

            strHTML.Append("Min Sec: <input type='text' size='15' value='-1.0' name='MnS'></input><br>");
            strHTML.Append("Max Sec: <input type='text' size='15' value='1.0' name='MxS'></input><br>");

            strHTML.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=CHECKBOX name='JB'>Use Jump Bridges<br>");

            strHTML.Append("<br><input type='submit' value='Get Route'></p></form>");

            return strHTML.ToString();
        }

        public string JumpRoutePage(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();

            strHTML.Append("<p>Please select your Route parameters:<p>");
            strHTML.Append("<form method=\"GET\" action=\"/EveHQRouteMap/JR_PLOT\">");

            strHTML.Append("<p>Ship:&nbsp;&nbsp;&nbsp;<select name='SH' style='width: 200px;'>");
            foreach (Ship sh in CapShips.Ships.Values)
            {
                strHTML.Append("<option");
                strHTML.Append(">" + sh.Name + "</option>");
            }
            strHTML.Append("</select><br>");

            strHTML.Append("<p>Start System:&nbsp;&nbsp;&nbsp;<select name='ST' style='width: 200px;'>");
            foreach (string ss in SystemList)
            {
                strHTML.Append("<option");
                if (context.Request.Headers[9].Equals(ss))
                    strHTML.Append(" selected='selected'");

                strHTML.Append(">" + ss + "</option>");
            }
            strHTML.Append("</select><br>");

            strHTML.Append("<p>Dest. System:&nbsp;&nbsp;&nbsp;<select name='DS' style='width: 200px;'>");
            foreach (string ss in LSSystemList)
            {
                strHTML.Append("<option");
                strHTML.Append(">" + ss + "</option>");
            }
            strHTML.Append("</select><br><br>");

            strHTML.Append("Min Sec: <input type='text' size='15' value='-1.0' name='MnS'></input><br>");
            strHTML.Append("Max Sec: <input type='text' size='15' value='1.0' name='MxS'></input><br>");

            strHTML.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=CHECKBOX name='JB'>Use Jump Bridges<br>");
            strHTML.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=CHECKBOX name='SS'>Use Station Systems<br>");
            strHTML.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=CHECKBOX name='CG'>Use Cyno Generators<br>");
            strHTML.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=CHECKBOX name='SF'>Use Safe Towers<br>");

            strHTML.Append("<br><input type='submit' value='Get Route'></p></form>");

            return strHTML.ToString();
        }

        public string MainPage(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();

            strHTML.Append("<br>");

            return strHTML.ToString();
        }

        public string RouteMapMenu(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            strHTML.Append("<font size=\"3\" color=\"gold\"><b> Route Map Online </b></font><br><br>");
            strHTML.Append("<font size=\"2\"><a href=/EveHQRouteMap>Route Map Home</a>  | <a href=/EveHQRouteMap/GateRoute>Gate Route</a>  | <a href=/EveHQRouteMap/JumpRoute>Jump Route</a></font>");
            strHTML.Append("<br><br>");

            return strHTML.ToString();
        }


        #endregion

        #region Load Save and Run

        public static bool CheckXML(XmlDocument apiXML)
        {
            if (apiXML != null)
            {
                XmlNodeList errlist = apiXML.SelectNodes("/eveapi/error");
                if (errlist.Count != 0)
                {
                    // Error - Now WTF do I do???
                    return false;
                }
            }
            return true;
        }

        public Form RunEveHQPlugIn()
        {
            // You need to make this form, it'll be the startup form for the plugin
            RMMF = new RouteMapMainForm();
            return RMMF;
        }

        private bool IsUpdateAvailable(string locVer, string remVer)
        {
            string[] local;
            string[] remot;
            bool retVal = false;

            if (locVer.Equals(remVer))
                return false;
            else
            {
                local = locVer.Split('.');
                remot = remVer.Split('.');

                for (int x = 0; x < 4; x++)
                {
                    if ((Convert.ToInt32(remot[x])) != (Convert.ToInt32(local[x])))
                    {
                        if ((Convert.ToInt32(remot[x])) > (Convert.ToInt32(local[x])))
                        {
                            return true;
                        }
                        else
                            retVal = false;
                    }
                }
            }

            return retVal;
        }

        public object GetPlugInData(object objData, int intDataType)
        {
            return null;
        }

        public static void LoadConfigFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;
            ConfigData cfgOld = new ConfigData();
            Config = new ConfigData2();

            fname = Path.Combine(RMapData_Path, "Configuration.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Config = (ConfigData2)myBf.Deserialize(cStr);
                }
                catch
                {
                    cStr.Close();
                    cStr = File.OpenRead(fname);
                    myBf = new BinaryFormatter();
                    // Need to check for, load, and convert an older config file
                    try
                    {
                        cfgOld = (ConfigData)myBf.Deserialize(cStr);

                        // If we get here, have an older configuration file
                        Config.ConvertOlderConfigData(cfgOld);
                        cStr.Close();
                        SaveConfigToDisk();
                    }
                    catch
                    {
                    }
                }
                cStr.Close();
            }
        }

        public static void SaveConfigToDisk()
        {
            string fname;

            if (InitialSetupConfig)
                return;

            fname = Path.Combine(RMapData_Path, "Configuration.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Config);
            pStream.Close();
        }

        public static void SaveRegionConstSel()
        {
            string fname;
            Stream pStream;
            BinaryFormatter pBF;

            fname = Path.Combine(RMapData_Path, "RC_Select.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, RCSelList);
            pStream.Close();
        }

        public static void LoadRegionConstSel()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(RMapData_Path, "RC_Select.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    RCSelList = (SortedList<string, RNode>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }
        }

        public static void SaveJKHist()
        {
            string fname;
            Stream pStream;
            BinaryFormatter pBF;

            try
            {
                fname = Path.Combine(RMapData_Path, "JK_History.bin");

                // Save the Serialized data to Disk
                pStream = File.Create(fname);
                pBF = new BinaryFormatter();
                pBF.Serialize(pStream, JKList);
                pStream.Close();
            }
            catch
            {
            }
        }

        public static void LoadJKHist()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(RMapData_Path, "JK_History.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    JKList = (API_Jump_Kill)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }
        }

        public static void LoadJBCynoFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;
            //ds_CynoJB.CynoTableRow ctr;
            SystemCelestials SC = new SystemCelestials();

            fname = Path.Combine(RMapData_Path, "JumpBridge.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    JumpBridgeList = (Dictionary<long, SortedList<string, JumpBridge>>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            fname = Path.Combine(RMapData_Path, "CynoGen.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    CynoGenJamList = (Dictionary<long, CynoGenJam>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            //foreach (CynoGenJam cgj in CynoGenJamList.Values)
            //{
            //    ctr = CynoJB.CynoTable.NewCynoTableRow();
            //    ctr["SID"] = cgj.GenSys.ID;
            //    ctr["MName"] = cgj.moon;
            //    SC.GetSystemCelestialForID(cgj.GenSys.ID);
            //    ctr["MID"] = SC.GetMoonIDForName(cgj.moon);
            //    ctr["SName"] = cgj.GenSys.Name;

            //    if (cgj.IsJammer)
            //        ctr["CType"] = "Jammer";
            //    else
            //        ctr["CType"] = "Generator";

            //    CynoJB.CynoTable.AddCynoTableRow(ctr);
            //}
        }

        public static JumpBridge DoesBridgeLinkExist(int s1ID, int s2ID)
        {
            if (!JumpBridgeList.ContainsKey(s1ID) && !JumpBridgeList.ContainsKey(s2ID))
                return null;

            if (JumpBridgeList.ContainsKey(s1ID))
            {
                foreach (JumpBridge JB in JumpBridgeList[s1ID].Values)
                {
                    if (JB.To.ID == s2ID)
                        return JB;
                }
            }
            if (JumpBridgeList.ContainsKey(s2ID))
            {
                foreach (JumpBridge JB in JumpBridgeList[s2ID].Values)
                {
                    if (JB.To.ID == s1ID)
                        return JB;
                }
            }

            return null;
        }

        public static void SaveJBCynoToDisk()
        {
            string fname;

            fname = Path.Combine(RMapData_Path, "JumpBridge.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, JumpBridgeList);
            pStream.Close();

            fname = Path.Combine(RMapData_Path, "CynoGen.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, CynoGenJamList);
            pStream.Close();
        }

        public static void LoadUniverseGraphFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(EveHQ.Core.HQ.appFolder, "UniverseGraph.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    UniverseGraph = (Dictionary<SolarSystem, List<SolarSystem>>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }
        }

        public static void SaveUniverseGraphToDisk()
        {
            string fname;

            fname = Path.Combine(EveHQ.Core.HQ.appFolder, "UniverseGraph.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, UniverseGraph);
            pStream.Close();
        }

        public static void MakeSystemCoordsListing(bool Update)
        {
            SystemCoordinates sc;
            Point oc;

            if (!Update)
                SystemCoords = new SortedList<long, SystemCoordinates>();

            // Go through all Systems - create the new System Coordinate Data Structure
            foreach (Region curRegion in GalMap.GalData.Regions.Values)
            {
                foreach (Constellation curConst in curRegion.Constellations.Values)
                {
                    foreach (SolarSystem ss in curConst.Systems.Values)
                    {
                        if (SystemCoords.ContainsKey(Convert.ToInt64(ss.ID)))
                        {
                            sc = SystemCoords[Convert.ToInt64(ss.ID)];
                            oc = new Point(Convert.ToInt32(Math.Round(ss.Coords.X / LY_DIV)), Convert.ToInt32(Math.Round(ss.Coords.Y / LY_DIV)));

                            if (!oc.Equals(sc.OrgCoord))
                                sc.Moved = true;
                            else
                                sc.Moved = false;

                            sc.BaseCoord = new Point(oc.X, oc.Y);
                        }
                        else
                        {
                            sc = new SystemCoordinates();
                            oc = new Point(Convert.ToInt32(Math.Round(ss.Coords.X / LY_DIV)), Convert.ToInt32(Math.Round(ss.Coords.Y / LY_DIV)));
                            sc.ID = Convert.ToInt64(ss.ID);

                            if (!oc.Equals(ss.OrgCoord))
                                sc.Moved = true;
                            else
                                sc.Moved = false;

                            sc.OrgCoord = new Point(oc.X, oc.Y);
                            sc.BaseCoord = new Point(oc.X, oc.Y);

                            SystemCoords.Add(sc.ID, sc);
                        }
                    }
                }
            }

            SaveSystemCoordsToDisk();
        }

        public static void LoadSystemCoordsFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(RMapData_Path, "SystemCoords.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    SystemCoords = (SortedList<long, SystemCoordinates>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }
            else
            {
                // File does not exist, make it
                MakeSystemCoordsListing(false);
            }
        }

        public static void SaveSystemCoordsToDisk()
        {
            string fname;

            fname = Path.Combine(RMapData_Path, "SystemCoords.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, SystemCoords);
            pStream.Close();
        }

        public static void LoadSystemDetailsFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;
            ds_SystemDetails mDS = new ds_SystemDetails();

            fname = Path.Combine(RMapData_Path, "SysDetail.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();
                try
                {
                    mDS = (ds_SystemDetails)myBf.Deserialize(cStr);
                }
                catch
                {
                    cStr.Close();
                }
                cStr.Close();
            }
            else
            {
                ConvertSystemDetails();
            }

            // Use a merge to easily allow the addition of new data tables and values
            SysD.Merge(mDS);
        }

        public static void SaveSystemDetailsToDisk()
        {
            string fname;

            fname = Path.Combine(RMapData_Path, "SysDetail.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, SysD);
            pStream.Close();

        }

        private static void ConvertSystemDetails()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;
            Dictionary<long, SystemDetail> OldSD = new Dictionary<long,SystemDetail>();
            SystemCelestials SC = new SystemCelestials();
            SolarSystem ss;
            DataRow[] sdr;
            
            fname = Path.Combine(RMapData_Path, "SysDetails.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    OldSD = (Dictionary<long, SystemDetail>)myBf.Deserialize(cStr);

                    // Got my data, now need to convert the PITA
                    SysD = new ds_SystemDetails();

                    foreach (var SDt in OldSD)
                    {
                        ss = PlugInData.GalMap.GetSystemByID(Convert.ToInt32(SDt.Key));
                        SC.GetSystemCelestial(ss);

                        sdr = SysD.SystemTable.Select("SID=" + ss.ID);

                        if (sdr.Length < 1)
                        {
                            ds_SystemDetails.SystemTableRow sr = SysD.SystemTable.NewSystemTableRow();
                            
                            sr.SID = ss.ID;
                            sr.CID = ss.ConstID;
                            sr.RID = ss.RegionID;

                            AddPlanetsAndDetails(ss.ID, SC, OldSD);
                            AddMoonsAndDetails(ss.ID, SC, OldSD, sr);

                            SysD.SystemTable.AddSystemTableRow(sr);
                        }
                     }
                }
                catch
                {
                }
                cStr.Close();
            }
            SaveSystemDetailsToDisk();
        }
               
        public static void AddPlanetsAndDetails(int sid, SystemCelestials sc, Dictionary<long, SystemDetail> OldSD)
        {
            bool planetExists;

            foreach (Planet p in sc.Planets.Values)
            {
                planetExists = false;

                if (OldSD.ContainsKey(sid))
                {
                    if (OldSD[sid].Details.ContainsKey(p.ID))
                    {
                        // Already exists, so add this way
                        // Does not exist, so add default values
                        ds_SystemDetails.PlanetTableRow pr = SysD.PlanetTable.NewPlanetTableRow();

                        pr.PID = p.ID;
                        pr.SID = sid;
                        pr.PName = OldSD[sid].Details[p.ID].Name;
                        pr.HasIHUB = OldSD[sid].Details[p.ID].HasIHub;
                        pr.HasTCU = OldSD[sid].Details[p.ID].HasTCU;
                        pr.CName = OldSD[sid].Details[p.ID].Corp;
                        pr.AName = OldSD[sid].Details[p.ID].Alliance;

                        SysD.PlanetTable.AddPlanetTableRow(pr);

                        planetExists = true;
                    }
                }

                if (!planetExists)
                {
                    // Does not exist, so add default values
                    ds_SystemDetails.PlanetTableRow pr = SysD.PlanetTable.NewPlanetTableRow();

                    pr.PID = p.ID;
                    pr.SID = sid;
                    pr.PName = p.Name;

                    SysD.PlanetTable.AddPlanetTableRow(pr);
                }
            }
        }
        
        public static void AddMoonsAndDetails(int sid, SystemCelestials sc, Dictionary<long, SystemDetail> OldSD, ds_SystemDetails.SystemTableRow sr)
        {
            bool moonExists;
            ds_SystemDetails.MoonTableRow mr;

            foreach (Moon m in sc.Moons.Values)
            {
                moonExists = false;

                if (OldSD.ContainsKey(sid))
                {
                    if (OldSD[sid].Details.ContainsKey(m.ID))
                    {
                        // Already exists, so add this way
                        // Does not exist, so add default values
                        mr = SysD.MoonTable.NewMoonTableRow();

                        mr.MID = m.ID;
                        mr.SID = sid;
                        mr.MName = m.Name;
                        mr.HasTCU = OldSD[sid].Details[m.ID].HasTCU;
                        mr.CName = OldSD[sid].Details[m.ID].Corp;
                        mr.AName = OldSD[sid].Details[m.ID].Alliance;
                        if (OldSD[sid].Details[m.ID].Name != m.Name)
                            mr.TName = OldSD[sid].Details[m.ID].Name;
                        else
                            mr.TName = "";
                        mr.TType = OldSD[sid].Details[m.ID].Type;
                        mr.TPassword = OldSD[sid].Details[m.ID].Password;

                        switch (OldSD[sid].Details[m.ID].Defenses)
                        {
                            case 1:
                                mr.TDefense = "Defended";
                                break;
                            case 2:
                                mr.TDefense = "Death Star";
                                break;
                            case 3:
                                mr.TDefense = "Dik-Star";
                                break;
                            default:
                                mr.TDefense = "None";
                                break;
                        }
                        if (mr.TType.Equals(""))
                            mr.TDefense = "None";

                        if (OldSD[sid].Details[m.ID].HasCynoJam)
                            mr.Cyno = "Jammer";
                        else if (OldSD[sid].Details[m.ID].HasCynoGen)
                            mr.Cyno = "Generator";
                        else
                            mr.Cyno = "None";

                        mr.Goo1 = OldSD[sid].Details[m.ID].MoonGoo[0].ToString();
                        mr.Goo2 = OldSD[sid].Details[m.ID].MoonGoo[1].ToString();
                        mr.Goo3 = OldSD[sid].Details[m.ID].MoonGoo[2].ToString();
                        mr.Goo4 = OldSD[sid].Details[m.ID].MoonGoo[3].ToString();
                        mr.Goo5 = OldSD[sid].Details[m.ID].MoonGoo[4].ToString();
                        mr.Goo6 = OldSD[sid].Details[m.ID].MoonGoo[5].ToString();
                        mr.Goo7 = OldSD[sid].Details[m.ID].MoonGoo[6].ToString();
                        mr.Goo8 = OldSD[sid].Details[m.ID].MoonGoo[7].ToString();

                        mr.Bridge = OldSD[sid].Details[m.ID].HasJumpBridge;
                        mr.CHA = OldSD[sid].Details[m.ID].HasCHA;
                        mr.SMA = OldSD[sid].Details[m.ID].HasSMA;
                        mr.CAsm = OldSD[sid].Details[m.ID].HasCapAssembly;
                        mr.CSMA = OldSD[sid].Details[m.ID].HasCapSMA;
                        mr.Mining = OldSD[sid].Details[m.ID].IsMining;
                        mr.Safe = OldSD[sid].Details[m.ID].CynoSafeSpot;

                        if (mr.Safe)
                        {
                            sr.Safe = true;
                            sr.Moon = mr.MName;
                            sr.Password = mr.TPassword;
                        }

                        foreach (Planet p in sc.Planets.Values)
                        {
                            if (m.CelIndex == p.CelIndex)
                            {
                                mr.PID = p.ID;
                                break;
                            }
                        }

                        SysD.MoonTable.AddMoonTableRow(mr);
                        moonExists = true;
                    }
                }

                if (!moonExists)
                {
                    // Does not exist, so add default values
                    mr = SysD.MoonTable.NewMoonTableRow();

                    mr.PID = m.ID;
                    mr.SID = sid;
                    mr.MName = m.Name;

                    foreach (Planet p in sc.Planets.Values)
                    {
                        if (m.CelIndex == p.CelIndex)
                        {
                            mr.PID = p.ID;
                            break;
                        }
                    }
                     
                    SysD.MoonTable.AddMoonTableRow(mr);
                }
            }
        }
       
        #endregion

        #region Logging_API_Misc

        public static void LogAPIError(int code, string text, string pilot)
        {
            string fname;

            fname = Path.Combine(RMapData_Path, "API_Errors.log");

            using (StreamWriter w = File.AppendText(fname))
            {
                w.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " --> API Error [ " + code + " | " + text + " ] Received for : " + pilot);
                w.Close();
            }
        }

        #endregion
    }

#endregion
}
