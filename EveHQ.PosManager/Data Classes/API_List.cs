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

namespace EveHQ.PosManager
{
    [Serializable]
    public class API_List
    {
        //public ArrayList apid;
        //public ArrayList apic;
        public SortedList apiTower;

        public API_List()
        {
            //apid = new ArrayList();
            //apic = new ArrayList();
            apiTower = new SortedList();
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
                    apiTower = (SortedList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public APITowerData GetAPIDataMemberForTowerID(int itemID)
        {
            if (apiTower.ContainsKey(itemID))
                return (APITowerData)apiTower.GetByIndex(apiTower.IndexOfKey(itemID));
            else
                return null;
        }

        private bool IsTypeIDATowerType(int typeID, TowerListing TL)
        {
            foreach (Tower t in TL.Towers)
            {
                if (typeID == t.typeID)
                    return true;
            }
            return false;
        }

        private string GetTowerNameForTowerTypeID(int typeID, TowerListing TL)
        {
            foreach (Tower t in TL.Towers)
            {
                if (typeID == t.typeID)
                    return t.Name;
            }
            return "Unknown Tower Type";
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

        public void LoadPOSDataFromAPI(TowerListing TL)
        {
            XmlDocument apiPOSList, apiPOSDetails;
            XmlNodeList posList, rsltList;
            APITowerData aptd;
            DataSet locData;
            string acctName, strSQL;
            int     sel, typeID;
            decimal qty;
            EveHQ.Core.EveAccount pilotAccount;

            apiTower.Clear();

            // Get API Files from disk as needed
            foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                if (selPilot.Active)
                {
                    acctName = selPilot.Account;
                    if (EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(acctName))
                    {
                        pilotAccount = (Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acctName];

                        // Get overall POS List associated with the given Pilot ID
                        sel = (int)EveHQ.Core.EveAPI.APIRequest.POSList;
                        apiPOSList = EveHQ.Core.EveAPI.GetAPIXML(sel, pilotAccount, selPilot.ID, 0);
                        if (!CheckXML(apiPOSList))
                        {
                            // Corrupted list, try again
                            apiPOSList = EveHQ.Core.EveAPI.GetAPIXML(sel, pilotAccount, selPilot.ID, 0);
                            if (!CheckXML(apiPOSList))
                            {
                                // Corrupted list, good bye
                                return;
                            }
                        }
                        if (apiPOSList == null)
                            continue;

                        posList = apiPOSList.SelectNodes("/eveapi/result/rowset/row");
                        foreach (XmlNode psN in posList)
                        {
                            aptd = new APITowerData();
                            aptd.itemID = Convert.ToInt32(psN.Attributes.GetNamedItem("itemID").Value.ToString());
                            aptd.towerID = Convert.ToInt32(psN.Attributes.GetNamedItem("typeID").Value.ToString());
                            aptd.locID  = Convert.ToInt32(psN.Attributes.GetNamedItem("locationID").Value.ToString());
                            aptd.moonID = Convert.ToInt32(psN.Attributes.GetNamedItem("moonID").Value.ToString());
                            aptd.corpID = Convert.ToInt32(selPilot.CorpID);
                            aptd.corpName = selPilot.Corp;
                            aptd.towerName = GetTowerNameForTowerTypeID(aptd.towerID, TL);

                            if (aptd.locID != 0)
                            {
                                strSQL = "SELECT itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + aptd.locID + ";";
                                locData = EveHQ.Core.DataFunctions.GetData(strSQL);
                                aptd.locName = locData.Tables[0].Rows[0].ItemArray[0].ToString();
                            }
                            else
                            {
                                aptd.locName = "Unknown";
                            }

                            // We have a tower, now get details for this tower
                            apiPOSDetails = EveHQ.Core.EveAPI.GetAPIXML(((int)EveHQ.Core.EveAPI.APIRequest.POSDetails), pilotAccount, selPilot.ID, aptd.itemID, 0);
                            if (!CheckXML(apiPOSDetails))
                            {
                                // Corrupted list, try again
                                apiPOSDetails = EveHQ.Core.EveAPI.GetAPIXML(sel, pilotAccount, selPilot.ID, 0);
                                if (!CheckXML(apiPOSDetails))
                                {
                                    // Corrupted list, good bye
                                    return;
                                }
                            }
                            if (apiPOSDetails == null)
                                continue;

                            // Have details, process them
                            // Since this file is a faster update frequency (1 every hour), use this timestamp
                            rsltList = apiPOSDetails.SelectNodes("/eveapi");
                            aptd.curTime = rsltList[0].ChildNodes[0].InnerText;
                            aptd.cacheUntil = rsltList[0].ChildNodes[2].InnerText;

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
                            // 4 = claim Sov
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
                            if (Convert.ToInt32(rsltList[0].ChildNodes[4].InnerText) > 0)
                                aptd.claimSov = true;
                            else
                                aptd.claimSov = false;

                            rsltList = apiPOSDetails.SelectNodes("/eveapi/result/combatSettings");
                            // 0 = Standing Drop
                            // 1 = Status Drop
                            // 2 = Agression
                            // 3 = War
                            aptd.standDrop = Convert.ToDecimal(rsltList[0].ChildNodes[0].Attributes.GetNamedItem("standing").Value.ToString());
                            if (Convert.ToInt32(rsltList[0].ChildNodes[1].Attributes.GetNamedItem("enabled").Value.ToString()) == 1)
                                aptd.onStatusDrop = true;
                            else
                                aptd.onStatusDrop = false;
                            aptd.statusDrop = Convert.ToDecimal(rsltList[0].ChildNodes[1].Attributes.GetNamedItem("standing").Value.ToString());
                            if (Convert.ToInt32(rsltList[0].ChildNodes[2].Attributes.GetNamedItem("enabled").Value.ToString()) == 1)
                                aptd.onAgression = true;
                            else
                                aptd.onAgression = false;
                            if (Convert.ToInt32(rsltList[0].ChildNodes[3].Attributes.GetNamedItem("enabled").Value.ToString()) == 1)
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
                                    case 44:    // Enr Uranium
                                        aptd.EnrUr = qty;
                                        break;
                                    case 3683:  // Oxygen
                                        aptd.Oxygn = qty;
                                        break;
                                    case 3689:  // Mech Parts
                                        aptd.MechP = qty;
                                        break;
                                    case 9832:  // Coolant
                                        aptd.Coolt = qty;
                                        break;
                                    case 9848:  // Robotics
                                        aptd.Robot = qty;
                                        break;
                                    case 16272: // Heavy Water
                                        aptd.HvyWt = qty;
                                        break;
                                    case 16273: // Liquid Ozone
                                        aptd.LiqOz = qty;
                                        break;
                                    case 24592: // Charter
                                    case 24593: // Charter
                                    case 24594: // Charter
                                    case 24595: // Charter
                                    case 24596: // Charter
                                    case 24597: // Charter
                                        aptd.Charters = qty;
                                        break;
                                    case 17888: // Nitrogen Isotopes
                                        aptd.N2Iso = qty;
                                        break;
                                    case 16274: // Helium Isotopes
                                        aptd.HeIso = qty;
                                        break;
                                    case 17889: // Hydrogen Isotopes
                                        aptd.H2Iso = qty;
                                        break;
                                    case 17887: // Oxygen Isotopes
                                        aptd.O2Iso = qty;
                                        break;
                                    case 16275: // Strontium
                                        aptd.Stront = qty;
                                        break;
                                }
                            }

                            // See if already in list - if so, then compare times
                            // If new time is more recent than old, overwrite the information
                            apiTower.Add(aptd.itemID, aptd);
                        }
                    }
                }
            }
        }

    }
}
