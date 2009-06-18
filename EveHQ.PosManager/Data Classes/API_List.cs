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
        public ArrayList apid;
        public ArrayList apic;

        public API_List()
        {
            apid = new ArrayList();
            apic = new ArrayList();
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

            fname = Path.Combine(PoSCache_Path, "API_Towers.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, apid);
            pStream.Close();

            fname = Path.Combine(PoSCache_Path, "API_Corps.bin");

            // Save the Serialized data to Disk
            pStream = File.Create(fname);
            pBF = new BinaryFormatter();
            pBF.Serialize(pStream, apic);
            pStream.Close();
        }

        public void LoadAPIListing(TowerListing TL)
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

            fname = Path.Combine(PoSCache_Path, "API_Towers.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    apid = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }

            fname = Path.Combine(PoSCache_Path, "API_Corps.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    apic = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }

            LoadCorpTowerDataFromAPI(TL);
            SaveAPIListing();
        }

        public void AddAPIToList(API_Data ap)
        {
            apid.Add(ap);
        }

        public void RemoveAPIFromList(API_Data ap)
        {
            int ind = 0;

            foreach (API_Data p in apid)
            {
                if (p.itemID == ap.itemID)
                {
                    apid.RemoveAt(ind);
                    break;
                }
                ind++;
            }
        }

        public void UpdateListAPI(API_Data ap)
        {
            int ind = 0;

            foreach (API_Data p in apid)
            {
                if (p.itemID == ap.itemID)
                {
                    p.CopyData(ap);
                    break;
                }
                ind++;
            }
        }

        public void AddCorpAPIToList(API_Data ap)
        {
            apic.Add(ap);
        }

        public void RemoveCorpAPIFromList(API_Data ap)
        {
            int ind = 0;

            foreach (API_Data p in apic)
            {
                if (p.itemID == ap.itemID)
                {
                    apic.RemoveAt(ind);
                    break;
                }
                ind++;
            }
        }

        public void UpdateCorpListAPI(API_Data ap)
        {
            int ind = 0;

            foreach (API_Data p in apic)
            {
                if (p.itemID == ap.itemID)
                {
                    p.CopyData(ap);
                    break;
                }
                ind++;
            }
        }

        public API_Data GetAPIDataMemberForTowerID(int itemID)
        {
            foreach (API_Data ap in apid)
            {
                if (ap.itemID == itemID)
                {
                    return ap;
                }
            }

            return null;
        }

        private bool IsAPIDataTimestampCurrent(string cacheUntil, int itemID)
        {
            string curDate;
            DateTime cd, nd;
            TimeSpan dd;
            double secDif;
            API_Data ap;

            ap = GetAPIDataMemberForTowerID(itemID);

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

        public API_Data GetCorpAPIDataMemberForCorpID(int corpID)
        {
            foreach (API_Data ap in apic)
            {
                if (ap.corpID == corpID)
                {
                    return ap;
                }
            }

            return null;
        }

        private void LoadCorpTowerDataFromAPI(TowerListing TL)
        {
            XmlDocument apiCorpSheet, apiCorpAssets;
            XmlNodeList corpSheet, corpAssets, subList;
            API_Data ap, ac;
            string PoSBase_Path, API_File_Path, fname;
            decimal qty;
            string ceoName, corpName, cacheDate, cacheUntil;
            int corpID, itemID, typeID, locID;
            int towerCount = 0;
            bool newTower = false;

            apiCorpSheet = new XmlDocument();
            apiCorpAssets = new XmlDocument();

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
                API_File_Path = EveHQ.Core.HQ.cacheFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
                API_File_Path = Path.Combine(PoSBase_Path , "Cache");
            }

            if (!Directory.Exists(API_File_Path))
                return;

            // When a tower gets linked to the API and vice versa, the towerItemID will be
            // stored in the POS data itself. This will allow for easy import of the fuel data.
            string[] fileList = Directory.GetFiles(API_File_Path);

            foreach (string s in fileList)
            {
                if (s.Contains("CorpSheet"))
                {
                    // Have a CorpSheet, need to parse out Char/etc... ID numbers
                    fname = s.Replace("CorpSheet", "AssetsCorp");

                    apiCorpSheet.Load(s);
                    if (!CheckXML(apiCorpSheet))
                        continue;

                    corpSheet = apiCorpSheet.SelectNodes("/eveapi");

                    corpSheet = apiCorpSheet.SelectNodes("/eveapi/result");
                    corpName = corpSheet[0].ChildNodes[1].InnerText;
                    corpID = Convert.ToInt32(corpSheet[0].ChildNodes[0].InnerText);
                    ceoName = corpSheet[0].ChildNodes[4].InnerText;

                    ac = GetCorpAPIDataMemberForCorpID(corpID);
                    if (ac == null)
                    {
                        ac = new API_Data();
                        ac.corpID = corpID;
                        ac.corpName = corpName;
                        ac.ceoName = ceoName;
                        AddCorpAPIToList(ac);
                    }

                    if (File.Exists(fname))
                    {
                        // Found a CorpSheet_AssetsCorp File Pair
                        apiCorpAssets.Load(fname);
                        if (!CheckXML(apiCorpAssets))
                            continue;

                        corpAssets = apiCorpAssets.SelectNodes("/eveapi");
                        cacheDate = corpAssets[0].ChildNodes[0].InnerText;
                        cacheUntil = corpAssets[0].ChildNodes[2].InnerText;

                        corpAssets = apiCorpAssets.SelectNodes("/eveapi/result/rowset/row");

                        foreach (XmlNode cs_node in corpAssets)
                        {
                            itemID = Convert.ToInt32(cs_node.Attributes.GetNamedItem("itemID").Value.ToString());
                            typeID = Convert.ToInt32(cs_node.Attributes.GetNamedItem("typeID").Value.ToString());
                            locID = Convert.ToInt32(cs_node.Attributes.GetNamedItem("locationID").Value.ToString());

                            if (IsTypeIDATowerType(Convert.ToInt32(typeID), TL))
                            {
                                ap = GetAPIDataMemberForTowerID(itemID);

                                if (ap == null)
                                {
                                    // Do not have any data for this Tower Item ID yet
                                    towerCount++;
                                    ap = new API_Data();
                                    newTower = true;
                                }
                                else if (IsAPIDataTimestampCurrent(cacheUntil, itemID))
                                {
                                    // Have data, data is current with timestamp given
                                    newTower = false;
                                    continue;
                                }
                                else
                                {
                                    // Have new data to apply
                                    newTower = false;
                                }

                                // A) I have the Data, or B) New Tower
                                // either way, read in my data and apply it now
                                ap.cacheDate = cacheDate;
                                ap.cacheUntil = cacheUntil;
                                ap.ceoName = ceoName;
                                ap.corpID = corpID;
                                ap.corpName = corpName;
                                ap.itemID = itemID;
                                ap.towerID = typeID;
                                ap.towerLocation = locID;
                                ap.towerName = GetTowerNameForTowerTypeID(typeID, TL);

                                // Get Table With Tower or Tower Item Information
                                ap.locName = "Unknown";

                                if (cs_node.HasChildNodes)
                                {
                                    subList = cs_node.ChildNodes[0].ChildNodes;
                                    foreach (XmlNode sl_node in subList)
                                    {
                                        typeID = Convert.ToInt32(sl_node.Attributes.GetNamedItem("typeID").Value.ToString());
                                        qty = Convert.ToDecimal(sl_node.Attributes.GetNamedItem("quantity").Value.ToString());

                                        switch (typeID)
                                        {
                                            case 44:    // Enr Uranium
                                                ap.EnrUr = qty;
                                                break;
                                            case 3683:  // Oxygen
                                                ap.Oxygn = qty;
                                                break;
                                            case 3689:  // Mech Parts
                                                ap.MechP = qty;
                                                break;
                                            case 9832:  // Coolant
                                                ap.Coolt = qty;
                                                break;
                                            case 9848:  // Robotics
                                                ap.Robot = qty;
                                                break;
                                            case 16272: // Heavy Water
                                                ap.HvyWt = qty;
                                                break;
                                            case 16273: // Liquid Ozone
                                                ap.LiqOz = qty;
                                                break;
                                            case 24592: // Charter
                                            case 24593: // Charter
                                            case 24594: // Charter
                                            case 24595: // Charter
                                            case 24596: // Charter
                                            case 24597: // Charter
                                                ap.Charters = qty;
                                                break;
                                            case 17888: // Nitrogen Isotopes
                                                ap.N2Iso = qty;
                                                break;
                                            case 16274: // Helium Isotopes
                                                ap.HeIso = qty;
                                                break;
                                            case 17889: // Hydrogen Isotopes
                                                ap.H2Iso = qty;
                                                break;
                                            case 17887: // Oxygen Isotopes
                                                ap.O2Iso = qty;
                                                break;
                                            case 16275: // Strontium
                                                ap.Stront = qty;
                                                break;
                                        }
                                    }
                                }

                                if (newTower)
                                    AddAPIToList(ap);
                                else
                                    UpdateListAPI(ap);
                            }
                        }
                    }
                }
            }
        }

    }
}
