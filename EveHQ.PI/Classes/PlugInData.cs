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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Net;
using System.Timers;

namespace EveHQ.PI
{
    public partial class PlugInData : EveHQ.Core.IEveHQPlugIn
    {
        public static PIMain PIM;
        public static SortedList<string, PINFacility> Facilities;
        public static SortedList<string, PIFacility> OldFac;
        public static SortedList<string, Reaction> Reactions;
        public static SortedList<string, Planet> Planets;
        public static SortedList<string, Component> Components;
        public static SortedList<string, Resource> Resources;

        public static LinkData BaseLink;

        public static string NewFacName, CurFacName;
        public static string NewFacPlanet;

        public static string PI_Path;
        public static string PIBase_Path, PIData_Path;
        public static string PICache_Path;
        public static int ExtractRate, CycleTime;

        private bool UseSerializableData = false;
        public string LastCacheRefresh = "2.4.0.3730";

        #region Plug-in Interface Functions

        public Boolean EveHQStartUp()
        {
            StreamReader sr;
            string cacheVers;
            
            Facilities = new SortedList<string, PINFacility>();
            Planets = new SortedList<string, Planet>();
            BaseLink = new LinkData();
            Reactions = new SortedList<string, Reaction>();
            Components = new SortedList<string, Component>();
            Resources = new SortedList<string, Resource>();

            UseSerializableData = false;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PIBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PIBase_Path = Application.StartupPath;
            }
            PI_Path = Path.Combine(PIBase_Path, "PI");
            PICache_Path = Path.Combine(PI_Path, "Cache");
            PIData_Path = Path.Combine(PI_Path, "Data");
            if (!Directory.Exists(PI_Path))
                Directory.CreateDirectory(PI_Path);
            if (!Directory.Exists(PIData_Path))
                Directory.CreateDirectory(PIData_Path);

            // Check for cache folder
            if (Directory.Exists(PICache_Path))
            {
                // Check for Last Version Text File
                if (File.Exists(Path.Combine(PICache_Path, "version.txt")))
                {
                    sr = new StreamReader(Path.Combine(PICache_Path, "version.txt"));
                    cacheVers = sr.ReadToEnd();
                    sr.Close();

                    if (IsUpdateAvailable(cacheVers, LastCacheRefresh))
                    {
                        UseSerializableData = false;
                    }
                    else
                        UseSerializableData = true;
                }
                else
                {
                    UseSerializableData = false;
                }
            }
            else
                Directory.CreateDirectory(PICache_Path);

            if (!UseSerializableData)
            {
                SetupReactions();
                LoadPlanetData();
                LoadResourceData();
                SaveDataFilesToDisk();
            }
            else
            {
                LoadDataFilesFromDisk();
            }

            LoadLinkData();
            LoadPIFacilitiesFromDisk();

            return true;
        }

        public void LoadDataFilesFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Planets.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Planets = (SortedList<string, Planet>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Reactions.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Reactions = (SortedList<string, Reaction>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Resources.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Resources = (SortedList<string, Resource>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Components.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Components = (SortedList<string, Component>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public void SaveDataFilesToDisk()
        {
            string fname;

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Planets.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Planets);
            pStream.Close();

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Reactions.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Reactions);
            pStream.Close();

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Resources.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Resources);
            pStream.Close();

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Components.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Components);
            pStream.Close();

            fname = Path.Combine(PlugInData.PICache_Path, "version.txt");
            StreamWriter sw = new StreamWriter(fname);
            sw.Write(LastCacheRefresh);
            sw.Close();
        }

        public static void SavePIFacilitiesToDisk()
        {
            string fname;

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Facilities.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Facilities);
            pStream.Close();
        }

        public static void LoadPIFacilitiesFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PlugInData.PIData_Path, "PI_Facilities.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Facilities = (SortedList<string, PINFacility>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                    cStr = File.OpenRead(fname);
                    myBf = new BinaryFormatter();
                    OldFac = (SortedList<string, PIFacility>)myBf.Deserialize(cStr);
                    cStr.Close();
                    ConvertToNewFacilityFormat();
                    SavePIFacilitiesToDisk();
                }
            }
        }

        public static void ConvertToNewFacilityFormat()
        {
            string s, pt;
            int Col, Row;

            foreach (PIFacility po in OldFac.Values)
            {
                PINFacility pn = new PINFacility();
                Col = 0;
                Row = 0;
                pn.Command = new CommandCenter(po.Command);
                pn.Command.CLoc = new Point(Col * 251, Row * 71);

                Row++;
                pn.LaunchPad = new SortedList<string, Launchpad>();
                foreach (var lp in po.LaunchPad)
                {
                    if (Row > 5)
                    {
                        Col++;
                        Row = 0;
                    }
                    lp.Value.CLoc = new Point(Col * 251, Row * 71);
                    pn.LaunchPad.Add(lp.Key, new Launchpad(lp.Value));
                    Row++;
                }

                pt = po.PlanetType.Replace("Planet (", "");
                pt = pt.Replace(")", "");
                pt = pt.Trim();

                pn.Extractors = new SortedList<string, Extractor>();
                foreach (var ex in po.Extractors)
                {
                    s = ex.Value.Name.Replace(pt, "");
                    s = s.Replace("Extractor", "");
                    s = s.Trim();
                    if (s.Contains("Aqueous Liquid"))
                        s = "Aqueous Liquids";

                    ex.Value.Extracting = s;

                    if (ex.Value.CycleTime < 1)
                        ex.Value.CycleTime = 30;
                    if (ex.Value.ExtRate < 1)
                        ex.Value.ExtRate = 1000;
                    if (ex.Value.RunTime < 1)
                        ex.Value.RunTime = 23;

                    if (Row > 5)
                    {
                        Col++;
                        Row = 0;
                    }
                    ex.Value.CLoc = new Point(Col * 251, Row * 71);
                   
                    pn.Extractors.Add(ex.Key, new Extractor(ex.Value));
                    Row++;
                }

                pn.Storage = new SortedList<string, StorageFacility>();
                foreach (var sf in po.Storage)
                {
                    if (Row > 5)
                    {
                        Col++;
                        Row = 0;
                    }
                    sf.Value.CLoc = new Point(Col * 251, Row * 71);
                    pn.Storage.Add(sf.Key, new StorageFacility(sf.Value));
                    Row++;
                }

                pn.Links = new ArrayList();
                foreach (Link l in po.Links)
                    pn.Links.Add(new Link(l));

                pn.Name = po.Name;
                pn.SystemName = po.SystemName;
                pn.PlanetName = po.PlanetName;
                pn.PlanetType = pt;
                pn.SystemID = po.SystemID;
                pn.PlanetID = po.PlanetID;
                pn.CPU = po.CPU;
                pn.Power = po.Power;
                pn.StoreCap = po.StoreCap;
                pn.inOverview = po.inOverview;
                pn.AvgLinkLength = po.AvgLinkLength;
                pn.NumLinks = po.NumLinks;
                pn.numMods = 0;
                pn.Converted = true;

                pn.Processors = new SortedList<string, Processor>();
                foreach (var pr in po.P1)
                {
                    pr.Value.ProcLevel = 1;
                    pr.Value.Reacting.level = 1;
                    pr.Value.Qty = 1;
                    if (Row > 5)
                    {
                        Col++;
                        Row = 0;
                    }
                    
                    pr.Value.CLoc = new Point(Col * 251, Row * 71);
                    pn.Processors.Add(pr.Key, new Processor(pr.Value));
                    Row++;
                }

                foreach (var pr in po.P2)
                {
                    pr.Value.ProcLevel = 2;
                    pr.Value.Reacting.level = 2;
                    if (Row > 5)
                    {
                        Col++;
                        Row = 0;
                    }

                    pr.Value.CLoc = new Point(Col * 211, Row * 71);
                    pn.Processors.Add(pr.Key, new Processor(pr.Value));
                    Row++;
                }

                foreach (var pr in po.P3)
                {
                    pr.Value.ProcLevel = 3;
                    pr.Value.Reacting.level = 3;
                    if (Row > 5)
                    {
                        Col++;
                        Row = 0;
                    }

                    pr.Value.CLoc = new Point(Col * 211, Row * 71);
                    pn.Processors.Add(pr.Key, new Processor(pr.Value));
                    Row++;
                }

                foreach (var pr in po.P4)
                {
                    pr.Value.ProcLevel = 4;
                    pr.Value.Reacting.level = 4;
                    if (Row > 5)
                    {
                        Col++;
                        Row = 0;
                    }

                    pr.Value.CLoc = new Point(Col * 211, Row * 71);
                    pn.Processors.Add(pr.Key, new Processor(pr.Value));
                    Row++;
                }

                Facilities.Add(pn.Name, pn);
            }
        }

        public static void ConvertToECUFacilityFormat()
        {
            // To convert to ECU format, we just need to remove all Extractors at this time
            foreach (PINFacility pn in Facilities.Values)
            {
                pn.Extractors = new SortedList<string, Extractor>();
            }
        }

        public static Planet GetPlanetData(string name)
        {
            if (name.Contains("Planet"))
            {
                name = name.Replace("Planet (", "");
                name = name.Replace(")", "");
                name = name.Trim();
            }

            return Planets[name];
        }

        private void LoadLinkData()
        {
            DataSet procData, procStatData;
            string strSQL;
            int typeID = 1036;
            int dgmT;

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + typeID + ";";
            procData = EveHQ.Core.DataFunctions.GetData(strSQL);

            DataRow dr = procData.Tables[0].Rows[0];

            typeID = Convert.ToInt32(dr.ItemArray[0].ToString());
            BaseLink.typeID = typeID;
            BaseLink.Name = dr.ItemArray[2].ToString();

            // Get Table With Tower or Tower Item Details
            strSQL = "SELECT * FROM (dgmTypeAttributes INNER JOIN dgmAttributeTypes ON " +
                     "dgmTypeAttributes.attributeID = dgmAttributeTypes.attributeID) WHERE " +
                     "dgmTypeAttributes.typeID=" + typeID + ";";
            procStatData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow mr in procStatData.Tables[0].Rows)
            {
                dgmT = Convert.ToInt32(mr.ItemArray[1].ToString());

                switch (dgmT)
                {
                    case 15:
                        BaseLink.Power = GetIntFromVariableIA(mr, 2, 3);
                        break;
                    case 49:
                        BaseLink.CPU = GetIntFromVariableIA(mr, 2, 3);
                        break;
                    case 1631:
                        BaseLink.MCPerHour = GetIntFromVariableIA(mr, 2, 3);
                        break;
                    case 1633:
                        BaseLink.MWPerKM = GetDoubleFromVariableIA(mr, 2, 3);
                        break;
                    case 1634:
                        BaseLink.CPUPerKM = GetDoubleFromVariableIA(mr, 2, 3);
                        break;
                    case 1635:
                        BaseLink.CPULoadMult = GetDoubleFromVariableIA(mr, 2, 3);
                        break;
                    case 1636:
                        BaseLink.MWLoadMult = GetDoubleFromVariableIA(mr, 2, 3);
                        break;
                    default:
                        break;
                }
            }
        }

        private int GetIntFromVariableIA(DataRow dr, int aI_1, int aI_2)
        {
            int retVal = 0;

            if (!dr.ItemArray[aI_1].Equals(System.DBNull.Value))
            {
                retVal = Convert.ToInt32(dr.ItemArray[aI_1]);
            }
            else
            {
                retVal = Convert.ToInt32(dr.ItemArray[aI_2]);
            }

            return retVal;
        }

        private double GetDoubleFromVariableIA(DataRow dr, int aI_1, int aI_2)
        {
            double retVal = 0;

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

        private List<string> GetPlanetsForResource(string rsrc)
        {
            List<string> retList = new List<string>();

            switch (rsrc)
            {
                case "Microorganisms":
                    retList.Add("Barren");
                    retList.Add("Ice");
                    retList.Add("Oceanic");
                    retList.Add("Temperate");
                    break;
                case "Carbon Compounds":
                    retList.Add("Barren");
                    retList.Add("Oceanic");
                    retList.Add("Temperate");
                    break;
                case "Planktic Colonies":
                    retList.Add("Ice");
                    retList.Add("Oceanic");
                    break;
                case "Non-CS Crystals":
                    retList.Add("Lava");
                    retList.Add("Plasma");
                    break;
                case "Ionic Solutions":
                    retList.Add("Gas");
                    retList.Add("Storm");
                    break;
                case "Autotrophs":
                    retList.Add("Temperate");
                    break;
                case "Reactive Gas":
                    retList.Add("Gas");
                    break;
                case "Noble Gas":
                    retList.Add("Gas");
                    retList.Add("Ice");
                    retList.Add("Storm");
                    break;
                case "Suspended Plasma":
                    retList.Add("Lava");
                    retList.Add("Plasma");
                    retList.Add("Storm");
                    break;
                case "Noble Metals":
                    retList.Add("Barren");
                    retList.Add("Plasma");
                    break;
                case "Complex Organisms":
                    retList.Add("Oceanic");
                    retList.Add("Temperate");
                    break;
                case "Base Metals":
                    retList.Add("Barren");
                    retList.Add("Gas");
                    retList.Add("Lava");
                    retList.Add("Plasma");
                    retList.Add("Storm");
                    break;
                case "Felsic Magma":
                    retList.Add("Lava");
                    break;
                case "Heavy Metals":
                    retList.Add("Ice");
                    retList.Add("Lava");
                    retList.Add("Plasma");
                    break;
                case "Aqueous Liquids":
                    retList.Add("Barren");
                    retList.Add("Gas");
                    retList.Add("Ice");
                    retList.Add("Oceanic");
                    retList.Add("Storm");
                    retList.Add("Temperate");
                    break;
                default:
                    retList.Add("Unknown");
                    break;
            }

            return retList;
        }

        private void LoadResourceData()
        {
            DataSet groupData, typeData;
            string strSQL;
            long groupID;
            Resource R;

            // Get Table With Category / Group Data for Planetary Resource information (Category 42)
            strSQL = "SELECT * FROM invGroups WHERE invGroups.categoryID=42;";
            groupData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow dr in groupData.Tables[0].Rows)
            {
                groupID = Convert.ToInt64(dr.ItemArray[0].ToString());

                // Get Table of item data for each Resource Item
                //strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + groupID + ";";
                strSQL = "SELECT invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.volume, invTypes.basePrice, eveIcons.iconFile";
                strSQL += " FROM invGroups INNER JOIN (invTypes INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
                strSQL += " WHERE (invTypes.groupID=" + groupID + ") AND (invTypes.published=1)";
                strSQL += " ORDER BY invTypes.typeName;";
                typeData = EveHQ.Core.DataFunctions.GetData(strSQL);

                foreach (DataRow tr in typeData.Tables[0].Rows)
                {
                    R = new Resource();
                    R.groupID = groupID;
                    R.typeID = Convert.ToInt64(tr.ItemArray[0].ToString());
                    R.Name = tr.ItemArray[1].ToString();
                    R.Desc = tr.ItemArray[2].ToString();
                    R.Volume = Convert.ToDouble(tr.ItemArray[4].ToString());
                    R.Cost = Convert.ToDouble(tr.ItemArray[5].ToString());
                    R.iconID = Convert.ToInt32(tr.ItemArray[3].ToString());
                    R.icon = tr.ItemArray[6].ToString();
                    R.Planets = GetPlanetsForResource(R.Name);

                    Resources.Add(R.Name, R);

                    //ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), R.typeID);
                }
            }
        }

         private void LoadPlanetData()
        {
            DataSet procData;
            string strSQL, pName;
            int typeID = 7;
            Planet p;

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + typeID + ";";
            procData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow dr in procData.Tables[0].Rows)
            {
                p = new Planet();

                p.typeID = Convert.ToInt64(dr.ItemArray[0].ToString());

                pName = dr.ItemArray[2].ToString();
                pName = pName.Replace("Planet (", "");
                pName = pName.Replace(")", "");
                p.TypeName = pName;
                p.Description = dr.ItemArray[3].ToString();
                p.graphicID = Convert.ToInt32(dr.ItemArray[4].ToString());
                p.LoadPlanetData(p.typeID);
                Planets.Add(p.TypeName, p);
            }
        }

        private void SetupReactions()
        {
            DataSet schemData, schemCompData, itemData;
            int schemID, compID, input;
            string strSQL;
            Reaction r;
            Component c;
            SortedList<string, string> repName = new SortedList<string, string>();

            // Get Table With Schematic Information
            strSQL = "SELECT * FROM planetSchematics;";
            schemData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow ps in schemData.Tables[0].Rows)
            {
                r = new Reaction();
                bool AddReaction = true;
                schemID = Convert.ToInt32(ps.ItemArray[0].ToString());
                r.typeID = schemID;
                r.reactName = ps.ItemArray[1].ToString();
                r.cycleTime = Convert.ToInt32(ps.ItemArray[2].ToString());

                // Get Table With Schematic Input and Output Information
                strSQL = "SELECT * FROM planetSchematicsTypeMap WHERE planetSchematicsTypeMap.schematicID="+schemID+";";
                schemCompData = EveHQ.Core.DataFunctions.GetData(strSQL);

                foreach (DataRow sc in schemCompData.Tables[0].Rows)
                {
                    c = new Component();
                    compID = Convert.ToInt32(sc.ItemArray[1].ToString());
                    c.ID = compID;
                    if (EveHQ.Core.HQ.itemData[c.ID.ToString()].Published == false)
                    {
                        AddReaction = false;
                    }
                    c.Qty = Convert.ToInt32(sc.ItemArray[2].ToString());
                    input = Convert.ToInt32(sc.ItemArray[3]);

                    // Get Table With Component Item Information
                    strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=" + compID + ";";
                    itemData = EveHQ.Core.DataFunctions.GetData(strSQL);

                    DataRow id = itemData.Tables[0].Rows[0];
                    if (!id.ItemArray[4].Equals(System.DBNull.Value))
                        c.graphicID = Convert.ToInt32(id.ItemArray[4].ToString());
                    c.Name = id.ItemArray[2].ToString();
                    c.Desc = id.ItemArray[3].ToString();
                    c.Volume = Convert.ToDouble(id.ItemArray[7].ToString());

                    if (repName.ContainsKey(c.Name))
                        c.Name = repName[c.Name];

                    if (input < 1)
                    {
                        if (c.Qty == 1)
                            r.level = 4;
                        else if (c.Qty == 3)
                            r.level = 3;
                        else if (c.Qty == 5)
                            r.level = 2;
                        else
                            r.level = 1;

                        if (!r.reactName.Equals(c.Name))
                        {
                            repName.Add(c.Name, r.reactName);
                            c.Name = r.reactName;
                        }

                        r.outputs.Add(compID, c);
                    }
                    else
                    {
                        r.inputs.Add(compID, c);
                    }

                    if (!Components.ContainsKey(c.Name))
                        Components.Add(c.Name, new Component(c));
                }

                if (AddReaction == true)
                {
                    Reactions.Add(r.reactName, r);
                }
            }
        }

        private Boolean IsUpdateAvailable(String locVer, String remVer)
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

                for (int x = 0; x < 4; x++)
                {
                    if ((Convert.ToInt32(remot[x])) != (Convert.ToInt32(local[x])))
                    {
                        if ((Convert.ToInt32(remot[x])) > (Convert.ToInt32(local[x])))
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

        public EveHQ.Core.PlugIn GetEveHQPlugInInfo()
        {
            EveHQ.Core.PlugIn EveHQPlugIn = new EveHQ.Core.PlugIn();

            EveHQPlugIn.Name = "PI Manager";
            EveHQPlugIn.Description = "Design PI Layouts";
            EveHQPlugIn.Author = "Jay Teague <aka: Sherksilver>";
            EveHQPlugIn.MainMenuText = "PI Manager";
            EveHQPlugIn.RunAtStartup = true;
            EveHQPlugIn.RunInIGB = false;
            EveHQPlugIn.MenuImage = Properties.Resources.pi_icon;
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
            PIM = new PIMain();
            return PIM;
        }

        public object GetPlugInData(object objData, int intDataType)
        {
            return null;
        }

        public static List<string> GetResourcesForPlanet(string pN)
        {
            List<string> retList = new List<string>();

            foreach (Resource R in Resources.Values)
            {
                if (R.Planets.Contains(pN))
                    retList.Add(R.Name);
            }

            return retList;
        }
    }
        #endregion

}
