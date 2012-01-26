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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace EveHQ.RouteMap
{
    [Serializable]
    public class Alliance_API
    {
        public Dictionary<int, Alliance_Data> Alliances;

        public Alliance_API()
        {
            Alliances = new Dictionary<int, Alliance_Data>();
        }

        public void SaveAllianceListing()
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
            RMapData_Path = Path.Combine(RMapManage_Path, "API");

            if (!Directory.Exists(RMapManage_Path))
                Directory.CreateDirectory(RMapManage_Path);

            if (!Directory.Exists(RMapData_Path))
                Directory.CreateDirectory(RMapData_Path);

            fname = Path.Combine(RMapData_Path, "API_Alliance.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Alliances);
            pStream.Close();

            return;
        }

        public void LoadAllianceListFromAPI(Object o)
        {
            LoadAllianceListFromDisk();
            LoadAllianceDataFromAPI();
            SaveAllianceListing();
            //PlugInData.resetEvents[4].Set();
        }

        public void LoadAllianceListFromDisk()
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
            RMapData_Path = Path.Combine(RMapManage_Path, "API");

            if (!Directory.Exists(RMapManage_Path))
                Directory.CreateDirectory(RMapManage_Path);

            if (!Directory.Exists(RMapData_Path))
                Directory.CreateDirectory(RMapData_Path);

            fname = Path.Combine(RMapData_Path, "API_Alliance.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Alliances = (Dictionary<int, Alliance_Data>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }
        }

        private bool IsAllianceDataTimestampCurrent(string cacheUntil)
        {
            string curDate;
            DateTime cd, nd;
            TimeSpan dd;
            double secDif;
            Alliance_Data ap;

            if (Alliances.Count <= 0)
                return false;

            ap = (Alliance_Data)Alliances.ElementAt(1).Value;

            if (ap == null)
                return false;

            curDate = ap.cacheUntil;

            cd = Convert.ToDateTime(curDate);
            nd = Convert.ToDateTime(cacheUntil);
            dd = cd.Subtract(nd);
            secDif = dd.TotalSeconds;

            if (secDif >= 0)
                return true;
            else
                return false;
        }

        private bool CheckXML(XmlDocument apiXML)
        {
            if (apiXML != null)
            {
                XmlNodeList errlist = apiXML.SelectNodes("/eveapi/error");
                if (errlist.Count != 0)
                {
                    // Error - Now WTF do I do???
                    return false;
                }
            }
            return true;
        }

        private void LoadAllianceDataFromAPI()
        {
            XmlDocument allianceData;
            XmlNodeList allyList, dateList, corpList;
            Alliance_Data ad;
            string cacheDate, cacheUntil;
            decimal corpID;
            int count = 0;
            List<Color> Rcolors;

            Rcolors = EveGalaxy.GenerateRandomizedColors();

            allianceData = new XmlDocument();
            // When a tower gets linked to the API and vice versa, the towerItemID will be
            // stored in the POS data itself. This will allow for easy import of the fuel data.
            try
            {
                EveAPI.EveAPIRequest APIReq;
                APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
                allianceData = APIReq.GetAPIXML(EveAPI.APITypes.AllianceList, EveAPI.APIReturnMethods.ReturnStandard);

                if (APIReq.LastAPIError > 0)
                {
                    if (!PlugInData.ThrottleList.ContainsKey("Alliance List"))
                    {
                        PlugInData.ThrottleList.Add("Alliance List", new PlugInData.throttlePlayer(1));
                    }
                    else
                    {
                        PlugInData.ThrottleList["Alliance List"].IncCount();
                    }
                    PlugInData.LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, "Alliance List");
                }

                if (!CheckXML(allianceData))
                    return;

                dateList = allianceData.SelectNodes("/eveapi");

                allyList = allianceData.SelectNodes("/eveapi/result/rowset/row");

                cacheDate = dateList[0].ChildNodes[0].InnerText;
                cacheUntil = dateList[0].ChildNodes[2].InnerText;

                if (IsAllianceDataTimestampCurrent(cacheUntil))
                {
                    // Nothing new to load at all
                    return;
                }
                Alliances.Clear();

                foreach (XmlNode ally in allyList)
                {
                    ad = new Alliance_Data();

                    ad.allianceID = Convert.ToInt32(ally.Attributes.GetNamedItem("allianceID").Value.ToString());
                    ad.execCorpID = Convert.ToInt32(ally.Attributes.GetNamedItem("executorCorpID").Value.ToString());
                    ad.members = Convert.ToInt32(ally.Attributes.GetNamedItem("memberCount").Value.ToString());
                    ad.name = ally.Attributes.GetNamedItem("name").Value.ToString();
                    ad.ticker = ally.Attributes.GetNamedItem("shortName").Value.ToString();
                    ad.cacheDate = cacheDate;
                    ad.cacheUntil = cacheUntil;
                    ad.startDate = ally.Attributes.GetNamedItem("startDate").Value.ToString();
                    if (count >= Rcolors.Count)
                        count = 0;
                    ad.AColor = Rcolors[count];
                    count++;
                    corpList = ally.ChildNodes[0].ChildNodes;

                    ad.corps.Clear();

                    foreach (XmlNode cid in corpList)
                    {
                        corpID = Convert.ToDecimal(cid.Attributes.GetNamedItem("corporationID").Value.ToString());
                        ad.corps.Add(corpID);
                    }

                    Alliances.Add(ad.allianceID, ad);
                }
            }
            catch
            {
                DialogResult dr = MessageBox.Show("An Error was encountered while Parsing Alliance API Data.", "PoSManager: API Error", MessageBoxButtons.OK);
            }
        }

    }
}
