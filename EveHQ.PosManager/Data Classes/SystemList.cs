using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.PosManager
{
    [Serializable]
    public class SystemList
    {
        public SortedList Systems;

        public SystemList()
        {
            Systems = new SortedList();
        }

        public void PopulateSystemListing(Object st)
        {
            string strSQL;
            Sov_Data sDat;
            string sysName;
            decimal sysID;
            DataSet sl;

            // Get System Listing
            strSQL = "SELECT mapSolarSystems.solarSystemName,mapSolarSystems.solarSystemID FROM mapSolarSystems ORDER BY mapSolarSystems.solarSystemName;";
            sl = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Go through the System Listing, 1 at a time, and Build the resultant system Listing
            if (sl.Tables.Count > 0)
            {
                // For Each Category
                foreach (DataRow dr in sl.Tables[0].Rows)
                {
                    sDat = new Sov_Data();
                    sysName = dr[0].ToString();
                    sysID = Convert.ToDecimal(dr[1]);
                    sDat.systemID = sysID;
                    sDat.systemName = sysName;

                    Systems.Add(sysName, sDat);
                }
            }
            SaveSystemListToDisk();
            PlugInData.resetEvents[5].Set();
        }

        public void LoadSystemListFromDisk()
        {
            string PoSBase_Path, PoSManage_Path, PoSCache_Path, fname;
            Stream cStr;
            BinaryFormatter myBf;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = Path.Combine(PoSBase_Path, "PoSManage");
            PoSCache_Path = Path.Combine(PoSManage_Path, "Cache");

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = Path.Combine(PoSCache_Path, "System_List.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Systems = (SortedList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public void SaveSystemListToDisk()
        {
            string PoSBase_Path, PoSManage_Path, PoSCache_Path, fname;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = Path.Combine(PoSBase_Path, "PoSManage");
            PoSCache_Path = Path.Combine(PoSManage_Path, "Cache");

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = Path.Combine(PoSCache_Path, "System_List.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Systems);
            pStream.Close();
        }


    }
}
