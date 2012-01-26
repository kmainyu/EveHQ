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
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace EveHQ.RouteMap
{
    [Serializable]
    public class CapitalShips
    {
        public Dictionary<string, Ship> Ships;

        public CapitalShips()
        {
            Ships = new Dictionary<string, Ship>();
        }

        public void SaveShipListing()
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

            fname = Path.Combine(RMapData_Path, "DB_ShipList.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Ships);
            pStream.Close();
        }

        public void LoadShipListFromDB(Object o)
        {
            LoadShipDataFromDB();
            SaveShipListing();
            //PlugInData.resetEvents[0].Set();
            if (Interlocked.Decrement(ref PlugInData.numBusy) == 0)
            {
                PlugInData.doneEvent.Set();
            }
        }

        private double GetDoubleFromVariableIA(DataRow dr, int aI_1, int aI_2)
        {
            double retVal = -1;

            if (!dr.ItemArray[aI_1].Equals(System.DBNull.Value))
            {
                retVal = Convert.ToDouble(dr.ItemArray[aI_1]);
            }
            else
            {
                retVal = Convert.ToDouble(dr.ItemArray[aI_2]);
            }

            return retVal;
        }

        private decimal GetDecimalFromVariableIA(DataRow dr, int aI_1, int aI_2)
        {
            decimal retVal = 0;

            if (!dr.ItemArray[aI_1].Equals(System.DBNull.Value))
            {
                retVal = Convert.ToDecimal(dr.ItemArray[aI_1]);
            }
            else
            {
                retVal = Convert.ToDecimal(dr.ItemArray[aI_2]);
            }

            return retVal;
        }

        public bool LoadShipDataFromDB()
        {
                Ship sh;
                string strSQL;
                DataSet shipData;
                bool first = true;
                int curTID = 0, typeID = 0;

                strSQL = "SELECT invTypes.typeID, invGroups.groupID, invTypes.typeName, invTypes.description, invTypes.mass, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, invGroups.groupName, invTypes.raceID";
                strSQL += " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID";
                strSQL += " WHERE ((invCategories.categoryID=6 AND invTypes.published=1) OR invTypes.typeID IN (601,596,588,606)) ORDER BY typeName, attributeID;";
                shipData = EveHQ.Core.DataFunctions.GetData(strSQL);

                try
                {
                    if (!shipData.Equals(System.DBNull.Value))
                    {
                        if (shipData.Tables[0].Rows.Count > 0)
                        {
                            Ships.Clear();
                            sh = new Ship();
                            foreach (DataRow row in shipData.Tables[0].Rows)
                            {
                                typeID = Convert.ToInt32(row.ItemArray[0]);

                                if (typeID != curTID)
                                {
                                    if (!first)
                                    {
                                        Ships.Add(sh.Name,sh);
                                        sh = new Ship();
                                    }
                                    curTID = typeID;
                                    sh.typeID = Convert.ToInt32(row.ItemArray[0]);
                                    sh.groupID = Convert.ToInt32(row.ItemArray[1]);
                                    sh.Name = row.ItemArray[2].ToString();
                                    sh.Desc = row.ItemArray[3].ToString();
                                    sh.Category = row.ItemArray[8].ToString();
                                    sh.raceID = Convert.ToInt32(row.ItemArray[9]);
                                    sh.Mass = Convert.ToDouble(row.ItemArray[4]);
                                    first = false;
                                }
                                
                                // Now there are many rows with the same ID that just correspond to other data points
                                // attId = 5, val-int = 6, valFloat = 7
                                switch (Convert.ToInt32(row.ItemArray[5]))
                                {
                                    case 867:           // Jump Range in LY
                                        sh.JumpDistance = GetDoubleFromVariableIA(row, 6, 7);
                                        break;
                                    case 866:           // Jump Fuel Type Used
                                        sh.fuelID = Convert.ToInt32(GetDecimalFromVariableIA(row, 6, 7));
                                        break;
                                    case 868:           // Jump Fuel Consumption
                                        sh.FuelConsumption = GetDoubleFromVariableIA(row, 6, 7);
                                        break;
                                    case 1254:          // Stargate Usage
                                        sh.CanGate = false;
                                        break;
                                    case 1549:          // Fuel Bay capacity
                                        sh.FuelBayCap = GetDoubleFromVariableIA(row, 6, 7);
                                        break;
                                }
                            }
                            Ships.Add(sh.Name,sh);
                        }
                    }
                }
                catch 
                {
                    DialogResult dr = MessageBox.Show("An Error was encountered while Reading in Ship Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
                }

            return true;
        }

        public void LoadShipListFromDisk()
        {
            string RMapBase_Path, RMapManage_Path, RMapData_Path, fname;
            Stream cStr;
            BinaryFormatter myBf;

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

            fname = Path.Combine(RMapData_Path, "DB_ShipList.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Ships = (Dictionary<string, Ship>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }
        }

    }
}
