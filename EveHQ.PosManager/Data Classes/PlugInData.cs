using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.PosManager
{
    #region Plug-in Interface Functions
    public partial class PlugInData : EveHQ.Core.IEveHQPlugIn
    {
        TowerListing TL = new TowerListing();
        ModuleListing ML = new ModuleListing();
        CategoryList CL = new CategoryList();
        SystemList SLs = new SystemList();
        public string PoSManage_Path;
        public string PoSBase_Path;
        public string PoSCache_Path;
        public bool UseSerializableData = false;
        public string LastCacheRefresh = "1.12.0.699";
        public static ManualResetEvent[] resetEvents;
        SystemSovList SL = new SystemSovList();
        AllianceList AL = new AllianceList();

        public Boolean EveHQStartUp()
        {
            StreamReader sr;
            string cacheVers;
            resetEvents = new ManualResetEvent[6];
            DateTime startT, endT;
            TimeSpan runT;

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
            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

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
                        Directory.Delete(PoSCache_Path,true);
                        UseSerializableData = false;
                    }
                    else
                        UseSerializableData = true;
                }
                else
                {
                        Directory.Delete(PoSCache_Path,true);
                        UseSerializableData = false;
                }
            }

            startT = DateTime.Now;
            if (!UseSerializableData)
            {
                resetEvents[0] = new ManualResetEvent(false);
                resetEvents[1] = new ManualResetEvent(false);
                resetEvents[2] = new ManualResetEvent(false);
                resetEvents[5] = new ManualResetEvent(false);
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
                resetEvents[5] = new ManualResetEvent(true);
            }

            // Load any API data - it could be updated or time for an update, so check
            resetEvents[3] = new ManualResetEvent(false);
            resetEvents[4] = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SL.LoadSovListFromAPI));
            ThreadPool.QueueUserWorkItem(new WaitCallback(AL.LoadAllianceListFromAPI));

            WaitHandle.WaitAll(resetEvents);

            endT = DateTime.Now;
            runT = endT.Subtract(startT);
            return true;
        }

        public EveHQ.Core.PlugIn GetEveHQPlugInInfo()
        {
            EveHQ.Core.PlugIn EveHQPlugIn = new EveHQ.Core.PlugIn();

            EveHQPlugIn.Name = "PoS Manager";
            EveHQPlugIn.Description = "Design PoS Layouts and Monitor PoS Status";
            EveHQPlugIn.Author = "Jay Teague <aka: Sherksilver>";
            EveHQPlugIn.MainMenuText = "PoS Manager";
            EveHQPlugIn.RunAtStartup = true;
            EveHQPlugIn.RunInIGB = false;
            EveHQPlugIn.MenuImage = Properties.Resources.plugin_icon;
            EveHQPlugIn.Version = Application.ProductVersion.ToString();

            return EveHQPlugIn;
        }

        public String IGBService(System.Net.HttpListenerContext context)
        {
            return "";
        }

        public Form RunEveHQPlugIn()
        {
            // You need to make this form, it'll be the startup form for the plugin
            return new PoSManMainForm();
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

                for (int x=0; x<4; x++)
                {
                    if( (Convert.ToInt32(remot[x])) != (Convert.ToInt32(local[x])) )
                    {
                        if ( (Convert.ToInt32(remot[x])) > (Convert.ToInt32(local[x])) )
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
    }
    #endregion
}
