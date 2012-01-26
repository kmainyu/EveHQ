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
        public SortedList<string, Sov_Data> Systems;
        public SortedList<long, string> SysNameConv;

        public SystemList()
        {
            Systems = new SortedList<string, Sov_Data>();
            SysNameConv = new SortedList<long, string>();
        }

        public void PopulateSystemListing(Object st)
        {
            string strSQL;
            Sov_Data sDat;
            string sysName;
            decimal sysID;
            DataSet sl;

            // Get System Listing
            strSQL = "SELECT mapSolarSystems.solarSystemName,mapSolarSystems.solarSystemID,mapSolarSystems.security FROM mapSolarSystems ORDER BY mapSolarSystems.solarSystemName;";
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
                    sDat.secLevel = Convert.ToDecimal(dr[2]);
                    sDat.systemID = sysID;
                    sDat.systemName = sysName;

                    Systems.Add(sysName, sDat);
                    SysNameConv.Add(Convert.ToInt32(sysID), sysName);
                }
            }
            SaveSystemListToDisk();
            PlugInData.resetEvents[5].Set();
        }

        public void LoadSystemListFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PlugInData.PoSCache_Path, "System_List.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Systems = (SortedList<string, Sov_Data>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }

            fname = Path.Combine(PlugInData.PoSCache_Path, "SysNam_List.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    SysNameConv = (SortedList<long, string>)myBf.Deserialize(cStr);
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
            string fname;

            fname = Path.Combine(PlugInData.PoSCache_Path, "System_List.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Systems);
            pStream.Close();

            fname = Path.Combine(PlugInData.PoSCache_Path, "SysNam_List.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, SysNameConv);
            pStream.Close();
        }


    }
}
