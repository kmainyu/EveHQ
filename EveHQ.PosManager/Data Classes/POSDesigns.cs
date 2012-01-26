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
    public class POSDesigns
    {
        public SortedList<string, POS> Designs;
        private static bool AccessControl = false;

        public POSDesigns()
        {
            Designs = new SortedList<string, POS>();
        }

        public void SaveDesignListing()
        {
            string fname;

            if (!AccessControl)
            {
                AccessControl = true;
                fname = Path.Combine(PlugInData.PoSSave_Path, "PoS_Designs.bin");

                // Save the Serialized data to Disk

                Stream pStream = File.Create(fname);
                BinaryFormatter pBF = new BinaryFormatter();
                pBF.Serialize(pStream, Designs);
                pStream.Close();
                AccessControl = false;
            }
        }

        public void LoadDesignListing()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            if (AccessControl)
                return;

            fname = Path.Combine(PlugInData.PoSSave_Path, "PoS_Designs.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Designs = (SortedList<string, POS>)myBf.Deserialize(cStr);
                }
                catch
                {
                    // We have the old structure type - need to load and convert it !
                    try
                    {
                        ArrayList OLDDesigns = new ArrayList();

                        cStr.Close();
                        cStr = File.OpenRead(fname);

                        OLDDesigns = (ArrayList)myBf.Deserialize(cStr);
                        cStr.Close();

                        // Preserve the old design file
                        File.Move(fname, fname.Replace("PoS_Designs.bin", "PoS_Designs_Old.bin"));

                        // Convert old to new, and save
                        ConvertOldToNewDesign(OLDDesigns);
                    }
                    catch
                    {
                        cStr.Close();
                    }
                }

            }
        }

        public void ConvertOldToNewDesign(ArrayList OD)
        {
            Designs.Clear();
            foreach (POS p in OD)
            {
                Designs.Add(p.Name, p);
            }
            SaveDesignListing();
        }

        public void AddDesignToList(POS des)
        {
            bool nameChange = false;

            if (Designs.ContainsKey(des.Name))
            {
                des.Name += "_1";
                nameChange = true;
            }
            if (!nameChange)
                Designs.Add(des.Name, des);
            else
                AddDesignToList(des);
        }

        public void RemoveDesignFromList(POS des)
        {
            if (Designs.ContainsKey(des.Name))
            {
                Designs.Remove(des.Name);
            }
        }

        public void RemoveDesignFromList(string desName)
        {
            if (Designs.ContainsKey(desName))
            {
                Designs.Remove(desName);
            }
        }

        public void UpdateListDesign(POS des)
        {
            if (Designs.ContainsKey(des.Name))
            {
                Designs[des.Name].CopyPOSData(des);
            }
        }

        public void UpdatePOSDesignData(TowerListing TL)
        {
            // Go through the active POS Designs, and update the data to reflect any
            // new data slots or values.
            // 11/30/09 -- Added FuelType.itemID for fuel costs
            //foreach (POS pl in Designs.Values)
            //{
            //    // FuelBays are: Fuel, D_Fuel, A_Fuel, T_Fuel
            //    if (PlugInData.TL.Towers.ContainsKey(pl.PosTower.typeID))
            //    {
            //        // Same base tower found, update the fuel bay data
            //        pl.PosTower.Fuel.SetFuelBaseValues(PlugInData.TL.Towers[pl.PosTower.typeID].Fuel);
            //        pl.PosTower.Fuel.SetFuelItemID(PlugInData.TL.Towers[pl.PosTower.typeID].Fuel);
            //        pl.PosTower.D_Fuel.SetFuelItemID(PlugInData.TL.Towers[pl.PosTower.typeID].Fuel);
            //        pl.PosTower.A_Fuel.SetFuelItemID(PlugInData.TL.Towers[pl.PosTower.typeID].Fuel);
            //        pl.PosTower.T_Fuel.SetFuelItemID(PlugInData.TL.Towers[pl.PosTower.typeID].Fuel);
            //    }

            //    if (pl.ReactionLinks == null)
            //    {
            //        pl.ReactionLinks = new ArrayList();
            //        pl.React_TS = DateTime.Now;
            //    }

            //    if (pl.Owner == null)
            //    {
            //        pl.Owner = "";
            //        pl.FuelTech = "";
            //        pl.ownerID = 0;
            //        pl.fuelTechID = 0;
            //    }

            //    if (pl.Owner == "")
            //        pl.Owner = pl.CorpName;

            //    // Actual testing reveals that the DB values are NOT correct for 
            //    // structure resistances, they are in reality ZERO
            //    pl.PosTower.Struct.EMP = 0;
            //    pl.PosTower.Struct.Explosive = 0;
            //    pl.PosTower.Struct.Kinetic = 0;
            //    pl.PosTower.Struct.Thermal = 0;

            //    foreach (Module m in pl.Modules)
            //    {
            //        if (m.ReactList == null)
            //            m.CopyMissingReactData();
            //        else
            //        {
            //            // Copy the Cache load data for Reactions, etc... Just in case it changed
            //            // or was updated.
            //            if (PlugInData.ML.Modules.ContainsKey(m.typeID))
            //            {
            //                m.ReactList = new ArrayList(PlugInData.ML.Modules[m.typeID].ReactList);
            //                m.MSRList = new ArrayList(PlugInData.ML.Modules[m.typeID].MSRList);
            //                m.InputList = new ArrayList(PlugInData.ML.Modules[m.typeID].InputList);
            //                m.OutputList = new ArrayList(PlugInData.ML.Modules[m.typeID].OutputList);
            //            }
            //        }
            //        if (m.curTS == null)
            //        {
            //            m.modID = 0;
            //            m.curTS = "";
            //        }
            //    }
            //}
        }

        public void CalculatePOSFuelRunTimes(API_List APIL, FuelBay fb)
        {
            // This function will calculate the fuel run times for the current
            // monitored POS list if the PoS has a timestamp for monitoring set.
            foreach (POS pl in Designs.Values)
            {
                if (pl.Monitored)
                {
                    pl.CalculatePOSFuelRunTime(APIL, fb);
                }
            }
            // To be added at some polong will be input from a Corp API download for
            // current PoS status, values, etc...
            SaveDesignListing();
        }

        public void UpdateTowerSiloValuesForAPI()
        {
            // Scan all towers
            foreach (POS pl in Designs.Values)
            {
                // Scan all modules
                foreach (Module m in pl.Modules)
                {
                    if (m.Category.Equals("Silo"))
                    {
                        if (m.modID != 0)
                        {
                            // Silo is linked to API, then get appropriate API info
                            if (PlugInData.CpML.ContainsKey(pl.CorpName))
                            {
                                if (PlugInData.CpML[pl.CorpName].ContainsKey(m.modID))
                                {
                                    if ((m.curTS == null) || !m.curTS.Equals(PlugInData.CpML[pl.CorpName][m.modID].updateTime))
                                    {
                                        // Update Silo contents from API
                                        if (PlugInData.CpML[pl.CorpName][m.modID].Items.Count > 0)
                                        {
                                            m.CapQty = PlugInData.CpML[pl.CorpName][m.modID].Items.Values[0].qty;
                                            m.CapVol = m.selMineral.volume * m.CapQty;
                                            m.curTS = PlugInData.CpML[pl.CorpName][m.modID].updateTime;

                                            // Set tower reaction timestamp to the API information for the tower module
                                            // so reaction calculations will still work correctly.
                                            // Note: This means that ALL tower silo's must be linked or calculations will
                                            //       Have errors. 
                                            pl.React_TS = Convert.ToDateTime(PlugInData.CpML[pl.CorpName][m.modID].updateTime);
                                            pl.React_TS = TimeZone.CurrentTimeZone.ToLocalTime(pl.React_TS);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool CalculatePOSReactions()
        {
            bool changed = false, anyChange = false;
            // This function will calculate the fuel run times for the current
            // monitored POS list if the PoS has a timestamp for monitoring set.
            foreach (POS pl in Designs.Values)
            {
                if (pl.Monitored)
                {
                    changed = pl.CalculateReactions();
                    if (changed)
                        anyChange = true;
                }
            }
            // To be added at some polong will be input from a Corp API download for
            // current PoS status, values, etc...
            SaveDesignListing();

            return anyChange;
        }


    }
}
