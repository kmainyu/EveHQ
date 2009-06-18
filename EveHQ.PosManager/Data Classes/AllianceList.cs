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

        public AllianceList()
        {
            alliances = new SortedList();
        }

        public void SaveAllianceListing()
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

            fname = Path.Combine(PoSCache_Path , "API_Alliance.bin");

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
            PoSManage_Path = Path.Combine(PoSBase_Path, "PoSManage");
            PoSCache_Path = Path.Combine(PoSManage_Path, "Cache");

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = Path.Combine(PoSCache_Path, "API_Alliance.bin");

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
            allianceData = EveHQ.Core.EveAPI.GetAPIXML((int)EveHQ.Core.EveAPI.APIRequest.AllianceList, 0);

            if (!CheckXML(allianceData))
                return;

            dateList = allianceData.SelectNodes("/eveapi");

            allyList = allianceData.SelectNodes("/eveapi/result/rowset/row");

            cacheDate = dateList[0].ChildNodes[0].InnerText;
            cacheUntil = dateList[0].ChildNodes[2].InnerText;

            if(IsAllianceDataTimestampCurrent(cacheUntil))
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

    }
}
