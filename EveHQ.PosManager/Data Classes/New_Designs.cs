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
    public class New_Designs
    {
        public SortedList<string, New_POS> Designs;
        private static bool AccessControl = false;

        public New_Designs()
        {
            Designs = new SortedList<string, New_POS>();
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
                    Designs = (SortedList<string, New_POS>)myBf.Deserialize(cStr);
                }
                catch
                {
                    // We have the old structure type - need to load and convert it !
                    try
                    {
                        SortedList<string, POS> OLDDesigns = new SortedList<string, POS>();

                        cStr.Close();
                        cStr = File.OpenRead(fname);

                        OLDDesigns = (SortedList<string, POS>)myBf.Deserialize(cStr);
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

        public void ConvertOldToNewDesign(SortedList<string, POS> OD)
        {
            Designs.Clear();
            foreach (POS p in OD.Values)
            {
                Designs.Add(p.Name, new New_POS(p));
            }
            SaveDesignListing();
        }

        public void AddDesignToList(New_POS des)
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

        public void RemoveDesignFromList(New_POS des)
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

        public void UpdateListDesign(New_POS des)
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

            // 1_21_12 - new structure, no new slots at this time
        }

        public void CalculatePOSFuelRunTimes(API_List APIL, TFuelBay fb)
        {
            // This function will calculate the fuel run times for the current
            // monitored POS list if the PoS has a timestamp for monitoring set.
            foreach (New_POS pl in Designs.Values)
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
            foreach (New_POS pl in Designs.Values)
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
            foreach (New_POS pl in Designs.Values)
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
