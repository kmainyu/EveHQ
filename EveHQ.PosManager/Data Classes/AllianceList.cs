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
        public ArrayList alliances;

        public AllianceList()
        {
            alliances = new ArrayList();
        }
        public AllianceList(AllianceList al)
        {
            alliances = new ArrayList(al.alliances);
        }
        public void CopyList(AllianceList al)
        {
            alliances = new ArrayList(al.alliances);
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
            PoSManage_Path = PoSBase_Path + @"\PoSManage";
            PoSCache_Path = PoSManage_Path + @"\Cache";

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = PoSCache_Path + @"\API_Alliance.bin";

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
            PoSManage_Path = PoSBase_Path + @"\PoSManage";
            PoSCache_Path = PoSManage_Path + @"\Cache";

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = PoSCache_Path + @"\API_Alliance.bin";

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    alliances = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public void AddAllianceToList(Alliance_Data ap)
        {
            alliances.Add(ap);
        }

        public void RemoveAllianceFromList(Alliance_Data ap)
        {
            int ind = 0;

            foreach (Alliance_Data ad in alliances)
            {
                if (ad.allianceID == ap.allianceID)
                {
                    alliances.RemoveAt(ind);
                    break;
                }
                ind++;
            }
        }

        public void UpdateListAPI(Alliance_Data ap)
        {
            int ind = 0;

            foreach (Alliance_Data p in alliances)
            {
                if (p.allianceID == ap.allianceID)
                {
                    p.CopyData(ap);
                    break;
                }
                ind++;
            }
        }

        public Alliance_Data GetAPIDataMemberForAllianceID(decimal allianceID)
        {
            foreach (Alliance_Data ad in alliances)
            {
                if (ad.allianceID == allianceID)
                {
                    return ad;
                }
            }

            return null;
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

            ap = (Alliance_Data)alliances[0];

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

        private string GetAllianceNameForAllianceID(int allianceID, AllianceList AL)
        {
            foreach (Alliance_Data t in AL.alliances)
            {
                if (allianceID == t.allianceID)
                    return t.name;
            }
            return "Unknown Alliance";
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
            string PoSBase_Path, API_File_Path, fname;
            string cacheDate, cacheUntil;
            decimal corpID;

            allianceData = new XmlDocument();

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
                API_File_Path = EveHQ.Core.HQ.cacheFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
                API_File_Path = PoSBase_Path + @"\Cache";
            }

            if (!Directory.Exists(API_File_Path))
                return;

            // When a tower gets linked to the API and vice versa, the towerItemID will be
            // stored in the POS data itself. This will allow for easy import of the fuel data.
            fname = API_File_Path + @"\EVEHQAPI_AllianceList.xml";

            if(!File.Exists(fname))
                return;
                
            allianceData.Load(fname);

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
                }
                AddAllianceToList(ad);
            }
        }

    }
}
