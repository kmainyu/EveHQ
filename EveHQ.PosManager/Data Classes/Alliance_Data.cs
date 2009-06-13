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
    public class Alliance_Data
    {
        public string  name, ticker;
        public decimal allianceID, execCorpID, members;
        public ArrayList corps;
        public string cacheDate, cacheUntil, startDate;

        public Alliance_Data()
        {
            name = "";
            ticker = "";
            execCorpID = 0;
            allianceID = 0;
            members = 0;
            corps = new ArrayList();
            cacheDate = "";
            cacheUntil = "";
            startDate = "";
        }

        public Alliance_Data(Alliance_Data sd)
        {
            name = sd.name;
            ticker = sd.ticker;
            execCorpID = sd.execCorpID;
            allianceID = sd.allianceID;
            members = sd.members;
            corps = new ArrayList(sd.corps);
            cacheUntil = sd.cacheUntil;
            cacheDate = sd.cacheDate;
            startDate = sd.startDate;
        }

        public void CopyData(Alliance_Data sd)
        {
            name = sd.name;
            ticker = sd.ticker;
            execCorpID = sd.execCorpID;
            allianceID = sd.allianceID;
            members = sd.members;
            corps = new ArrayList(sd.corps);
            cacheUntil = sd.cacheUntil;
            cacheDate = sd.cacheDate;
            startDate = sd.startDate;
        }

    }
}
