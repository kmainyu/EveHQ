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
    public class AllianceList
    {
        public SortedList alliances;
        public bool GotError = false;

        public AllianceList()
        {
            alliances = new SortedList();
        }

        public void SaveAllianceListing()
        {
            string fname;

            fname = Path.Combine(PlugInData.PoSCache_Path, "API_Alliance.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, alliances);
            pStream.Close();
        }

        public void LoadAllianceListFromAPI(Object o)
        {
            LoadAllianceListFromDisk();
            LoadAllianceDataFromAPI();
            SaveAllianceListing();
            PlugInData.resetEvents[4].Set();
        }

        public void LoadAllianceListFromDisk()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PlugInData.PoSCache_Path, "API_Alliance.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    alliances = (SortedList)myBf.Deserialize(cStr);
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

            if (alliances.Count <= 0)
                return false;

            ap = (Alliance_Data)alliances.GetByIndex(0);

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

            allianceData = new XmlDocument();
            // When a tower gets linked to the API and vice versa, the towerItemID will be
            // stored in the POS data itself. This will allow for easy import of the fuel data.
            try
            {
                EveAPI.EveAPIRequest APIReq;
                APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
                allianceData = APIReq.GetAPIXML(EveAPI.APITypes.AllianceList, EveAPI.APIReturnMethods.ReturnStandard);

                if ((APIReq.LastAPIError > 0) && (APIReq.LastAPIResult != EveAPI.APIResults.ReturnedCached))
                {
                    if (!PlugInData.ThrottleList.ContainsKey("AllianceList"))
                    {
                        PlugInData.ThrottleList.Add("AllianceList", new PlugInData.throttlePlayer(1));
                    }
                    else
                    {
                        PlugInData.ThrottleList["AllianceList"].IncCount();
                    }
                    PlugInData.LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, "AllianceList");
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
                alliances.Clear();

                foreach (XmlNode ally in allyList)
                {
                    ad = new Alliance_Data();

                    ad.allianceID = Convert.ToDecimal(ally.Attributes.GetNamedItem("allianceID").Value.ToString());
                    ad.execCorpID = Convert.ToDecimal(ally.Attributes.GetNamedItem("executorCorpID").Value.ToString());
                    ad.members = Convert.ToDecimal(ally.Attributes.GetNamedItem("memberCount").Value.ToString());
                    ad.name = ally.Attributes.GetNamedItem("name").Value.ToString();
                    ad.ticker = ally.Attributes.GetNamedItem("shortName").Value.ToString();
                    ad.cacheDate = cacheDate;
                    ad.cacheUntil = cacheUntil;
                    ad.startDate = ally.Attributes.GetNamedItem("startDate").Value.ToString();

                    corpList = ally.ChildNodes[0].ChildNodes;

                    ad.corps.Clear();

                    foreach (XmlNode cid in corpList)
                    {
                        corpID = Convert.ToDecimal(cid.Attributes.GetNamedItem("corporationID").Value.ToString());
                        ad.corps.Add(corpID);
                        alliances.Add(corpID, ad);
                    }
                }
            }
            catch
            {
                if (!GotError)
                {
                    DialogResult dr = MessageBox.Show("An Error was encountered while Parsing Alliance API Data.", "PoSManager: API Error", MessageBoxButtons.OK);
                    GotError = true;
                }
            }
        }

    }
}
