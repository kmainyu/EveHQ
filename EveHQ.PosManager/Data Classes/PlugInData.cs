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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Net;
using System.Timers;
using System.Linq;

namespace EveHQ.PosManager
{
    #region Plug-in Interface Functions
    public partial class PlugInData : EveHQ.Core.IEveHQPlugIn
    {
        public struct ModuleItem
        {
            public long typeID;
            public long itemID;
            public string name;
            public long qty;
            public SortedList<long, ModuleItem> SubItems; // Items contained inside the Item
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

        public static ArrayList POSSecList = new ArrayList();
        public static ArrayList IHubSecList = new ArrayList();
        public static SortedList<long, Player> SecUsers = new SortedList<long, Player>();
        public static TowerListing TL = new TowerListing();
        public static ModuleListing ML = new ModuleListing();
        public static CategoryList CL = new CategoryList();
        public static SystemList SLs = new SystemList();
        public static New_Designs PDL = new New_Designs();
        public static SystemSovList SL = new SystemSovList();
        public static AllianceList AL = new AllianceList();
        public static API_List API_D = new API_List();                   // API Tower Listing (if it Exists)
        public static PlayerList PL = new PlayerList();
        public static NotificationList NL = new NotificationList();
        public static Configuration Config = new Configuration();
        public static SortedList<string, SortedList<long, APIModule>> CpML;
        public static SortedList<string, SortedList<long, APIModule>> ChML;
        public static SortedList<long, Station> StationList;
        public static SortedList<long, string> SystemIDToStr;
        public static SortedList<long, string> ItemIDToName;
        public static SortedList<long, string> CorpIDToName;
        public static SortedList<long, ModuleLink> ModuleLinks;           // modID, ModuleLink 
        public static SortedList<string, SortedList<string, SystemIHub>> iHubs; // corp name, iHub location, IHub
        public static FuelBay BFStats, FBMults;
        public static TFuelBay BBStats;
        public static SortedList<string, throttlePlayer> ThrottleList;
        public static bool UpdateReactTime = false;

        public static string PoSManage_Path;
        public static string PoSBase_Path, PoSSave_Path;
        public static string PoSCache_Path, PosExport_Path;
        public static ModuleLink LinkInProgress;
        public static LinkModule LMInProgress;
        public bool UseSerializableData = false;
        public string LastCacheRefresh = "2.1.2";
        public static ManualResetEvent[] resetEvents;
        public static PoSManMainForm PMF = null;
        public static BackgroundWorker bgw_APIUpdate = new System.ComponentModel.BackgroundWorker();
        public static BackgroundWorker bgw_SendNotify = new System.ComponentModel.BackgroundWorker();
        private System.Timers.Timer t_MonitorUpdate, ReactionTimer;
        DateTime timeStart;
        static string ActiveReactTower = "";
        public static string SelectedTower = "New POS";
        const int RandomSeed = 2;  
        static Random _random;
        public List<long> TowerMods;
        public static bool ManualUpdate = false;
  
        public static Color[] LColor = new Color[] {Color.Blue, Color.Green, Color.Orange, Color.Violet, Color.LimeGreen,
                                              Color.LightBlue, Color.Red, Color.Yellow, Color.Coral, Color.DarkBlue,
                                              Color.DarkGreen, Color.DarkOrange, Color.DarkRed, Color.DarkViolet,
                                              Color.ForestGreen, Color.Gold, Color.Gray, Color.Lavender,
                                              Color.LightCyan, Color.Olive};

        private long[] SkipRegions = new long[]
                                        {
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

        // 1 - Director
        // 1024 - Factory Manager
        // 2048 - Station Manager
        // 2199023255552 - Equipment Config
        // 9007199254740992 - Starbase Config
        private long[] SelRoles = new long[] { 1, 1024, 2048, 2199023255552, 9007199254740992 };

        
        #region Plug In and Start Up

        public Boolean EveHQStartUp()
        {
            StreamReader sr;
            string cacheVers;
            resetEvents = new ManualResetEvent[6];
            ModuleLinks = new SortedList<long, ModuleLink>();
            DateTime startT, endT;
            TimeSpan runT;
            TowerMods = new List<long>();
            CpML = new SortedList<string, SortedList<long, APIModule>>();
            ChML = new SortedList<string, SortedList<long, APIModule>>();
            StationList = new SortedList<long, Station>();
            SystemIDToStr = new SortedList<long, string>();
            LinkInProgress = new ModuleLink();
            ItemIDToName = new SortedList<long, string>();
            CorpIDToName = new SortedList<long, string>();
            iHubs = new SortedList<string, SortedList<string, SystemIHub>>();
            BFStats = new FuelBay();
            FBMults = new FuelBay();
            BBStats = new TFuelBay();

            // Setup a feul bay object with multipliers to use for Fuel Block Qty calculations.
            FBMults.Coolant.Qty = 8;
            FBMults.EnrUran.Qty = 8;
            FBMults.H2Iso.Qty = 400;
            FBMults.HeIso.Qty = 400;
            FBMults.O2Iso.Qty = 400;
            FBMults.N2Iso.Qty = 400;
            FBMults.MechPart.Qty = 4;
            FBMults.Oxygen.Qty = 20;
            FBMults.Robotics.Qty = 1;
            FBMults.HvyWater.Qty = 150;
            FBMults.LiqOzone.Qty = 150;

            ThrottleList = new SortedList<string, throttlePlayer>();

            _random = new Random(RandomSeed);

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = Path.Combine(PoSBase_Path , "PoSManage");
            PoSCache_Path = Path.Combine(PoSManage_Path, "Cache");
            PoSSave_Path = Path.Combine(PoSManage_Path, "PoSData");
            PosExport_Path = Path.Combine(PoSManage_Path, "Exports");
            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);
            if (!Directory.Exists(PoSSave_Path))
                Directory.CreateDirectory(PoSSave_Path);
            if (!Directory.Exists(PosExport_Path))
                Directory.CreateDirectory(PosExport_Path);

            // Check for cache folder
            if (Directory.Exists(PoSCache_Path))
            {
                // Check for Last Version Text File
                if(File.Exists(Path.Combine(PoSCache_Path , "version.txt")))
                {
                    sr = new StreamReader(Path.Combine(PoSCache_Path , "version.txt"));
                    cacheVers = sr.ReadToEnd();
                    sr.Close();

                    if (IsUpdateAvailable(cacheVers, LastCacheRefresh))
                    {
                        UseSerializableData = false;
                    }
                    else
                        UseSerializableData = true;
                }
                else
                {
                    UseSerializableData = false;
                }
            }
            else
                Directory.CreateDirectory(PoSCache_Path);

            startT = DateTime.Now;

            if (!UseSerializableData)
            {
                BuildStations();
                resetEvents[0] = new ManualResetEvent(false);
                resetEvents[1] = new ManualResetEvent(false);
                resetEvents[2] = new ManualResetEvent(false);
                resetEvents[5] = new ManualResetEvent(false);
                resetEvents[3] = new ManualResetEvent(true);
                resetEvents[4] = new ManualResetEvent(true);
                ThreadPool.QueueUserWorkItem(new WaitCallback(TL.PopulateTowerListing), LastCacheRefresh);
                ThreadPool.QueueUserWorkItem(new WaitCallback(ML.PopulateModuleListing));
                ThreadPool.QueueUserWorkItem(new WaitCallback(CL.PopulateCategoryList));
                ThreadPool.QueueUserWorkItem(new WaitCallback(SLs.PopulateSystemListing));
            }
            else
            {
                resetEvents[0] = new ManualResetEvent(true);
                resetEvents[1] = new ManualResetEvent(true);
                resetEvents[2] = new ManualResetEvent(true);
                resetEvents[3] = new ManualResetEvent(true);
                resetEvents[4] = new ManualResetEvent(true);
                resetEvents[5] = new ManualResetEvent(true);
                TL.LoadTowerListing();
                ML.LoadModuleListing();
                CL.LoadCategoryList();
                SLs.LoadSystemListFromDisk();
                LoadStationListing();
            }

            // Load any API data - it could be updated or time for an update, so check
            WaitHandle.WaitAll(resetEvents);

            GetAllySovLists();
            
            foreach (Sov_Data sd in SLs.Systems.Values)
                SystemIDToStr.Add((int)sd.systemID, sd.systemName);

            LoadBFStatsFromDB();            // Load Base Fuel Stats
            PDL.LoadDesignListing();        // Load Tower Designs from Disk
            Config.LoadConfiguration();     // Load PoS Manager Configuration Information
            API_D.LoadAPIListing();         // Load Tower API Data from Disk
            PL.LoadPlayerList();            // Load Player Listing
            NL.LoadNotificationList();      // Load Notifications
            LoadIHubListing();              // Load I-Hub data - if it exists
            LoadModuleLinkListFromDisk();   // Load Module Link List data
            LoadSecurityListing();
            if (Config.data.Extra.Count <= 0)
                Config.data.Extra.Add((int)400);
            if (Config.data.Extra.Count < 2)
                Config.data.Extra.Add((int)0);

            if (!UseSerializableData)
            {
                // Uncheck - force now un-used DG columns from being visible.
                Config.data.dgMonBool[10] = false;
                Config.data.dgMonBool[11] = false;
                Config.data.dgMonBool[12] = false;
                Config.data.dgMonBool[13] = false;
                Config.data.dgMonBool[14] = false;
                Config.data.dgMonBool[15] = false;
                Config.data.dgMonBool[16] = false;
                Config.SaveConfiguration();
            }
            // 
            // bgw_APIUpdate
            // 
            bgw_APIUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_APIUpdate_DoWork);
            bgw_APIUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_APIUpdate_RunWorkerCompleted);
            // 
            // bgw_SendNotify
            // 
            bgw_SendNotify.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_SendNotify_DoWork);
            //
            // Reaction Calculation Timer
            //
            ReactionTimer = new System.Timers.Timer();
            ReactionTimer.Interval = 30000;
            ReactionTimer.Elapsed += new ElapsedEventHandler(ReactionTimer_Tick);
            ReactionTimer.Enabled = true;
            ReactionTimer.AutoReset = true;
            ReactionTimer.Start();

            endT = DateTime.Now;
            runT = endT.Subtract(startT);

            // Setup poll timer, and give the system a check of 30 seconds to allow full EveHQ Startup
            t_MonitorUpdate = new System.Timers.Timer();
            t_MonitorUpdate.Elapsed += new ElapsedEventHandler(UdateMonitorInformation);
            t_MonitorUpdate.AutoReset = false;
            t_MonitorUpdate.Interval = 180;
            t_MonitorUpdate.Enabled = true;

            return true;
        }

        private void LoadBFStatsFromDB()
        {
            DataSet fuelData;
            string strSQL;

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=44;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.EnrUran.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.EnrUran.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.EnrUran.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.EnrUran.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.EnrUran.itemID = "44";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=3683;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.Oxygen.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.Oxygen.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.Oxygen.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.Oxygen.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.Oxygen.itemID = "3683";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=3689;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.MechPart.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.MechPart.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.MechPart.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.MechPart.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.MechPart.itemID = "3689";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=9832;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.Coolant.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.Coolant.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.Coolant.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.Coolant.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.Coolant.itemID = "9832";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=9848;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.Robotics.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.Robotics.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.Robotics.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.Robotics.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.Robotics.itemID = "9848";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=16272;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.HvyWater.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.HvyWater.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.HvyWater.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.HvyWater.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.HvyWater.itemID = "16272";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=16273;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.LiqOzone.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.LiqOzone.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.LiqOzone.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.LiqOzone.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.LiqOzone.itemID = "16273";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=24592;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.Charters.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.Charters.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.Charters.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.Charters.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.Charters.itemID = "24592";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=17888;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.N2Iso.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.N2Iso.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.N2Iso.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.N2Iso.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.N2Iso.itemID = "17888";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=16274;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.HeIso.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.HeIso.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.HeIso.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.HeIso.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.HeIso.itemID = "16274";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=17889;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.H2Iso.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.H2Iso.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.H2Iso.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.H2Iso.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.H2Iso.itemID = "17889";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=17887;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.O2Iso.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.O2Iso.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.O2Iso.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.O2Iso.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.O2Iso.itemID = "17887";

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=16275;"; // EU
            fuelData = EveHQ.Core.DataFunctions.GetData(strSQL);

            BFStats.Strontium.Name = fuelData.Tables[0].Rows[0].ItemArray[10].ToString();
            BFStats.Strontium.BaseVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[7]);
            BFStats.Strontium.QtyVol = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[9]);
            BFStats.Strontium.Cost = Convert.ToDecimal(fuelData.Tables[0].Rows[0].ItemArray[11]);
            BFStats.Strontium.itemID = "16275";

            // Here is where I would get information on Fuel Blocks - if it existed in the DB yet. For now - hard code it
            BBStats.Blocks.Name = "Fuel Blocks";
            BBStats.Blocks.BaseVol = 5;
            BBStats.Blocks.QtyVol = 1;
            BBStats.Blocks.BaseQty = 1;
            BBStats.Blocks.APIPerQty = 1;
            BBStats.Blocks.Cost = 1000;
            BBStats.Blocks.itemID = "0";
        }

        private void UdateMonitorInformation(object sender, EventArgs e)
        {
             if (!bgw_APIUpdate.IsBusy)
            {
                bgw_APIUpdate.RunWorkerAsync();
            }
        }

        public EveHQ.Core.PlugIn GetEveHQPlugInInfo()
        {
            EveHQ.Core.PlugIn EveHQPlugIn = new EveHQ.Core.PlugIn();

            EveHQPlugIn.Name = "PoS Manager";
            EveHQPlugIn.Description = "Design PoS Layouts and Monitor PoS Status";
            EveHQPlugIn.Author = "Jay Teague <aka: Sherksilver>";
            EveHQPlugIn.MainMenuText = "PoS Manager";
            EveHQPlugIn.RunAtStartup = true;
            EveHQPlugIn.RunInIGB = true;
            EveHQPlugIn.MenuImage = Properties.Resources.plugin_icon;
            EveHQPlugIn.Version = Application.ProductVersion.ToString();

            return EveHQPlugIn;
        }

        public Form RunEveHQPlugIn()
        {
            // You need to make this form, it'll be the startup form for the plugin
            PMF = new PoSManMainForm();
            return PMF;
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

        public object GetPlugInData(object objData, int longDataType)
        {
            return null;
        }

        #endregion

        #region In Game Browser

        public String IGBService(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            string cmd;
            bool POS_ACCESS, HUB_ACCESS;

            POS_ACCESS = CheckForSiteAccess(context, 0);
            HUB_ACCESS = CheckForSiteAccess(context, 1);

            timeStart = DateTime.Now;
            strHTML.Append(EveHQ.Core.IGB.IGBHTMLHeader(context, "PosManager",0));
            strHTML.Append(PosManagerMenu(context));

            cmd = context.Request.Url.AbsolutePath.ToUpper();

            if (cmd.Equals("/POSMANAGER") || cmd.Equals("/POSMANAGER/"))
                strHTML.Append(MainPage(context));
            else if ((cmd.Equals("/POSMANAGER/RTLOOKUP") || cmd.Equals("/POSMANAGER/RTLOOKUP/")) && POS_ACCESS)
                strHTML.Append(RTLookupPage(context));
            else if ((cmd.Equals("/POSMANAGER/IHLOOKUP") || cmd.Equals("/POSMANAGER/IHLOOKUP/")) && HUB_ACCESS)
                strHTML.Append(IHubLookupPage(context));
            else if ((cmd.Contains("/POSMANAGER/CHUBS")) && HUB_ACCESS)
                strHTML.Append(IHubDisplayPage(context));
            else if (cmd.Contains("/POSMANAGER/UREG"))
                strHTML.Append(UserRegDisplayPage(context));
            else if ((cmd.Contains("/POSMANAGER/TFLU_BYVOL")) && POS_ACCESS)
                strHTML.Append(FuelNeedForTowerByVol(context));
            else if ((cmd.Contains("/POSMANAGER/TFLU")) && POS_ACCESS)
                strHTML.Append(FuelNeedForTower(context));
            else if ((cmd.Contains("/POSMANAGER/RMNG")) && POS_ACCESS)
                strHTML.Append(ManageReactionForTower(context));
            else if ((cmd.Contains("/POSMANAGER/RCHG")) && POS_ACCESS)
                strHTML.Append(ChangeReactionForTower(context));
            else if ((cmd.Contains("/POSMANAGER/TWRUL")) && POS_ACCESS)
                strHTML.Append(ImportTowerConfig(context));
            else if ((cmd.Contains("/POSMANAGER/IMPORT")) && POS_ACCESS)
                strHTML.Append(ImportTower(context));
            else
                strHTML.Append(SorryNoAccess(context));

            strHTML.Append(EveHQ.Core.IGB.IGBHTMLFooter(context));

            return strHTML.ToString();
        }

        public string MainPage(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();

            strHTML.Append("<br>");

            return strHTML.ToString();
        }

        public string UserRegDisplayPage(System.Net.HttpListenerContext context)
        {
            long UID, CID, AID;
            long corp_role;
            string Name, Corp, Ally, Trust;
            StringBuilder strHTML = new StringBuilder();
            Player np;

            Trust = context.Request.Headers["EVE_TRUSTED"].ToString();

            if (Trust.Equals("no"))
            {
                strHTML.Append("<font size=\"3\" color=\"gold\"><b> You MUST set this Site to Trusted First ! </b></font><br><br>");
                strHTML.Append("<br><br>");
                strHTML.Append(SorryNoAccess(context));
            }
            else
            {
                corp_role = Convert.ToInt64(context.Request.Headers["EVE_CORPROLE"]);
                UID = Convert.ToInt32(context.Request.Headers["EVE_CHARID"]);
                CID = Convert.ToInt32(context.Request.Headers["EVE_CORPID"]);
                AID = Convert.ToInt32(context.Request.Headers["EVE_ALLIANCEID"]);
                Name = context.Request.Headers["EVE_CHARNAME"].ToString();
                Corp = context.Request.Headers["EVE_CORPNAME"].ToString();
                Ally = context.Request.Headers["EVE_ALLIANCENAME"].ToString();

                if (SecUsers.ContainsKey(UID))
                {
                    strHTML.Append("<font size=\"3\" color=\"gold\"><b> You have already Registered. Please talk to Site Admin for Activation. </b></font><br><br>");
                    strHTML.Append("<br><br>");
                }
                else
                {
                    np = new Player();
                    np.Name = Name;
                    np.Corp = Corp;
                    np.Ally = Ally;
                    np.AllyID = AID;
                    np.CorpID = CID;
                    np.UserID = UID;
                    np.Roles = corp_role;
                    SecUsers.Add(UID, np);
                    strHTML.Append("<font size=\"3\" color=\"gold\"><b> " + Name + " thank you for Registering. Please talk to Site Admin for Activation. </b></font><br><br>");
                    strHTML.Append("<br><br>");
                }
            }
            return strHTML.ToString();
        }

        public string ChangeReactionForTower(System.Net.HttpListenerContext context)
        {
            string action;
            string[] ActData;
            string[] spt;
            string modS;
            long fillV = 0, modID;
            ArrayList fills = new ArrayList();
            StringBuilder strHTML = new StringBuilder();
            New_POS p;

             // Get corret POS Object
            p = PDL.Designs[ActiveReactTower];

            action = context.Request.Url.Query;
            action = action.Replace("?", "");
            ActData = action.Split('&');

            foreach (string s in ActData)
            {
                if (s.Contains("Set+Fill"))
                {
                    // We are setting a fill level
                    spt = s.Split('=');
                    modS = spt[0].Replace("Fill", "");
                    modID = Convert.ToInt32(modS);

                    foreach (string t in ActData)
                    {
                        if (t.Contains("FillV" + modID))
                        {
                            spt = t.Split('=');
                            fillV = Convert.ToInt32(spt[1]);
                            break;
                        }
                    }

                    foreach (Module m in p.Modules)
                    {
                        if (m.ModuleID == modID)
                        {
                            m.CapQty = fillV;
                        }
                    }
                    PDL.SaveDesignListing();
                    break;
                }
                else if (s.Contains("Set+Full"))
                {
                    spt = s.Split('=');
                    modS = spt[0].Replace("Full", "");
                    modID = Convert.ToInt32(modS);
                    foreach (Module m in p.Modules)
                    {
                        if (m.ModuleID == modID)
                        {
                            m.CapQty = m.MaxQty;
                        }
                    }
                    PDL.SaveDesignListing();
                    break;
                }
                else if (s.Contains("Set+Empty"))
                {
                    spt = s.Split('=');
                    modS = spt[0].Replace("Empty", "");
                    modID = Convert.ToInt32(modS);
                    foreach (Module m in p.Modules)
                    {
                        if (m.ModuleID == modID)
                        {
                            m.CapQty = 0;
                        }
                    }
                    PDL.SaveDesignListing();
                    break;
                }
            }

            return DisplayReactionForTower();
        }

        public void SetModuleWarnOnValueAndTime()
        {
            bool fInp, fOutp;
            decimal xfIn, xfOut;

            foreach (New_POS p in PDL.Designs.Values)
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
        
        public string ManageReactionForTower(System.Net.HttpListenerContext context)
        {
            string tower;

            tower = context.Request.Url.AbsolutePath;
            tower = tower.Replace("/PosManager/RMNG/", "");
            tower = tower.Replace("_spc_", " ");
            tower = tower.Replace("_-_", ">");
            tower = tower.Replace("/", "");

            ActiveReactTower = tower;

            return DisplayReactionForTower();
        }

        public string DisplayReactionForTower()
        {
            ArrayList reacts = new ArrayList();
            StringBuilder strHTML = new StringBuilder();
            StringBuilder hvstHTML = new StringBuilder();
            StringBuilder reactHTML = new StringBuilder();
            StringBuilder siloHTML = new StringBuilder();
            string line, CapText;
            New_POS p;

            SetModuleWarnOnValueAndTime();
            // Get corret POS Object
            p = PDL.Designs[ActiveReactTower];

            // Display Silos - Mineral, Current Qty, Current Volume - Set Qty, Set Full, Set Empty
            strHTML.Append("<form method=\"GET\" action=\"/PosManager/RCHG\">");
            strHTML.Append("<table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"500\" cellpadding=\"1\" cellspacing=\"1\">");
            strHTML.Append("<tr><td><b><font size=\"3\" color=\"green\">Tower Name: " + p.Name + "</a></font></b></td></tr>");
            if (p.Moon != "")
                line = p.Moon;
            else
                line = p.System;
            strHTML.Append("<tr><td><b><font size=\"2\" color=\"#0033CC\">Location: " + line + "</font></b></td></tr>");
            strHTML.Append("</table>");
            foreach (Module m in p.Modules)
            {
                if (m.State == "Online")
                {
                    if (m.Category == "Silo")
                    {
                        strHTML.Append("<table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"1175\" cellpadding=\"1\" cellspacing=\"1\">");
                        strHTML.Append("<tr>");
                        strHTML.Append("<td width=\"150\" align=\"center\"><b><font size=\"2\" color=\"lime\">" + m.Name + "</font></b></td>");
                        strHTML.Append("<td width=\"150\" align=\"center\"><b><font size=\"2\" color=\"lime\">" + m.selMineral.name + "</font></b></td>");
                        // 400
                        CapText = m.CapQty + " / " + Math.Truncate(m.MaxQty);
                        strHTML.Append("<td width=\"175\" align=\"center\"><b><font size=\"2\" color=\"#0033CC\">" + CapText + "</font></b></td>");
                        // 575
                        strHTML.Append("<td width=\"125\" align=\"center\"><b><font size=\"2\" color=\"#0033CC\">" + m.CapVol + " m3</font></b></td>");
                        // 700
                        strHTML.Append("<td width=\"75\" align=\"center\"><b><font size=\"2\" color=\"#0033CC\"><input name=Full" + m.ModuleID + " type='submit' value='Set Full'></font></b></td>");
                        // 775
                        strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"#0033CC\"><input name=Empty" + m.ModuleID + " type='submit' value='Set Empty'></font></b></td>");
                        // 875
                        strHTML.Append("<td width=\"65\" align=\"center\"><b><font size=\"2\" color=\"#0033CC\"><input type=\"text\" size=\"10\" name=FillV" + m.ModuleID + "></font></b></td>");
                        // 1000
                        strHTML.Append("<td width=\"75\" align=\"center\"><b><font size=\"2\" color=\"#0033CC\"><input name=Fill" + m.ModuleID + " type='submit' value='Set Fill'></font></b></td>");
                        // 1075
                        strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"#0033CC\">" + ConvertReactionHoursToTextDisplay(m.FillEmptyTime) + "</font></b></td>");
                        // 1175
                        strHTML.Append("</tr>");
                        // Display Name, ID, selected moon goo type
                    }
                }
            }
            strHTML.Append("</table>");
            return strHTML.ToString();
        }

        public string RTLookupPage(System.Net.HttpListenerContext context)
        {
            string cache, line, apiTime, refStr = "";
            string linkName;
            APITowerData apid;
            ArrayList ReactRet;
            StringBuilder strHTML = new StringBuilder();
            List<New_POS> sorted = new List<New_POS>();
            DateTime cTim, ref_TM, now;
            TimeSpan diffT;
            bool reinf = false;
            decimal ReactTime;

            SetModuleWarnOnValueAndTime();
            foreach (New_POS p in PDL.Designs.Values)
            {
                if (p.Monitored)
                {
                    sorted.Add(p);
                }
            }

            sorted.Sort(delegate(New_POS p1, New_POS p2) { return p1.PosTower.Data["F_RunTime"].CompareTo(p2.PosTower.Data["F_RunTime"]); });
            foreach (New_POS p in sorted)
            {
                // Put a blank line between each tower
                strHTML.Append("<br>");

                // We have a tower, add or Start a new Row
                if (p.itemID != 0)
                {
                    apid = PlugInData.API_D.GetAPIDataMemberForTowerID(p.itemID);
                    // Get Table With Tower or Tower Item Information
                    if (apid != null)
                    {
                        cache = apid.cacheUntil;
                        cTim = Convert.ToDateTime(cache);
                        cTim = TimeZone.CurrentTimeZone.ToLocalTime(cTim);
                        now = DateTime.Now;
                        diffT = now.Subtract(cTim);
                        if (diffT.Hours > 1)
                            apiTime = "<td width=\"20%\"><font size=\"2\" color=\"lightcoral\">API: " + cache + "</font></td>";
                        else
                            apiTime = "<td width=\"20%\"><font size=\"2\" color=\"deepskyblue\">API: " + cache + "</font></td>";

                        if (apid.stateV == 3)
                        {
                            ref_TM = Convert.ToDateTime(apid.stateTS);
                            
                            refStr = ref_TM.ToString("MM/dd HH:mm:ss");
                            p.PosTower.State = "Reinforced";
                            reinf = true;
                        }
                        else
                            reinf = false;
                    }
                    else
                    {
                        apiTime = "<td width=\"20%\"><font size=\"2\">No API Data Found</font></td>";
                    }
                }
                else
                {
                    apiTime = "<td width=\"20%\"><font size=\"2\">Not Linked</font></td>";
                }

                strHTML.Append("<table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"100%\" cellpadding=\"1\" cellspacing=\"1\">");

                strHTML.Append("<tr>");

                ReactRet = GetLongestSiloRunTime(p);
                ReactTime = (decimal)ReactRet[0];
                linkName = p.Name.Replace(" ", "_spc_");
                linkName = linkName.Replace(">", "_-_");

                strHTML.Append("<td width=\"50%\"><b><font size=\"3\" color=\"green\">" + p.Name + "</a></font></b></td>");
                if (p.Moon != "")
                    line = p.Moon;
                else
                    line = p.System;

                strHTML.Append("<td width=\"40%\"><b><font size=\"3\" color=\"#0033CC\">" + line + "</font></b></td>");
                strHTML.Append("<td width=\"10%\" rowspan=\"2\"><b><font size=\"3\"><a href=/PosManager/TWRUL/" + linkName + "/>Import Config</a></font></b></td></tr>");

                strHTML.Append("<td width=\"50%\"><font size=\"2\" color=\"#9977EE\">" + p.PosTower.Name + "</font></td>");
                strHTML.Append("<td width=\"40%\"><b><font size=\"2\">&nbsp;</font></b></td></tr>");
                strHTML.Append("<tr>");
                strHTML.Append("<table border=\"1\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"100%\" cellpadding=\"1\" cellspacing=\"1\">");

                if (p.PosTower.Data["F_RunTime"] < 24)
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"red\">Run: " + ConvertHoursToTextDisplay(p.PosTower.Data["F_RunTime"]) + "</font></td>");
                else if (p.PosTower.Data["F_RunTime"] < 48)
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"gold\">Run: " + ConvertHoursToTextDisplay(p.PosTower.Data["F_RunTime"]) + "</font></td>");
                else
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"lime\">Run: " + ConvertHoursToTextDisplay(p.PosTower.Data["F_RunTime"]) + "</font></td>");

                if (reinf)
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"red\">Out at: " + refStr + " Eve Time</font></td>");
                else
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"darkorange\">Ref_T: " + ConvertHoursToTextDisplay(p.PosTower.Fuel.Strontium.RunTime) + "</font></td>");

                if (p.PosTower.State.Equals("Online"))
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"green\">State: " + p.PosTower.State + "</font></td>");
                else if (p.PosTower.State.Equals("Offline"))
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"gold\">State: " + p.PosTower.State + "</font></td>");
                else
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"red\">State: " + p.PosTower.State + "</font></td>");

                strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"cyan\">CPU: " + String.Format("{0:#,0.#}", p.PosTower.Data["CPU_Used"]) + "</font></td>");
                strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"orange\">Power: " + String.Format("{0:#,0.#}", p.PosTower.Data["Power_Used"]) + "</font></td>");
             
                if (ReactTime < 6)
                    strHTML.Append("<tr><td width=\"20%\"><a href=/PosManager/RMNG/" + linkName + "/><font size=\"2\" color=\"red\">Rection Run: " + ReactRet[1].ToString() + "</font></a></td>");
                else if (ReactTime < 24)
                    strHTML.Append("<tr><td width=\"20%\"><a href=/PosManager/RMNG/" + linkName + "/><font size=\"2\" color=\"gold\">Rection Run: " + ReactRet[1].ToString() + "</font></a></td>");
                else if (ReactRet[1].ToString().Length > 0)
                    strHTML.Append("<tr><td width=\"20%\"><a href=/PosManager/RMNG/" + linkName + "/><font size=\"2\" color=\"#00FF99\">Rection Run: " + ReactRet[1].ToString() + "</font></a></td>");
                else
                    strHTML.Append("<tr><td width=\"20%\"><font size=\"2\" color=\"#FFFFFF\">No Reaction Defined</font></td>");

                if (p.Owner.Length > 0)
                    strHTML.Append("<td width=\"20%\"><font size=\"2\">Owner: " + p.Owner + "</font></td>");
                else
                    strHTML.Append("<td width=\"20%\"><font size=\"2\">Owner: None</font></td>");

                if (p.FuelTech.Length > 0)
                    strHTML.Append("<td width=\"20%\"><font size=\"2\">Fuel Tech: " + p.FuelTech + "</font></td>");
                else
                    strHTML.Append("<td width=\"20%\"><font size=\"2\">Fuel Tech: Not Assigned</font></td>");

                strHTML.Append(apiTime);
                strHTML.Append("<td width=\"20%\"><b><a href=/PosManager/TFLU/" + linkName + "/><font size=\"2\" color=\"#0033CC\"> Get Fuel Need for Fill </font></a></b></td>");

                strHTML.Append("</tr></table>");
                strHTML.Append("</table>");
            }

            return strHTML.ToString();
        }

        public string IHubLookupPage(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            string corp;

            strHTML.Append("<table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"50%\" cellpadding=\"1\" cellspacing=\"1\">");

            foreach (string Corp in iHubs.Keys)
            {
                corp = Corp.Replace(" ", "_spc_");
                corp = corp.Replace(">", "_-_");

                strHTML.Append("<tr><td width=\"50%\" rowspan=\"2\"><b><font size=\"3\"><a href=/PosManager/CHUBS/" + corp + "/>" + Corp + "</a></font></b></td></tr>");
            }

            strHTML.Append("</table>");

            return strHTML.ToString();
        }

        public string IHubDisplayPage(System.Net.HttpListenerContext context)
        {
            String corp;
            long lvl;
            StringBuilder strHTML = new StringBuilder();
            InfrastructureUG[] Ore, Entrap, Survey, Pirate, QFlux, Strat;

            corp = context.Request.Url.AbsolutePath;
            corp = corp.Replace("/PosManager/CHUBS/", "");
            corp = corp.Replace("_spc_", " ");
            corp = corp.Replace("_-_", ">");
            corp = corp.Replace("/", "");

             foreach (var hub in iHubs[corp])
            {
                Ore = new InfrastructureUG[5];
                Entrap = new InfrastructureUG[5];
                Survey = new InfrastructureUG[5];
                Pirate = new InfrastructureUG[5];
                QFlux = new InfrastructureUG[5];
                Strat = new InfrastructureUG[5];
                foreach (InfrastructureUG IUG in hub.Value.OreUG.Values)
                {
                    lvl = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                    Ore[lvl - 1] = new InfrastructureUG(IUG);
                }

                foreach (InfrastructureUG IUG in hub.Value.EntUG.Values)
                {
                    lvl = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                    Entrap[lvl - 1] = new InfrastructureUG(IUG);
                }

                foreach (InfrastructureUG IUG in hub.Value.SrvUG.Values)
                {
                    lvl = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                    Survey[lvl - 1] = new InfrastructureUG(IUG);
                }

                foreach (InfrastructureUG IUG in hub.Value.PrtUG.Values)
                {
                    lvl = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                    Pirate[lvl - 1] = new InfrastructureUG(IUG);
                }

                foreach (InfrastructureUG IUG in hub.Value.FlxUG.Values)
                {
                    lvl = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                    QFlux[lvl - 1] = new InfrastructureUG(IUG);
                }

                foreach (InfrastructureUG IUG in hub.Value.StratUG.Values)
                {
                    if (IUG.name.Contains("Logistics"))
                        Strat[0] = new InfrastructureUG(IUG);
                    else if (IUG.name.Contains("Navigation"))
                        Strat[1] = new InfrastructureUG(IUG);
                    else if (IUG.name.Contains("Suppresion"))
                        Strat[2] = new InfrastructureUG(IUG);
                    else if (IUG.name.Contains("Supercapital"))
                        Strat[3] = new InfrastructureUG(IUG);
                }

                strHTML.Append("<table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"600\" cellpadding=\"1\" cellspacing=\"1\">");
                strHTML.Append("<tr><td colspan=13><b><font size=\"3\" color=\"#33EE33\">Hub Location: " + hub.Value.location + "</a></font></b></td></tr>");
                strHTML.Append("<tr>");
                strHTML.Append("<td colspan=6 width=\"300\" align=\"center\"><b><font size=\"2\" color=\"#33EE33\">Industry</font></b></td>");
                strHTML.Append("<td colspan=6 width=\"300\" align=\"center\"><b><font size=\"2\" color=\"red\">Military</font></b></td>");
                strHTML.Append("<td colspan=1 width=\"100\" align=\"center\"><b><font size=\"2\" color=\"#00AAFF\">Strategic</font></b></td>");
                strHTML.Append("</tr><tr>");

                strHTML.Append("<td width=\"100\" align=\"left\"><b><font size=\"2\" color=\"#33EE33\">Ore Prspct</font></b></td>");
                for (int x = 0; x < 5; x++)
                {
                    if ((Ore[x] == null) || Ore[x].name.Equals(""))
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"black\"> </font></b></td>");
                    }
                    else
                    {
                        if (Ore[x].online)
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"lime\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                        else
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"#ff0000\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                    }
                }
                strHTML.Append("<td width=\"100\" align=\"left\"><b><font size=\"2\" color=\"#33EE33\">Entrapment</font></b></td>");
                for (int x = 0; x < 5; x++)
                {
                    if ((Entrap[x] == null) || Entrap[x].name.Equals(""))
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"black\"> </font></b></td>");
                    }
                    else
                    {
                        if (Entrap[x].online)
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"lime\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                        else
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"#ff0000\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                    }
                }

                if (Strat[0] != null)
                {
                    if (Strat[0].online)
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"lime\"><b><font size=\"2\" color=\"black\">Bridge</font></b></td>");
                    }
                    else
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"#ff0000\"><b><font size=\"2\" color=\"black\">Bridge</font></b></td>");
                    }
                }
                else
                    strHTML.Append("<td width=\"100\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"lightblue\">Bridge</font></b></td>");

                strHTML.Append("</tr><tr>");
                strHTML.Append("<td width=\"100\" align=\"left\"><b><font size=\"2\" color=\"#33EE33\">Survey</font></b></td>");
                for (int x = 0; x < 5; x++)
                {
                    if ((Survey[x] == null) || Survey[x].name.Equals(""))
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"black\"> </font></b></td>");
                    }
                    else
                    {
                        if (Survey[x].online)
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"lime\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                        else
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"#ff0000\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                    }
                }
                strHTML.Append("<td width=\"100\" align=\"left\"><b><font size=\"2\" color=\"#33EE33\">Pirate</font></b></td>");
                for (int x = 0; x < 5; x++)
                {
                    if ((Pirate[x] == null) || Pirate[x].name.Equals(""))
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"black\"> </font></b></td>");
                    }
                    else
                    {
                        if (Pirate[x].online)
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"lime\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                        else
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"#ff0000\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                    }
                }

                if (Strat[1] != null)
                {
                    if (Strat[1].online)
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"lime\"><b><font size=\"2\" color=\"black\">Cyno-Gen</font></b></td>");
                    }
                    else
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"#ff0000\"><b><font size=\"2\" color=\"black\">Cyno-Gen</font></b></td>");
                    }
                }
                else
                    strHTML.Append("<td width=\"100\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"lightblue\">Cyno-Gen</font></b></td>");

                strHTML.Append("</tr><tr>");
                strHTML.Append("<td width=\"100\" align=\"left\"><b><font size=\"2\" color=\"#33EE33\"></font></b></td>");
                for (int x = 0; x < 5; x++)
                {
                    strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"black\"> </font></b></td>");
                }
                strHTML.Append("<td width=\"100\" align=\"left\"><b><font size=\"2\" color=\"#33EE33\">Qntm. Flux</font></b></td>");
                for (int x = 0; x < 5; x++)
                {
                    if ((QFlux[x] == null) || QFlux[x].name.Equals(""))
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"black\"> </font></b></td>");
                    }
                    else
                    {
                        if (QFlux[x].online)
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"lime\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                        else
                            strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"#ff0000\"><b><font size=\"2\" color=\"black\">" + (x + 1) + "</font></b></td>");
                    }
                }

                if (Strat[2] != null)
                {
                    if (Strat[2].online)
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"lime\"><b><font size=\"2\" color=\"black\">Cyno-Jam</font></b></td>");
                    }
                    else
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"#ff0000\"><b><font size=\"2\" color=\"black\">Cyno-Jam</font></b></td>");
                    }
                }
                else
                    strHTML.Append("<td width=\"100\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"lightblue\">Cyno-Jam</font></b></td>");

                strHTML.Append("</tr><tr>");
                strHTML.Append("<td width=\"100\" align=\"left\"><b><font size=\"2\" color=\"#33EE33\"></font></b></td>");
                for (int x = 0; x < 5; x++)
                {
                    strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"black\"> </font></b></td>");
                }
                strHTML.Append("<td width=\"100\" align=\"left\"><b><font size=\"2\" color=\"#33EE33\"></font></b></td>");
                for (int x = 0; x < 5; x++)
                {
                    strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"black\"> </font></b></td>");
                }

                if (Strat[3] != null)
                {
                    if (Strat[3].online)
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"lime\"><b><font size=\"2\" color=\"black\">Supercap</font></b></td>");
                    }
                    else
                    {
                        strHTML.Append("<td width=\"40\" align=\"center\" bgcolor=\"#ff0000\"><b><font size=\"2\" color=\"black\">Supercap</font></b></td>");
                    }
                }
                else
                    strHTML.Append("<td width=\"100\" align=\"center\" bgcolor=\"black\"><b><font size=\"2\" color=\"lightblue\">Supercap</font></b></td>");

                strHTML.Append("</tr></table><br />");
            }

            return strHTML.ToString();
        }

        public string FuelNeedForTowerByVol(System.Net.HttpListenerContext context)
        {
            string vol;
            decimal vlm, sov_mod, totVol = 0;
            string tower, line, twrnospc;
            New_POS p;
            TFuelBay sfb = new TFuelBay();
            string[,] fVal;

            tower = context.Request.Url.AbsolutePath;
            tower = tower.Replace("/PosManager/TFLU_BYVOL/", "");
            twrnospc = tower;
            tower = tower.Replace("_spc_", " ");
            tower = tower.Replace("_-_", ">");
            tower = tower.Replace("/", "");

            p = PDL.Designs[tower];
            sov_mod = p.GetSovMultiple();

            vol = context.Request.QueryString["vol"].ToString();
            vlm = Convert.ToDecimal(vol);
            sfb = p.ComputeMaxPosRunForVolume(vlm);
            sfb.SetCurrentFuelVolumes();
            sfb.SetFuelRunTimes(sov_mod);
            p.PosTower.Fuel.SetFuelRunTimes(sov_mod);
            fVal = sfb.GetFuelAmountsAndBayTotals(p, sov_mod);

            StringBuilder strHTML = new StringBuilder();

            strHTML.Append("<table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"700\" cellpadding=\"1\" cellspacing=\"1\">");
            strHTML.Append("<tr><td><b><font size=\"3\" color=\"green\">Tower Name: " + p.Name + "</a></font></b></td></tr>");
            if (p.Moon != "")
                line = p.Moon;
            else
                line = p.System;
            strHTML.Append("<tr><td><b><font size=\"2\" color=\"blue\">Location: " + line + "</font></b></td></tr>");

            strHTML.Append("</table><table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"700\" cellpadding=\"1\" cellspacing=\"1\">");
            strHTML.Append("<tr>");
            strHTML.Append("<td width=\"200\" align=\"center\"><b><font size=\"2\" color=\"green\">Fuel Type</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"blue\">Transport Qty</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"blue\">Tport Run Tm</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"blue\">Tport Volume</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"blue\">Final Qty</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"blue\">Final Run Tm</font></b></td>");
            strHTML.Append("</tr>");

            for (int x = 0; x < 2; x++)
            {
                if ((!p.UseChart) && (x == 1))
                    continue;

                strHTML.Append("<tr>");
                strHTML.Append("<td width=\"200\"><b><font size=\"2\">" + fVal[x, 0] + "</font></b></td>");
                strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\">" + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + "</font></b></td>");
                strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\">" + ConvertReactionHoursToTextDisplay(Convert.ToDecimal(fVal[x, 2])) + "</font></b></td>");
                strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\">" + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + " m3</font></b></td>");
                strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\">" + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 4])) + "</font></b></td>");
                strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\">" + ConvertReactionHoursToTextDisplay(Convert.ToDecimal(fVal[x, 5])) + "</font></b></td>");
                strHTML.Append("</tr>");

                if (Convert.ToDecimal(fVal[x, 3]) > 0)
                    totVol += Convert.ToDecimal(fVal[x, 3]);
            }

            strHTML.Append("<tr>");
            strHTML.Append("<td width=\"200\"><b><font size=\"2\" color=\"lime\">Transport Volume</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\"> </font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\"> </font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"lime\">" + String.Format("{0:#,0.#}", totVol) + " m3</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\"> </font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\"> </font></b></td>");
            strHTML.Append("</tr>");
            strHTML.Append("</table>");

            return strHTML.ToString();
        }

        public string FuelNeedForTower(System.Net.HttpListenerContext context)
        {
            decimal sov_mod, period, totVol = 0;
            string tower, line, twrnospc;
            New_POS p;
            TFuelBay sfb = new TFuelBay();
            string[,] fVal;

            StringBuilder strHTML = new StringBuilder();

            tower = context.Request.Url.AbsolutePath;
            tower = tower.Replace("/PosManager/TFLU/", "");
            twrnospc = tower;
            tower = tower.Replace("_spc_", " ");
            tower = tower.Replace("_-_", ">");
            tower = tower.Replace("/", "");

            p = PDL.Designs[tower];
            sov_mod = p.GetSovMultiple();

            period = p.ComputePosFuelUsageForFillTracking(4, 99999, Config.data.FuelCosts);

            sfb = new TFuelBay(PDL.Designs[tower].PosTower.T_Fuel);
            sfb.SetCurrentFuelVolumes();
            sfb.SetCurrentFuelCosts(PlugInData.Config.data.FuelCosts);
            fVal = sfb.GetFuelBayAndBurnTotals(p, sov_mod);

            strHTML.Append("<form method='GET' action=\"/PosManager/TFLU_BYVOL/" + twrnospc + "/\">");
            strHTML.Append("<table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"750\" cellpadding=\"1\" cellspacing=\"1\">");
            strHTML.Append("<tr><td><b><font size=\"3\" color=\"green\">Tower Name: " + p.Name + "</a></font></b></td></tr>");
            if (p.Moon != "")
                line = p.Moon;
            else
                line = p.System;
            strHTML.Append("<tr><td><b><font size=\"2\" color=\"blue\">Location: " + line + "</font></b></td></tr>");
//          strHTML.Append("<tr><td><b><font size=\"2\" color=\"cyan\">Run Time Full: " + ConvertHoursToTextDisplay(period) + "</font></b></td></tr>");

            strHTML.Append("</table><table border=\"2\" bordercolor=\"#666666\" style=\"background-color:#000000\" width=\"750\" cellpadding=\"1\" cellspacing=\"1\">");
            strHTML.Append("<tr>");
            strHTML.Append("<td width=\"200\" align=\"center\"><b><font size=\"2\" color=\"green\">Fuel Type</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"blue\">Current Amt</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"blue\">Cur Run Tm</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"blue\">Qty/Hour</font></b></td>");
            strHTML.Append("<td width=\"150\" align=\"center\"><b><font size=\"2\" color=\"blue\">Fill Qty Needed</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\" color=\"blue\">Fill Run Tm</font></b></td>");
            strHTML.Append("</tr>");

            for (int x = 0; x < 3; x++)
            {
                if ((!p.UseChart) && (x == 1))
                    continue;

                strHTML.Append("<tr>");
                strHTML.Append("<td width=\"200\"><b><font size=\"2\">" + fVal[x, 0] + "</font></b></td>");
                strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\">" + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + "</font></b></td>");
                strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\">" + ConvertReactionHoursToTextDisplay(Convert.ToDecimal(fVal[x, 5])) + "</font></b></td>");
                strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\">" + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 4])) + "</font></b></td>");
                strHTML.Append("<td width=\"150\" align=\"center\"><b><font size=\"2\">" + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + "</font></b></td>");
                strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\">" + ConvertReactionHoursToTextDisplay(period) + "</font></b></td>");
                strHTML.Append("</tr>");

                if (Convert.ToDecimal(fVal[x, 2]) > 0)
                    totVol += Convert.ToDecimal(fVal[x, 2]);
            }

            strHTML.Append("<tr>");
            strHTML.Append("<td width=\"250\"><b><font size=\"2\" color=\"lime\">Transport Volume</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\"> </font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\"> </font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\"> </font></b></td>");
            strHTML.Append("<td width=\"150\" align=\"center\"><b><font size=\"2\" color=\"lime\">" + String.Format("{0:#,0.#}", totVol) + " m3</font></b></td>");
            strHTML.Append("<td width=\"100\" align=\"center\"><b><font size=\"2\"> </font></b></td>");
            strHTML.Append("</tr>");
            strHTML.Append("</table>");
            strHTML.Append("<br><br><font size=\"2\">Get Max Fill by Ship Cargo Volume:  </font>");
            strHTML.Append("<input type=\"text\" name=\"vol\">");
            strHTML.Append("<input type='submit' value='Compute Fill'></form>");
            strHTML.Append("<br><br>");

            return strHTML.ToString();
        }

        public string SorryNoAccess(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            strHTML.Append("<font size=\"3\" color=\"gold\"><b> You do Not have permissions to view this Page. </b></font><br><br>");
            strHTML.Append("<br><br>");
            return strHTML.ToString();
        }

        public bool CheckForSiteAccess(System.Net.HttpListenerContext context, long type)
        {
            long UID, CID, AID;
            long corp_role;
            string Name, Corp, Ally, Trust;
            bool Access = false, GrantAccess = false;

            Trust = context.Request.Headers["EVE_TRUSTED"].ToString();

            if (Trust.Equals("Yes"))
            {
                corp_role = Convert.ToInt64(context.Request.Headers["EVE_CORPROLE"]);
                UID = Convert.ToInt32(context.Request.Headers["EVE_CHARID"]);
                CID = Convert.ToInt32(context.Request.Headers["EVE_CORPID"]);
                AID = Convert.ToInt32(context.Request.Headers["EVE_ALLIANCEID"]);
                Name = context.Request.Headers["EVE_CHARNAME"].ToString();
                Corp = context.Request.Headers["EVE_CORPNAME"].ToString();
                Ally = context.Request.Headers["EVE_ALLIANCENAME"].ToString();

                switch (type)
                {
                    case 0:         // New_POS - View
                        foreach (IGBSecurity igbS in POSSecList)
                        {
                            if (!igbS.Active)
                                Access = true; 
                            else if (igbS.Option.Equals("Anyone"))
                                Access = true;
                            else if (igbS.Option.Equals("Player") && (igbS.Modifier.Equals(Name)))
                                Access = true;
                            else if (igbS.Option.Equals("Corporation") && (igbS.Modifier.Equals(Corp)))
                                Access = true;
                            else if (igbS.Option.Equals("Alliance") && (igbS.Modifier.Equals(Ally)))
                                Access = true;
                            else if (igbS.Option.Equals("Role"))
                            {
                                if (igbS.Modifier.Equals("Director"))
                                {
                                    if ((SelRoles[0] | corp_role) > 0)
                                        Access = true;
                                }
                                else if (igbS.Modifier.Equals("Factory Manager"))
                                {
                                    if ((SelRoles[1] | corp_role) > 0)
                                        Access = true;
                                }
                                else if (igbS.Modifier.Equals("Station Manager"))
                                {
                                    if ((SelRoles[2] | corp_role) > 0)
                                        Access = true;
                                }
                                else if (igbS.Modifier.Equals("Equipment Config"))
                                {
                                    if ((SelRoles[3] | corp_role) > 0)
                                        Access = true;
                                }
                                else if (igbS.Modifier.Equals("Starbase Config"))
                                {
                                    if ((SelRoles[4] | corp_role) > 0)
                                        Access = true;
                                }
                                // More complicated as multiple roles can / could be allowed
                            }

                            if (igbS.AND && !Access)
                                return false;        // AND condition failed - done man
                            else if (Access)
                                GrantAccess = true;
                        }
                        break;
                    case 1:         // iHub - View
                        foreach (IGBSecurity igbS in IHubSecList)
                        {
                            if (igbS.Option.Equals("Anyone"))
                                Access = true;
                            else if (igbS.Option.Equals("Player") && (igbS.Modifier.Equals(Name)))
                                Access = true;
                            else if (igbS.Option.Equals("Corporation") && (igbS.Modifier.Equals(Corp)))
                                Access = true;
                            else if (igbS.Option.Equals("Alliance") && (igbS.Modifier.Equals(Ally)))
                                Access = true;
                            else if (igbS.Option.Equals("Role"))
                            {
                                if (igbS.Modifier.Equals("Director"))
                                {
                                    if ((SelRoles[0] | corp_role) > 0)
                                        Access = true;
                                }
                                else if (igbS.Modifier.Equals("Factory Manager"))
                                {
                                    if ((SelRoles[1] | corp_role) > 0)
                                        Access = true;
                                }
                                else if (igbS.Modifier.Equals("Station Manager"))
                                {
                                    if ((SelRoles[2] | corp_role) > 0)
                                        Access = true;
                                }
                                else if (igbS.Modifier.Equals("Equipment Config"))
                                {
                                    if ((SelRoles[3] | corp_role) > 0)
                                        Access = true;
                                }
                                else if (igbS.Modifier.Equals("Starbase Config"))
                                {
                                    if ((SelRoles[4] | corp_role) > 0)
                                        Access = true;
                                }
                                // More complicated as multiple roles can / could be allowed
                            }

                            if (igbS.AND && !Access)
                                return false;        // AND condition failed - done man
                            else if (Access)
                                GrantAccess = true;
                        }
                        break;
                    default:        // Undefined - so Access denied
                        return false;
                }
            }

            return GrantAccess;
        }

        public string PosManagerMenu(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            bool PosAccess, HubAccess;
            long UID;

            PosAccess = CheckForSiteAccess(context, 0);
            HubAccess = CheckForSiteAccess(context, 1);
            
            strHTML.Append("<font size=\"3\" color=\"gold\"><b> EveHQ POS & I-Hub Manager </b></font><br><br>");
            strHTML.Append("<font size=\"2\"><a href=/PosManager> --Home-- </a> ");
            if (PosAccess)
            {
                strHTML.Append("| <a href=/PosManager/RTLookup>POS Run-Time Lookup</a> ");
            }
            if (HubAccess)
            {
                strHTML.Append("| <a href=/PosManager/IHLookup>I-Hub Status</a> ");
            }

            UID = Convert.ToInt32(context.Request.Headers["EVE_CHARID"]);

            if (!SecUsers.ContainsKey(UID))
            {
                strHTML.Append("| <a href=/PosManager/UREG> Register User </a> ");
            }

            strHTML.Append("</font><br><br>");

            if (!PosAccess && !HubAccess)
            {
                strHTML.Append(SorryNoAccess(context));
            }

            return strHTML.ToString();
        }

        public string ImportTowerConfig(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            string tower;

            tower = context.Request.Url.AbsolutePath;
            tower = tower.Replace("/PosManager/TWRUL/", "");

            strHTML.Append("<form method='POST' action=\"/PosManager/IMPORT/" + tower + "/\">");
            strHTML.Append("<TEXTAREA NAME='Upload' COLS=120 ROWS=10></TEXTAREA>");
            strHTML.Append("<br><br>");
            strHTML.Append("<input type='submit' value='Submit'> to upload the Data!</form>");

            return strHTML.ToString();
        }

        public string ImportTower(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            string body;
            string[] split1, modS, lnkS, twrS, Mod;
            string tower, twrName, selReact, selMineral, state, locn, srcNm, dstNm;
            decimal qty, cpu = 0, pwr = 0;
            long tID, modID, lnkID, inpID, outID, XfrQ, XferV;
            Module nm;
            New_POS p;
            Reaction nr;
            MoonSiloReactMineral msr;
            ReactionLink rl;

            tower = context.Request.Url.AbsolutePath;
            tower = tower.Replace("/PosManager/IMPORT/", "");
            tower = tower.Replace("_spc_", " ");
            tower = tower.Replace("_-_", ">");
            tower = tower.Replace("/", "");

            // Get tower object for the tower we are changing
            p = PDL.Designs[tower];

            using (StreamReader rdr = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                body = rdr.ReadToEnd();
            }

            body = body.Substring(body.IndexOf("=", 0) + 1);
            body = body.Replace("%0D%0A", "#");
            body = body.Replace("%2C", ",");
            body = body.Replace("%7C", "|");
            body = body.Replace("+", " ");

            if (!body.Contains("#") || !body.Contains(",") || !body.Contains("|"))
            {
                strHTML.Append("<font size=\"3\" color=\"red\"><b> Tower Data is Missing - Import Canceled </b></font>");
                return strHTML.ToString();
            }
            split1 = body.Split('#');
            twrS = split1[0].Split(',');
            modS = split1[1].Split('|');
            lnkS = split1[2].Split('|');

            // Get new tower name
            if (twrS.Length >= 2)
            {
                twrName = twrS[0];
                tID = Convert.ToInt32(twrS[1]);

                if (twrName != tower)
                {
                    // Potential name difference, ignore it for now
                }
                if (p.PosTower.typeID != tID)
                    if (!TL.Towers.ContainsKey(tID))
                    {
                        strHTML.Append("<font size=\"3\" color=\"red\"><b> Tower Type ID not Found - Import Canceled </b></font>");
                        return strHTML.ToString();
                    }
                    else
                        p.PosTower = new New_Tower(TL.Towers[tID]);
            }

            // Modules
            p.Modules.Clear();
            for (int x = 1; x < modS.Length; x++)
            {
                Mod = modS[x].Split(',');
                tID = Convert.ToInt32(Mod[0]);
                qty = Convert.ToInt32(Mod[1]);

                modID = Convert.ToInt32(Mod[2]);
                selReact = Mod[3];
                selMineral = Mod[4];
                state = Mod[0];
                locn = Mod[0];

                if (ML.Modules.ContainsKey(tID))
                {
                    nm = new Module(ML.Modules[tID]);
                    nm.Qty = qty;
                    nm.ModuleID = modID;

                    nr = GetReactionForName(selReact, nm);
                    msr = GetMineralForName(selMineral, nm);

                    nm.selReact = new Reaction(nr);
                    nm.selMineral = new MoonSiloReactMineral(msr);
                    nm.State = state;
                    nm.Location = locn;

                    p.Modules.Add(nm);
                    cpu += (nm.CPU_Used * nm.Qty);
                    pwr += (nm.Power_Used * nm.Qty);
                }
                else
                {
                    // No module with that type ID exists, discard the data
                    strHTML.Append("<font size=\"3\" color=\"red\"><b> Module ID not Found [" + tID + "] Module Skipped.</b></font>");
                }
            }

            // Reaction Links
            p.ReactionLinks.Clear();
            for (int x = 1; x < lnkS.Length; x++)
            {
                Mod = lnkS[x].Split(',');
                lnkID = Convert.ToInt32(Mod[0]);
                inpID = Convert.ToInt32(Mod[1]);
                outID = Convert.ToInt32(Mod[2]);
                XfrQ = Convert.ToInt32(Mod[3]);
                XferV = Convert.ToInt32(Mod[4]);
                
                srcNm = Mod[5];
                dstNm = Mod[6];

                rl = new ReactionLink(lnkID, inpID, outID, XfrQ, XferV, LColor[x-1], srcNm, dstNm);
                p.ReactionLinks.Add(rl);
            }

            PDL.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PDL.CalculatePOSReactions();
            if (PMF != null)
            {
                PMF.UpdatePOSForNewData();
            }
            PDL.SaveDesignListing();

            return strHTML.ToString();
        }

        private Reaction GetReactionForName(string name, Module m)
        {
            foreach (Reaction r in m.ReactList)
            {
                if (r.reactName == name)
                    return r;
            }
            return null;
        }

        private MoonSiloReactMineral GetMineralForName(string name, Module m)
        {
            foreach (MoonSiloReactMineral r in m.MSRList)
            {
                if (r.name == name)
                    return r;
            }
            return null;
        }


        #endregion

        #region IGB Security


        public static void AddSecurityItem(IGBSecurity igbS, long list)
        {
            if (list == 0)
            {
                // POS
                POSSecList.Add(new IGBSecurity(igbS));
            }
            else if (list == 1)
            {
                // IHUB
                IHubSecList.Add(new IGBSecurity(igbS));
            }
            SaveSecurityListing();
        }

        public static void RemoveSecurityItem(IGBSecurity igbS, long list)
        {
            if (list == 0)
            {
                // POS
                foreach (IGBSecurity i in POSSecList)
                {
                    if (i.IsEqualTo(igbS))
                        POSSecList.Remove(i);
                }
            }
            else if (list == 1)
            {
                // IHUB
                foreach (IGBSecurity i in IHubSecList)
                {
                    if (i.IsEqualTo(igbS))
                        IHubSecList.Remove(i);
                }
            }
            SaveSecurityListing();
        }

        public static void SaveSecurityListing()
        {
            string fname;

            fname = Path.Combine(PoSSave_Path, "POS_Security.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, POSSecList);
            pStream.Close();

            fname = Path.Combine(PoSSave_Path, "IHub_Security.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, IHubSecList);
            pStream.Close();

            fname = Path.Combine(PoSSave_Path, "User_Security.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, SecUsers);
            pStream.Close();
        }

        public static void LoadSecurityListing()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PoSSave_Path, "POS_Security.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    POSSecList = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }

            fname = Path.Combine(PoSSave_Path, "IHub_Security.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    IHubSecList = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }

            fname = Path.Combine(PoSSave_Path, "User_Security.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    SecUsers = (SortedList<long, Player>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }


        #endregion

        #region Background Data Workers (API and Notifications)

        private void ReactionTimer_Tick(object sender, EventArgs e)
        {
            PDL.CalculatePOSReactions();
        }

        private void bgw_SendNotify_DoWork(object sender, DoWorkEventArgs e)
        {
            NL.CheckAndSendNotificationIfActive();
        }

        private void bgw_APIUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string cache;
            long seconds;
            DateTime now, cTim;
            TimeSpan diffT;
            SystemSovList SSL = new SystemSovList();

            SSL.LoadSovListFromAPI(1);
            SLs.LoadSystemListFromDisk();
            AL.LoadAllianceListFromAPI(1);
            API_D.SaveAPIListing();
            PDL.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            // Need to apply linked module values here
            PDL.UpdateTowerSiloValuesForAPI();

            PDL.CalculatePOSReactions();

            // I know that data is available 1 time per hour, so schedule the next check to take place
            // 60 minutes (3600 sec) from now.
            // Check for new Data in 60 minutes 
            cache = API_D.cacheUntil;
            if (cache != "")
            {
                cTim = Convert.ToDateTime(cache);
                cTim = TimeZone.CurrentTimeZone.ToLocalTime(cTim);
                now = DateTime.Now;
                diffT = cTim.Subtract(now);

                seconds = ((diffT.Minutes * 60) + diffT.Seconds + 15) * 1000; // 15 second buffer to ensure available on next request
            }
            else
            {
                seconds = 90000;    // Default to 15 minute checks if no data is available
            }
            if (seconds < 1)
                seconds = 90000;

            t_MonitorUpdate.Interval = seconds;
            t_MonitorUpdate.Enabled = true;

            if (PMF != null)
            {
                PMF.UpdatePOSForNewData();
                PMF.PopulateStoredFuelDisplay();
                PMF.PopulateIHUBOwners();
                UpdateReactTime = true;
            }
            PDL.SaveDesignListing();
            SaveIHubListing();
        }

        private void bgw_APIUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet cd, mt;
            string strSQL;

            if (PMF != null)
            {
                PMF.DataUpdateInProgress();
            }

            if (TowerMods.Count < 1)
            {
                // Get Category Listing
                strSQL = "SELECT invGroups.groupID FROM invGroups WHERE invGroups.categoryID=23 AND invGroups.published=1;";
                cd = EveHQ.Core.DataFunctions.GetData(strSQL);
                foreach (DataRow dr in cd.Tables[0].Rows)
                {

                    strSQL = "SELECT invTypes.typeID FROM invTypes WHERE invTypes.groupID=" + Convert.ToInt32(dr.ItemArray[0].ToString()) + ";";
                    mt = EveHQ.Core.DataFunctions.GetData(strSQL);

                    foreach (DataRow mr in mt.Tables[0].Rows)
                        TowerMods.Add(Convert.ToInt32(mr.ItemArray[0].ToString()));
                }
            }

            if ((Config.data.AutoAPI > 0) || ManualUpdate)
            {
                API_D.LoadPOSDataFromAPI();
                GetAllySovLists();
                GetCharAndCorpAssets();
                ManualUpdate = false;
            }

            if (PMF != null)
               PMF.PopulateCorpList();
        }

        private void GetAllySovLists()
        {
            SystemSovList SSL = new SystemSovList();
            PlugInData.AL = new AllianceList();

            PlugInData.AL.LoadAllianceListFromAPI(1);
            SSL.LoadSovListFromAPI(1);
        }

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
            else
                return false;

            return true;
        }

        private long ConvertOfficeSystemID(long sysID)
        {
            long newID = 0;

            if ((sysID >= 66000000) && (sysID < 67000000))
                newID = sysID - 6000001;
            else if ((sysID >= 67000000) && (sysID < 68000000))
                newID = sysID - 6000001;
            else
                newID = sysID;

            return newID;
        }

        public static bool IsPilotAllowedAccessToAPI(EveHQ.Core.Pilot plt)
        {
            string acct;
            EveHQ.Core.EveAccount eveAcct;

            if (plt.CorpRoles == null)
                return false;

            if (!plt.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Director))
                return false;
            
            acct = plt.Account;
            if (EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(acct))
            {
                eveAcct = (EveHQ.Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acct];
                if (!eveAcct.APIKeyType.Equals(EveHQ.Core.APIKeyTypes.Full))
                    return false;

                if (eveAcct.APIAccountStatus.Equals(EveHQ.Core.APIAccountStatuses.Disabled))
                    return false;
            }
            else 
                return false;

            return true;
        }

        public static void LogAPIError(int code, string text, string pilot)
        {
            string fname;

            fname = Path.Combine(PoSSave_Path, "API_Errors.log");

            using (StreamWriter w = File.AppendText(fname))
            {
                w.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " --> API Error [ " + code + " | " + text + " ] Received for : " + pilot);
                w.Close();
            }
        }

        public void GetCharAndCorpAssets()
        {
            foreach (EveHQ.Core.EveAccount pilotAccount in EveHQ.Core.HQ.EveHQSettings.Accounts)
            {
                if (pilotAccount.APIAccountStatus.Equals(Core.APIAccountStatuses.Active))
                {
                    if (pilotAccount.APIKeySystem.Equals(Core.APIKeySystems.Version1))
                    {
                        foreach (string pName in pilotAccount.Characters)
                        {
                            if (EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(pName))
                            {
                                EveHQ.Core.Pilot selPilot = (EveHQ.Core.Pilot)EveHQ.Core.HQ.EveHQSettings.Pilots[pName];
                                GetCorpAssetsV1(selPilot);
                                GetCharAssetsV1(selPilot);
                            }
                        }
                    }
                    else if (pilotAccount.APIKeySystem.Equals(Core.APIKeySystems.Version2))
                    {
                        if (pilotAccount.APIKeyType.Equals(Core.APIKeyTypes.Corporation))
                        {
                            GetCorpAssetsV2(pilotAccount);
                        }
                        else if (pilotAccount.APIKeyType.Equals(Core.APIKeyTypes.Character) || pilotAccount.APIKeyType.Equals(Core.APIKeyTypes.Account))
                        {
                            foreach (string pName in pilotAccount.Characters)
                            {
                                if (EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(pName))
                                {
                                    EveHQ.Core.Pilot selPilot = (EveHQ.Core.Pilot)EveHQ.Core.HQ.EveHQSettings.Pilots[pName];
                                    GetCharAssetsV2(pilotAccount, selPilot);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GetCorpAssetsV1(EveHQ.Core.Pilot selPilot)
        {
            string acctName, curTime;
            long typeID, stFlag, locID, iugID;
            int itmCell;
            APIModule apm;
            EveHQ.Core.EveAccount pilotAccount;
            XmlDocument apiXML = new XmlDocument();
            XmlNodeList modList, itemList, timeList;
            XmlNode itmNode;
            string ihLoc, itmName;
            bool hubExist = false;
            EveAPI.APITypes sel;
            SystemIHub IH;
            InfrastructureUG IUG;

            if (selPilot.Active && IsPilotAllowedAccessToAPI(selPilot))
            {
                acctName = selPilot.Account;
                try
                {
                    pilotAccount = (Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acctName];
                    sel = EveAPI.APITypes.AssetsCorp;
                    EveAPI.EveAPIRequest APIReq;
                    APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
                    apiXML = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);

                    if ((APIReq.LastAPIError > 0) && (APIReq.LastAPIResult != EveAPI.APIResults.ReturnedCached))
                    {
                        if (!ThrottleList.ContainsKey(selPilot.Name))
                        {
                            ThrottleList.Add(selPilot.Name, new throttlePlayer(1));
                        }
                        else
                        {
                            ThrottleList[selPilot.Name].IncCount();
                        }
                        LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, selPilot.Name);
                    }
                    if (!CheckXML(apiXML))
                    {
                        return;
                    }
                }
                catch
                {
                    DialogResult dr = MessageBox.Show("An Error was encountered in Pilot Corp Assets API Download for --> " + selPilot.Name, "PoSManager: API Error", MessageBoxButtons.OK);
                }

                timeList = apiXML.SelectNodes("/eveapi");
                curTime = timeList[0].ChildNodes[0].InnerText;

                if (!CpML.ContainsKey(selPilot.Corp))
                {
                    CpML.Add(selPilot.Corp, new SortedList<long, APIModule>());
                }

                modList = apiXML.SelectNodes("/eveapi/result/rowset/row");
                foreach (XmlNode psN in modList)
                {
                    typeID = Convert.ToInt64(psN.Attributes.GetNamedItem("typeID").Value.ToString());
                    if (typeID.Equals(32458))
                    {
                        // I-Hub found
                        locID = Convert.ToInt64(psN.Attributes.GetNamedItem("locationID").Value.ToString());
                        if (SystemIDToStr.ContainsKey(locID))
                            ihLoc = SystemIDToStr[locID];
                        else
                            ihLoc = "Unknown System - Help me....";

                        if ((iHubs.ContainsKey(selPilot.Corp)) && (iHubs[selPilot.Corp].ContainsKey(ihLoc)))
                        {
                            // Hub already exists
                            IH = iHubs[selPilot.Corp][ihLoc];
                            hubExist = true;
                        }
                        else
                        {
                            IH = new SystemIHub();
                            IH.ID = Convert.ToInt64(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                            IH.owner = selPilot.Corp;

                            if (!iHubs.ContainsKey(selPilot.Corp))
                                iHubs.Add(IH.owner, new SortedList<string, SystemIHub>());

                            IH.locID = locID;
                            IH.location = ihLoc;
                            IH.ownerID = Convert.ToInt64(selPilot.CorpID);
                            IH.online = true;

                            // Query to get Item names by typeID
                            if (EveHQ.Core.HQ.itemData.ContainsKey(typeID.ToString()) == false)
                                continue;

                            IH.itmName = EveHQ.Core.HQ.itemData[typeID.ToString()].Name;

                        }

                        // check for installed upgrades
                        if (psN.ChildNodes.Count > 0)
                        {
                            itmNode = psN.ChildNodes[0];
                            itemList = itmNode.ChildNodes;
                            foreach (XmlNode itN in itemList)
                            {
                                iugID = Convert.ToInt64(itN.Attributes.GetNamedItem("itemID").Value.ToString());

                                IUG = new InfrastructureUG();
                                IUG.ID = iugID;
                                IUG.typeID = Convert.ToInt64(itN.Attributes.GetNamedItem("typeID").Value.ToString());
                                stFlag = Convert.ToInt32(itN.Attributes.GetNamedItem("flag").Value.ToString());

                                if (stFlag == 144)
                                    IUG.online = true;
                                else
                                    IUG.online = false;

                                // Query to get Item names by typeID
                                if (EveHQ.Core.HQ.itemData.ContainsKey(IUG.typeID.ToString()) == false)
                                    continue;

                                IUG.name = EveHQ.Core.HQ.itemData[IUG.typeID.ToString()].Name;

                                if (IUG.name.Contains("Ore"))
                                {
                                    itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                    itmName = "Ore " + itmCell;
                                    itmCell += 0;
                                    IUG.cell = itmCell;
                                    IUG.colNm = itmName;
                                    if (IH.OreUG.ContainsKey(IUG.name))
                                    {
                                        IH.OreUG[IUG.name].online = IUG.online;
                                    }
                                    else
                                        IH.OreUG.Add(IUG.name, IUG);
                                }
                                else if (IUG.name.Contains("Survey"))
                                {
                                    itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                    itmName = "Surv " + itmCell;
                                    itmCell += 5;
                                    IUG.cell = itmCell;
                                    IUG.colNm = itmName;
                                    if (IH.SrvUG.ContainsKey(IUG.name))
                                    {
                                        IH.SrvUG[IUG.name].online = IUG.online;
                                    }
                                    else
                                        IH.SrvUG.Add(IUG.name, IUG);
                                }
                                else if (IUG.name.Contains("Pirate"))
                                {
                                    itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                    itmName = "Pirt " + itmCell;
                                    itmCell += 15;
                                    IUG.cell = itmCell;
                                    IUG.colNm = itmName;
                                    if (IH.PrtUG.ContainsKey(IUG.name))
                                    {
                                        IH.PrtUG[IUG.name].online = IUG.online;
                                    }
                                    else
                                        IH.PrtUG.Add(IUG.name, IUG);
                                }
                                else if (IUG.name.Contains("Entrapment"))
                                {
                                    itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                    itmName = "Entp " + itmCell;
                                    itmCell += 10;
                                    IUG.cell = itmCell;
                                    IUG.colNm = itmName;
                                    if (IH.EntUG.ContainsKey(IUG.name))
                                    {
                                        IH.EntUG[IUG.name].online = IUG.online;
                                    }
                                    else
                                        IH.EntUG.Add(IUG.name, IUG);
                                }
                                else if (IUG.name.Contains("Quantum"))
                                {
                                    itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                    itmName = "Q.Flx " + itmCell;
                                    itmCell += 20;
                                    IUG.cell = itmCell;
                                    IUG.colNm = itmName;
                                    if (IH.FlxUG.ContainsKey(IUG.name))
                                    {
                                        IH.FlxUG[IUG.name].online = IUG.online;
                                    }
                                    else
                                        IH.FlxUG.Add(IUG.name, IUG);
                                }
                                else if (IUG.name.Contains("Logistics"))
                                {
                                    IUG.cell = 26;
                                    IUG.colNm = "Bridge";
                                    if (IH.StratUG.ContainsKey(IUG.name))
                                    {
                                        IH.StratUG[IUG.name].online = IUG.online;
                                    }
                                    else
                                        IH.StratUG.Add(IUG.name, IUG);
                                }
                                else if (IUG.name.Contains("Supercapital"))
                                {
                                    IUG.cell = 29;
                                    IUG.colNm = "S-Cap";
                                    if (IH.StratUG.ContainsKey(IUG.name))
                                    {
                                        IH.StratUG[IUG.name].online = IUG.online;
                                    }
                                    else
                                        IH.StratUG.Add(IUG.name, IUG);
                                }
                                else if (IUG.name.Contains("Navigation"))
                                {
                                    IUG.cell = 27;
                                    IUG.colNm = "CynoGen";
                                    if (IH.StratUG.ContainsKey(IUG.name))
                                    {
                                        IH.StratUG[IUG.name].online = IUG.online;
                                    }
                                    else
                                        IH.StratUG.Add(IUG.name, IUG);
                                }
                                else if (IUG.name.Contains("Suppresion"))
                                {
                                    IUG.cell = 28;
                                    IUG.colNm = "CynoJam";
                                    if (IH.StratUG.ContainsKey(IUG.name))
                                    {
                                        IH.StratUG[IUG.name].online = IUG.online;
                                    }
                                    else
                                        IH.StratUG.Add(IUG.name, IUG);
                                }
                            }
                        }
                        if (!hubExist)
                        {
                            iHubs[IH.owner].Add(IH.location, IH);
                        }
                    }
                    else
                    {
                        // Something Else
                        apm = new APIModule();
                        apm.typeID = typeID;
                        apm.updateTime = curTime;
                        apm.modID = Convert.ToInt64(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                        apm.systemID = ConvertOfficeSystemID(Convert.ToInt32(psN.Attributes.GetNamedItem("locationID").Value.ToString()));
                        apm.Qty = Convert.ToInt64(psN.Attributes.GetNamedItem("quantity").Value.ToString());
                        apm.corpID = Convert.ToInt64(selPilot.CorpID);
                        apm.corpName = selPilot.Corp;
                        apm.ownerName = selPilot.Corp;

                        if (!CorpIDToName.ContainsKey(Convert.ToInt64(selPilot.CorpID)))
                            CorpIDToName.Add(Convert.ToInt64(selPilot.CorpID), selPilot.Corp);

                        // Query to get Item names by typeID
                        if (EveHQ.Core.HQ.itemData.ContainsKey(typeID.ToString()) == false)
                            continue;

                        apm.name = EveHQ.Core.HQ.itemData[typeID.ToString()].Name;

                        if (StationList.ContainsKey(apm.systemID))
                        {
                            apm.InStation = true;
                            apm.locName = StationList[apm.systemID].Name;
                        }
                        else if (SLs.SysNameConv.ContainsKey(apm.systemID))
                        {
                            // Item is in a system
                            apm.InSpace = true;
                            apm.locName = SLs.SysNameConv[apm.systemID];
                        }
                        else
                        {
                            apm.locName = "In Space";
                        }

                        if (!ItemIDToName.ContainsKey(apm.modID))
                            ItemIDToName.Add(apm.modID, apm.name);

                        if (psN.ChildNodes.Count > 0)
                        {
                            itmNode = psN.ChildNodes[0];
                            ScanLocationAssetts(itmNode.ChildNodes, apm);
                        }

                        if (CheckTypeIDForFuelOrMod(apm.typeID, false))
                        {
                            if (!CpML[apm.corpName].ContainsKey(apm.modID))
                                CpML[apm.corpName].Add(apm.modID, apm);
                        }
                    }
                }
            }
        }

        private void GetCorpAssetsV2(EveHQ.Core.EveAccount pilotAccount)
        {
            string curTime;
            long typeID, stFlag, locID, iugID, cID = 0;
            int itmCell;
            APIModule apm;
            SortedList<long, APIModule> alm = new SortedList<long, APIModule>();
            XmlDocument apiXML, apiCorpDetails; 
            XmlNodeList modList, itemList, timeList;
            XmlNode itmNode, CnL;
            string ihLoc, itmName, corpName = "", corpID = "";
            bool hubExist = false;
            SystemIHub IH;
            InfrastructureUG IUG;

            apiXML = new XmlDocument();

            // If account does not have access to this information, then do not try to get it !
            if (!pilotAccount.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.CorporationSheet) ||
                !pilotAccount.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.AssetList))
                return;

            try
            {
                // Get my Corporation Details - do not have a setup pilot at this point
                EveAPI.EveAPIRequest APIReq;
                APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);

                apiCorpDetails = APIReq.GetAPIXML(EveAPI.APITypes.CorpSheet, pilotAccount.ToAPIAccount(), "", EveAPI.APIReturnMethods.ReturnStandard);
                if (!PlugInData.CheckXML(apiCorpDetails))
                    return;

                // Pull out corporation name and ID <-- used for later data storage and usage
                CnL = apiCorpDetails.SelectSingleNode("/eveapi/result");
                corpName = CnL["corporationName"].InnerText;
                corpID = CnL["corporationID"].InnerText;
                cID = Convert.ToInt32(corpID);

                apiXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, pilotAccount.ToAPIAccount(), "", EveAPI.APIReturnMethods.ReturnStandard);

                if ((APIReq.LastAPIError > 0) && (APIReq.LastAPIResult != EveAPI.APIResults.ReturnedCached))
                {
                    if (!ThrottleList.ContainsKey(pilotAccount.FriendlyName))
                    {
                        ThrottleList.Add(pilotAccount.FriendlyName, new throttlePlayer(1));
                    }
                    else
                    {
                        ThrottleList[pilotAccount.FriendlyName].IncCount();
                    }
                    LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText,pilotAccount.FriendlyName);
                }
                if (!CheckXML(apiXML))
                {
                    return;
                }
            }
            catch
            {
                DialogResult dr = MessageBox.Show("An Error was encountered in Pilot Corp Assets API Download for --> " + pilotAccount.FriendlyName, "PoSManager: API Error", MessageBoxButtons.OK);
            }

            // If no corp name or ID - then just exit
            if (corpName.Equals("") || cID.Equals(0))
                return;

            timeList = apiXML.SelectNodes("/eveapi");
            curTime = timeList[0].ChildNodes[0].InnerText;

            if (!CpML.ContainsKey(corpName))
            {
                CpML.Add(corpName, new SortedList<long, APIModule>());
            }

            modList = apiXML.SelectNodes("/eveapi/result/rowset/row");
            foreach (XmlNode psN in modList)
            {
                typeID = Convert.ToInt64(psN.Attributes.GetNamedItem("typeID").Value.ToString());
                if (typeID.Equals(32458))
                {
                    // I-Hub found
                    locID = Convert.ToInt64(psN.Attributes.GetNamedItem("locationID").Value.ToString());
                    if (SystemIDToStr.ContainsKey(locID))
                        ihLoc = SystemIDToStr[locID];
                    else
                        ihLoc = "Unknown System - Help me....";


                    if ((iHubs.ContainsKey(corpName)) && (iHubs[corpName].ContainsKey(ihLoc)))
                    {
                        // Hub already exists
                        IH = iHubs[corpName][ihLoc];
                        hubExist = true;
                    }
                    else
                    {
                        IH = new SystemIHub();
                        IH.ID = Convert.ToInt64(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                        IH.owner = corpName;

                        if (!iHubs.ContainsKey(corpName))
                            iHubs.Add(IH.owner, new SortedList<string, SystemIHub>());

                        IH.locID = locID;
                        IH.location = ihLoc;
                        IH.ownerID = Convert.ToInt64(corpName);
                        IH.online = true;

                        // Query to get Item names by typeID
                        if (EveHQ.Core.HQ.itemData.ContainsKey(typeID.ToString()) == false)
                            continue;

                        IH.itmName = EveHQ.Core.HQ.itemData[typeID.ToString()].Name;

                    }

                    // check for installed upgrades
                    if (psN.ChildNodes.Count > 0)
                    {
                        itmNode = psN.ChildNodes[0];
                        itemList = itmNode.ChildNodes;
                        foreach (XmlNode itN in itemList)
                        {
                            iugID = Convert.ToInt64(itN.Attributes.GetNamedItem("itemID").Value.ToString());

                            IUG = new InfrastructureUG();
                            IUG.ID = iugID;
                            IUG.typeID = Convert.ToInt64(itN.Attributes.GetNamedItem("typeID").Value.ToString());
                            stFlag = Convert.ToInt32(itN.Attributes.GetNamedItem("flag").Value.ToString());

                            if (stFlag == 144)
                                IUG.online = true;
                            else
                                IUG.online = false;

                            // Query to get Item names by typeID
                            if (EveHQ.Core.HQ.itemData.ContainsKey(IUG.typeID.ToString()) == false)
                                continue;

                            IUG.name = EveHQ.Core.HQ.itemData[IUG.typeID.ToString()].Name;

                            if (IUG.name.Contains("Ore"))
                            {
                                itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                itmName = "Ore " + itmCell;
                                itmCell += 0;
                                IUG.cell = itmCell;
                                IUG.colNm = itmName;
                                if (IH.OreUG.ContainsKey(IUG.name))
                                {
                                    IH.OreUG[IUG.name].online = IUG.online;
                                }
                                else
                                    IH.OreUG.Add(IUG.name, IUG);
                            }
                            else if (IUG.name.Contains("Survey"))
                            {
                                itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                itmName = "Surv " + itmCell;
                                itmCell += 5;
                                IUG.cell = itmCell;
                                IUG.colNm = itmName;
                                if (IH.SrvUG.ContainsKey(IUG.name))
                                {
                                    IH.SrvUG[IUG.name].online = IUG.online;
                                }
                                else
                                    IH.SrvUG.Add(IUG.name, IUG);
                            }
                            else if (IUG.name.Contains("Pirate"))
                            {
                                itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                itmName = "Pirt " + itmCell;
                                itmCell += 15;
                                IUG.cell = itmCell;
                                IUG.colNm = itmName;
                                if (IH.PrtUG.ContainsKey(IUG.name))
                                {
                                    IH.PrtUG[IUG.name].online = IUG.online;
                                }
                                else
                                    IH.PrtUG.Add(IUG.name, IUG);
                            }
                            else if (IUG.name.Contains("Entrapment"))
                            {
                                itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                itmName = "Entp " + itmCell;
                                itmCell += 10;
                                IUG.cell = itmCell;
                                IUG.colNm = itmName;
                                if (IH.EntUG.ContainsKey(IUG.name))
                                {
                                    IH.EntUG[IUG.name].online = IUG.online;
                                }
                                else
                                    IH.EntUG.Add(IUG.name, IUG);
                            }
                            else if (IUG.name.Contains("Quantum"))
                            {
                                itmCell = Convert.ToInt32(IUG.name.Substring(IUG.name.Length - 1, 1));
                                itmName = "Q.Flx " + itmCell;
                                itmCell += 20;
                                IUG.cell = itmCell;
                                IUG.colNm = itmName;
                                if (IH.FlxUG.ContainsKey(IUG.name))
                                {
                                    IH.FlxUG[IUG.name].online = IUG.online;
                                }
                                else
                                    IH.FlxUG.Add(IUG.name, IUG);
                            }
                            else if (IUG.name.Contains("Logistics"))
                            {
                                IUG.cell = 26;
                                IUG.colNm = "Bridge";
                                if (IH.StratUG.ContainsKey(IUG.name))
                                {
                                    IH.StratUG[IUG.name].online = IUG.online;
                                }
                                else
                                    IH.StratUG.Add(IUG.name, IUG);
                            }
                            else if (IUG.name.Contains("Supercapital"))
                            {
                                IUG.cell = 29;
                                IUG.colNm = "S-Cap";
                                if (IH.StratUG.ContainsKey(IUG.name))
                                {
                                    IH.StratUG[IUG.name].online = IUG.online;
                                }
                                else
                                    IH.StratUG.Add(IUG.name, IUG);
                            }
                            else if (IUG.name.Contains("Navigation"))
                            {
                                IUG.cell = 27;
                                IUG.colNm = "CynoGen";
                                if (IH.StratUG.ContainsKey(IUG.name))
                                {
                                    IH.StratUG[IUG.name].online = IUG.online;
                                }
                                else
                                    IH.StratUG.Add(IUG.name, IUG);
                            }
                            else if (IUG.name.Contains("Suppresion"))
                            {
                                IUG.cell = 28;
                                IUG.colNm = "CynoJam";
                                if (IH.StratUG.ContainsKey(IUG.name))
                                {
                                    IH.StratUG[IUG.name].online = IUG.online;
                                }
                                else
                                    IH.StratUG.Add(IUG.name, IUG);
                            }
                        }
                    }
                    if (!hubExist)
                    {
                        iHubs[IH.owner].Add(IH.location, IH);
                    }
                }
                else
                {
                    // Something Else
                    apm = new APIModule();
                    apm.typeID = typeID;
                    apm.updateTime = curTime;
                    apm.modID = Convert.ToInt64(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                    apm.systemID = ConvertOfficeSystemID(Convert.ToInt32(psN.Attributes.GetNamedItem("locationID").Value.ToString()));
                    apm.Qty = Convert.ToInt64(psN.Attributes.GetNamedItem("quantity").Value.ToString());
                    apm.corpID = Convert.ToInt64(corpID);
                    apm.corpName = corpName;
                    apm.ownerName = corpName;

                    if (!CorpIDToName.ContainsKey(Convert.ToInt64(corpID)))
                        CorpIDToName.Add(Convert.ToInt64(corpID), corpName);

                    // Query to get Item names by typeID
                    if (EveHQ.Core.HQ.itemData.ContainsKey(typeID.ToString()) == false)
                        continue;

                    apm.name = EveHQ.Core.HQ.itemData[typeID.ToString()].Name;

                    if (StationList.ContainsKey(apm.systemID))
                    {
                        apm.InStation = true;
                        apm.locName = StationList[apm.systemID].Name;
                    }
                    else if (SLs.SysNameConv.ContainsKey(apm.systemID))
                    {
                        // Item is in a system
                        apm.InSpace = true;
                        apm.locName = SLs.SysNameConv[apm.systemID];
                    }
                    else
                    {
                        apm.locName = "In Space";
                    }

                    if (!ItemIDToName.ContainsKey(apm.modID))
                        ItemIDToName.Add(apm.modID, apm.name);

                    if (psN.ChildNodes.Count > 0)
                    {
                        itmNode = psN.ChildNodes[0];
                        ScanLocationAssetts(itmNode.ChildNodes, apm);
                    }

                    if (CheckTypeIDForFuelOrMod(apm.typeID, false))
                    {
                        if (!CpML[apm.corpName].ContainsKey(apm.modID))
                            CpML[apm.corpName].Add(apm.modID, apm);
                    }
                }
            }
        }

        public bool CheckTypeIDForFuelOrMod(long ID, bool FuelOnly)
        {
            switch (ID)
            {
                case 44:    // Enr Uranium
                case 3683:  // Oxygen
                case 3689:  // Mech Parts
                case 9832:  // Coolant
                case 9848:  // Robotics
                case 16272: // Heavy Water
                case 16273: // Liquid Ozone
                case 24592: // Charter
                case 24593: // Charter
                case 24594: // Charter
                case 24595: // Charter
                case 24596: // Charter
                case 24597: // Charter
                case 17888: // Nitrogen Isotopes
                case 16274: // Helium Isotopes
                case 17889: // Hydrogen Isotopes
                case 17887: // Oxygen Isotopes
                case 16275: // Strontium
                    return true;
                default:
                    if ((TowerMods.Contains(ID)) && !FuelOnly)
                        return true;

                    return false;
            }
        }

        public void SaveStationListing()
        {
            string fname;

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = Path.Combine(PoSCache_Path, "API_Stations.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, StationList);
            pStream.Close();
        }

        public void LoadStationListing()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PoSCache_Path, "API_Stations.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    StationList = (SortedList<long, Station>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public void BuildStations()
        {
            string strSQL;
            Station ns;
            DataSet stnData;
            long rgnID = 0, cnID = 0, sysID = 0;

            strSQL = "SELECT * FROM staStations";
            strSQL += " WHERE staStations.stationTypeID < 12242;";
            stnData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!stnData.Equals(System.DBNull.Value))
            {
                if (stnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in stnData.Tables[0].Rows)
                    {
                        rgnID = Convert.ToInt32(row.ItemArray[10]);
                        cnID = Convert.ToInt32(row.ItemArray[9]);

                        if (SkipRegions.Contains(rgnID))
                            continue;

                        ns = new Station();
                        ns.ID = Convert.ToInt32(row.ItemArray[0]);
                        ns.stationTypeID = Convert.ToInt32(row.ItemArray[6]);
                        ns.corpID = Convert.ToInt32(row.ItemArray[7]);
                        ns.Name = row.ItemArray[11].ToString();
                        ns.regionID = rgnID;
                        ns.constID = cnID;
                        ns.X = Convert.ToDouble(row.ItemArray[12]);
                        ns.Y = Convert.ToDouble(row.ItemArray[13]);
                        ns.Z = Convert.ToDouble(row.ItemArray[14]);
                        ns.Faction = Convert.ToInt32(row.ItemArray[7]);
                        sysID = Convert.ToInt32(row.ItemArray[8]);

                        if (!StationList.ContainsKey(ns.ID))
                            StationList.Add(ns.ID, ns);
                    }
                }
            }

            GetConqStationsFromAPI();

            SaveStationListing();
        }

        public void GetConqStationsFromAPI()
        {
            XmlDocument stnData;
            XmlNodeList cqList, dateList;
            Station cs;

            stnData = new XmlDocument();

            EveAPI.EveAPIRequest APIReq;
            APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
            stnData = APIReq.GetAPIXML(EveAPI.APITypes.Conquerables, EveAPI.APIReturnMethods.ReturnStandard);

            if ((APIReq.LastAPIError > 0) && (APIReq.LastAPIResult != EveAPI.APIResults.ReturnedCached))
            {
                if (!ThrottleList.ContainsKey("ConqStations"))
                {
                    ThrottleList.Add("ConqStations", new throttlePlayer(1));
                }
                else
                {
                    ThrottleList["ConqStations"].IncCount();
                }
                LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, "ConqStations");
            }
                
            if (!CheckXML(stnData))
                return;

            // Get conquerable station list API information
            // Parse API data longo my stuff
            // columns="stationID,stationName,stationTypeID,solarSystemID,corporationID,corporationName"
            try
            {
                dateList = stnData.SelectNodes("/eveapi");
                cqList = stnData.SelectNodes("/eveapi/result/rowset/row");

                foreach (XmlNode stn in cqList)
                {
                    cs = new Station();

                    cs.ID = Convert.ToInt32(stn.Attributes.GetNamedItem("stationID").Value.ToString());
                    cs.Name = stn.Attributes.GetNamedItem("stationName").Value.ToString();
                    cs.stationTypeID = Convert.ToInt32(stn.Attributes.GetNamedItem("stationTypeID").Value.ToString());
                    cs.SSysID = Convert.ToInt32(stn.Attributes.GetNamedItem("solarSystemID").Value.ToString());
                    cs.corpID = Convert.ToInt32(stn.Attributes.GetNamedItem("corporationID").Value.ToString());
                    cs.Owner = stn.Attributes.GetNamedItem("corporationName").Value.ToString();
                    cs.isConquerable = true;

                    if (!StationList.ContainsKey(cs.ID))
                        StationList.Add(cs.ID, cs);
                }
            }
            catch
            {
                DialogResult dr = MessageBox.Show("An Error was encountered while Parsing Conquerable Station API.", "RouteMap: API Error", MessageBoxButtons.OK);
            }
        }

        private void ScanLocationAssetts(XmlNodeList XNL, APIModule APMI)
        {
            XmlNode siNode;
            ModuleItem mitm;
            string strSQL;
            DataSet itmNM;

            foreach (XmlNode itN in XNL)
            {
                mitm = new ModuleItem();
                mitm.typeID = Convert.ToInt64(itN.Attributes.GetNamedItem("typeID").Value.ToString());
                mitm.itemID = Convert.ToInt64(itN.Attributes.GetNamedItem("itemID").Value.ToString());
                mitm.qty = Convert.ToInt64(itN.Attributes.GetNamedItem("quantity").Value.ToString());

                // Database query to get Item names by typeID
                strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + mitm.typeID + ";";
                itmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                if ((itmNM == null) || (itmNM.Tables[0].Rows.Count < 1))
                    continue;

                mitm.name = itmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                if (!ItemIDToName.ContainsKey(mitm.itemID))
                    ItemIDToName.Add(mitm.itemID, mitm.name);

                mitm.SubItems = new SortedList<long, ModuleItem>();
                if (itN.ChildNodes.Count > 0)
                {
                    // Contents of Cans, Ships, Etc... (in theory)
                    siNode = itN.ChildNodes[0];
                    ScanModuleAssetts(siNode.ChildNodes, mitm);
                }

                try
                {
                    APMI.Items.Add(mitm.itemID, mitm);
                }
                catch
                { 
                }
            }
        }

        private void ScanModuleAssetts(XmlNodeList XNL, ModuleItem MI)
        {
            XmlNode siNode;
            ModuleItem mitm;
            string strSQL;
            DataSet itmNM;

            foreach (XmlNode itN in XNL)
            {
                mitm = new ModuleItem();
                mitm.typeID = Convert.ToInt64(itN.Attributes.GetNamedItem("typeID").Value.ToString());
                mitm.itemID = Convert.ToInt64(itN.Attributes.GetNamedItem("itemID").Value.ToString());
                mitm.qty = Convert.ToInt64(itN.Attributes.GetNamedItem("quantity").Value.ToString());

                // Database query to get Item names by typeID
                strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + mitm.typeID + ";";
                itmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                if ((itmNM != null) || (itmNM.Tables[0].Rows.Count >= 1))
                {
                    mitm.name = itmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                    if (!ItemIDToName.ContainsKey(mitm.itemID))
                        ItemIDToName.Add(mitm.itemID, mitm.name);

                    mitm.SubItems = new SortedList<long, ModuleItem>();

                    if (itN.ChildNodes.Count > 0)
                    {
                        // Contents of Cans, Ships, Etc... (in theory)
                        siNode = itN.ChildNodes[0];
                        ScanModuleAssetts(siNode.ChildNodes, mitm);
                    }

                    try
                    {
                        MI.SubItems.Add(mitm.itemID, mitm);
                    }
                    catch 
                    { 
                    }
                }
            }
        }

        private void GetCharAssetsV1(EveHQ.Core.Pilot selPilot)
        {
            string acctName, curTime;
            long pilotID;
            APIModule apm;
            EveHQ.Core.EveAccount pilotAccount;
            XmlDocument apiXML = new XmlDocument();
            XmlNodeList modList, timeList;
            XmlNode itmNode;
            string strSQL;
            DataSet itmNM;
            EveAPI.APITypes sel;

            acctName = selPilot.Account;
            if (EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(acctName))
            {
                try
                {
                    pilotAccount = (Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acctName];
                    sel = EveAPI.APITypes.AssetsChar;
                    EveAPI.EveAPIRequest APIReq;
                    APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
                    apiXML = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);

                    if ((APIReq.LastAPIError > 0) && (APIReq.LastAPIResult != EveAPI.APIResults.ReturnedCached))
                    {
                        if (!ThrottleList.ContainsKey(selPilot.Name))
                        {
                            ThrottleList.Add(selPilot.Name, new throttlePlayer(1));
                        }
                        else
                        {
                            ThrottleList[selPilot.Name].IncCount();
                        }
                        LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, selPilot.Name);
                    }

                    if (!CheckXML(apiXML))
                        return;
                }
                catch
                {
                    DialogResult dr = MessageBox.Show("An Error was encountered in Pilot Char. Assets API Download for --> " + selPilot.Name, "PoSManager: API Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                return;
            }

            timeList = apiXML.SelectNodes("/eveapi");
            curTime = timeList[0].ChildNodes[0].InnerText;

            // Adding the pilot
            pilotID = Convert.ToInt32(selPilot.ID);
            if (!ChML.ContainsKey(selPilot.Name))
            {
                ChML.Add(selPilot.Name, new SortedList<long, APIModule>());
            }

            modList = apiXML.SelectNodes("/eveapi/result/rowset/row");

            // Top level scan - pulls out system location, station location, etc...
            foreach (XmlNode psN in modList)
            {
                apm = new APIModule();
                apm.typeID = Convert.ToInt32(psN.Attributes.GetNamedItem("typeID").Value.ToString());
                apm.updateTime = curTime;
                apm.modID = Convert.ToInt64(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                apm.systemID = Convert.ToInt32(psN.Attributes.GetNamedItem("locationID").Value.ToString());
                apm.Qty = Convert.ToInt32(psN.Attributes.GetNamedItem("quantity").Value.ToString());
                apm.charID = Convert.ToInt32(selPilot.ID);
                apm.corpName = selPilot.Corp;
                apm.ownerName = selPilot.Name;

                // Database query to get Item names by typeID
                strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + apm.typeID + ";";
                itmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                if ((itmNM == null) || (itmNM.Tables[0].Rows.Count < 1))
                    continue;

                apm.name = itmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                if (StationList.ContainsKey(apm.systemID))
                {
                    apm.InStation = true;
                    apm.locName = StationList[apm.systemID].Name;
                }
                else if (SLs.SysNameConv.ContainsKey(apm.systemID))
                {
                    // Item is in a system
                    apm.InSpace = true;
                    apm.locName = SLs.SysNameConv[apm.systemID];
                }
                else
                {
                    apm.locName = "In Space";
                }

                if (!ItemIDToName.ContainsKey(apm.modID))
                    ItemIDToName.Add(apm.modID, apm.name);

                if (psN.ChildNodes.Count > 0)
                {
                    itmNode = psN.ChildNodes[0];
                    ScanLocationAssetts(itmNode.ChildNodes, apm);
                }

                if (!ChML[selPilot.Name].ContainsKey(apm.modID))
                {
                    ChML[selPilot.Name].Add(apm.modID, apm);
                }
                else
                {
                    ChML[selPilot.Name].Remove(apm.modID);
                    ChML[selPilot.Name].Add(apm.modID, apm);
                }
            }
        }

        private void GetCharAssetsV2(EveHQ.Core.EveAccount pilotAccount, EveHQ.Core.Pilot selPilot)
        {
            string curTime;
            long pilotID;
            APIModule apm;
            XmlDocument apiXML = new XmlDocument();
            XmlNodeList modList, timeList;
            XmlNode itmNode;
            string strSQL;
            DataSet itmNM;
            EveAPI.EveAPIRequest APIReq;

            if (!pilotAccount.CanUseCharacterAPI(EveAPI.CharacterAccessMasks.AssetList))
                return;

            try
            {
                APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
                apiXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);

                if ((APIReq.LastAPIError > 0) && (APIReq.LastAPIResult != EveAPI.APIResults.ReturnedCached))
                {
                    if (!ThrottleList.ContainsKey(pilotAccount.FriendlyName))
                    {
                        ThrottleList.Add(pilotAccount.FriendlyName, new throttlePlayer(1));
                    }
                    else
                    {
                        ThrottleList[pilotAccount.FriendlyName].IncCount();
                    }
                    LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, pilotAccount.FriendlyName);
                }

                if (!CheckXML(apiXML))
                    return;
            }
            catch
            {
                DialogResult dr = MessageBox.Show("An Error was encountered in Pilot Char. Assets API Download for --> " + pilotAccount.FriendlyName, "PoSManager: API Error", MessageBoxButtons.OK);
            }

            timeList = apiXML.SelectNodes("/eveapi");
            curTime = timeList[0].ChildNodes[0].InnerText;

            pilotID = Convert.ToInt32(selPilot.ID);
            if (!ChML.ContainsKey(selPilot.Name))
            {
                ChML.Add(selPilot.Name, new SortedList<long, APIModule>());
            }

            modList = apiXML.SelectNodes("/eveapi/result/rowset/row");

            // Need to search through ALL assetes for Tower Fuel ID's
            // Stations, Corp Hangers, Tower Modules, Etc...
            // If Fuel is found, then include the item in the Array
            foreach (XmlNode psN in modList)
            {
                apm = new APIModule();
                apm.typeID = Convert.ToInt32(psN.Attributes.GetNamedItem("typeID").Value.ToString());
                apm.updateTime = curTime;
                apm.modID = Convert.ToInt64(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                apm.systemID = Convert.ToInt32(psN.Attributes.GetNamedItem("locationID").Value.ToString());
                apm.Qty = Convert.ToInt32(psN.Attributes.GetNamedItem("quantity").Value.ToString());
                apm.charID = Convert.ToInt32(selPilot.ID);
                apm.corpName = selPilot.Corp;
                apm.ownerName = selPilot.Name;

                // Database query to get Item names by typeID
                strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + apm.typeID + ";";
                itmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                if ((itmNM == null) || (itmNM.Tables[0].Rows.Count < 1))
                    continue;

                apm.name = itmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                if (StationList.ContainsKey(apm.systemID))
                {
                    apm.InStation = true;
                    apm.locName = StationList[apm.systemID].Name;
                }
                else if (SLs.SysNameConv.ContainsKey(apm.systemID))
                {
                    // Item is in a system
                    apm.InSpace = true;
                    apm.locName = SLs.SysNameConv[apm.systemID];
                }
                else
                {
                    apm.locName = "In Space";
                }

                if (!ItemIDToName.ContainsKey(apm.modID))
                    ItemIDToName.Add(apm.modID, apm.name);

                if (psN.ChildNodes.Count > 0)
                {
                    itmNode = psN.ChildNodes[0];
                    ScanLocationAssetts(itmNode.ChildNodes, apm);
                }

                if (!ChML[selPilot.Name].ContainsKey(apm.modID))
                {
                    ChML[selPilot.Name].Add(apm.modID, apm);
                }
                else
                {
                    ChML[selPilot.Name].Remove(apm.modID);
                    ChML[selPilot.Name].Add(apm.modID, apm);
                }
            }
        }

        public static void SaveIHubListing()
        {
            string fname;

            fname = Path.Combine(PoSSave_Path, "API_IHub.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, iHubs);
            pStream.Close();
        }

        public static void LoadIHubListing()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PoSSave_Path, "API_IHub.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    iHubs = (SortedList<string, SortedList<string, SystemIHub>>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }


 
        #endregion

        #region Conversions - Time to Strings

        public static string ConvertHoursToShortTextDisplay(decimal hours, long index)
        {
            string retVal = "", week = "", day = "", hour = "";
            long days = 0, weeks = 0;

            if (hours >= 999999)
            {
                retVal = "Infinite";
            }
            else
            {
                if (index == 2)
                    hours = hours * (24 * 7);
                else if (index == 1)
                    hours = hours * 24;

                if (index >= 2)
                {
                    weeks = Convert.ToInt32(Math.Truncate(hours / (24 * 7)));
                    if (weeks > 0)
                    {
                        week = weeks + " Wks";
                        hours = hours - (weeks * (24 * 7));
                    }
                }

                if (index >= 1)
                {
                    days = Convert.ToInt32(Math.Truncate(hours / 24));
                    if (days > 0)
                    {
                        day = days + " Dys";
                        hours = hours - (days * 24);
                    }
                }

                if (hours > 0)
                    hour = hours + " Hrs";

                if (weeks > 0)
                    retVal = week;

                if (days > 0)
                {
                    if (retVal.Length > 0)
                        retVal += ", " + day;
                    else
                        retVal = day;
                }

                if (hours >= 1)
                {
                    if (retVal.Length > 0)
                        retVal += ", " + hour;
                    else
                        retVal = hour;
                }
            }
            return retVal;
        }

        public static string ConvertHoursToTextDisplay(decimal hours)
        {
            string retVal = "";
            long days;

            hours = Math.Floor(hours);

            if (hours >= 999999)
            {
                retVal = "Infinite";
            }
            else
            {
                days = Convert.ToInt32(Math.Truncate(hours / 24));

                if (days > 0)
                {
                    retVal = String.Format("{0:0}", days) + " Days";
                    hours = hours - (days * 24);
                }

                if ((days > 0) && (hours > 0))
                    retVal += ", ";

                if (hours > 0)
                    retVal += String.Format("{0:0}", hours) + " Hrs";

                if ((days <= 0) && (hours <= 0))
                    retVal = "Zero - Shutdown";
            }
            return retVal;
        }

        public static string ConvertSecondsToTextDisplay(decimal secs)
        {
            string retVal = "Pending...";
            long hours, mins;

            hours = Convert.ToInt32(Math.Truncate(secs / 3600));
            if (hours >= 999999)
            {
                retVal = "Infinite";
            }
            else
            {

                if (hours > 0)
                {
                    retVal = String.Format("{0:0}", hours) + " h";
                    secs = secs - (hours * 3600);
                }

                mins = Convert.ToInt32(Math.Truncate(secs / 60));

                if (mins > 0)
                {
                    if (hours > 0)
                        retVal += ", " + String.Format("{0:0}", mins) + " m";
                    else
                        retVal = String.Format("{0:0}", mins) + " m";

                    secs = secs - (mins * 60);
                }

                if (secs >= 1)
                {
                    if ((hours > 0) || (mins > 0))
                        retVal += ", " + String.Format("{0:0}", secs) + " s";
                    else
                        retVal = String.Format("{0:0}", secs) + " s";
                }
            }
            return retVal;
        }

        public static string ConvertReactionHoursToTextDisplay(decimal hours)
        {
            string retVal = "";
            long days;

            hours = Math.Floor(hours);

            if (hours >= 999999)
            {
                retVal = "";
            }
            else if (hours == 0)
            {
                retVal = "Unknown";
            }
            else
            {
                days = Convert.ToInt32(Math.Truncate(hours / 24));

                if (days > 0)
                {
                    retVal = String.Format("{0:0}", days) + " D";
                    hours = hours - (days * 24);
                }

                if ((days > 0) && (hours > 0))
                    retVal += ", ";

                if (hours > 0)
                    retVal += String.Format("{0:0}", hours) + " H";

                if ((days <= 0) && (hours <= 0))
                    retVal = "Zero - Down";
            }
            return retVal;
        }

        public static ArrayList GetLongestSiloRunTime(New_POS p)
        {
            decimal runT = 9999999999999999;
            string rText, rName = "";
            ArrayList retVal = new ArrayList();

            foreach (Module m in p.Modules)
            {
                switch (Convert.ToInt64(m.ModType))
                {
                    case 2:
                    case 8:
                    case 9:
                    case 11:
                    case 12:
                    case 13:
                        if ((m.FillEmptyTime < runT) && (m.State == "Online"))
                        {
                            rName = m.selMineral.name;
                            runT = m.FillEmptyTime;
                        }
                        break;
                    default:
                        break;
                }
            }

            rText = ConvertReactionHoursToTextDisplay(runT);
            if (rText.Length > 0)
                rText += " <" + rName + ">";

            retVal.Add(runT);
            retVal.Add(rText);

            return retVal;
        }



        #endregion

        #region Module Links

        public static Color GetNextLinkColor()
        {
            return GetRandomColor();
        }

        public static Color GetRandomColor()
        {
             byte[] colorBytes = new byte[3];  
             colorBytes[0] = (byte)(_random.Next(128) + 127);  
             colorBytes[1] = (byte)(_random.Next(128) + 127);  
             colorBytes[2] = (byte)(_random.Next(128) + 127);

             Color c = Color.FromArgb(255, colorBytes[0], colorBytes[1], colorBytes[2]);
       
             return c;  
        }

        public static void LoadModuleLinkListFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PlugInData.PoSSave_Path, "ModLink_List.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    ModuleLinks = (SortedList<long, ModuleLink>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }

                if (ModuleLinks == null)
                    ModuleLinks = new SortedList<long, ModuleLink>();
            }
        }

        public static void SaveModuleLinkListToDisk()
        {
            string fname;

            fname = Path.Combine(PlugInData.PoSSave_Path, "ModLink_List.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, ModuleLinks);
            pStream.Close();
        }



        #endregion

    }
    #endregion
}
