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
        POSDesigns PDL = new POSDesigns();

        public string PoSManage_Path;
        public string PoSBase_Path;
        public string PoSCache_Path;
        public bool UseSerializableData = false;
        public string LastCacheRefresh = "1.15.2.1810";
        public static ManualResetEvent[] resetEvents;
        SystemSovList SL = new SystemSovList();
        AllianceList AL = new AllianceList();
        public static FuelBay BFStats;

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
            BFStats = new FuelBay();

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
                        //Directory.Delete(PoSCache_Path,true);
                        UseSerializableData = false;
                    }
                    else
                        UseSerializableData = true;
                }
                else
                {
                        //Directory.Delete(PoSCache_Path,true);
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
            }

            // Load any API data - it could be updated or time for an update, so check
            WaitHandle.WaitAll(resetEvents);

            resetEvents[3] = new ManualResetEvent(false);
            resetEvents[4] = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SL.LoadSovListFromAPI));
            ThreadPool.QueueUserWorkItem(new WaitCallback(AL.LoadAllianceListFromAPI));

            WaitHandle.WaitAll(resetEvents);

            // If we re-parsed the DB, need to update tower data structure for any possible changes
            // to existing tower data information
            // 11/30/2009 --> Update to include fuel itemID in FuelType data
            if (!UseSerializableData)
            {
                PDL.LoadDesignListing();
                PDL.UpdatePOSDesignData(TL);
                PDL.SaveDesignListing();
            }
            LoadBFStatsFromDB();

            endT = DateTime.Now;
            runT = endT.Subtract(startT);
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
