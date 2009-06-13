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
    public class SystemSovList
    {
        public ArrayList SovList;

        public SystemSovList()
        {
            SovList = new ArrayList();
        }
        public SystemSovList(SystemSovList al)
        {
            SovList = new ArrayList(al.SovList);
        }
        public void CopyList(SystemSovList al)
        {
            SovList = new ArrayList(al.SovList);
        }

        public void SaveSovList()
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

            fname = PoSCache_Path + @"\API_SovList.bin";

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, SovList);
            pStream.Close();
        }

        public void LoadSovListFromAPI(Object o)
        {
            LoadSovListFromDisk();
            LoadSovListDataFromAPI();
            SaveSovList();
            PlugInData.resetEvents[3].Set();
        }

        public void LoadSovListFromDisk()
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

            fname = PoSCache_Path + @"\API_SovList.bin";

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    SovList = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public void AddSovToList(Sov_Data sd)
        {
            SovList.Add(sd);
        }

        public void RemoveAllianceFromList(Sov_Data sd)
        {
            int ind = 0;

            foreach (Sov_Data ad in SovList)
            {
                if (ad.systemID == sd.systemID)
                {
                    SovList.RemoveAt(ind);
                    break;
                }
                ind++;
            }
        }

        public void UpdateListAPI(Sov_Data ap)
        {
            int ind = 0;

            foreach (Sov_Data p in SovList)
            {
                if (p.systemID == ap.systemID)
                {
                    p.CopyData(ap);
                    break;
                }
                ind++;
            }
        }

        public Sov_Data GetAPIDataMemberForSystemID(decimal systemID)
        {
            foreach (Sov_Data ad in SovList)
            {
                if (ad.systemID == systemID)
                {
                    return ad;
                }
            }

            return null;
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

            ap = (Sov_Data)SovList[0];

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

        private string GetSystemNameForSystemID(int systemID, SystemSovList SL)
        {
            foreach (Sov_Data sd in SL.SovList)
            {
                if (systemID == sd.systemID)
                    return sd.systemName;
            }
            return "Unknown System";
        }

        private decimal GetSystemSovLevelForSystemID(int systemID, SystemSovList SL)
        {
            foreach (Sov_Data sd in SL.SovList)
            {
                if (systemID == sd.systemID)
                    return sd.sovLevel;
            }
            return 0;
        }

        private decimal GetSystemConstellationSov(int systemID, SystemSovList SL)
        {
            foreach (Sov_Data sd in SL.SovList)
            {
                if (systemID == sd.systemID)
                    return sd.constSov;
            }
            return 0;
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
            XmlNodeList sovList, dateList;
            Sov_Data sd;
            string PoSBase_Path, API_File_Path, fname;
            string cacheDate, cacheUntil;

            sovData = new XmlDocument();

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
            fname = API_File_Path + @"\EVEHQAPI_Sovereignty.xml";

            if (!File.Exists(fname))
                return;

            sovData.Load(fname);

            if (!CheckXML(sovData))
                return;

            dateList = sovData.SelectNodes("/eveapi");

            sovList = sovData.SelectNodes("/eveapi/result/rowset/row");

            cacheDate = dateList[0].ChildNodes[0].InnerText;
            cacheUntil = dateList[0].ChildNodes[2].InnerText;

            if (IsSovDataTimestampCurrent(cacheUntil))
            {
                // Nothing new to load at all
                return;
            }

            SovList.Clear();

            foreach (XmlNode syst in sovList)
            {
                sd = new Sov_Data();

                sd.systemID = Convert.ToDecimal(syst.Attributes.GetNamedItem("solarSystemID").Value.ToString());
                sd.allianceID = Convert.ToDecimal(syst.Attributes.GetNamedItem("allianceID").Value.ToString());
                sd.constSov = Convert.ToDecimal(syst.Attributes.GetNamedItem("constellationSovereignty").Value.ToString());
                sd.sovLevel = Convert.ToDecimal(syst.Attributes.GetNamedItem("sovereigntyLevel").Value.ToString());
                sd.systemName = syst.Attributes.GetNamedItem("solarSystemName").Value.ToString();
                sd.cacheDate = cacheDate;
                sd.cacheUntil = cacheUntil;

                AddSovToList(sd);
            }
        }


    }
}
