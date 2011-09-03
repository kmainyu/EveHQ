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

namespace EveHQ.PosManager
{
    #region Plug-in Interface Functions
    public partial class PlugInData : EveHQ.Core.IEveHQPlugIn
    {
        public struct ModuleItem
        {
            public int typeID;
            public int itemID;
            public string name;
            public int qty;
            public SortedList<int, ModuleItem> SubItems; // Items contained inside the Item
        };

        public static TowerListing TL = new TowerListing();
        public static ModuleListing ML = new ModuleListing();
        public static CategoryList CL = new CategoryList();
        public static SystemList SLs = new SystemList();
        public static POSDesigns PDL = new POSDesigns();
        public static SystemSovList SL = new SystemSovList();
        public static AllianceList AL = new AllianceList();
        public static API_List API_D = new API_List();                   // API Tower Listing (if it Exists)
        public static PlayerList PL = new PlayerList();
        public static NotificationList NL = new NotificationList();
        public static Configuration Config = new Configuration();
        public static SortedList<int, SortedList<int, APIModule>> CML;
        public static SortedList<string, SortedList<int, APIModule>> ChML;
        public static SortedList<int, string> SystemIDToStr;
        public static SortedList<int, ModuleLink> ModuleLinks;           // modID, ModuleLink 

        public static string PoSManage_Path;
        public static string PoSBase_Path, PoSSave_Path;
        public static string PoSCache_Path, PosExport_Path;
        public static ModuleLink LinkInProgress;
        public bool UseSerializableData = false;
        public string LastCacheRefresh = "1.16.2.22";
        public static ManualResetEvent[] resetEvents;
        public static PoSManMainForm PMF = null;
        public static BackgroundWorker bgw_APIUpdate = new System.ComponentModel.BackgroundWorker();
        public static BackgroundWorker bgw_SendNotify = new System.ComponentModel.BackgroundWorker();
        private System.Timers.Timer t_MonitorUpdate;
        DateTime timeStart;
        static string ActiveReactTower = "";
        const int RandomSeed = 2;  
        static Random _random;
        public List<int> TowerMods;
  
        public static Color[] LColor = new Color[] {Color.Blue, Color.Green, Color.Orange, Color.Violet, Color.LimeGreen,
                                              Color.LightBlue, Color.Red, Color.Yellow, Color.Coral, Color.DarkBlue,
                                              Color.DarkGreen, Color.DarkOrange, Color.DarkRed, Color.DarkViolet,
                                              Color.ForestGreen, Color.Gold, Color.Gray, Color.Lavender,
                                              Color.LightCyan, Color.Olive};


        #region Plug In and Start Up

        public Boolean EveHQStartUp()
        {
            StreamReader sr;
            string cacheVers;
            resetEvents = new ManualResetEvent[6];
            ModuleLinks = new SortedList<int, ModuleLink>();
            DateTime startT, endT;
            TimeSpan runT;
            TowerMods = new List<int>();
            CML = new SortedList<int, SortedList<int, APIModule>>();
            ChML = new SortedList<string, SortedList<int, APIModule>>();
            SystemIDToStr = new SortedList<int, string>();
            LinkInProgress = new ModuleLink();

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
            }

            // Load any API data - it could be updated or time for an update, so check
            WaitHandle.WaitAll(resetEvents);

            resetEvents[3] = new ManualResetEvent(false);
            resetEvents[4] = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SL.LoadSovListFromAPI));
            ThreadPool.QueueUserWorkItem(new WaitCallback(AL.LoadAllianceListFromAPI));

            WaitHandle.WaitAll(resetEvents);

            foreach (Sov_Data sd in SLs.Systems.Values)
                SystemIDToStr.Add((int)sd.systemID, sd.systemName);

            // If we re-parsed the DB, need to update tower data structure for any possible changes
            // to existing tower data information
            // 11/30/2009 --> Update to include fuel itemID in FuelType data
            PDL.LoadDesignListing();

            if (!UseSerializableData)
            {
                PDL.UpdatePOSDesignData(TL);
                PDL.SaveDesignListing();
            }

            Config.LoadConfiguration();     // Load PoS Manager Configuration Information
            API_D.LoadAPIListing();       // Load Tower API Data from Disk
            PL.LoadPlayerList();
            NL.LoadNotificationList();

            LoadModuleLinkListFromDisk();

            if (Config.data.Extra.Count <= 0)
                Config.data.Extra.Add((int)400);
            // 
            // bgw_APIUpdate
            // 
            bgw_APIUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_APIUpdate_DoWork);
            bgw_APIUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_APIUpdate_RunWorkerCompleted);
            // 
            // bgw_SendNotify
            // 
            bgw_SendNotify.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_SendNotify_DoWork);

            endT = DateTime.Now;
            runT = endT.Subtract(startT);

            t_MonitorUpdate = new System.Timers.Timer();
            t_MonitorUpdate.Elapsed += new ElapsedEventHandler(UdateMonitorInformation);
            t_MonitorUpdate.AutoReset = false;

            if (!bgw_APIUpdate.IsBusy)
            {
                PlugInData.bgw_APIUpdate.RunWorkerAsync();
            }

            return true;
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

        public object GetPlugInData(object objData, int intDataType)
        {
            return null;
        }

        #endregion

        #region In Game Browser

        public String IGBService(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            string cmd;

            timeStart = DateTime.Now;
            strHTML.Append(EveHQ.Core.IGB.IGBHTMLHeader(context, "PosManager"));
            strHTML.Append(PosManagerMenu(context));

            cmd = context.Request.Url.AbsolutePath.ToUpper();

            if (cmd.Equals("/POSMANAGER") || cmd.Equals("/POSMANAGER/"))
                strHTML.Append(MainPage(context));
            else if (cmd.Equals("/POSMANAGER/RTLOOKUP") || cmd.Equals("/POSMANAGER/RTLOOKUP/"))
                strHTML.Append(RTLookupPage(context));
            else if (cmd.Contains("/POSMANAGER/TFLU_BYVOL"))
                strHTML.Append(FuelNeedForTowerByVol(context));
            else if (cmd.Contains("/POSMANAGER/TFLU"))
                strHTML.Append(FuelNeedForTower(context));
            else if (cmd.Contains("/POSMANAGER/RMNG"))
                strHTML.Append(ManageReactionForTower(context));
            else if (cmd.Contains("/POSMANAGER/RCHG"))
                strHTML.Append(ChangeReactionForTower(context));
            else if (cmd.Contains("/POSMANAGER/TWRUL"))
                strHTML.Append(ImportTowerConfig(context));
            else if (cmd.Contains("/POSMANAGER/IMPORT"))
                strHTML.Append(ImportTower(context));

            strHTML.Append(EveHQ.Core.IGB.IGBHTMLFooter(context));

            return strHTML.ToString();
        }

        public string MainPage(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();

            strHTML.Append("<br>");

            return strHTML.ToString();
        }

        public string ChangeReactionForTower(System.Net.HttpListenerContext context)
        {
            string action;
            string[] ActData;
            string[] spt;
            string modS;
            int fillV = 0, modID;
            ArrayList fills = new ArrayList();
            StringBuilder strHTML = new StringBuilder();
            POS p;

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

            foreach (POS p in PDL.Designs.Values)
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
            POS p;

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
            List<POS> sorted = new List<POS>();
            DateTime cTim, ref_TM, now;
            TimeSpan diffT;
            bool reinf = false;
            decimal ReactTime;

            SetModuleWarnOnValueAndTime();
            foreach (POS p in PDL.Designs.Values)
            {
                if (p.Monitored)
                {
                    sorted.Add(p);
                }
            }

            sorted.Sort(delegate(POS p1, POS p2) { return p1.PosTower.F_RunTime.CompareTo(p2.PosTower.F_RunTime); });
            foreach (POS p in sorted)
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

                if (p.PosTower.F_RunTime < 24)
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"red\">Run: " + ConvertHoursToTextDisplay(p.PosTower.F_RunTime) + "</font></td>");
                else if (p.PosTower.F_RunTime < 48)
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"gold\">Run: " + ConvertHoursToTextDisplay(p.PosTower.F_RunTime) + "</font></td>");
                else
                    strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"lime\">Run: " + ConvertHoursToTextDisplay(p.PosTower.F_RunTime) + "</font></td>");

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

                strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"cyan\">CPU: " + String.Format("{0:#,0.#}", p.PosTower.CPU_Used) + "</font></td>");
                strHTML.Append("<td width=\"20%\"><font size=\"2\" color=\"orange\">Power: " + String.Format("{0:#,0.#}", p.PosTower.Power_Used) + "</font></td>");
             
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

        public string FuelNeedForTowerByVol(System.Net.HttpListenerContext context)
        {
            string vol;
            decimal vlm, sov_mod, totVol = 0;
            string tower, line, twrnospc;
            POS p;
            FuelBay sfb = new FuelBay();
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
            sfb.SetFuelRunTimes(p.PosTower.CPU, p.PosTower.CPU_Used, p.PosTower.Power, p.PosTower.Power_Used, sov_mod);
            p.PosTower.Fuel.SetFuelRunTimes(p.PosTower.CPU, p.PosTower.CPU_Used, p.PosTower.Power, p.PosTower.Power_Used, sov_mod);
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

            for (int x = 0; x < 12; x++)
            {
                if ((!p.UseChart) && (x == 11))
                    continue;

                if ((p.PosTower.T_Fuel.N2Iso.Name == "") && (x == 7))
                    continue;

                if ((p.PosTower.T_Fuel.HeIso.Name == "") && (x == 5))
                    continue;

                if ((p.PosTower.T_Fuel.H2Iso.Name == "") && (x == 6))
                    continue;

                if ((p.PosTower.T_Fuel.O2Iso.Name == "") && (x == 8))
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
            POS p;
            FuelBay sfb = new FuelBay();
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

            sfb = new FuelBay(PDL.Designs[tower].PosTower.T_Fuel);
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

            for (int x = 0; x < 13; x++)
            {
                if ((!p.UseChart) && (x == 11))
                    continue;

                if ((p.PosTower.T_Fuel.N2Iso.Name == "") && (x == 7))
                    continue;

                if ((p.PosTower.T_Fuel.HeIso.Name == "") && (x == 5))
                    continue;

                if ((p.PosTower.T_Fuel.H2Iso.Name == "") && (x == 6))
                    continue;

                if ((p.PosTower.T_Fuel.O2Iso.Name == "") && (x == 8))
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

        public string PosManagerMenu(System.Net.HttpListenerContext context)
        {
            StringBuilder strHTML = new StringBuilder();
            strHTML.Append("<font size=\"3\" color=\"gold\"><b> POS Manager Online </b></font><br><br>");
            strHTML.Append("<font size=\"2\"><a href=/PosManager>POS Manager Home</a>  | <a href=/PosManager/RTLookup>POS Run-Time Lookup</a></font>");
            strHTML.Append("<br><br>");

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
            int tID, modID, lnkID, inpID, outID, XfrQ, XferV;
            Module nm;
            POS p;
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
                        p.PosTower = new Tower(TL.Towers[tID]);
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

        #region Background Data Workers (API and Notifications)

        private void bgw_SendNotify_DoWork(object sender, DoWorkEventArgs e)
        {
            NL.CheckAndSendNotificationIfActive();
        }

        private void bgw_APIUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string cache;
            int seconds;
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
            }
            PDL.SaveDesignListing();
        }

        private void bgw_APIUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet cd, mt;
            string strSQL;

            if (PMF != null)
            {
                PMF.DataUpdateInProgress();
            }

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
          
            GetCorpAssets();
            //GetCharAssets();
            GetAllySovLists();
            API_D.LoadPOSDataFromAPI();

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

        private void GetCorpAssets()
        {
            string acctName, curTime;
            int corpID;
            APIModule apm;
            SortedList<int, APIModule> alm = new SortedList<int, APIModule>();
            EveHQ.Core.EveAccount pilotAccount;
            XmlDocument apiXML = new XmlDocument();
            XmlNodeList modList, itemList, timeList, siList;
            XmlNode itmNode, siNode;
            ModuleItem mitm, msitm;
            string strSQL;
            bool hasStuff = false;
            DataSet itmNM, sitmNM;
            EveAPI.APITypes sel;

            foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                if (selPilot.Active)
                {
                    acctName = selPilot.Account;
                    if (EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(acctName))
                    {
                        pilotAccount = (Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acctName];
                        sel = EveAPI.APITypes.AssetsCorp;
                        EveAPI.EveAPIRequest APIReq;
                        APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQSettings.APIRSAddress, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
                        apiXML = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);
                        if (!CheckXML(apiXML))
                        {
                            apiXML = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);
                            // Errors, try one more time. Data will not be used later if it contains errors
                            if (!CheckXML(apiXML))
                                continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    timeList = apiXML.SelectNodes("/eveapi");
                    curTime = timeList[0].ChildNodes[0].InnerText;

                    corpID = Convert.ToInt32(selPilot.CorpID);
                    if (!CML.ContainsKey(corpID))
                    {
                        CML.Add(corpID, new SortedList<int, APIModule>());
                    }

                    modList = apiXML.SelectNodes("/eveapi/result/rowset/row");
                    foreach (XmlNode psN in modList)
                    {
                        hasStuff = false;
                        apm = new APIModule();
                        apm.typeID = Convert.ToInt32(psN.Attributes.GetNamedItem("typeID").Value.ToString());
                        apm.updateTime = curTime;
                        apm.modID = Convert.ToInt32(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                        apm.systemID = Convert.ToInt32(psN.Attributes.GetNamedItem("locationID").Value.ToString());
                        apm.Qty = Convert.ToInt32(psN.Attributes.GetNamedItem("quantity").Value.ToString());
                        apm.corpID = Convert.ToInt32(selPilot.CorpID);
                        apm.corpName = selPilot.Corp;

                        // Database query to get Item names by typeID
                        strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + apm.typeID + ";";
                        itmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                        apm.name = itmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                        if (CheckTypeIDForFuelOrMod(apm.typeID, false))
                        {
                            if (!CML[corpID].ContainsKey(apm.modID))
                                CML[corpID].Add(apm.modID, apm);
                        }

                        if (psN.ChildNodes.Count > 0)
                        {
                            itmNode = psN.ChildNodes[0];
                            itemList = itmNode.ChildNodes;
                            foreach (XmlNode itN in itemList)
                            {
                                mitm = new ModuleItem();
                                mitm.typeID = Convert.ToInt32(itN.Attributes.GetNamedItem("typeID").Value.ToString());
                                mitm.itemID = Convert.ToInt32(itN.Attributes.GetNamedItem("itemID").Value.ToString());
                                mitm.qty = Convert.ToInt32(itN.Attributes.GetNamedItem("quantity").Value.ToString());

                                //if (CheckTypeIDForFuelOrMod(mitm.typeID))
                                    hasStuff = true;

                                // Database query to get Item names by typeID
                                strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + mitm.typeID + ";";
                                itmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                                if (itmNM == null)
                                    continue;

                                if (itmNM.Tables[0].Rows.Count < 1)
                                    continue;

                                mitm.name = itmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                                if (itN.ChildNodes.Count > 0)
                                {
                                    // Contents of Cans, Ships, Etc... (in theory)
                                    siNode = itN.ChildNodes[0];
                                    siList = siNode.ChildNodes;

                                    foreach (XmlNode siN in siList)
                                    {
                                        msitm = new ModuleItem();
                                        msitm.typeID = Convert.ToInt32(siN.Attributes.GetNamedItem("typeID").Value.ToString());
                                        msitm.itemID = Convert.ToInt32(siN.Attributes.GetNamedItem("itemID").Value.ToString());
                                        msitm.qty = Convert.ToInt32(siN.Attributes.GetNamedItem("quantity").Value.ToString());

                                        if (CheckTypeIDForFuelOrMod(msitm.typeID, false))
                                            hasStuff = true;

                                        // Database query to get Item names by typeID
                                        strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + msitm.typeID + ";";
                                        sitmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                                        msitm.name = sitmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                                        if (mitm.SubItems == null)
                                            mitm.SubItems = new SortedList<int, ModuleItem>();

                                        if (mitm.SubItems.ContainsKey(msitm.itemID))
                                        {
                                            mitm.SubItems.Remove(msitm.itemID);
                                            mitm.SubItems.Add(msitm.itemID, msitm);
                                        }
                                        else
                                        {
                                            mitm.SubItems.Add(msitm.itemID, msitm);
                                        }
                                    }
                                }

                                if (hasStuff)
                                {
                                    if (!CML[corpID].ContainsKey(apm.modID))
                                        CML[corpID].Add(apm.modID, apm);

                                    // Module already present
                                    if (CML[apm.corpID][apm.modID].Items.ContainsKey(mitm.itemID))
                                    {
                                        // Item is already present, replace it
                                        CML[apm.corpID][apm.modID].Items.Remove(mitm.itemID);
                                        CML[apm.corpID][apm.modID].Items.Add(mitm.itemID, mitm);
                                    }
                                    else
                                    {
                                        // Item not present, add it
                                        CML[apm.corpID][apm.modID].Items.Add(mitm.itemID, mitm);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool CheckTypeIDForFuelOrMod(int ID, bool FuelOnly)
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

        private void GetCharAssets()
        {
            string acctName, curTime;
            int pilotID;
            APIModule apm;
            SortedList<int, APIModule> alm = new SortedList<int, APIModule>();
            EveHQ.Core.EveAccount pilotAccount;
            XmlDocument apiXML = new XmlDocument();
            XmlNodeList modList, itemList, timeList, siList;
            XmlNode itmNode, siNode;
            ModuleItem mitm, msitm;
            string strSQL;
            DataSet itmNM, sitmNM;
            EveAPI.APITypes sel;

            foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                if (selPilot.Active)
                {
                    acctName = selPilot.Account;
                    if (EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(acctName))
                    {
                        pilotAccount = (Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acctName];
                        sel = EveAPI.APITypes.AssetsChar;
                        EveAPI.EveAPIRequest APIReq;
                        APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQSettings.APIRSAddress, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
                        apiXML = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);
                        if (!CheckXML(apiXML))
                        {
                            apiXML = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);
                            // Errors, try one more time. Data will not be used later if it contains errors
                            if (!CheckXML(apiXML))
                                continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    timeList = apiXML.SelectNodes("/eveapi");
                    curTime = timeList[0].ChildNodes[0].InnerText;

                    pilotID = Convert.ToInt32(selPilot.ID);
                    if (!ChML.ContainsKey(selPilot.Name))
                    {
                        ChML.Add(selPilot.Name, new SortedList<int, APIModule>());
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
                        apm.modID = Convert.ToInt32(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                        apm.systemID = Convert.ToInt32(psN.Attributes.GetNamedItem("locationID").Value.ToString());
                        apm.Qty = Convert.ToInt32(psN.Attributes.GetNamedItem("quantity").Value.ToString());
                        apm.charID = Convert.ToInt32(selPilot.ID);
                        apm.corpName = selPilot.Corp;
                        apm.ownerName = selPilot.Name;
                        // Database query to get Item names by typeID
                        strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + apm.typeID + ";";
                        itmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                        if (itmNM.Tables[0].Rows.Count < 1)
                            continue;

                        apm.name = itmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                        if (!ChML[selPilot.Name].ContainsKey(apm.modID))
                            ChML[selPilot.Name].Add(apm.modID, apm);

                        if (psN.ChildNodes.Count > 0)
                        {
                            itmNode = psN.ChildNodes[0];
                            itemList = itmNode.ChildNodes;
                            foreach (XmlNode itN in itemList)
                            {
                                mitm = new ModuleItem();
                                mitm.typeID = Convert.ToInt32(itN.Attributes.GetNamedItem("typeID").Value.ToString());
                                mitm.itemID = Convert.ToInt32(itN.Attributes.GetNamedItem("itemID").Value.ToString());
                                mitm.qty = Convert.ToInt32(itN.Attributes.GetNamedItem("quantity").Value.ToString());

                                // Database query to get Item names by typeID
                                strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + mitm.typeID + ";";
                                itmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                                mitm.name = itmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                                if (itN.ChildNodes.Count > 0)
                                {
                                    // Contents of Cans, Ships, Etc... (in theory)
                                    siNode = itN.ChildNodes[0];
                                    siList = siNode.ChildNodes;

                                    foreach (XmlNode siN in siList)
                                    {
                                        msitm = new ModuleItem();
                                        msitm.typeID = Convert.ToInt32(siN.Attributes.GetNamedItem("typeID").Value.ToString());
                                        msitm.itemID = Convert.ToInt32(siN.Attributes.GetNamedItem("itemID").Value.ToString());
                                        msitm.qty = Convert.ToInt32(siN.Attributes.GetNamedItem("quantity").Value.ToString());

                                        // Database query to get Item names by typeID
                                        strSQL = "SELECT invTypes.typeName FROM invTypes WHERE invTypes.typeID=" + msitm.typeID + ";";
                                        sitmNM = EveHQ.Core.DataFunctions.GetData(strSQL);

                                        msitm.name = sitmNM.Tables[0].Rows[0].ItemArray[0].ToString();

                                        if (mitm.SubItems == null)
                                            mitm.SubItems = new SortedList<int, ModuleItem>();

                                        if (mitm.SubItems.ContainsKey(msitm.itemID))
                                        {
                                            mitm.SubItems.Remove(msitm.itemID);
                                            mitm.SubItems.Add(msitm.itemID, msitm);
                                        }
                                        else
                                        {
                                            mitm.SubItems.Add(msitm.itemID, msitm);
                                        }
                                    }
                                }

                                // Module already present
                                if (ChML[selPilot.Name][apm.modID].Items.ContainsKey(mitm.itemID))
                                {
                                    // Item is already present, replace it
                                    ChML[selPilot.Name][apm.modID].Items.Remove(mitm.itemID);
                                    ChML[selPilot.Name][apm.modID].Items.Add(mitm.itemID, mitm);
                                }
                                else
                                {
                                    // Item not present, add it
                                    ChML[selPilot.Name][apm.modID].Items.Add(mitm.itemID, mitm);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GetCorpSheet()
        {
            string acctName;
            EveAPI.APITypes sel;
            EveHQ.Core.EveAccount pilotAccount;
            XmlDocument apiXML;

            foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                if (selPilot.Active)
                {
                    acctName = selPilot.Account;
                    if (EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(acctName))
                    {
                        pilotAccount = (Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acctName];
                        sel = EveAPI.APITypes.CorpSheet;
                        EveAPI.EveAPIRequest APIReq;
                        APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQSettings.APIRSAddress, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
                        apiXML = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);
                        if (!CheckXML(apiXML))
                        {
                            apiXML = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);
                            // Errors, try one more time. Data will not be used later if it contains errors
                        }
                    }
                }
            }
        }


        #endregion

        #region Conversions - Time to Strings

        public static string ConvertHoursToShortTextDisplay(decimal hours, int index)
        {
            string retVal = "", week = "", day = "", hour = "";
            int days = 0, weeks = 0;

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
            int days;

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
            string retVal = "";
            int hours, mins;

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
            int days;

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

        public static ArrayList GetLongestSiloRunTime(POS p)
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
                    ModuleLinks = (SortedList<int, ModuleLink>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }

                if (ModuleLinks == null)
                    ModuleLinks = new SortedList<int, ModuleLink>();
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
