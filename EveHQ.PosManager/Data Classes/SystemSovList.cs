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
        public SystemList SovList;

        public SystemSovList()
        {
            SovList = new SystemList();
        }

        public void LoadSovListFromAPI(Object o)
        {
            SovList.LoadSystemListFromDisk();
            LoadSovListDataFromAPI();
            SovList.SaveSystemListToDisk();
            PlugInData.resetEvents[3].Set();
        }

        public void LoadSovListFromActiveAPI(Object o)
        {
            LoadSovListDataFromAPI();
            SovList.SaveSystemListToDisk();
            PlugInData.resetEvents[3].Set();
        }

        private bool IsSovDataTimestampCurrent(string cacheUntil)
        {
            string curDate;
            DateTime cd, nd;
            TimeSpan dd;
            double secDif;
            Sov_Data ap;

            if (SovList.Systems.Count <= 0)
                return false;

            ap = (Sov_Data)SovList.Systems.GetByIndex(0);

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

        private decimal GetSystemIDForSystemName(string sysName, SystemSovList SL)
        {
            Sov_Data sd;
            sd = (Sov_Data)SL.SovList.Systems[sysName];

            if (sd != null)
                return sd.systemID;
            else
                return 0;
        }

        private decimal GetSystemSovLevelForSystemName(string sysName, SystemSovList SL)
        {
            Sov_Data sd;
            sd = (Sov_Data)SL.SovList.Systems[sysName];

            if (sd != null)
                return 1;
            else
                return 0;
        }

        private decimal GetSystemConstellationSov(string sysName, SystemSovList SL)
        {
            Sov_Data sd;
            sd = (Sov_Data)SL.SovList.Systems[sysName];

            if (sd != null)
                return 1;
            else
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

        private Sov_Data GetDataForSystemName(string sysName)
        {
            Sov_Data sd;
            sd = (Sov_Data)SovList.Systems[sysName];

            return sd;
        }

        private void LoadSovListDataFromAPI()
        {
            XmlDocument sovData;
            XmlNodeList svList, dateList;
            Sov_Data sd;
            string cacheDate, cacheUntil, sysName;

            sovData = new XmlDocument();
            // When a tower gets linked to the API and vice versa, the towerItemID will be
            // stored in the POS data itself. This will allow for easy import of the fuel data.
            sovData = EveHQ.Core.EveAPI.GetAPIXML((int)EveHQ.Core.EveAPI.APIRequest.Sovereignty, 0);
            if (!CheckXML(sovData))
                return;

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
                    sysName = syst.Attributes.GetNamedItem("solarSystemName").Value.ToString();
                    sd = GetDataForSystemName(sysName);

                    if (sd == null)
                    {
                        sd = new Sov_Data();
                        sd.systemName = sysName;
                        sd.systemID = Convert.ToDecimal(syst.Attributes.GetNamedItem("solarSystemID").Value.ToString());
                        sd.allianceID = Convert.ToDecimal(syst.Attributes.GetNamedItem("allianceID").Value.ToString());
                        sd.factionID = Convert.ToDecimal(syst.Attributes.GetNamedItem("factionID").Value.ToString());
                        sd.corpID = Convert.ToDecimal(syst.Attributes.GetNamedItem("corporationID").Value.ToString());
                        sd.cacheDate = cacheDate;
                        sd.cacheUntil = cacheUntil;
                        SovList.Systems.Add(sysName, sd);
                    }
                    else
                    {
                        sd.systemID = Convert.ToDecimal(syst.Attributes.GetNamedItem("solarSystemID").Value.ToString());
                        sd.allianceID = Convert.ToDecimal(syst.Attributes.GetNamedItem("allianceID").Value.ToString());
                        sd.factionID = Convert.ToDecimal(syst.Attributes.GetNamedItem("factionID").Value.ToString());
                        sd.corpID = Convert.ToDecimal(syst.Attributes.GetNamedItem("corporationID").Value.ToString()); 
                        sd.cacheDate = cacheDate;
                        sd.cacheUntil = cacheUntil;
                    }
                }
            }
            catch
            {
                DialogResult dr = MessageBox.Show("An Error was encountered while Parsing System Sov API Data.", "PoSManager: API Error", MessageBoxButtons.OK);
            }

        }
    }
}
