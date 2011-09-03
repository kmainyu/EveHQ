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
    public class MiscData
    {
        public SortedList<int, string> Factions;
        public SortedList<int, Corporation> Corporations;
        public SortedList<int, string> Divisions;
        public ArrayList IceTypes;
        public ArrayList OreTypes;
        public ArrayList PlayerCorpList;
        public ArrayList TowerTypes;
        public ArrayList GooTypes;
        public ArrayList WHTypes;
        public SortedList<string, Ship> PilotShipSave;
        public SortedList<string, long> WH_Nm2Cls;
        public SortedList<int, SortedList<string, ArrayList>> WHExpByClass;
        public SortedList<long, long> WHRegnToClass;


        public SortedList<long, SortedList<long, long>> WH_Classes;

        public MiscData()
        {
            Factions = new SortedList<int, string>();
            Corporations = new SortedList<int, Corporation>();
            Divisions = new SortedList<int, string>();
            IceTypes = new ArrayList();
            OreTypes = new ArrayList();
            PlayerCorpList = new ArrayList();
            TowerTypes = new ArrayList();
            GooTypes = new ArrayList();
            WHTypes = new ArrayList();
            PilotShipSave = new SortedList<string, Ship>();
            WH_Classes = new SortedList<long, SortedList<long, long>>();
            WH_Nm2Cls = new SortedList<string, long>();
            WHExpByClass = new SortedList<int, SortedList<string, ArrayList>>();
            WHRegnToClass = new SortedList<long, long>();
        }

        public void LoadAllMiscData(Object o)
        {
            LoadFactionData();
            LoadCorporationData();
            LoadDivisionData();
            LoadIceAndOreData();
            LoadTowerData();
            LoadMoonGooData();
            ImportPlayerCorpsFromXML();
            SaveMisc();
            if (Interlocked.Decrement(ref PlugInData.numBusy) == 0)
            {
                PlugInData.doneEvent.Set();
            }
        }

        public void SetupWHExpStuff()
        {
            WHExpByClass.Add(1, new SortedList<string, ArrayList>());
            WHExpByClass[1].Add("Cosmic Anomalies",new ArrayList());
            WHExpByClass[1]["Cosmic Anomalies"].Add("Perimeter Ambush Point");
            WHExpByClass[1]["Cosmic Anomalies"].Add("Perimeter Camp");
            WHExpByClass[1]["Cosmic Anomalies"].Add("Phase Catalyst Node");
            WHExpByClass[1]["Cosmic Anomalies"].Add("The Line");
            WHExpByClass[1].Add("Magnetometric",new ArrayList());
            WHExpByClass[1]["Magnetometric"].Add("Forgotten Perimeter Coronation Platform");
            WHExpByClass[1]["Magnetometric"].Add("Forgotten Perimeter Power Array");
            WHExpByClass[1].Add("Radar",new ArrayList());
            WHExpByClass[1]["Radar"].Add("Unsecured Perimeter Amplifier");
            WHExpByClass[1]["Radar"].Add("Unsecured Perimeter Information Center");
            WHExpByClass.Add(2, new SortedList<string, ArrayList>());
            WHExpByClass[2].Add("Cosmic Anomalies",new ArrayList());
            WHExpByClass[2]["Cosmic Anomalies"].Add("Perimeter Checkpoint");
            WHExpByClass[2]["Cosmic Anomalies"].Add("Perimeter Hangar");
            WHExpByClass[2]["Cosmic Anomalies"].Add("The Ruins of Enclave Cohort");
            WHExpByClass[2]["Cosmic Anomalies"].Add("Sleeper Data Sanctuary");
            WHExpByClass[2].Add("Magnetometric",new ArrayList());
            WHExpByClass[2]["Magnetometric"].Add("Forgotten Perimeter Gateway");
            WHExpByClass[2]["Magnetometric"].Add("Forgotten Perimeter Habitation Coils");
            WHExpByClass[2].Add("Radar",new ArrayList());
            WHExpByClass[2]["Radar"].Add("Unsecured Perimeter Comms Relay");
            WHExpByClass[2]["Radar"].Add("Unsecured Perimeter Transponder Farm");
            WHExpByClass.Add(3, new SortedList<string, ArrayList>());
            WHExpByClass[3].Add("Cosmic Anomalies",new ArrayList());
            WHExpByClass[3]["Cosmic Anomalies"].Add("Fortification Frontier Stronghold");
            WHExpByClass[3]["Cosmic Anomalies"].Add("Outpost Frontier Stronghold");
            WHExpByClass[3]["Cosmic Anomalies"].Add("Solar Cell");
            WHExpByClass[3]["Cosmic Anomalies"].Add("The Oruze Construct");
            WHExpByClass[3].Add("Magnetometric",new ArrayList());
            WHExpByClass[3]["Magnetometric"].Add("Forgotten Frontier Quarantine Outpost");
            WHExpByClass[3]["Magnetometric"].Add("Forgotten Frontier Recursive Depot");
            WHExpByClass[3].Add("Radar",new ArrayList());
            WHExpByClass[3]["Radar"].Add("Unsecured Frontier Database");
            WHExpByClass[3]["Radar"].Add("Unsecured Frontier Receiver");
            WHExpByClass.Add(4, new SortedList<string, ArrayList>());
            WHExpByClass[4].Add("Cosmic Anomalies",new ArrayList());
            WHExpByClass[4]["Cosmic Anomalies"].Add("Frontier Barracks");
            WHExpByClass[4]["Cosmic Anomalies"].Add("Frontier Command Post");
            WHExpByClass[4]["Cosmic Anomalies"].Add("Integrated Terminus");
            WHExpByClass[4]["Cosmic Anomalies"].Add("Sleeper Information Sanctum");
            WHExpByClass[4].Add("Magnetometric",new ArrayList());
            WHExpByClass[4]["Magnetometric"].Add("Forgotten Frontier Conversion Module");
            WHExpByClass[4]["Magnetometric"].Add("Forgotten Frontier Evacuation Center");
            WHExpByClass[4].Add("Radar",new ArrayList());
            WHExpByClass[4]["Radar"].Add("Unsecured Frontier Digital Nexus");
            WHExpByClass[4]["Radar"].Add("Unsecured Frontier Trinary Hub");
            WHExpByClass.Add(5, new SortedList<string, ArrayList>());
            WHExpByClass[5].Add("Cosmic Anomalies",new ArrayList());
            WHExpByClass[5]["Cosmic Anomalies"].Add("Core Garrison");
            WHExpByClass[5]["Cosmic Anomalies"].Add("Core Stronghold");
            WHExpByClass[5]["Cosmic Anomalies"].Add("Oruze Osobnyk");
            WHExpByClass[5]["Cosmic Anomalies"].Add("Quarantine Area");
            WHExpByClass[5].Add("Magnetometric",new ArrayList());
            WHExpByClass[5]["Magnetometric"].Add("Forgotten Core Data Field");
            WHExpByClass[5]["Magnetometric"].Add("Forgotten Core Information Pen");
            WHExpByClass[5].Add("Radar",new ArrayList());
            WHExpByClass[5]["Radar"].Add("Unsecured Frontier Enclave Relay");
            WHExpByClass[5]["Radar"].Add("Unsecured Frontier Server Bank");
            WHExpByClass.Add(6, new SortedList<string, ArrayList>());
            WHExpByClass[6].Add("Cosmic Anomalies",new ArrayList());
            WHExpByClass[6]["Cosmic Anomalies"].Add("Core Citadel");
            WHExpByClass[6]["Cosmic Anomalies"].Add("Core Bastion");
            WHExpByClass[6]["Cosmic Anomalies"].Add("Strange Energy Readings");
            WHExpByClass[6]["Cosmic Anomalies"].Add("The Mirror");
            WHExpByClass[6].Add("Magnetometric",new ArrayList());
            WHExpByClass[6]["Magnetometric"].Add("Forgotten Core Assembly Hall");
            WHExpByClass[6]["Magnetometric"].Add("Forgotten Core Circuitry Disassembler");
            WHExpByClass[6].Add("Radar",new ArrayList());
            WHExpByClass[6]["Radar"].Add("Unsecured Core Backup Array");
            WHExpByClass[6]["Radar"].Add("Unsecured Core Emergence");
            WHExpByClass.Add(0, new SortedList<string, ArrayList>());
            WHExpByClass[0].Add("Ladar",new ArrayList());
            WHExpByClass[0]["Ladar"].Add("Barren Perimeter Reservoir");
            WHExpByClass[0]["Ladar"].Add("Bountiful Frontier Reservoir");
            WHExpByClass[0]["Ladar"].Add("Instrumental Core Reservoir");
            WHExpByClass[0]["Ladar"].Add("Minor Perimeter Reservoir");
            WHExpByClass[0]["Ladar"].Add("Ordinary Perimeter Reservoir");
            WHExpByClass[0]["Ladar"].Add("Sizeable Perimeter Reservoir");
            WHExpByClass[0]["Ladar"].Add("Token Perimeter Reservoir");
            WHExpByClass[0]["Ladar"].Add("Vast Frontier Reservoir");
            WHExpByClass[0]["Ladar"].Add("Vital Core Reservoir");
            WHExpByClass[0].Add("Gravimetric",new ArrayList());
            WHExpByClass[0]["Gravimetric"].Add("Average Frontier Deposit");
            WHExpByClass[0]["Gravimetric"].Add("Common Perimeter Deposit");
            WHExpByClass[0]["Gravimetric"].Add("Exceptional Core Deposit");
            WHExpByClass[0]["Gravimetric"].Add("Infrequent Core Deposit");
            WHExpByClass[0]["Gravimetric"].Add("Isolated Core Deposit");
            WHExpByClass[0]["Gravimetric"].Add("Ordinary Perimeter Deposit");
            WHExpByClass[0]["Gravimetric"].Add("Rarified Core Deposit");
            WHExpByClass[0]["Gravimetric"].Add("Unusual Core Deposit");
            WHExpByClass[0]["Gravimetric"].Add("Uncommon Core Deposit");
            WHExpByClass[0]["Gravimetric"].Add("Unexceptional Frontier Deposit");

        }

        public void ImportPlayerCorpsFromXML()
        {
            XmlDocument pCorps = new XmlDocument();
            XmlNodeList crpLst;
            string cName;

            pCorps.LoadXml(Properties.Resources.corporations);

            crpLst = pCorps.SelectNodes("/content/corporations/corporation");

            foreach (XmlNode corp in crpLst)
            {
                cName = corp.Attributes.GetNamedItem("corporationName").Value.ToString();
                if (!PlayerCorpList.Contains(cName))
                    PlayerCorpList.Add(cName);
            }
        }

        public void LoadWHNm2Class()
        {
            string strSQL;
            DataSet whData;
            string whName;
            long whCls;

            strSQL = "SELECT * from invTypes WHERE groupID=988;";
            whData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!whData.Equals(System.DBNull.Value))
            {
                if (whData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in whData.Tables[0].Rows)
                    {
                        whName = row.ItemArray[2].ToString();

                        if (whName.Contains("Test"))
                            continue;

                        whName = whName.Replace("Wormhole", "");
                        whName = whName.Trim();
                        whCls = Convert.ToInt64(row.ItemArray[0]);

                        WH_Nm2Cls.Add(whName, whCls);
                        WHTypes.Add(whName);
                    }
                }
            }
        }

        public void LoadWHRegionToClass()
        {
            string strSQL;
            DataSet whData;
            long whCls, RID;

            strSQL = "SELECT * from mapLocationWormholeClasses;";
            whData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!whData.Equals(System.DBNull.Value))
            {
                if (whData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in whData.Tables[0].Rows)
                    {
                        RID = Convert.ToInt64(row.ItemArray[0]);
                        whCls = Convert.ToInt64(row.ItemArray[1]);

                        if (PlugInData.WHRegions.Contains(RID))
                        {
                            WHRegnToClass.Add(RID, whCls);
                        }
                    }
                }
            }
        }

        public string GetTargetClassForTypeID(long tid)
        {
            string retVal = "Unknown";

            if (WH_Classes.ContainsKey(tid))
            {
                foreach (var val in WH_Classes[tid])
                {
                    if (val.Key.Equals(1381))
                    {
                        if (val.Value < 7)
                            retVal = "Class " + val.Value + " Wormhole System";
                        else if (val.Value.Equals(7))
                            retVal = "Hi-Security System";
                        else if (val.Value.Equals(8))
                            retVal = "Low-Security System";
                        else
                            retVal = "Null-Security System";

                        break;
                    }
                }
            }

            return retVal;
        }

        public void LoadWHAttributes()
        {
            WH_Classes.Add(30463, new SortedList<long, long>());
            WH_Classes.Add(30579, new SortedList<long, long>());
            WH_Classes.Add(30583, new SortedList<long, long>());
            WH_Classes.Add(30584, new SortedList<long, long>());
            WH_Classes.Add(30642, new SortedList<long, long>());
            WH_Classes.Add(30643, new SortedList<long, long>());
            WH_Classes.Add(30644, new SortedList<long, long>());
            WH_Classes.Add(30645, new SortedList<long, long>());
            WH_Classes.Add(30646, new SortedList<long, long>());
            WH_Classes.Add(30647, new SortedList<long, long>());
            WH_Classes.Add(30648, new SortedList<long, long>());
            WH_Classes.Add(30649, new SortedList<long, long>());
            WH_Classes.Add(30657, new SortedList<long, long>());
            WH_Classes.Add(30658, new SortedList<long, long>());
            WH_Classes.Add(30659, new SortedList<long, long>());
            WH_Classes.Add(30660, new SortedList<long, long>());
            WH_Classes.Add(30661, new SortedList<long, long>());
            WH_Classes.Add(30662, new SortedList<long, long>());
            WH_Classes.Add(30663, new SortedList<long, long>());
            WH_Classes.Add(30664, new SortedList<long, long>());
            WH_Classes.Add(30665, new SortedList<long, long>());
            WH_Classes.Add(30666, new SortedList<long, long>());
            WH_Classes.Add(30667, new SortedList<long, long>());
            WH_Classes.Add(30668, new SortedList<long, long>());
            WH_Classes.Add(30671, new SortedList<long, long>());
            WH_Classes.Add(30672, new SortedList<long, long>());
            WH_Classes.Add(30673, new SortedList<long, long>());
            WH_Classes.Add(30674, new SortedList<long, long>());
            WH_Classes.Add(30675, new SortedList<long, long>());
            WH_Classes.Add(30676, new SortedList<long, long>());
            WH_Classes.Add(30677, new SortedList<long, long>());
            WH_Classes.Add(30678, new SortedList<long, long>());
            WH_Classes.Add(30679, new SortedList<long, long>());
            WH_Classes.Add(30680, new SortedList<long, long>());
            WH_Classes.Add(30681, new SortedList<long, long>());
            WH_Classes.Add(30682, new SortedList<long, long>());
            WH_Classes.Add(30683, new SortedList<long, long>());
            WH_Classes.Add(30684, new SortedList<long, long>());
            WH_Classes.Add(30685, new SortedList<long, long>());
            WH_Classes.Add(30686, new SortedList<long, long>());
            WH_Classes.Add(30687, new SortedList<long, long>());
            WH_Classes.Add(30688, new SortedList<long, long>());
            WH_Classes.Add(30689, new SortedList<long, long>());
            WH_Classes.Add(30690, new SortedList<long, long>());
            WH_Classes.Add(30691, new SortedList<long, long>());
            WH_Classes.Add(30692, new SortedList<long, long>());
            WH_Classes.Add(30693, new SortedList<long, long>());
            WH_Classes.Add(30694, new SortedList<long, long>());
            WH_Classes.Add(30695, new SortedList<long, long>());
            WH_Classes.Add(30696, new SortedList<long, long>());
            WH_Classes.Add(30697, new SortedList<long, long>());
            WH_Classes.Add(30698, new SortedList<long, long>());
            WH_Classes.Add(30699, new SortedList<long, long>());
            WH_Classes.Add(30700, new SortedList<long, long>());
            WH_Classes.Add(30701, new SortedList<long, long>());
            WH_Classes.Add(30702, new SortedList<long, long>());
            WH_Classes.Add(30703, new SortedList<long, long>());
            WH_Classes.Add(30704, new SortedList<long, long>());
            WH_Classes.Add(30705, new SortedList<long, long>());
            WH_Classes.Add(30706, new SortedList<long, long>());
            WH_Classes.Add(30707, new SortedList<long, long>());
            WH_Classes.Add(30708, new SortedList<long, long>());
            WH_Classes.Add(30709, new SortedList<long, long>());
            WH_Classes.Add(30710, new SortedList<long, long>());
            WH_Classes.Add(30711, new SortedList<long, long>());
            WH_Classes.Add(30712, new SortedList<long, long>());
            WH_Classes.Add(30713, new SortedList<long, long>());
            WH_Classes.Add(30714, new SortedList<long, long>());
            WH_Classes.Add(30715, new SortedList<long, long>());
            WH_Classes[30463].Add(1381, 9);
            WH_Classes[30463].Add(1382, 3600);
            WH_Classes[30463].Add(1383, 1000000000);
            WH_Classes[30463].Add(1384, 50000000);
            WH_Classes[30463].Add(1385, 150000000);
            WH_Classes[30463].Add(1457, 0);
            WH_Classes[30579].Add(1381, 1);
            WH_Classes[30579].Add(1382, 960);
            WH_Classes[30579].Add(1383, 100000000);
            WH_Classes[30579].Add(1384, 0);
            WH_Classes[30579].Add(1385, 20000000);
            WH_Classes[30579].Add(1457, 296);
            WH_Classes[30583].Add(1381, 2);
            WH_Classes[30583].Add(1382, 960);
            WH_Classes[30583].Add(1383, 750000000);
            WH_Classes[30583].Add(1384, 0);
            WH_Classes[30583].Add(1385, 300000000);
            WH_Classes[30583].Add(1457, 297);
            WH_Classes[30584].Add(1381, 3);
            WH_Classes[30584].Add(1382, 1440);
            WH_Classes[30584].Add(1383, 1000000000);
            WH_Classes[30584].Add(1384, 0);
            WH_Classes[30584].Add(1385, 300000000);
            WH_Classes[30584].Add(1457, 298);
            WH_Classes[30642].Add(1381, 4);
            WH_Classes[30642].Add(1382, 1440);
            WH_Classes[30642].Add(1383, 1000000000);
            WH_Classes[30642].Add(1384, 0);
            WH_Classes[30642].Add(1385, 300000000);
            WH_Classes[30642].Add(1457, 299);
            WH_Classes[30643].Add(1381, 5);
            WH_Classes[30643].Add(1382, 1440);
            WH_Classes[30643].Add(1383, 3000000000);
            WH_Classes[30643].Add(1384, 0);
            WH_Classes[30643].Add(1385, 1350000000);
            WH_Classes[30643].Add(1457, 300);
            WH_Classes[30644].Add(1381, 5);
            WH_Classes[30644].Add(1382, 1440);
            WH_Classes[30644].Add(1383, 3000000000);
            WH_Classes[30644].Add(1384, 0);
            WH_Classes[30644].Add(1385, 1000000000);
            WH_Classes[30644].Add(1457, 300);
            WH_Classes[30645].Add(1381, 6);
            WH_Classes[30645].Add(1382, 2880);
            WH_Classes[30645].Add(1383, 5000000000);
            WH_Classes[30645].Add(1384, 0);
            WH_Classes[30645].Add(1385, 300000000);
            WH_Classes[30645].Add(1457, 301);
            WH_Classes[30646].Add(1381, 6);
            WH_Classes[30646].Add(1382, 2880);
            WH_Classes[30646].Add(1383, 3000000000);
            WH_Classes[30646].Add(1384, 0);
            WH_Classes[30646].Add(1385, 1350000000);
            WH_Classes[30646].Add(1457, 301);
            WH_Classes[30647].Add(1381, 7);
            WH_Classes[30647].Add(1382, 960);
            WH_Classes[30647].Add(1383, 2000000000);
            WH_Classes[30647].Add(1384, 0);
            WH_Classes[30647].Add(1385, 1000000000);
            WH_Classes[30647].Add(1457, 302);
            WH_Classes[30648].Add(1381, 8);
            WH_Classes[30648].Add(1382, 1440);
            WH_Classes[30648].Add(1383, 3000000000);
            WH_Classes[30648].Add(1384, 0);
            WH_Classes[30648].Add(1385, 1350000000);
            WH_Classes[30648].Add(1457, 303);
            WH_Classes[30649].Add(1381, 9);
            WH_Classes[30649].Add(1382, 1440);
            WH_Classes[30649].Add(1383, 3000000000);
            WH_Classes[30649].Add(1384, 0);
            WH_Classes[30649].Add(1385, 1350000000);
            WH_Classes[30649].Add(1457, 304);
            WH_Classes[30657].Add(1381, 7);
            WH_Classes[30657].Add(1382, 960);
            WH_Classes[30657].Add(1383, 2000000000);
            WH_Classes[30657].Add(1384, 0);
            WH_Classes[30657].Add(1385, 1000000000);
            WH_Classes[30657].Add(1457, 302);
            WH_Classes[30658].Add(1381, 8);
            WH_Classes[30658].Add(1382, 960);
            WH_Classes[30658].Add(1383, 3000000000);
            WH_Classes[30658].Add(1384, 0);
            WH_Classes[30658].Add(1385, 1000000000);
            WH_Classes[30658].Add(1457, 303);
            WH_Classes[30659].Add(1381, 9);
            WH_Classes[30659].Add(1382, 1440);
            WH_Classes[30659].Add(1383, 3000000000);
            WH_Classes[30659].Add(1384, 0);
            WH_Classes[30659].Add(1385, 1000000000);
            WH_Classes[30659].Add(1457, 304);
            WH_Classes[30660].Add(1381, 1);
            WH_Classes[30660].Add(1382, 960);
            WH_Classes[30660].Add(1383, 500000000);
            WH_Classes[30660].Add(1384, 0);
            WH_Classes[30660].Add(1385, 20000000);
            WH_Classes[30660].Add(1457, 296);
            WH_Classes[30661].Add(1381, 2);
            WH_Classes[30661].Add(1382, 960);
            WH_Classes[30661].Add(1383, 1000000000);
            WH_Classes[30661].Add(1384, 0);
            WH_Classes[30661].Add(1385, 20000000);
            WH_Classes[30661].Add(1457, 297);
            WH_Classes[30662].Add(1381, 3);
            WH_Classes[30662].Add(1382, 960);
            WH_Classes[30662].Add(1383, 1000000000);
            WH_Classes[30662].Add(1384, 0);
            WH_Classes[30662].Add(1385, 20000000);
            WH_Classes[30662].Add(1457, 298);
            WH_Classes[30663].Add(1381, 4);
            WH_Classes[30663].Add(1382, 960);
            WH_Classes[30663].Add(1383, 1000000000);
            WH_Classes[30663].Add(1384, 0);
            WH_Classes[30663].Add(1385, 20000000);
            WH_Classes[30663].Add(1457, 299);
            WH_Classes[30664].Add(1381, 5);
            WH_Classes[30664].Add(1382, 1440);
            WH_Classes[30664].Add(1383, 1000000000);
            WH_Classes[30664].Add(1384, 0);
            WH_Classes[30664].Add(1385, 20000000);
            WH_Classes[30664].Add(1457, 300);
            WH_Classes[30665].Add(1381, 6);
            WH_Classes[30665].Add(1382, 1440);
            WH_Classes[30665].Add(1383, 1000000000);
            WH_Classes[30665].Add(1384, 0);
            WH_Classes[30665].Add(1385, 20000000);
            WH_Classes[30665].Add(1457, 301);
            WH_Classes[30666].Add(1381, 7);
            WH_Classes[30666].Add(1382, 1440);
            WH_Classes[30666].Add(1383, 1000000000);
            WH_Classes[30666].Add(1384, 0);
            WH_Classes[30666].Add(1385, 20000000);
            WH_Classes[30666].Add(1457, 302);
            WH_Classes[30667].Add(1381, 8);
            WH_Classes[30667].Add(1382, 1440);
            WH_Classes[30667].Add(1383, 1000000000);
            WH_Classes[30667].Add(1384, 0);
            WH_Classes[30667].Add(1385, 20000000);
            WH_Classes[30667].Add(1457, 303);
            WH_Classes[30668].Add(1381, 9);
            WH_Classes[30668].Add(1382, 1440);
            WH_Classes[30668].Add(1383, 1000000000);
            WH_Classes[30668].Add(1384, 0);
            WH_Classes[30668].Add(1385, 20000000);
            WH_Classes[30668].Add(1457, 304);
            WH_Classes[30671].Add(1381, 1);
            WH_Classes[30671].Add(1382, 960);
            WH_Classes[30671].Add(1383, 500000000);
            WH_Classes[30671].Add(1384, 0);
            WH_Classes[30671].Add(1385, 20000000);
            WH_Classes[30671].Add(1457, 296);
            WH_Classes[30672].Add(1381, 2);
            WH_Classes[30672].Add(1382, 960);
            WH_Classes[30672].Add(1383, 2000000000);
            WH_Classes[30672].Add(1384, 0);
            WH_Classes[30672].Add(1385, 300000000);
            WH_Classes[30672].Add(1457, 297);
            WH_Classes[30673].Add(1381, 3);
            WH_Classes[30673].Add(1382, 960);
            WH_Classes[30673].Add(1383, 2000000000);
            WH_Classes[30673].Add(1384, 0);
            WH_Classes[30673].Add(1385, 300000000);
            WH_Classes[30673].Add(1457, 298);
            WH_Classes[30674].Add(1381, 4);
            WH_Classes[30674].Add(1382, 960);
            WH_Classes[30674].Add(1383, 2000000000);
            WH_Classes[30674].Add(1384, 0);
            WH_Classes[30674].Add(1385, 300000000);
            WH_Classes[30674].Add(1457, 299);
            WH_Classes[30675].Add(1381, 5);
            WH_Classes[30675].Add(1382, 1440);
            WH_Classes[30675].Add(1383, 3000000000);
            WH_Classes[30675].Add(1384, 0);
            WH_Classes[30675].Add(1385, 300000000);
            WH_Classes[30675].Add(1457, 300);
            WH_Classes[30676].Add(1381, 6);
            WH_Classes[30676].Add(1382, 1440);
            WH_Classes[30676].Add(1383, 3000000000);
            WH_Classes[30676].Add(1384, 0);
            WH_Classes[30676].Add(1385, 300000000);
            WH_Classes[30676].Add(1457, 301);
            WH_Classes[30677].Add(1381, 7);
            WH_Classes[30677].Add(1382, 1440);
            WH_Classes[30677].Add(1383, 2000000000);
            WH_Classes[30677].Add(1384, 0);
            WH_Classes[30677].Add(1385, 300000000);
            WH_Classes[30677].Add(1457, 302);
            WH_Classes[30678].Add(1381, 8);
            WH_Classes[30678].Add(1382, 1440);
            WH_Classes[30678].Add(1383, 2000000000);
            WH_Classes[30678].Add(1384, 0);
            WH_Classes[30678].Add(1385, 300000000);
            WH_Classes[30678].Add(1457, 303);
            WH_Classes[30679].Add(1381, 9);
            WH_Classes[30679].Add(1382, 1440);
            WH_Classes[30679].Add(1383, 2000000000);
            WH_Classes[30679].Add(1384, 0);
            WH_Classes[30679].Add(1385, 300000000);
            WH_Classes[30679].Add(1457, 304);
            WH_Classes[30680].Add(1381, 1);
            WH_Classes[30680].Add(1382, 960);
            WH_Classes[30680].Add(1383, 500000000);
            WH_Classes[30680].Add(1384, 0);
            WH_Classes[30680].Add(1385, 20000000);
            WH_Classes[30680].Add(1457, 296);
            WH_Classes[30681].Add(1381, 2);
            WH_Classes[30681].Add(1382, 960);
            WH_Classes[30681].Add(1383, 2000000000);
            WH_Classes[30681].Add(1384, 0);
            WH_Classes[30681].Add(1385, 300000000);
            WH_Classes[30681].Add(1457, 297);
            WH_Classes[30682].Add(1381, 3);
            WH_Classes[30682].Add(1382, 960);
            WH_Classes[30682].Add(1383, 2000000000);
            WH_Classes[30682].Add(1384, 0);
            WH_Classes[30682].Add(1385, 300000000);
            WH_Classes[30682].Add(1457, 298);
            WH_Classes[30683].Add(1381, 4);
            WH_Classes[30683].Add(1382, 960);
            WH_Classes[30683].Add(1383, 2000000000);
            WH_Classes[30683].Add(1384, 0);
            WH_Classes[30683].Add(1385, 300000000);
            WH_Classes[30683].Add(1457, 299);
            WH_Classes[30684].Add(1381, 5);
            WH_Classes[30684].Add(1382, 1440);
            WH_Classes[30684].Add(1383, 3000000000);
            WH_Classes[30684].Add(1384, 0);
            WH_Classes[30684].Add(1385, 300000000);
            WH_Classes[30684].Add(1457, 300);
            WH_Classes[30685].Add(1381, 6);
            WH_Classes[30685].Add(1382, 1440);
            WH_Classes[30685].Add(1383, 3000000000);
            WH_Classes[30685].Add(1384, 0);
            WH_Classes[30685].Add(1385, 300000000);
            WH_Classes[30685].Add(1457, 301);
            WH_Classes[30686].Add(1381, 7);
            WH_Classes[30686].Add(1382, 1440);
            WH_Classes[30686].Add(1383, 3000000000);
            WH_Classes[30686].Add(1384, 0);
            WH_Classes[30686].Add(1385, 300000000);
            WH_Classes[30686].Add(1457, 302);
            WH_Classes[30687].Add(1381, 8);
            WH_Classes[30687].Add(1382, 1440);
            WH_Classes[30687].Add(1383, 3000000000);
            WH_Classes[30687].Add(1384, 0);
            WH_Classes[30687].Add(1385, 300000000);
            WH_Classes[30687].Add(1457, 303);
            WH_Classes[30688].Add(1381, 9);
            WH_Classes[30688].Add(1382, 1440);
            WH_Classes[30688].Add(1383, 3000000000);
            WH_Classes[30688].Add(1384, 0);
            WH_Classes[30688].Add(1385, 300000000);
            WH_Classes[30688].Add(1457, 304);
            WH_Classes[30689].Add(1381, 1);
            WH_Classes[30689].Add(1382, 960);
            WH_Classes[30689].Add(1383, 500000000);
            WH_Classes[30689].Add(1384, 0);
            WH_Classes[30689].Add(1385, 20000000);
            WH_Classes[30689].Add(1457, 296);
            WH_Classes[30690].Add(1381, 2);
            WH_Classes[30690].Add(1382, 960);
            WH_Classes[30690].Add(1383, 2000000000);
            WH_Classes[30690].Add(1384, 0);
            WH_Classes[30690].Add(1385, 300000000);
            WH_Classes[30690].Add(1457, 297);
            WH_Classes[30691].Add(1381, 3);
            WH_Classes[30691].Add(1382, 960);
            WH_Classes[30691].Add(1383, 2000000000);
            WH_Classes[30691].Add(1384, 0);
            WH_Classes[30691].Add(1385, 300000000);
            WH_Classes[30691].Add(1457, 298);
            WH_Classes[30692].Add(1381, 4);
            WH_Classes[30692].Add(1382, 960);
            WH_Classes[30692].Add(1383, 2000000000);
            WH_Classes[30692].Add(1384, 0);
            WH_Classes[30692].Add(1385, 300000000);
            WH_Classes[30692].Add(1457, 299);
            WH_Classes[30693].Add(1381, 5);
            WH_Classes[30693].Add(1382, 1440);
            WH_Classes[30693].Add(1383, 3000000000);
            WH_Classes[30693].Add(1384, 0);
            WH_Classes[30693].Add(1385, 300000000);
            WH_Classes[30693].Add(1457, 300);
            WH_Classes[30694].Add(1381, 6);
            WH_Classes[30694].Add(1382, 1440);
            WH_Classes[30694].Add(1383, 3000000000);
            WH_Classes[30694].Add(1384, 0);
            WH_Classes[30694].Add(1385, 300000000);
            WH_Classes[30694].Add(1457, 301);
            WH_Classes[30695].Add(1381, 7);
            WH_Classes[30695].Add(1382, 1440);
            WH_Classes[30695].Add(1383, 5000000000);
            WH_Classes[30695].Add(1384, 0);
            WH_Classes[30695].Add(1385, 300000000);
            WH_Classes[30695].Add(1457, 302);
            WH_Classes[30696].Add(1381, 8);
            WH_Classes[30696].Add(1382, 1440);
            WH_Classes[30696].Add(1383, 3000000000);
            WH_Classes[30696].Add(1384, 0);
            WH_Classes[30696].Add(1385, 1350000000);
            WH_Classes[30696].Add(1457, 303);
            WH_Classes[30697].Add(1381, 9);
            WH_Classes[30697].Add(1382, 1440);
            WH_Classes[30697].Add(1383, 5000000000);
            WH_Classes[30697].Add(1384, 0);
            WH_Classes[30697].Add(1385, 1800000000);
            WH_Classes[30697].Add(1457, 304);
            WH_Classes[30698].Add(1381, 1);
            WH_Classes[30698].Add(1382, 960);
            WH_Classes[30698].Add(1383, 500000000);
            WH_Classes[30698].Add(1384, 0);
            WH_Classes[30698].Add(1385, 20000000);
            WH_Classes[30698].Add(1457, 296);
            WH_Classes[30699].Add(1381, 2);
            WH_Classes[30699].Add(1382, 960);
            WH_Classes[30699].Add(1383, 1000000000);
            WH_Classes[30699].Add(1384, 0);
            WH_Classes[30699].Add(1385, 300000000);
            WH_Classes[30699].Add(1457, 297);
            WH_Classes[30700].Add(1381, 3);
            WH_Classes[30700].Add(1382, 960);
            WH_Classes[30700].Add(1383, 1000000000);
            WH_Classes[30700].Add(1384, 0);
            WH_Classes[30700].Add(1385, 300000000);
            WH_Classes[30700].Add(1457, 298);
            WH_Classes[30701].Add(1381, 4);
            WH_Classes[30701].Add(1382, 960);
            WH_Classes[30701].Add(1383, 2000000000);
            WH_Classes[30701].Add(1384, 0);
            WH_Classes[30701].Add(1385, 300000000);
            WH_Classes[30701].Add(1457, 299);
            WH_Classes[30702].Add(1381, 5);
            WH_Classes[30702].Add(1382, 1440);
            WH_Classes[30702].Add(1383, 3000000000);
            WH_Classes[30702].Add(1384, 0);
            WH_Classes[30702].Add(1385, 1350000000);
            WH_Classes[30702].Add(1457, 300);
            WH_Classes[30703].Add(1381, 6);
            WH_Classes[30703].Add(1382, 1440);
            WH_Classes[30703].Add(1383, 3000000000);
            WH_Classes[30703].Add(1384, 0);
            WH_Classes[30703].Add(1385, 1350000000);
            WH_Classes[30703].Add(1457, 301);
            WH_Classes[30704].Add(1381, 7);
            WH_Classes[30704].Add(1382, 1440);
            WH_Classes[30704].Add(1383, 3000000000);
            WH_Classes[30704].Add(1384, 0);
            WH_Classes[30704].Add(1385, 1000000000);
            WH_Classes[30704].Add(1457, 302);
            WH_Classes[30705].Add(1381, 8);
            WH_Classes[30705].Add(1382, 1440);
            WH_Classes[30705].Add(1383, 3000000000);
            WH_Classes[30705].Add(1384, 0);
            WH_Classes[30705].Add(1385, 1350000000);
            WH_Classes[30705].Add(1457, 303);
            WH_Classes[30706].Add(1381, 9);
            WH_Classes[30706].Add(1382, 1440);
            WH_Classes[30706].Add(1383, 3000000000);
            WH_Classes[30706].Add(1384, 0);
            WH_Classes[30706].Add(1385, 1350000000);
            WH_Classes[30706].Add(1457, 304);
            WH_Classes[30707].Add(1381, 1);
            WH_Classes[30707].Add(1382, 960);
            WH_Classes[30707].Add(1383, 500000000);
            WH_Classes[30707].Add(1384, 0);
            WH_Classes[30707].Add(1385, 20000000);
            WH_Classes[30707].Add(1457, 296);
            WH_Classes[30708].Add(1381, 2);
            WH_Classes[30708].Add(1382, 960);
            WH_Classes[30708].Add(1383, 2000000000);
            WH_Classes[30708].Add(1384, 0);
            WH_Classes[30708].Add(1385, 300000000);
            WH_Classes[30708].Add(1457, 297);
            WH_Classes[30709].Add(1381, 3);
            WH_Classes[30709].Add(1382, 960);
            WH_Classes[30709].Add(1383, 2000000000);
            WH_Classes[30709].Add(1384, 0);
            WH_Classes[30709].Add(1385, 300000000);
            WH_Classes[30709].Add(1457, 298);
            WH_Classes[30710].Add(1381, 4);
            WH_Classes[30710].Add(1382, 960);
            WH_Classes[30710].Add(1383, 2000000000);
            WH_Classes[30710].Add(1384, 0);
            WH_Classes[30710].Add(1385, 300000000);
            WH_Classes[30710].Add(1457, 299);
            WH_Classes[30711].Add(1381, 5);
            WH_Classes[30711].Add(1382, 1440);
            WH_Classes[30711].Add(1383, 3000000000);
            WH_Classes[30711].Add(1384, 0);
            WH_Classes[30711].Add(1385, 1350000000);
            WH_Classes[30711].Add(1457, 300);
            WH_Classes[30712].Add(1381, 6);
            WH_Classes[30712].Add(1382, 1440);
            WH_Classes[30712].Add(1383, 3000000000);
            WH_Classes[30712].Add(1384, 0);
            WH_Classes[30712].Add(1385, 1350000000);
            WH_Classes[30712].Add(1457, 301);
            WH_Classes[30713].Add(1381, 7);
            WH_Classes[30713].Add(1382, 1440);
            WH_Classes[30713].Add(1383, 5000000000);
            WH_Classes[30713].Add(1384, 0);
            WH_Classes[30713].Add(1385, 300000000);
            WH_Classes[30713].Add(1457, 302);
            WH_Classes[30714].Add(1381, 8);
            WH_Classes[30714].Add(1382, 1440);
            WH_Classes[30714].Add(1383, 5000000000);
            WH_Classes[30714].Add(1384, 0);
            WH_Classes[30714].Add(1385, 1800000000);
            WH_Classes[30714].Add(1457, 303);
            WH_Classes[30715].Add(1381, 9);
            WH_Classes[30715].Add(1382, 1440);
            WH_Classes[30715].Add(1383, 5000000000);
            WH_Classes[30715].Add(1384, 0);
            WH_Classes[30715].Add(1385, 1800000000);
            WH_Classes[30715].Add(1457, 304);
        }

        public void LoadIceAndOreData()
        {
            string strSQL;
            DataSet OreData;
            string oreName;

            OreTypes = new ArrayList();
            IceTypes = new ArrayList();
            strSQL = "SELECT * FROM invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID WHERE invGroups.categoryID=25 AND invGroups.published=1 AND invTypes.published=1;";
            OreData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!OreData.Equals(System.DBNull.Value))
            {
                if (OreData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in OreData.Tables[0].Rows)
                    {
                        switch (Convert.ToInt32(row.ItemArray[0].ToString()))
                        {
                            case 465:
                                oreName = row.ItemArray[14].ToString();

                                if(!oreName.Contains("Compressed"))
                                    IceTypes.Add(oreName);
                                break;
                            case 519:
                            case 903:
                                break;
                            default:
                                oreName = row.ItemArray[14].ToString();
                                if (!oreName.Contains("Compressed"))
                                {
                                    if (Convert.ToInt32(row.ItemArray[12].ToString()) < 28617)
                                        OreTypes.Add(oreName);
                                }
                                break;
                        }
                    }
                }
            }
        }

        public void LoadFactionData()
        {
            string strSQL;
            DataSet FactionData;
            int facID;
            string facName;
            
            strSQL = "SELECT * FROM chrFactions ORDER BY factionID;";
            FactionData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!FactionData.Equals(System.DBNull.Value))
            {
                if (FactionData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in FactionData.Tables[0].Rows)
                    {
                        facID = Convert.ToInt32(row.ItemArray[0]);
                        facName = row.ItemArray[1].ToString();

                        Factions.Add(facID, facName);
                    }
                }
            }
        }

        public void LoadCorporationData()
        {
            string strSQL;
            DataSet corpData;
            Corporation corp;

            strSQL = "SELECT * FROM crpNPCCorporations INNER JOIN eveNames ON crpNPCCorporations.corporationID = eveNames.itemID ORDER BY corporationID;";
            corpData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!corpData.Equals(System.DBNull.Value))
            {
                if (corpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in corpData.Tables[0].Rows)
                    {
                        corp = new Corporation();
                        corp.ID = Convert.ToInt32(row.ItemArray[0]);
                        corp.name = row.ItemArray[29].ToString();
                        corp.facID = Convert.ToInt32(row.ItemArray[22]);
                        corp.desc = row.ItemArray[26].ToString();
                        corp.systemID = Convert.ToInt32(row.ItemArray[3]);

                        Corporations.Add(corp.ID, corp);
                    }
                }
            }
        }

        public void LoadDivisionData()
        {
            string strSQL;
            DataSet divData;
            int divID;
            string divName;

            strSQL = "SELECT * FROM crpNPCDivisions ORDER BY divisionID;";
            divData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!divData.Equals(System.DBNull.Value))
            {
                if (divData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in divData.Tables[0].Rows)
                    {
                        divID = Convert.ToInt32(row.ItemArray[0]);
                        divName = row.ItemArray[1].ToString();

                        Divisions.Add(divID, divName);
                    }
                }
            }
        }

        public void LoadTowerData()
        {
            string strSQL;
            DataSet divData;
            string twrName;

            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=365 AND invTypes.published=1 ORDER BY invTypes.typeName;";
            divData = EveHQ.Core.DataFunctions.GetData(strSQL);

            TowerTypes.Clear();
            if (!divData.Equals(System.DBNull.Value))
            {
                if (divData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in divData.Tables[0].Rows)
                    {
                        twrName = row.ItemArray[2].ToString();
                        TowerTypes.Add(twrName);
                    }
                }
            }
        }

        public void LoadMoonGooData()
        {
            string strSQL;
            DataSet divData;
            string gooName;

            strSQL = "SELECT * FROM invTypes WHERE invTypes.groupID=427 AND invTypes.published=1 ORDER BY invTypes.typeName;";
            divData = EveHQ.Core.DataFunctions.GetData(strSQL);

            GooTypes.Clear();
            if (!divData.Equals(System.DBNull.Value))
            {
                if (divData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in divData.Tables[0].Rows)
                    {
                        gooName = row.ItemArray[2].ToString();
                        GooTypes.Add(gooName);
                    }
                }
            }
        }

        public bool SaveMisc()
        {
            string fname;

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Factions.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Factions);
            pStream.Close();

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Corps.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Corporations);
            pStream.Close();

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Divisions.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Divisions);
            pStream.Close();

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Ores.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, OreTypes);
            pStream.Close();

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Ices.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, IceTypes);
            pStream.Close();

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_PCorp.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, PlayerCorpList);
            pStream.Close();

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Towers.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, TowerTypes);
            pStream.Close();

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_MoonGoo.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, GooTypes);
            pStream.Close();

            return true;
        }

        public bool LoadMisc()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            LoadWHAttributes();
            LoadWHNm2Class();
            SetupWHExpStuff();
            LoadWHRegionToClass();

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Factions.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Factions = (SortedList<int, string>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Corps.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Corporations = (SortedList<int, Corporation>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Divisions.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Divisions = (SortedList<int, string>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Ores.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    OreTypes = (ArrayList)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Ices.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    IceTypes = (ArrayList)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_PCorp.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    PlayerCorpList = (ArrayList)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_Towers.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    TowerTypes = (ArrayList)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_MoonGoo.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    GooTypes = (ArrayList)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_PilotShip.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    PilotShipSave = (SortedList<string, Ship>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }

            return true;
        }

        public bool SavePSMisc()
        {
            string fname;

            fname = Path.Combine(PlugInData.RMapData_Path, "DB_PilotShip.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, PilotShipSave);
            pStream.Close();

            return true;
        }


    }
}
