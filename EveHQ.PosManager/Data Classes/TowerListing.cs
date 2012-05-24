﻿// ========================================================================
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
    public class TowerListing
    {
        public SortedList<long, New_Tower> Towers;
        public DataSet invTypeData, towerStatData, towerFuelData;

        public TowerListing()
        {
            Towers = new SortedList<long, New_Tower>();
        }

        private void GetItemData(long typeID, long groupID)
        {
            string strSQL;

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=" + typeID + ";";
            invTypeData = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Get Table With Tower or Tower Item Details
            strSQL = "SELECT * FROM ((dgmTypeAttributes INNER JOIN dgmAttributeTypes ON " +
                     "dgmTypeAttributes.attributeID = dgmAttributeTypes.attributeID) INNER JOIN " +
                     "eveUnits ON eveUnits.unitID = dgmAttributeTypes.unitID) WHERE "+
                     "dgmTypeAttributes.typeID=" + typeID + ";";
            towerStatData = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Get Table with Tower Fuel Usage Information
            if (invTypeData.Tables[0].Rows[0].ItemArray[2].ToString().Contains("Tower"))
            {
                strSQL = "SELECT * FROM ((invControlTowerResources INNER JOIN invControlTowerResourcePurposes" +
                                          " ON invControlTowerResources.purpose = invControlTowerResourcePurposes.purpose)" +
                                          " INNER JOIN invTypes ON invControlTowerResources.resourceTypeID = invTypes.typeID)" +
                                          " WHERE invControlTowerResources.controlTowerTypeID=" + typeID + ";";
                towerFuelData = EveHQ.Core.DataFunctions.GetData(strSQL);
            }
            else
            {
                if (towerFuelData != null)
                {
                    towerFuelData.Clear();
                    towerFuelData.Dispose();
                    towerFuelData = null;
                }
            }
        }

        private double GetDoubleFromVariableIA(DataRow dr, long aI_1, long aI_2)
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

        private decimal GetDecimalFromVariableIA(DataRow dr, long aI_1, long aI_2)
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

        private void PlaceDataIntoTowerList()
        {
            New_Tower nt;
            DataRow row;
            long numItem;
            string extL = "";
            string extT = "";
            double timVal;
            decimal bQty = 40;
            nt = new New_Tower();

            // Place data into the data table
            nt.Name = invTypeData.Tables[0].Rows[0].ItemArray[2].ToString();
            nt.Desc = invTypeData.Tables[0].Rows[0].ItemArray[3].ToString();
            nt.typeID = Convert.ToInt64(invTypeData.Tables[0].Rows[0].ItemArray[0]);
            nt.groupID = Convert.ToInt64(invTypeData.Tables[0].Rows[0].ItemArray[1]);
            nt.Data["Capacity"] = Convert.ToDecimal(invTypeData.Tables[0].Rows[0].ItemArray[8]);
            nt.Data["Volume"] = Convert.ToDecimal(invTypeData.Tables[0].Rows[0].ItemArray[7]);
            nt.Data["Cost"] = Convert.ToDecimal(invTypeData.Tables[0].Rows[0].ItemArray[11]);
            nt.Category = "Control Tower";

            if (towerFuelData != null)
            {
                nt.Fuel.FuelCap = Convert.ToDecimal(invTypeData.Tables[0].Rows[0].ItemArray[8]);

                // We are looking at a tower, parse out Fuel Information
                // This is made more difficult as Fuel Blocks are not currently in the DB
                numItem = towerFuelData.Tables[0].Rows.Count;
                for (int x = 0; x < numItem; x++)
                {
                    row = towerFuelData.Tables[0].Rows[x];

                    switch (Convert.ToInt64(row.ItemArray[1]))
                    {
                        case 24592: // Charter
                        case 24593: // Charter
                        case 24594: // Charter
                        case 24595: // Charter
                        case 24596: // Charter
                        case 24597: // Charter
                            nt.Fuel.Charters.Name = "Faction Charters";
                            nt.Fuel.Charters.itemID = row.ItemArray[1].ToString();
                            nt.Fuel.Charters.BaseQty = Convert.ToDecimal(row.ItemArray[3]);
                            nt.Fuel.Charters.PeriodQty = nt.Fuel.Charters.BaseQty;
                            nt.Fuel.Charters.BaseVol = Convert.ToDecimal(row.ItemArray[15]);
                            nt.Fuel.Charters.QtyVol = Convert.ToDecimal(row.ItemArray[towerFuelData.Tables[0].Columns["portionSize"].Ordinal]);
                            nt.Fuel.Charters.Cost = Convert.ToDecimal(row.ItemArray[towerFuelData.Tables[0].Columns["basePrice"].Ordinal]);
                            nt.Fuel.Charters.UsedFor = row.ItemArray[7].ToString();
                            break;
                        case 17888: // Nitrogen Isotopes
                            nt.Data["Req_Isotope"] = 1;   // N2 = 1, O2 = 2, He = 3, H2 = 4
                            nt.Data["Block_Qty"] = Convert.ToDecimal(row.ItemArray[3]);
                            bQty = nt.ComputeBlocksForTower(nt.Data["Block_Qty"]);
                            nt.Fuel.Blocks.BaseQty = bQty;
                            nt.Fuel.Blocks.PeriodQty = bQty;
                            break;
                        case 16274: // Helium Isotopes
                            nt.Data["Req_Isotope"] = 3;   // N2 = 1, O2 = 2, He = 3, H2 = 4
                            nt.Data["Block_Qty"] = Convert.ToDecimal(row.ItemArray[3]);
                            bQty = nt.ComputeBlocksForTower(nt.Data["Block_Qty"]);
                            nt.Fuel.Blocks.BaseQty = bQty;
                            nt.Fuel.Blocks.PeriodQty = bQty;
                            break;
                        case 17889: // Hydrogen Isotopes
                            nt.Data["Req_Isotope"] = 4;   // N2 = 1, O2 = 2, He = 3, H2 = 4
                            nt.Data["Block_Qty"] = Convert.ToDecimal(row.ItemArray[3]);
                            bQty = nt.ComputeBlocksForTower(nt.Data["Block_Qty"]);
                            nt.Fuel.Blocks.BaseQty = bQty;
                            nt.Fuel.Blocks.PeriodQty = bQty;
                            break;
                        case 17887: // Oxygen Isotopes
                            nt.Data["Req_Isotope"] = 2;   // N2 = 1, O2 = 2, He = 3, H2 = 4
                            nt.Data["Block_Qty"] = Convert.ToDecimal(row.ItemArray[3]);
                            bQty = nt.ComputeBlocksForTower(nt.Data["Block_Qty"]);
                            nt.Fuel.Blocks.BaseQty = bQty;
                            nt.Fuel.Blocks.PeriodQty = bQty;
                            break;
                        case 16275: // Strontium
                            nt.Fuel.Strontium.Name = row.ItemArray[10].ToString();
                            nt.Fuel.Strontium.itemID = row.ItemArray[1].ToString();
                            nt.Fuel.Strontium.BaseQty = Convert.ToDecimal(row.ItemArray[3]);
                            nt.Fuel.Strontium.PeriodQty = nt.Fuel.Strontium.BaseQty;
                            nt.Fuel.Strontium.BaseVol = Convert.ToDecimal(row.ItemArray[15]);
                            nt.Fuel.Strontium.QtyVol = Convert.ToDecimal(row.ItemArray[towerFuelData.Tables[0].Columns["portionSize"].Ordinal]);
                            nt.Fuel.Strontium.Cost = Convert.ToDecimal(row.ItemArray[towerFuelData.Tables[0].Columns["basePrice"].Ordinal]);
                            nt.Fuel.Strontium.UsedFor = row.ItemArray[7].ToString();
                            break;
                    }
                }
            }

            // Parse out Item/Other information
            numItem = towerStatData.Tables[0].Rows.Count;

            for (int x = 0; x < numItem; x++)
            {
                row = towerStatData.Tables[0].Rows[x];
                switch (Convert.ToInt64(row.ItemArray[1]))
                {
                    case 9:             // Structure HP - dec
                        nt.Struct.Amount = GetDecimalFromVariableIA(row, 2, 3);
                        break;
                    case 11:            // Power Grid - double
                        nt.Data["Power"] = GetDecimalFromVariableIA(row, 2, 3);
                        break;
                    case 30:            // Power Need - double
                        nt.Data["Power_Used"] = GetDecimalFromVariableIA(row, 2, 3);
                        break;
                    case 48:            // CPU - double
                        nt.Data["CPU"] = GetDecimalFromVariableIA(row, 2, 3);
                        break;
                    case 50:            // CPU Need - double
                        nt.Data["CPU_Used"] = GetDecimalFromVariableIA(row, 2, 3);
                        break;
                    case 263:           // Shield HP - float
                        nt.Shield.Amount = GetDecimalFromVariableIA(row, 2, 3);
                        break;
                    case 265:           // Armor HP - double
                        nt.Armor.Amount = GetDecimalFromVariableIA(row, 2, 3);
                        break;
                    case 271:           // Shield EM Res - float
                        nt.Shield.EMP = (100 - (100 * GetDoubleFromVariableIA(row, 2, 3)));
                        break;
                    case 272:           // Shield Exp Res - float
                        nt.Shield.Explosive = (100 - (100 * GetDoubleFromVariableIA(row, 2, 3)));
                        break;
                    case 273:           // Shield Kin Res - float
                        nt.Shield.Kinetic = (100 - (100 * GetDoubleFromVariableIA(row, 2, 3)));
                        break;
                    case 274:           // Shield Thermal Res - float
                        nt.Shield.Thermal = (100 - (100 * GetDoubleFromVariableIA(row, 2, 3)));
                        break;
                    case 479:           // Shield Regen
                        extL = (GetDoubleFromVariableIA(row, 2, 3) / 1000).ToString();
                        nt.Shield.Extra.Add(extL);
                        break;
                    case 552:           // Signature Radius
                        nt.Data["Sig_Radius"] = GetDecimalFromVariableIA(row, 2, 3);
                        break;
                    case 556:   // Anchor
                        timVal = GetDoubleFromVariableIA(row, 2, 3);
                        timVal = timVal / 1000; // convert ms to seconds
                        nt.Data["Anchor_Time"] = Convert.ToDecimal(timVal);
                        break;
                    case 676:   // UnAnchor
                        timVal = GetDoubleFromVariableIA(row, 2, 3);
                        timVal = timVal / 1000; // convert ms to seconds
                        nt.Data["UnAnchor_Time"] = Convert.ToDecimal(timVal);
                        break;
                    case 677:   // Online
                        timVal = GetDoubleFromVariableIA(row, 2, 3);
                        timVal = timVal / 1000; // convert ms to seconds
                        nt.Data["Online_Time"] = Convert.ToDecimal(timVal);
                        break;
                    case 722:   // Tower Period
                        // Comes in as ms, convert to Minutes (60000ms per minute)
                        nt.Data["Cycle_Period"] = Convert.ToDecimal(GetDecimalFromVariableIA(row, 2, 3) / 60000);
                        break;
                    case 728:   // Laser Dmg Bonus
                        nt.Bonuses.LaserDmg = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 750:   // Laser Optimal Bonus
                        nt.Bonuses.LaserOpt = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 751:   // Hybrid Optimal Bonus
                        nt.Bonuses.HybOpt = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 752:   // Projectile Optimal Bonus
                        nt.Bonuses.PrjOpt = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 753:   // Projectile FallOff Bonus
                        nt.Bonuses.PrjFallOff = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 754:   // Projectile ROF Bonus
                        nt.Bonuses.PrjROF = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 755:   // Missile ROF Bonus
                        nt.Bonuses.MslROF = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 756:   // Moon Harvest CPU Bonus
                        nt.Bonuses.MoonHvstCPU = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 757:   // Silo Capacity Bonus
                        nt.Bonuses.SiloCap = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 760:   // Laser Prox Range Bonus
                        nt.Bonuses.LaserProxRange = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 761:   // Projectile Prox Range Bonus
                        nt.Bonuses.PrjProxRange = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 762:   // Hybrid Prox Range Bonus
                        nt.Bonuses.HybProxRange = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 764:   // EW ROF Bonus
                        nt.Bonuses.EWRof = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 766:   // Hybrid Dmg Bonus
                        nt.Bonuses.HybDmg = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 770:   // EW Target Switch Bonus
                        nt.Bonuses.EWTargetSwitch = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 792:   // Missile Velocity Bonus
                        nt.Bonuses.MslVel = GetDoubleFromVariableIA(row, 2, 3);
                        break;
                    case 974:   // Hull EM Res - float
                        // Actual testing reveals this to be ZERO
                        //nt.Struct.EMP = (100 - (100 * GetDoubleFromVariableIA(row, 2, 3)));
                        nt.Struct.EMP = 0;
                        break;
                    case 975:   // Hull Exp Res - float
                        // Actual testing reveals this to be ZERO
                        //nt.Struct.Explosive = (100 - (100 * GetDoubleFromVariableIA(row, 2, 3)));
                        nt.Struct.EMP = 0;
                        break;
                    case 976:   // Hull Kin Res - float
                        // Actual testing reveals this to be ZERO
                        //nt.Struct.Kinetic = (100 - (100 * GetDoubleFromVariableIA(row, 2, 3)));
                        nt.Struct.EMP = 0;
                        break;
                    case 977:   // Hull Thermal Res - float
                        // Actual testing reveals this to be ZERO
                        //nt.Struct.Thermal = (100 - (100 * GetDoubleFromVariableIA(row, 2, 3)));
                        nt.Struct.EMP = 0;
                        break;
                    case 1233:  // Strontium Bay
                        nt.Fuel.StrontCap = GetDecimalFromVariableIA(row, 2, 3);
                        break;
                    default:    // All other information
                        break;
                }
                extT = row.ItemArray[10].ToString();
                if (extT.Length <= 0)
                    extT = row.ItemArray[5].ToString();

                // Now build data display for item selection information
                switch (Convert.ToInt64(row.ItemArray[1]))
                {
                    case 9:     // Structure HP - dec
                    case 11:    // Power Grid - double
                    case 30:    // Power Need - double
                    case 48:    // CPU - double
                    case 50:    // CPU Need - double
                    case 263:   // Shield HP - float
                    case 265:   // Armor HP - double
                    case 271:   // Shield EM Res - float
                    case 272:   // Shield Exp Res - float
                    case 273:   // Shield Kin Res - float
                    case 274:   // Shield Thermal Res - float
                    case 728:   // Laser Dmg Bonus
                    case 750:   // Laser Optimal Bonus
                    case 751:   // Hybrid Optimal Bonus
                    case 752:   // Projectile Optimal Bonus
                    case 753:   // Projectile FallOff Bonus
                    case 754:   // Projectile ROF Bonus
                    case 755:   // Missile ROF Bonus
                    case 756:   // Moon Harvest CPU Bonus
                    case 757:   // Silo Capacity Bonus
                    case 760:   // Laser Prox Range Bonus
                    case 761:   // Projectile Prox Range Bonus
                    case 762:   // Hybrid Prox Range Bonus
                    case 764:   // EW ROF Bonus
                    case 766:   // Hybrid Dmg Bonus
                    case 770:   // EW Target Switch Bonus
                    case 792:   // Missile Velocity Bonus
                    case 974:   // Hull EM Res - float
                    case 975:   // Hull Exp Res - float
                    case 976:   // Hull Kin Res - float
                    case 977:   // Hull Thermal Res - float
                    case 1233:  // Strontium Bay
                        break;
                    case 479:   // Shield Regen
                    case 556:   // Anchor
                    case 676:   // UnAnchor
                    case 677:   // Online
                        extL = (GetDoubleFromVariableIA(row, 2, 3)/1000).ToString();
                        nt.Extra.Add(extT + "\n" + extL + "<" + Convert.ToInt64(row.ItemArray[1]) + ">\n");
                        nt.OtherInfo += extT + " <" + extL + " s>\n";
                        break;
                    case 722:   // Tower Period
                        // Comes in as ms, convert to Minutes (60000ms per minute)
                        extL = (GetDecimalFromVariableIA(row, 2, 3) / 60000).ToString();
                        nt.Extra.Add(extT + "\n" + extL + "<" + Convert.ToInt64(row.ItemArray[1]) + ">\n");
                        nt.OtherInfo += extT + " <" + extL + " min>\n";
                        break;
                    case 552:   // Signature Radius
                    default:    // All other information
                        extL = (GetDecimalFromVariableIA(row, 2, 3)).ToString();
                        nt.Extra.Add(extT + "\n" + extL + "<" + Convert.ToInt64(row.ItemArray[1]) + ">\n");
                        nt.OtherInfo += extT + " <" + extL + " " + row.ItemArray[18].ToString() + ">\n";
                        break;
                }
            }

            nt.D_Fuel = new TFuelBay(nt.Fuel);
            nt.A_Fuel = new TFuelBay(nt.Fuel);
            nt.T_Fuel = new TFuelBay(nt.Fuel);

            Towers.Add(nt.typeID, nt);
        }

        public void PopulateTowerListing(Object st)
        {
            string strSQL;
            string lastCache;
            long groupID;
            long typeID;
            DataSet cd, scd;
            Image img;

            lastCache = (string)st;

            // Get Category Listing
            strSQL = "SELECT invGroups.groupID,invGroups.groupName FROM invGroups WHERE invGroups.categoryID=23 AND invGroups.published=1 ORDER BY invGroups.groupName;";
            cd = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Go through the Catagory Listing, 1 at a time, and Build the resultant Data Table Set
            // For use during program processing
            if (cd.Tables.Count > 0)
            {
                // For Each Category
                foreach (DataRow dr in cd.Tables[0].Rows)
                {
                    groupID = Convert.ToInt64(dr[0]);
                    strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + groupID + " AND invTypes.published=1 ORDER BY invTypes.typeName;";
                    scd = EveHQ.Core.DataFunctions.GetData(strSQL);

                    if (scd.Tables.Count > 0)
                    {
                        foreach (DataRow row in scd.Tables[0].Rows)
                        {
                            // For each Tower do:
                            // 1. Get Graphic
                            // 2. Store Tower Info into TowerListing
                            img = EveHQ.Core.ImageHandler.GetImage(row[0].ToString());

                            if (dr[1].ToString().Contains("Tower"))
                            {
                                // We have a control Tower
                                typeID = Convert.ToInt64(row[0]);
                                GetItemData(typeID, groupID);
                                PlaceDataIntoTowerList();
                            }
                        }
                    }
                }
            }
            SaveTowerListing(lastCache);
            PlugInData.resetEvents[0].Set();
        }

        public void SaveTowerListing(string LastCache)
        {
            string fname;

            fname = Path.Combine(PlugInData.PoSCache_Path , "Tower_List.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Towers);
            pStream.Close();

            fname = Path.Combine(PlugInData.PoSCache_Path, "version.txt");
            StreamWriter sw = new StreamWriter(fname);
            sw.Write(LastCache);
            sw.Close();
        }

        public void LoadTowerListing()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PlugInData.PoSCache_Path, "Tower_List.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Towers = (SortedList<long, New_Tower>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }
    }
}
