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

namespace EveHQ.PI
{
    [Serializable]
    public class Planet
    {
        public long typeID;
        public string TypeName;
        public string Description;
        public int graphicID;
        public SortedList<string, Processor> Processors;  // Key = Processor Name
        public SortedList<string, Extractor> Extractors;  // Key = Extractor Name
        public SortedList<string, Launchpad> LaunchPads;  // Key = LaunchPad Name
        public SortedList<string, StorageFacility> Storage; // Key = Storage Name
        public SortedList<string, CommandCenter> CmdCenter; // Key = Command Center Name
        public ExtControlUnit ECU;

        public Planet()
        {
            typeID = 0;
            TypeName = "";
            Description = "";
            graphicID = 0;
            Processors = new SortedList<string, Processor>();
            Extractors = new SortedList<string, Extractor>();
            LaunchPads = new SortedList<string, Launchpad>();
            Storage = new SortedList<string, StorageFacility>();
            CmdCenter = new SortedList<string, CommandCenter>();
            ECU = new ExtControlUnit();
        }

        public Planet(Planet p)
        {
            typeID = p.typeID;
            TypeName = p.TypeName;
            Description = p.Description;
            graphicID = p.graphicID;
            Processors = new SortedList<string, Processor>();
            foreach (Processor pr in p.Processors.Values)
                Processors.Add(pr.Name, pr);

            Extractors = new SortedList<string, Extractor>();
            foreach (Extractor ex in p.Extractors.Values)
                Extractors.Add(ex.Name, ex);

            LaunchPads = new SortedList<string, Launchpad>();
            foreach (Launchpad l in p.LaunchPads.Values)
                LaunchPads.Add(l.Name, l);

            Storage = new SortedList<string, StorageFacility>();
            foreach (StorageFacility s in p.Storage.Values)
                Storage.Add(s.Name, s);

            CmdCenter = new SortedList<string, CommandCenter>();
            foreach (CommandCenter c in p.CmdCenter.Values)
                CmdCenter.Add(c.Name, c);

            ECU = new ExtControlUnit(p.ECU);
        }

        public void LoadPlanetData(long typeID)
        {
            LoadCommandCenterData(typeID);
            LoadExtractorData(typeID);
            LoadProcessorData(typeID);
            LoadLaunchPadData(typeID);
            LoadStorageData(typeID);
            LoadECUData(typeID);
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

        private void LoadECUData(long tid)
        {
            DataSet procData, procStatData;
            string strSQL, str;
            long typeID = 1063;
            int dgmT;
            double val;
            ExtControlUnit p;

            // Get Table With Extractor Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + typeID + ";";
            procData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow dr in procData.Tables[0].Rows)
            {
                p = new ExtControlUnit();
                p.typeID = Convert.ToInt64(dr.ItemArray[0].ToString());
                p.Name = dr.ItemArray[2].ToString();
                p.Desc = dr.ItemArray[3].ToString();
                p.graphicID = Convert.ToInt32(dr.ItemArray[4].ToString());
                p.Mass = Convert.ToInt32(dr.ItemArray[6].ToString());
                p.Volume = Convert.ToInt32(dr.ItemArray[7].ToString());
                p.Capacity = Convert.ToInt32(dr.ItemArray[8].ToString());
                str = dr.ItemArray[11].ToString();
                val = Convert.ToDouble(str);
                p.Cost = Convert.ToInt32(val);

                // Get Table With Tower or Tower Item Details
                strSQL = "SELECT * FROM (dgmTypeAttributes INNER JOIN dgmAttributeTypes ON " +
                         "dgmTypeAttributes.attributeID = dgmAttributeTypes.attributeID) WHERE " +
                         "dgmTypeAttributes.typeID=" + p.typeID + ";";
                procStatData = EveHQ.Core.DataFunctions.GetData(strSQL);

                foreach (DataRow mr in procStatData.Tables[0].Rows)
                {
                    dgmT = Convert.ToInt32(mr.ItemArray[1].ToString());

                    switch (dgmT)
                    {
                        case 15:
                            p.Power = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 49:
                        case 50:
                            p.CPU = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 709:
                            p.HarvestType = 0;
                            break;
                        case 1632:
                            p.ptypeID = GetIntFromVariableIA(mr, 2, 3);
                           break;
                        case 1642:
                            p.ExtRate = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1643:
                            p.CycleTime = (GetIntFromVariableIA(mr, 2, 3) / 60);
                            break;
                        case 1644:
                            p.DepRange = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1645:
                            p.DepRate = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1690:
                            p.Head_CPU = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1691:
                            p.Head_Power = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        default:
                            break;
                    }
                }

                p.RunTime = 1;

                if (p.ptypeID != tid)
                    continue;

                ECU = new ExtControlUnit(p);
            }
        }


        private void LoadStorageData(long tid)
        {
            DataSet procData, procStatData;
            string strSQL, str;
            long typeID = 1029;
            int dgmT;
            double val;
            StorageFacility p;

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + typeID + ";";
            procData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow dr in procData.Tables[0].Rows)
            {
                p = new StorageFacility();

                p.typeID = Convert.ToInt64(dr.ItemArray[0].ToString());
                p.Name = dr.ItemArray[2].ToString();
                p.Desc = dr.ItemArray[3].ToString();
                p.graphicID = Convert.ToInt32(dr.ItemArray[4].ToString());
                p.Mass = Convert.ToInt32(dr.ItemArray[6].ToString());
                p.Volume = Convert.ToInt32(dr.ItemArray[7].ToString());
                p.Capacity = Convert.ToInt32(dr.ItemArray[8].ToString());
                str = dr.ItemArray[11].ToString();
                val = Convert.ToDouble(str);
                p.Cost = Convert.ToInt32(val);

                // Get Table With Tower or Tower Item Details
                strSQL = "SELECT * FROM (dgmTypeAttributes INNER JOIN dgmAttributeTypes ON " +
                         "dgmTypeAttributes.attributeID = dgmAttributeTypes.attributeID) WHERE " +
                         "dgmTypeAttributes.typeID=" + p.typeID + ";";
                procStatData = EveHQ.Core.DataFunctions.GetData(strSQL);

                foreach (DataRow mr in procStatData.Tables[0].Rows)
                {
                    dgmT = Convert.ToInt32(mr.ItemArray[1].ToString());

                    switch (dgmT)
                    {
                        case 15:
                            p.Power = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 49:
                        case 50:
                            p.CPU = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1632:
                            p.ptypeID = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        default:
                            break;
                    }
                }
                if (p.ptypeID != tid)
                    continue;

                Storage.Add(p.Name, p);
            }
        }

        private void LoadLaunchPadData(long tid)
        {
            DataSet procData, procStatData;
            string strSQL, str;
            long typeID = 1030;
            int dgmT;
            double val;
            Launchpad p;

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + typeID + ";";
            procData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow dr in procData.Tables[0].Rows)
            {
                p = new Launchpad();

                p.typeID = Convert.ToInt64(dr.ItemArray[0].ToString());
                p.Name = dr.ItemArray[2].ToString();
                p.Desc = dr.ItemArray[3].ToString();
                p.graphicID = Convert.ToInt32(dr.ItemArray[4].ToString());
                p.Mass = Convert.ToInt32(dr.ItemArray[6].ToString());
                p.Volume = Convert.ToInt32(dr.ItemArray[7].ToString());
                p.Capacity = Convert.ToInt32(dr.ItemArray[8].ToString());
                str = dr.ItemArray[11].ToString();
                val = Convert.ToDouble(str);
                p.Cost = Convert.ToInt32(val);

                // Get Table With Tower or Tower Item Details
                strSQL = "SELECT * FROM (dgmTypeAttributes INNER JOIN dgmAttributeTypes ON " +
                         "dgmTypeAttributes.attributeID = dgmAttributeTypes.attributeID) WHERE " +
                         "dgmTypeAttributes.typeID=" + p.typeID + ";";
                procStatData = EveHQ.Core.DataFunctions.GetData(strSQL);

                foreach (DataRow mr in procStatData.Tables[0].Rows)
                {
                    dgmT = Convert.ToInt32(mr.ItemArray[1].ToString());

                    switch (dgmT)
                    {
                        case 15:
                            p.Power = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 49:
                        case 50:
                            p.CPU = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1632:
                            p.ptypeID = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1638:
                            p.InpTax = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1639:
                            p.ExpTax = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        default:
                            break;
                    }
                }
                if (p.ptypeID != tid)
                    continue;

                LaunchPads.Add(p.Name, p);
            }
        }

        private void LoadExtractorData(long tid)
        {
            DataSet procData, procStatData;
            string strSQL, str;
            long typeID = 1026;
            int dgmT;
            double val;
            Extractor p;

            // Get Table With Extractor Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + typeID + ";";
            procData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow dr in procData.Tables[0].Rows)
            {
                p = new Extractor();

                p.typeID = Convert.ToInt64(dr.ItemArray[0].ToString());
                p.Name = dr.ItemArray[2].ToString();
                p.Desc = dr.ItemArray[3].ToString();
                p.graphicID = Convert.ToInt32(dr.ItemArray[4].ToString());
                p.Mass = Convert.ToInt32(dr.ItemArray[6].ToString());
                p.Volume = Convert.ToInt32(dr.ItemArray[7].ToString());
                p.Capacity = Convert.ToInt32(dr.ItemArray[8].ToString());
                str = dr.ItemArray[11].ToString();
                val = Convert.ToDouble(str);
                p.Cost = Convert.ToInt32(val);

                // Get Table With Tower or Tower Item Details
                strSQL = "SELECT * FROM (dgmTypeAttributes INNER JOIN dgmAttributeTypes ON " +
                         "dgmTypeAttributes.attributeID = dgmAttributeTypes.attributeID) WHERE " +
                         "dgmTypeAttributes.typeID=" + p.typeID + ";";
                procStatData = EveHQ.Core.DataFunctions.GetData(strSQL);

                foreach (DataRow mr in procStatData.Tables[0].Rows)
                {
                    dgmT = Convert.ToInt32(mr.ItemArray[1].ToString());

                    switch (dgmT)
                    {
                        case 15:
                            p.Power = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 49:
                        case 50:
                            p.CPU = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 709:
                            p.HarvestType = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1632:
                            p.ptypeID = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1642:
                            break;
                        case 1643:
                            break;
                        case 1644:
                            p.DepRange = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1645:
                            p.DepRate = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        default:
                            break;
                    }
                }
                if (p.ptypeID != tid)
                    continue;

                p.ExtRate = 1000;
                p.CycleTime = 30;
                p.RunTime = 23;
                Extractors.Add(p.Name, p);
            }
        }

        private void LoadCommandCenterData(long tid)
        {
            DataSet procData, procStatData;
            string strSQL, str;
            long typeID = 1027;
            int dgmT;
            double val;
            CommandCenter p;

            // Get Table With Command Center Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + typeID + ";";
            procData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow dr in procData.Tables[0].Rows)
            {
                p = new CommandCenter();

                p.typeID = Convert.ToInt64(dr.ItemArray[0].ToString());
                p.Name = dr.ItemArray[2].ToString();
                p.Desc = dr.ItemArray[3].ToString();
                p.graphicID = Convert.ToInt32(dr.ItemArray[4].ToString());
                p.Mass = Convert.ToInt32(dr.ItemArray[6].ToString());
                p.Volume = Convert.ToInt32(dr.ItemArray[7].ToString());
                p.Capacity = Convert.ToInt32(dr.ItemArray[8].ToString());
                str = dr.ItemArray[11].ToString();
                val = Convert.ToDouble(str);
                p.Cost = Convert.ToInt32(val);

                // Get Table With Tower or Tower Item Details
                strSQL = "SELECT * FROM (dgmTypeAttributes INNER JOIN dgmAttributeTypes ON " +
                         "dgmTypeAttributes.attributeID = dgmAttributeTypes.attributeID) WHERE " +
                         "dgmTypeAttributes.typeID=" + p.typeID + ";";
                procStatData = EveHQ.Core.DataFunctions.GetData(strSQL);

                foreach (DataRow mr in procStatData.Tables[0].Rows)
                {
                    dgmT = Convert.ToInt32(mr.ItemArray[1].ToString());

                    switch (dgmT)
                    {
                        case 11:
                            p.Power = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 48:
                            p.CPU = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1632:
                            p.ptypeID = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1639:
                            p.ExportTax = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        default:
                            break;
                    }
                }
                if (p.ptypeID != tid)
                    continue;

                CmdCenter.Add(p.Name, p);
            }
        }

        private void LoadProcessorData(long tid)
        {
            DataSet procData, procStatData, reactData;
            string strSQL, str;
            string error;
            long typeID = 1028;
            int dgmT;
            string schmID;
            double val;
            Processor p;
            List<string> FullList = new List<string>();

            // Get Table With Tower or Tower Item Information
            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=" + typeID + ";";
            procData = EveHQ.Core.DataFunctions.GetData(strSQL);

            foreach (DataRow dr in procData.Tables[0].Rows)
            {
                p = new Processor();

                p.typeID = Convert.ToInt64(dr.ItemArray[0].ToString());
                
                p.Name = dr.ItemArray[2].ToString();
                if (p.Name.Contains("Basic"))
                {
                    p.Name.Replace("Basic", "Basic (P1)");
                    p.ProcLevel = 1;
                }
                else if (p.Name.Contains("High Tech"))
                {
                    p.Name.Replace("High Tech", "High Tech (P4)");
                    p.ProcLevel = 4;
                }
                else if (p.Name.Contains("High-Tech"))
                {
                    p.Name.Replace("High-Tech", "High-Tech (P4)");
                    p.ProcLevel = 4;
                }
                else
                    p.ProcLevel = 5;

                p.Desc = dr.ItemArray[3].ToString();
                p.graphicID = Convert.ToInt32(dr.ItemArray[4].ToString());
                p.Mass = Convert.ToInt32(dr.ItemArray[6].ToString());
                p.Volume = Convert.ToInt32(dr.ItemArray[7].ToString());
                p.Capacity = Convert.ToInt32(dr.ItemArray[8].ToString());
                str = dr.ItemArray[11].ToString();
                val = Convert.ToDouble(str);
                p.Cost = Convert.ToInt32(val);

                // Get Table With Processor Details
                strSQL = "SELECT * FROM (dgmTypeAttributes INNER JOIN dgmAttributeTypes ON " +
                         "dgmTypeAttributes.attributeID = dgmAttributeTypes.attributeID) WHERE " +
                         "dgmTypeAttributes.typeID=" + p.typeID + ";";
                procStatData = EveHQ.Core.DataFunctions.GetData(strSQL);

                foreach (DataRow mr in procStatData.Tables[0].Rows)
                {
                    dgmT = Convert.ToInt32(mr.ItemArray[1].ToString());

                    switch (dgmT)
                    {
                        case 15:
                            p.Power = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 49:
                        case 50:
                            p.CPU = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        case 1632:
                            p.ptypeID = GetIntFromVariableIA(mr, 2, 3);
                            break;
                        default:
                            break;
                    }
                }

                if (p.ptypeID != tid)
                    continue;

                // Get Table With Tower or Tower Item Information
                strSQL = "SELECT * FROM planetSchematicsPinMap INNER JOIN planetSchematics ON " +
                        "planetSchematicsPinMap.schematicID=planetSchematics.schematicID WHERE " +
                        "planetSchematicsPinMap.pinTypeID=" + p.typeID + ";";
                reactData = EveHQ.Core.DataFunctions.GetData(strSQL);

                foreach (DataRow rd in reactData.Tables[0].Rows)
                {
                    schmID = rd.ItemArray[3].ToString();
                    FullList.Add(schmID);
                }

                if (p.ProcLevel > 4)
                {
                    string buName;
                    buName = p.Name;

                    p.AvailReactions = new List<string>(SetAvailListByLevel(FullList, 2));
                    p.ProcLevel = 2;
                    p.Name = buName.Replace("Advanced", "Advanced (P2)");
                    Processors.Add(p.Name, new Processor(p));

                    p.AvailReactions = new List<string>(SetAvailListByLevel(FullList, 3));
                    p.ProcLevel = 3;
                    p.Name = buName.Replace("Advanced", "Advanced (P3)");
                    if (Processors.ContainsKey(p.Name))
                    {
                        // Should never reach here - but anything is possible. 
                        error = "Processor already exists for: [" + buName + "] to [" + p.Name + "] Please notify Sherk";
                        MessageBox.Show(error, "PI Processor Error", MessageBoxButtons.OK);
                    }
                    else
                        Processors.Add(p.Name, p);
                }
                else
                {
                    if (Processors.ContainsKey(p.Name))
                    {
                        error = "Processor already exists for: [" + p.Name + "] Please notify Sherk";
                        MessageBox.Show(error, "PI Processor Error", MessageBoxButtons.OK);
                    }
                    else
                        Processors.Add(p.Name, p);
                }
            }
        }

        public List<string> SetAvailListByLevel(List<string> fullList, int Level)
        {
            List<string> retVal = new List<string>();

            foreach (string rs in fullList)
            {
                foreach (Component c in PlugInData.Reactions[rs].inputs.Values)
                {
                    if ((c.Qty > 10) && (Level.Equals(2)))
                    {
                        retVal.Add(rs);
                        break;
                    }
                    else if ((c.Qty <= 10) && (Level.Equals(3)))
                    {
                        retVal.Add(rs);
                        break;
                    }
                }
            }

            return retVal;
        }


    }
}
