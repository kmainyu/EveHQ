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

namespace EveHQ.RouteMap
{
    [Serializable]
    public class ConqStationList
    {
        public Dictionary<int, SolarSystem> Systems;
        public string cacheDate, cacheUntil;

        public ConqStationList()
        {
            Systems = new Dictionary<int,SolarSystem>();
            cacheDate = "";
            cacheUntil = "";
        }

        public void UpdateConqStationsData()
        {
            LoadCStationsFromDisk();
            GetConqStationsFromAPI();
            SaveCStationListing();
        }

        private bool IsAPITimestampCurrent(string curDate)
        {
            DateTime cd, nd;
            TimeSpan dd;
            double secDif;

            if (Systems.Count <= 0)
                return false;

            if (cacheUntil == "")
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

        public void GetConqStationsFromAPI()
        {
            XmlDocument stnData;
            XmlNodeList cqList, dateList;
            ConqStation cs;

            stnData = new XmlDocument();
            EveAPI.EveAPIRequest APIReq;
            APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
            stnData = APIReq.GetAPIXML(EveAPI.APITypes.Conquerables, EveAPI.APIReturnMethods.ReturnStandard);

            if (APIReq.LastAPIError > 0)
            {
                if (!PlugInData.ThrottleList.ContainsKey("ConqStations"))
                {
                    PlugInData.ThrottleList.Add("ConqStations", new PlugInData.throttlePlayer(1));
                }
                else
                {
                    PlugInData.ThrottleList["ConqStations"].IncCount();
                }
                PlugInData.LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, "ConqStations");
            }

            if (!CheckXML(stnData))
                return;

            // Get conquerable station list API information
            // Parse API data into my stuff
            // columns="stationID,stationName,stationTypeID,solarSystemID,corporationID,corporationName"
            try
            {
                dateList = stnData.SelectNodes("/eveapi");

                if (IsAPITimestampCurrent(dateList[0].ChildNodes[2].InnerText))
                    return;

                cacheDate = dateList[0].ChildNodes[0].InnerText;
                cacheUntil = dateList[0].ChildNodes[2].InnerText;

                cqList = stnData.SelectNodes("/eveapi/result/rowset/row");

                Systems.Clear();
                foreach (XmlNode stn in cqList)
                {
                    cs = new ConqStation();

                    cs.ID = Convert.ToInt32(stn.Attributes.GetNamedItem("stationID").Value.ToString());
                    cs.Name = stn.Attributes.GetNamedItem("stationName").Value.ToString();
                    cs.StnTypeID = Convert.ToInt32(stn.Attributes.GetNamedItem("stationTypeID").Value.ToString());
                    cs.SSysID = Convert.ToInt32(stn.Attributes.GetNamedItem("solarSystemID").Value.ToString());
                    cs.CorpID = Convert.ToInt32(stn.Attributes.GetNamedItem("corporationID").Value.ToString());
                    cs.CorpName = stn.Attributes.GetNamedItem("corporationName").Value.ToString();

                    cs.Cloning = true;
                    cs.Services.Add("Cloning");
                    cs.Services.Add("Fitting");
                    cs.Services.Add("Insurance");
                    
                    switch (cs.StnTypeID)
                    {
                        case 21644:
                            cs.Factory = true;
                            cs.Services.Add("Factory");
                            break;
                        case 21646:
                            cs.Refine = 0.30;
                            cs.Services.Add("Refining");
                            cs.Services.Add("Reprocessing");
                            break;
                        case 21642:
                            cs.Labs = true;
                            cs.Services.Add("Laboratory");
                         break;
                        case 12294:
                            cs.Refine = 0.50;
                            cs.Services.Add("Refining");
                            cs.Services.Add("Reprocessing");
                            break;
                        case 12242:
                            cs.Factory = true;
                            cs.Labs = true;
                            cs.Services.Add("Factory");
                            cs.Services.Add("Laboratory");
                            break;
                        case 12295:
                            cs.Factory = true;
                            cs.Labs = true;
                            cs.Services.Add("Factory");
                            cs.Services.Add("Laboratory");
                            break;
                    }

                    if (!Systems.ContainsKey(cs.SSysID))
                        Systems.Add(cs.SSysID, new SolarSystem());

                    if (Systems[cs.SSysID].ConqStations == null)
                        Systems[cs.SSysID].ConqStations = new SortedList<int, ConqStation>();

                    Systems[cs.SSysID].ConqStations.Add(cs.ID, cs);
                }
            }
            catch
            {
                DialogResult dr = MessageBox.Show("An Error was encountered while Parsing Conquerable Station API.", "RouteMap: API Error", MessageBoxButtons.OK);
            }
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

        public void LoadCStationsFromDisk()
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

            fname = Path.Combine(RMapData_Path, "API_CStationList.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Systems = (Dictionary<int, SolarSystem>)myBf.Deserialize(cStr);
                }
                catch
                {
                }
                cStr.Close();
            }
        }

        public void SaveCStationListing()
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

            fname = Path.Combine(RMapData_Path, "API_CStationList.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Systems);
            pStream.Close();
        }

    }
}
