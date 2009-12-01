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
        public ArrayList Designs;

        public POSDesigns()
        {
            Designs = new ArrayList();
        }

        public void SaveDesignListing()
        {
            string PoSBase_Path, PoSManage_Path, PoSSave_Path, fname;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = Path.Combine(PoSBase_Path , "PoSManage");
            PoSSave_Path = Path.Combine(PoSManage_Path , "PoSData");

            if (!Directory.Exists(PoSSave_Path))
                Directory.CreateDirectory(PoSSave_Path);

            fname = Path.Combine(PoSSave_Path , "PoS_Designs.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Designs);
            pStream.Close();
        }

        public void LoadDesignListing()
        {
            string PoSBase_Path, PoSManage_Path, PoSSave_Path, fname;
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
            PoSSave_Path = Path.Combine(PoSManage_Path, "PoSData");

            if (!Directory.Exists(PoSManage_Path))
                return;
            if (!Directory.Exists(PoSSave_Path))
                return;

            fname = Path.Combine(PoSSave_Path, "PoS_Designs.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Designs = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public void AddDesignToList(POS des)
        {
            bool nameChange = false;

            foreach (POS p in Designs)
            {
                if (p.Name == des.Name)
                {
                    des.Name += "_1";
                    nameChange = true;
                    break;
                }
            }
            if (!nameChange)
                Designs.Add(des);
            else
                AddDesignToList(des);
        }

        public void RemoveDesignFromList(POS des)
        {
            int ind = 0;

            foreach (POS p in Designs)
            {
                if (p.Name == des.Name)
                {
                    Designs.RemoveAt(ind);
                    break;
                }
                ind++;
            }
        }

        public void RemoveDesignFromList(string desName)
        {
            int ind = 0;

            foreach (POS p in Designs)
            {
                if (p.Name == desName)
                {
                    Designs.RemoveAt(ind);
                    break;
                }
                ind++;
            }
        }

        public void UpdateListDesign(POS des)
        {
            //int ind = 0;

            foreach (POS p in Designs)
            {
                if (p.Name == des.Name)
                {
                    p.CopyPOSData(des);
                    break;
                }
                //ind++;
            }

            //AddDesignToList(des);
        }

        public void UpdatePOSDesignData(TowerListing TL)
        {
            // Go through the active POS Designs, and update the data to reflect any
            // new data slots or values.
            // 11/30/09 -- Added FuelType.itemID for fuel costs
            foreach (POS p in Designs)
            {
                // FuelBays are: Fuel, D_Fuel, A_Fuel, T_Fuel
                foreach (Tower t in TL.Towers)
                {
                    if (t.typeID == p.PosTower.typeID)
                    {
                        // Same base tower found, update the fuel bay data
                        p.PosTower.Fuel.SetFuelItemID(t.Fuel);
                        p.PosTower.D_Fuel.SetFuelItemID(t.Fuel);
                        p.PosTower.A_Fuel.SetFuelItemID(t.Fuel);
                        p.PosTower.T_Fuel.SetFuelItemID(t.Fuel);
                    }
                }

            }
        }

        public void CalculatePOSFuelRunTimes(API_List APIL, FuelBay fb)
        {
            // This function will calculate the fuel run times for the current
            // monitored POS list if the PoS has a timestamp for monitoring set.
            foreach (POS pl in Designs)
            {
                if (pl.Monitored)
                {
                    pl.CalculatePOSFuelRunTime(APIL, fb);
                }
            }
            // To be added at some point will be input from a Corp API download for
            // current PoS status, values, etc...
            SaveDesignListing();
        }

        public bool CalculatePOSReactions()
        {
            bool changed = false, anyChange = false;
            // This function will calculate the fuel run times for the current
            // monitored POS list if the PoS has a timestamp for monitoring set.
            foreach (POS pl in Designs)
            {
                if (pl.Monitored)
                {
                    changed = pl.CalculateReactions();
                    if (changed)
                        anyChange = true;
                }
            }
            // To be added at some point will be input from a Corp API download for
            // current PoS status, values, etc...
            SaveDesignListing();

            return anyChange;
        }


    }
}
