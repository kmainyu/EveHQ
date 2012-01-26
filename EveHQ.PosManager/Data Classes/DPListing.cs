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
    public class DPListing
    {
        public ArrayList DPL;

        public DPListing()
        {
            DPL = new ArrayList();
        }

        public DPListing(DPListing d)
        {
            DPL = new ArrayList(d.DPL);
        }

        public void SaveDPListing()
        {
            string PoSBase_Path, PoSManage_Path, PoSData_Path, fname;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = Path.Combine(PoSBase_Path , "PoSManage");
            PoSData_Path = Path.Combine(PoSManage_Path , "PoSData");

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSData_Path))
                Directory.CreateDirectory(PoSData_Path);

            fname = Path.Combine(PoSData_Path , "DP_List.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, DPL);
            pStream.Close();
        }

        public void SetupDefaultDPs()
        {
            DamageProfile dp = new DamageProfile();

            if (DPL.Count <= 0)
            {
                // We have no profiles - so create the default ones
                dp = new DamageProfile("Omni-Type", 25, 25, 25, 25);
                DPL.Add(dp);
                dp = new DamageProfile("EM", 100, 0, 0, 0);
                DPL.Add(dp);
                dp = new DamageProfile("Explosive", 0, 100, 0, 0);
                DPL.Add(dp);
                dp = new DamageProfile("Kinetic", 0, 0, 100, 0);
                DPL.Add(dp);
                dp = new DamageProfile("Thermal", 0, 0, 0, 100);
                DPL.Add(dp);
                SaveDPListing();
            }
        }

        public void LoadDPListing()
        {
            string PoSBase_Path, PoSManage_Path, PoSData_Path, fname;
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
            PoSData_Path = Path.Combine(PoSManage_Path, "PoSData");

            if ((!Directory.Exists(PoSManage_Path)) || (!Directory.Exists(PoSData_Path)))
            {
                SetupDefaultDPs();
                return;
            }

            fname = Path.Combine(PoSData_Path, "DP_List.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    DPL = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
            SetupDefaultDPs();
        }
    }
}
