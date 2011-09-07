// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
// 
// This file is part of the "EveHQ RouteMap" plug-in
//
// EveHQ RouteMap derives from copyrighted works licensed under the GNU 
// General Public License originally created by Lhyda Souljacker.
//
// This version has been modified pursuant to the GNU General Public 
// License as of 4 September 2011, and as distributed, it includes or is 
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
    public class Sov_API
    {
        public Dictionary<int, Sov_Data> SovList;
        private bool GotError = false;

        public Sov_API()
        {
            SovList = new Dictionary<int, Sov_Data>();
        }

        public void LoadSovListFromDisk()
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

            fname = Path.Combine(RMapData_Path, "API_SovList.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    SovList = (Dictionary<int, Sov_Data>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }
        }

        public void SaveSovListing()
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

            fname = Path.Combine(RMapData_Path, "API_SovList.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, SovList);
            pStream.Close();

            return;
        }

        public void LoadSovListFromAPI(Object o)
        {
            LoadSovListFromDisk();
            LoadSovListDataFromAPI();
            SaveSovListing();
            //PlugInData.resetEvents[3].Set();
        }

        private bool IsSovDataTimestampCurrent(string cacheUntil)
        {
            string curDate;
            DateTime cd, nd;
            TimeSpan dd;
            double secDif;
            Sov_Data ap;

            if (SovList.Count <= 0)
                return false;

            ap = (Sov_Data)SovList.ElementAt(0).Value;

            if (ap == null)
                return false;

            curDate = ap.cacheUntil;
            if (curDate == "")
                return false;

            cd = Convert.ToDateTime(curDate);
            nd = Convert.ToDateTime(cacheUntil);
            dd = cd.Subtract(nd);
            secDif = dd.TotalSeconds;

            if (secDif >= 0)
                return true;
            else
                return false;
        }

        public Dictionary<int, Sov_Data> GetSovSystemFromName(string name)
        {
            IEnumerable<KeyValuePair<int, Sov_Data>> SovSystem = from solarSystem in SovList
                                                                 where solarSystem.Value.systemName.Equals(name)
                                                                 select solarSystem;
            return SovSystem.ToDictionary(n => n.Value.systemID, n => n.Value);
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

        private void LoadSovListDataFromAPI()
        {
            XmlDocument sovData;
            XmlNodeList svList, dateList;
            Sov_Data sd;
            string cacheDate, cacheUntil;
            int systemID;

            sovData = new XmlDocument();
            // When a tower gets linked to the API and vice versa, the towerItemID will be
            // stored in the POS data itself. This will allow for easy import of the fuel data.
            EveAPI.EveAPIRequest APIReq;
            APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
            sovData = APIReq.GetAPIXML(EveAPI.APITypes.Sovereignty, EveAPI.APIReturnMethods.ReturnStandard);

            if (APIReq.LastAPIError > 0)
            {
                if (!PlugInData.ThrottleList.ContainsKey("SovList"))
                {
                    PlugInData.ThrottleList.Add("SovList", new PlugInData.throttlePlayer(1));
                }
                else
                {
                    PlugInData.ThrottleList["SovList"].IncCount();
                }
                PlugInData.LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, "SovList");
            }

            if ((sovData == null) || !CheckXML(sovData))
            {
                if (!GotError)
                {
                    MessageBox.Show("An Error was encountered while Parsing Sov API Data, Will continue without the Data.", "PoSManager: API Error", MessageBoxButtons.OK);
                    GotError = true;
                }
                return;
            }

            dateList = sovData.SelectNodes("/eveapi");

            svList = sovData.SelectNodes("/eveapi/result/rowset/row");

            cacheDate = dateList[0].ChildNodes[0].InnerText;
            cacheUntil = dateList[0].ChildNodes[2].InnerText;

            if (IsSovDataTimestampCurrent(cacheUntil))
            {
                // Nothing new to load at all
                return;
            }

            try
            {
                foreach (XmlNode syst in svList)
                {
                    systemID = Convert.ToInt32(syst.Attributes.GetNamedItem("solarSystemID").Value.ToString());

                    if (!SovList.ContainsKey(systemID))
                    {
                        sd = new Sov_Data();
                        sd.systemName = syst.Attributes.GetNamedItem("solarSystemName").Value.ToString();
                        sd.systemID = Convert.ToInt32(syst.Attributes.GetNamedItem("solarSystemID").Value.ToString());
                        sd.allianceID = Convert.ToInt32(syst.Attributes.GetNamedItem("allianceID").Value.ToString());
                        sd.factionID = Convert.ToInt32(syst.Attributes.GetNamedItem("factionID").Value.ToString());
                        sd.corpID = Convert.ToInt32(syst.Attributes.GetNamedItem("corporationID").Value.ToString());
                        sd.cacheDate = cacheDate;
                        sd.cacheUntil = cacheUntil;
                        SovList.Add(systemID, sd);
                    }
                    else
                    {
                        SovList[systemID].systemID = Convert.ToInt32(syst.Attributes.GetNamedItem("solarSystemID").Value.ToString());
                        SovList[systemID].allianceID = Convert.ToInt32(syst.Attributes.GetNamedItem("allianceID").Value.ToString());
                        SovList[systemID].factionID = Convert.ToInt32(syst.Attributes.GetNamedItem("factionID").Value.ToString());
                        SovList[systemID].corpID = Convert.ToInt32(syst.Attributes.GetNamedItem("corporationID").Value.ToString());
                        SovList[systemID].cacheDate = cacheDate;
                        SovList[systemID].cacheUntil = cacheUntil;
                    }
                }
            }
            catch
            {
                if (!GotError)
                {
                    DialogResult dr = MessageBox.Show("An Error was encountered while Parsing System Sov API Data.", "PoSManager: API Error", MessageBoxButtons.OK);
                    GotError = true;
                }
            }

        }
    }
}
