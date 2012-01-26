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
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;

namespace EveHQ.PosManager
{
    [Serializable]
    public class API_List
    {
        //public ArrayList apid;
        //public ArrayList apic;
        public SortedList<long, APITowerData> apiTower;
        public Computer myComp;
        public bool Updated;
        public string cacheUntil;
        public bool cleared = false;

        public API_List()
        {
            //apid = new ArrayList();
            //apic = new ArrayList();
            apiTower = new SortedList<long, APITowerData>();
            myComp = new Computer();
            Updated = false;
            cacheUntil = "";
        }

        public void SaveAPIListing()
        {
            string PoSBase_Path, PoSManage_Path, PoSCache_Path, fname;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = Path.Combine(PoSBase_Path , "PoSManage");
            PoSCache_Path = Path.Combine(PoSManage_Path , "Cache");

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = Path.Combine(PoSCache_Path, "API_POSDetails.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, apiTower);
            pStream.Close();
        }

        public void LoadAPIListing()
        {
            string PoSBase_Path, PoSManage_Path, PoSCache_Path, fname;
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
            PoSManage_Path = Path.Combine(PoSBase_Path , "PoSManage");
            PoSCache_Path = Path.Combine(PoSManage_Path, "Cache");

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = Path.Combine(PoSCache_Path, "API_POSDetails.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    apiTower = (SortedList<long, APITowerData>)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public APITowerData GetAPIDataMemberForTowerID(long itemID)
        {
            if (apiTower.ContainsKey(itemID))
                return (APITowerData)apiTower[itemID];
            else
                return null;
        }

        private bool IsTypeIDATowerType(long typeID)
        {
            if (PlugInData.TL.Towers.ContainsKey(typeID))
                return true;
            
            return false;
        }

        private string GetTowerNameForTowerTypeID(long typeID)
        {
            if (PlugInData.TL.Towers.ContainsKey(typeID))
            {
                return PlugInData.TL.Towers[typeID].Name;
            }

            return "Unknown Tower Type";
        }

        private bool IsPOS_APIDataNewer(APITowerData oldD, APITowerData newD)
        {
            DateTime cd, nd;
            TimeSpan dd;
            double secDif;

            cd = Convert.ToDateTime(oldD.curTime);
            nd = Convert.ToDateTime(newD.curTime);
            dd = nd.Subtract(cd);
            secDif = dd.TotalSeconds;

            if (secDif > 0)
                return true;
            else
                return false;
        }

        public void LoadPOSDataFromAPI()
        {
            cleared = false;
            foreach (EveHQ.Core.EveAccount pilotAccount in EveHQ.Core.HQ.EveHQSettings.Accounts)
            {
                if (pilotAccount.APIAccountStatus.Equals(Core.APIAccountStatuses.Active))
                {
                    if (pilotAccount.APIKeySystem.Equals(Core.APIKeySystems.Version1))
                    {
                        foreach (string pName in pilotAccount.Characters)
                        {
                            if (EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(pName))
                            {
                                EveHQ.Core.Pilot selPilot = (EveHQ.Core.Pilot)EveHQ.Core.HQ.EveHQSettings.Pilots[pName];
                                LoadPOSDataFromV1API(selPilot);
                            }
                        }
                    }
                    else if (pilotAccount.APIKeySystem.Equals(Core.APIKeySystems.Version2))
                    {
                        if (pilotAccount.APIKeyType.Equals(Core.APIKeyTypes.Corporation))
                        {
                            LoadPOSDataFromV2API(pilotAccount);
                        }
                        else
                        {
                            //PlugInData.LogAPIError(9999, "V2 API Bypassed for POS - Key is NOT Corporate ", pilotAccount.APIKeyType.ToString() + " | " + pilotAccount.APIKeyExpiryDate.ToString() + " | " + pilotAccount.APIAccountStatus.ToString() + " for " + pilotAccount.FriendlyName);
                        }
                    }
                    else
                    {
                        PlugInData.LogAPIError(9999, "API Bypassed for POS ", pilotAccount.APIKeyType.ToString() + " | " + pilotAccount.APIKeyExpiryDate.ToString() + " | " + pilotAccount.APIAccountStatus.ToString() + " for " + pilotAccount.FriendlyName);
                    }
                }
                else
                {
                    PlugInData.LogAPIError(9999, "API Bypassed for POS ", " for " + pilotAccount.FriendlyName + " as Account is Not Active!");
                }
            }

        }

        public void LoadPOSDataFromV2API(EveHQ.Core.EveAccount pilotAccount)
        {
            XmlDocument apiPOSList, apiPOSDetails;
            XmlNodeList posList, rsltList;
            XmlNode RsltL;
            APITowerData aptd;
            DataSet locData;
            string corpName, corpID, strSQL, errS;
            long typeID;
            decimal qty;

            // STUFF I NEED TO DO
            // 1. Detect / clear the cache of existing Tower API data at some longerval (every 24 hours?)
            // 2. If a current monitored tower that is linked, does NOT show up - mark it as offline
            //      - Or query user and ask if tower should be removed !

            // Get API Files from disk (or API server) as needed.
            // Additions here for V2 of the API Keys - makes things a touch tricky... ... ...
            if (pilotAccount.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.StarbaseList) &&
                pilotAccount.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.StarbaseDetail))
            {
                // Get my Corporation Details - do not have a setup pilot at this point
                EveAPI.EveAPIRequest APIReq;
                APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);

                // Get overall POS List associated with the given Pilot ID
                apiPOSList = APIReq.GetAPIXML(EveAPI.APITypes.POSList, pilotAccount.ToAPIAccount(), "", EveAPI.APIReturnMethods.ReturnStandard);

                if ((APIReq.LastAPIError > 0) && (APIReq.LastAPIResult != EveAPI.APIResults.ReturnedCached))
                {
                    if (!PlugInData.ThrottleList.ContainsKey(pilotAccount.FriendlyName))
                    {
                        PlugInData.ThrottleList.Add(pilotAccount.FriendlyName, new PlugInData.throttlePlayer(1));
                    }
                    else
                    {
                        PlugInData.ThrottleList[pilotAccount.FriendlyName].IncCount();
                    }
                    PlugInData.LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, pilotAccount.FriendlyName);
                }

                if (!PlugInData.CheckXML(apiPOSList))
                    return;

                try
                {
                    posList = apiPOSList.SelectNodes("/eveapi/result/rowset/row");
                    foreach (XmlNode psN in posList)
                    {
                        if (!cleared)
                        {
                            apiTower.Clear();
                            cleared = true;
                        }
                        aptd = new APITowerData();
                        aptd.itemID = Convert.ToInt64(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                        aptd.towerID = Convert.ToInt32(psN.Attributes.GetNamedItem("typeID").Value.ToString());
                        aptd.locID = Convert.ToInt32(psN.Attributes.GetNamedItem("locationID").Value.ToString());
                        aptd.moonID = Convert.ToInt32(psN.Attributes.GetNamedItem("moonID").Value.ToString());
                        corpID = psN.Attributes.GetNamedItem("standingOwnerID").Value.ToString();
                        aptd.corpID = Convert.ToInt32(corpID);

                        corpName = "Unknown";
                        if (pilotAccount.Characters.Count > 0)
                            corpName = pilotAccount.Characters[0].ToString();

                        aptd.corpName = corpName;
                        aptd.towerName = GetTowerNameForTowerTypeID(aptd.towerID);

                        if (aptd.locID != 0)
                        {
                            strSQL = "SELECT itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + aptd.locID + ";";
                            locData = EveHQ.Core.DataFunctions.GetData(strSQL);
                            if (locData.Tables[0].Rows.Count > 0)
                                aptd.locName = locData.Tables[0].Rows[0].ItemArray[0].ToString();
                            else
                                aptd.locName = "Unknown";
                        }
                        else
                        {
                            aptd.locName = "Unknown";
                        }

                        // We have a tower, now get details for this tower
                        apiPOSDetails = APIReq.GetAPIXML(EveAPI.APITypes.POSDetails, pilotAccount.ToAPIAccount(), "", aptd.itemID, EveAPI.APIReturnMethods.ReturnStandard);
                        if (!PlugInData.CheckXML(apiPOSDetails))
                        {
                            // Corrupted list, try again
                            apiPOSDetails = APIReq.GetAPIXML(EveAPI.APITypes.POSDetails, pilotAccount.ToAPIAccount(), "", EveAPI.APIReturnMethods.ReturnStandard);
                            if (!PlugInData.CheckXML(apiPOSDetails))
                            {
                                // Corrupted list, good bye
                                continue;
                            }
                        }

                        // Have details, process them
                        // Since this file is a faster update frequency (1 every hour), use this timestamp
                        RsltL = apiPOSDetails.SelectSingleNode("/eveapi");
                        aptd.curTime = RsltL["currentTime"].InnerText;
                        aptd.cacheUntil = RsltL["cachedUntil"].InnerText;

                        // Store time here, to use for next check timer!
                        cacheUntil = aptd.cacheUntil;

                        RsltL = apiPOSDetails.SelectSingleNode("/eveapi/result");
                        // 0 = state
                        // 1 = state timestamp
                        // 2 = online timestamp
                        // 3 = general settings list
                        // 4 = combat settings list
                        // 5 = rowset - fuel listing
                        aptd.stateV = Convert.ToInt32(RsltL["state"].InnerText);
                        aptd.stateTS = RsltL["stateTimestamp"].InnerText;
                        aptd.onlineTS = RsltL["onlineTimestamp"].InnerText;

                        RsltL = apiPOSDetails.SelectSingleNode("/eveapi/result/generalSettings");
                        // 0 = usage flags
                        // 1 = deploy flags
                        // 2 = allow corp members
                        // 3 = allow alliance members
                        aptd.useFlag = RsltL["usageFlags"].InnerText;
                        aptd.depFlag = RsltL["deployFlags"].InnerText;
                        if (Convert.ToInt32(RsltL["allowCorporationMembers"].InnerText) > 0)
                            aptd.allowCorp = true;
                        else
                            aptd.allowCorp = false;
                        if (Convert.ToInt32(RsltL["allowAllianceMembers"].InnerText) > 0)
                            aptd.allowAlliance = true;
                        else
                            aptd.allowAlliance = false;
                        aptd.claimSov = false;

                        rsltList = apiPOSDetails.SelectNodes("/eveapi/result/combatSettings");
                        // 0 = Standings From ID
                        // 1 = Standing Drop
                        // 2 = Status Drop
                        // 3 = Agression
                        // 4 = War
                        aptd.standDrop = Convert.ToDecimal(rsltList[0].ChildNodes[1].Attributes.GetNamedItem("standing").Value.ToString());
                        if (Convert.ToInt32(rsltList[0].ChildNodes[2].Attributes.GetNamedItem("enabled").Value.ToString()) == 1)
                            aptd.onStatusDrop = true;
                        else
                            aptd.onStatusDrop = false;
                        aptd.statusDrop = Convert.ToDecimal(rsltList[0].ChildNodes[2].Attributes.GetNamedItem("standing").Value.ToString());
                        if (Convert.ToInt32(rsltList[0].ChildNodes[3].Attributes.GetNamedItem("enabled").Value.ToString()) == 1)
                            aptd.onAgression = true;
                        else
                            aptd.onAgression = false;
                        if (Convert.ToInt32(rsltList[0].ChildNodes[4].Attributes.GetNamedItem("enabled").Value.ToString()) == 1)
                            aptd.onWar = true;
                        else
                            aptd.onWar = false;

                        rsltList = apiPOSDetails.SelectNodes("/eveapi/result/rowset/row");
                        foreach (XmlNode fuelN in rsltList)
                        {
                            typeID = Convert.ToInt32(fuelN.Attributes.GetNamedItem("typeID").Value.ToString());
                            qty = Convert.ToDecimal(fuelN.Attributes.GetNamedItem("quantity").Value.ToString());

                            switch (typeID)
                            {
                                //case 44:    // Enr Uranium
                                //    aptd.EnrUr = qty;
                                //    break;
                                //case 3683:  // Oxygen
                                //    aptd.Oxygn = qty;
                                //    break;
                                //case 3689:  // Mech Parts
                                //    aptd.MechP = qty;
                                //    break;
                                //case 9832:  // Coolant
                                //    aptd.Coolt = qty;
                                //    break;
                                //case 9848:  // Robotics
                                //    aptd.Robot = qty;
                                //    break;
                                //case 16272: // Heavy Water
                                //    aptd.HvyWt = qty;
                                //    break;
                                //case 16273: // Liquid Ozone
                                //    aptd.LiqOz = qty;
                                //    break;
                                case 24592: // Charter
                                case 24593: // Charter
                                case 24594: // Charter
                                case 24595: // Charter
                                case 24596: // Charter
                                case 24597: // Charter
                                    aptd.Charters = qty;
                                    break;
                                //case 17888: // Nitrogen Isotopes
                                //    aptd.N2Iso = qty;
                                //    break;
                                //case 16274: // Helium Isotopes
                                //    aptd.HeIso = qty;
                                //    break;
                                //case 17889: // Hydrogen Isotopes
                                //    aptd.H2Iso = qty;
                                //    break;
                                //case 17887: // Oxygen Isotopes
                                //    aptd.O2Iso = qty;
                                //    break;
                                case 16275: // Strontium
                                    aptd.Stront = qty;
                                    break;
                            }
                        }

                        // See if already in list - if not, then add it
                        if (!apiTower.ContainsKey(aptd.itemID))
                            apiTower.Add(aptd.itemID, aptd);
                    }
                }
                catch
                {
                    DialogResult dr = MessageBox.Show("An Error was encountered while Parsing Tower API Data.\n"
                                                        + " for Pilot [ " + pilotAccount.FriendlyName + " ]", "PoSManager: API Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                errS = "";
                if (!pilotAccount.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.StarbaseList))
                    errS += " No StarbaseList Access!";
                if (!pilotAccount.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.StarbaseDetail))
                    errS += " No StarbaseDetail Access!";

                PlugInData.LogAPIError(9999, "V2 API Bypassed for POS ", errS + " for " + pilotAccount.FriendlyName);
            }
        }

        public void LoadPOSDataFromV1API(EveHQ.Core.Pilot selPilot)
        {
            XmlDocument apiPOSList, apiPOSDetails;
            XmlNodeList posList, rsltList;
            APITowerData aptd;
            DataSet locData;
            string acctName, strSQL;
            long    typeID;
            decimal qty;
            EveAPI.APITypes sel;
            EveHQ.Core.EveAccount pilotAccount;

            // STUFF I NEED TO DO
            // 1. Detect / clear the cache of existing Tower API data at some longerval (every 24 hours?)
            // 2. If a current monitored tower that is linked, does NOT show up - mark it as offline
            //      - Or query user and ask if tower should be removed !

            // Get API Files from disk (or API server) as needed.
            // Additions here for V2 of the API Keys - makes things a touch tricky... ... ...
            if (selPilot.Active && PlugInData.IsPilotAllowedAccessToAPI(selPilot))
            {
                acctName = selPilot.Account;
                pilotAccount = (Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acctName];

                // Get overall POS List associated with the given Pilot ID
                sel = EveAPI.APITypes.POSList;
                EveAPI.EveAPIRequest APIReq;
                APIReq = new EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder);
                apiPOSList = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);

                if ((APIReq.LastAPIError > 0) && (APIReq.LastAPIResult != EveAPI.APIResults.ReturnedCached))
                {
                    if (!PlugInData.ThrottleList.ContainsKey(selPilot.Name))
                    {
                        PlugInData.ThrottleList.Add(selPilot.Name, new PlugInData.throttlePlayer(1));
                    }
                    else
                    {
                        PlugInData.ThrottleList[selPilot.Name].IncCount();
                    }
                    PlugInData.LogAPIError(APIReq.LastAPIError, APIReq.LastAPIErrorText, selPilot.Name);
                }
                    
                if (!PlugInData.CheckXML(apiPOSList))
                    return;

                try
                {
                    posList = apiPOSList.SelectNodes("/eveapi/result/rowset/row");
                    foreach (XmlNode psN in posList)
                    {
                        if (!cleared)
                        {
                            apiTower.Clear();
                            cleared = true;
                        }
                        aptd = new APITowerData();
                        aptd.itemID = Convert.ToInt64(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                        aptd.towerID = Convert.ToInt32(psN.Attributes.GetNamedItem("typeID").Value.ToString());
                        aptd.locID = Convert.ToInt32(psN.Attributes.GetNamedItem("locationID").Value.ToString());
                        aptd.moonID = Convert.ToInt32(psN.Attributes.GetNamedItem("moonID").Value.ToString());
                        aptd.corpID = Convert.ToInt32(selPilot.CorpID);
                        aptd.corpName = selPilot.Corp;
                        aptd.towerName = GetTowerNameForTowerTypeID(aptd.towerID);

                        if (aptd.locID != 0)
                        {
                            strSQL = "SELECT itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + aptd.locID + ";";
                            locData = EveHQ.Core.DataFunctions.GetData(strSQL);
                            if (locData.Tables[0].Rows.Count > 0)
                                aptd.locName = locData.Tables[0].Rows[0].ItemArray[0].ToString();
                            else
                                aptd.locName = "Unknown";
                        }
                        else
                        {
                            aptd.locName = "Unknown";
                        }

                        // We have a tower, now get details for this tower
                        apiPOSDetails = APIReq.GetAPIXML((EveAPI.APITypes.POSDetails), pilotAccount.ToAPIAccount(), selPilot.ID, aptd.itemID, EveAPI.APIReturnMethods.ReturnStandard);
                        if (!PlugInData.CheckXML(apiPOSDetails))
                        {
                            // Corrupted list, try again
                            apiPOSDetails = APIReq.GetAPIXML(sel, pilotAccount.ToAPIAccount(), selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard);
                            if (!PlugInData.CheckXML(apiPOSDetails))
                            {
                                // Corrupted list, good bye
                                continue;
                            }
                        }

                        // Have details, process them
                        // Since this file is a faster update frequency (1 every hour), use this timestamp
                        rsltList = apiPOSDetails.SelectNodes("/eveapi");
                        aptd.curTime = rsltList[0].ChildNodes[0].InnerText;
                        aptd.cacheUntil = rsltList[0].ChildNodes[2].InnerText;

                        // Store time here, to use for next check timer!
                        cacheUntil = aptd.cacheUntil;

                        rsltList = apiPOSDetails.SelectNodes("/eveapi/result");
                        // 0 = state
                        // 1 = state timestamp
                        // 2 = online timestamp
                        // 3 = general settings list
                        // 4 = combat settings list
                        // 5 = rowset - fuel listing
                        aptd.stateV = Convert.ToInt32(rsltList[0].ChildNodes[0].InnerText);
                        aptd.stateTS = rsltList[0].ChildNodes[1].InnerText;
                        aptd.onlineTS = rsltList[0].ChildNodes[2].InnerText;

                        rsltList = apiPOSDetails.SelectNodes("/eveapi/result/generalSettings");
                        // 0 = usage flags
                        // 1 = deploy flags
                        // 2 = allow corp members
                        // 3 = allow alliance members
                        aptd.useFlag = rsltList[0].ChildNodes[0].InnerText;
                        aptd.depFlag = rsltList[0].ChildNodes[1].InnerText;
                        if (Convert.ToInt32(rsltList[0].ChildNodes[2].InnerText) > 0)
                            aptd.allowCorp = true;
                        else
                            aptd.allowCorp = false;
                        if (Convert.ToInt32(rsltList[0].ChildNodes[3].InnerText) > 0)
                            aptd.allowAlliance = true;
                        else
                            aptd.allowAlliance = false;
                        aptd.claimSov = false;

                        rsltList = apiPOSDetails.SelectNodes("/eveapi/result/combatSettings");
                        // 0 = Standings From ID
                        // 1 = Standing Drop
                        // 2 = Status Drop
                        // 3 = Agression
                        // 4 = War
                        aptd.standDrop = Convert.ToDecimal(rsltList[0].ChildNodes[1].Attributes.GetNamedItem("standing").Value.ToString());
                        if (Convert.ToInt32(rsltList[0].ChildNodes[2].Attributes.GetNamedItem("enabled").Value.ToString()) == 1)
                            aptd.onStatusDrop = true;
                        else
                            aptd.onStatusDrop = false;
                        aptd.statusDrop = Convert.ToDecimal(rsltList[0].ChildNodes[2].Attributes.GetNamedItem("standing").Value.ToString());
                        if (Convert.ToInt32(rsltList[0].ChildNodes[3].Attributes.GetNamedItem("enabled").Value.ToString()) == 1)
                            aptd.onAgression = true;
                        else
                            aptd.onAgression = false;
                        if (Convert.ToInt32(rsltList[0].ChildNodes[4].Attributes.GetNamedItem("enabled").Value.ToString()) == 1)
                            aptd.onWar = true;
                        else
                            aptd.onWar = false;

                        rsltList = apiPOSDetails.SelectNodes("/eveapi/result/rowset/row");
                        foreach (XmlNode fuelN in rsltList)
                        {
                            typeID = Convert.ToInt32(fuelN.Attributes.GetNamedItem("typeID").Value.ToString());
                            qty = Convert.ToDecimal(fuelN.Attributes.GetNamedItem("quantity").Value.ToString());

                            switch (typeID)
                            {
                                //case 44:    // Enr Uranium
                                //    aptd.EnrUr = qty;
                                //    break;
                                //case 3683:  // Oxygen
                                //    aptd.Oxygn = qty;
                                //    break;
                                //case 3689:  // Mech Parts
                                //    aptd.MechP = qty;
                                //    break;
                                //case 9832:  // Coolant
                                //    aptd.Coolt = qty;
                                //    break;
                                //case 9848:  // Robotics
                                //    aptd.Robot = qty;
                                //    break;
                                //case 16272: // Heavy Water
                                //    aptd.HvyWt = qty;
                                //    break;
                                //case 16273: // Liquid Ozone
                                //    aptd.LiqOz = qty;
                                //    break;
                                case 24592: // Charter
                                case 24593: // Charter
                                case 24594: // Charter
                                case 24595: // Charter
                                case 24596: // Charter
                                case 24597: // Charter
                                    aptd.Charters = qty;
                                    break;
                                //case 17888: // Nitrogen Isotopes
                                //    aptd.N2Iso = qty;
                                //    break;
                                //case 16274: // Helium Isotopes
                                //    aptd.HeIso = qty;
                                //    break;
                                //case 17889: // Hydrogen Isotopes
                                //    aptd.H2Iso = qty;
                                //    break;
                                //case 17887: // Oxygen Isotopes
                                //    aptd.O2Iso = qty;
                                //    break;
                                case 16275: // Strontium
                                    aptd.Stront = qty;
                                    break;
                            }
                        }

                        // See if already in list - if not, then add it
                        if (!apiTower.ContainsKey(aptd.itemID))
                            apiTower.Add(aptd.itemID, aptd);
                    }
                }
                catch
                {
                    DialogResult dr = MessageBox.Show("An Error was encountered while Parsing Tower API Data.\n"
                                                        + " for Pilot [ " + selPilot.Name + " ]", "PoSManager: API Error", MessageBoxButtons.OK);
                }
            }
        }
    }
}
