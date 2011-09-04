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
    public class API_Jump_Kill
    {
        public SortedList<DateTime, SortedList<int, long>> Jumps;
        public SortedList<DateTime, SortedList<int, ArrayList>> Kills;
        public int MaxToTrack;
        public DateTime TimeForNewJump, TimeForNewKill;

        public API_Jump_Kill()
        {
            Jumps = new SortedList<DateTime, SortedList<int, long>>();
            Kills = new SortedList<DateTime, SortedList<int, ArrayList>>();
            MaxToTrack = 59;
            TimeForNewJump = DateTime.Now;
            TimeForNewKill = DateTime.Now;
        }

        public ArrayList GetLatestJumpKillForSID(int sysID)
        {
            ArrayList retList = new ArrayList();

            if (Jumps.Count <= 0)
                retList.Add(0);
            else if (Jumps.ElementAt(Jumps.Count - 1).Value.ContainsKey(sysID))
                retList.Add(Jumps.ElementAt(Jumps.Count - 1).Value[sysID]);
            else
                retList.Add(0);

            if (Kills.Count <= 0)
            {
                retList.Add(0);
                retList.Add(0);
                retList.Add(0);
            }
            else if (Kills.ElementAt(Kills.Count - 1).Value.ContainsKey(sysID))
            {
                foreach (long lv in Kills.ElementAt(Kills.Count - 1).Value[sysID])
                    retList.Add(lv);
            }
            else
            {
                retList.Add(0);
                retList.Add(0);
                retList.Add(0);
            }

            return retList;
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
                return true;
            }
            else
                return false;
        }

        public void CheckAndReadInNewAPIData()
        {
            ReadInNewJumpAPI();
            ReadInNewKillAPI();
        }

        public void TrimJumpList()
        {
            while (Jumps.Count > MaxToTrack)
                Jumps.RemoveAt(0);
        }

        public void TrimKillsList()
        {
            while (Kills.Count > MaxToTrack)
                Kills.RemoveAt(0);
        }

        public void ReadInNewJumpAPI()
        {
            XmlDocument jumpData;
            XmlNodeList sysList, dateList, dataList;
            string cacheDate, cacheUntil, dataDate;
            TimeSpan apiC;
            DateTime dataT, cacheT;
            int sysID;
            long jmp;

            apiC = DateTime.Now.Subtract(TimeForNewJump);
            if (apiC.TotalSeconds < 0)
                return;

            jumpData = new XmlDocument();

            EveAPI.EveAPIRequest APIReq;
            APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
            jumpData = APIReq.GetAPIXML(EveAPI.APITypes.MapJumps, EveAPI.APIReturnMethods.ReturnStandard);

            if (APIReq.LastAPIError != 0)
            {
                if (!PlugInData.ThrottleList.ContainsKey("Jumps Data"))
                {
                    PlugInData.ThrottleList.Add("Jumps Data", new PlugInData.throttlePlayer(1));
                }
                else
                {
                    PlugInData.ThrottleList["Jumps Data"].IncCount();
                }
                PlugInData.LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, "Jumps Data");
            }

            if (!CheckXML(jumpData))
                return;

            TrimJumpList();
            dateList = jumpData.SelectNodes("/eveapi");
            sysList = jumpData.SelectNodes("/eveapi/result/rowset/row");
            dataList = jumpData.SelectNodes("/eveapi/result");

            cacheDate = dateList[0].ChildNodes[0].InnerText;
            cacheUntil = dateList[0].ChildNodes[2].InnerText;
            dataDate = dataList[0].ChildNodes[1].InnerText;

            cacheT = Convert.ToDateTime(cacheUntil);
            cacheT = cacheT.ToLocalTime();

            dataT = Convert.ToDateTime(dataDate);
            dataT = dataT.ToLocalTime();

            if (!Jumps.ContainsKey(dataT))
            {
                // New data exists, read it in man
                Jumps.Add(dataT, new SortedList<int, long>());
                try
                {
                    // Create my new data !
                    foreach (XmlNode syst in sysList)
                    {
                        sysID = Convert.ToInt32(syst.Attributes.GetNamedItem("solarSystemID").Value.ToString());
                        jmp = Convert.ToInt32(syst.Attributes.GetNamedItem("shipJumps").Value.ToString());
                        
                        // There will only be one per read of a new time
                        Jumps[dataT].Add(sysID, jmp);
                    }
                    TimeForNewJump = cacheT;
                }
                catch
                {
                    DialogResult dr = MessageBox.Show("An Error was encountered while Parsing System Jumps Data.", "RouteMap: API Error", MessageBoxButtons.OK);
                }
            }
        }

        public void ReadInNewKillAPI()
        {
            XmlDocument jumpData;
            XmlNodeList sysList, dateList, dataList;
            string cacheDate, cacheUntil, dataDate;
            TimeSpan apiC;
            DateTime dataT, cacheT;
            int sysID;
            long sk, pk, fk;

            apiC = DateTime.Now.Subtract(TimeForNewKill);
            if (apiC.TotalSeconds < 0)
                return;

            jumpData = new XmlDocument();

            EveAPI.EveAPIRequest APIReq;
            APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
            jumpData = APIReq.GetAPIXML(EveAPI.APITypes.MapKills, EveAPI.APIReturnMethods.ReturnStandard);

            if (APIReq.LastAPIError != 0)
            {
                if (!PlugInData.ThrottleList.ContainsKey("Kills Data"))
                {
                    PlugInData.ThrottleList.Add("Kills Data", new PlugInData.throttlePlayer(1));
                }
                else
                {
                    PlugInData.ThrottleList["Kills Data"].IncCount();
                }
                PlugInData.LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, "Kills Data");
            }

            if (!CheckXML(jumpData))
                return;

             TrimKillsList();
            dateList = jumpData.SelectNodes("/eveapi");
            sysList = jumpData.SelectNodes("/eveapi/result/rowset/row");
            dataList = jumpData.SelectNodes("/eveapi/result");

            cacheDate = dateList[0].ChildNodes[0].InnerText;
            cacheUntil = dateList[0].ChildNodes[2].InnerText;
            dataDate = dataList[0].ChildNodes[1].InnerText;

            cacheT = Convert.ToDateTime(cacheUntil);
            cacheT = cacheT.ToLocalTime();

            dataT = Convert.ToDateTime(dataDate);
            dataT = dataT.ToLocalTime();

            if (!Kills.ContainsKey(dataT))
            {
                // New data exists, read it in man
                Kills.Add(dataT, new SortedList<int, ArrayList>());
                try
                {
                    // Create my new data !
                    foreach (XmlNode syst in sysList)
                    {
                        sysID = Convert.ToInt32(syst.Attributes.GetNamedItem("solarSystemID").Value.ToString());
                        fk = Convert.ToInt32(syst.Attributes.GetNamedItem("factionKills").Value.ToString());
                        sk = Convert.ToInt32(syst.Attributes.GetNamedItem("shipKills").Value.ToString());
                        pk = Convert.ToInt32(syst.Attributes.GetNamedItem("podKills").Value.ToString());

                        // There will only be one per read of a new time
                        Kills[dataT].Add(sysID, new ArrayList());
                        Kills[dataT][sysID].Add(sk);
                        Kills[dataT][sysID].Add(pk);
                        Kills[dataT][sysID].Add(fk);
                    }
                    TimeForNewKill = cacheT;
                }
                catch
                {
                    DialogResult dr = MessageBox.Show("An Error was encountered while Parsing System Kills Data.", "RouteMap: API Error", MessageBoxButtons.OK);
                }
            }
        }

    }
}
