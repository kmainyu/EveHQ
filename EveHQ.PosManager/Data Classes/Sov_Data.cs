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
    public class Sov_Data
    {
        public decimal     systemID, allianceID, constSov, sovLevel;
        public string      systemName;
        public string cacheDate, cacheUntil;

        public Sov_Data()
        {
            systemID = 0;
            allianceID = 0;
            constSov = 0;
            sovLevel = 0;
            systemName = "";
            cacheDate = "";
            cacheUntil = "";
        }

        public Sov_Data(Sov_Data sd)
        {
            systemID = sd.systemID;
            allianceID = sd.allianceID;
            constSov = sd.constSov;
            sovLevel = sd.sovLevel;
            systemName = sd.systemName;
            cacheUntil = sd.cacheUntil;
            cacheDate = sd.cacheDate;
        }

        public void CopyData(Sov_Data sd)
        {
            systemID = sd.systemID;
            allianceID = sd.allianceID;
            constSov = sd.constSov;
            sovLevel = sd.sovLevel;
            systemName = sd.systemName;
            cacheUntil = sd.cacheUntil;
            cacheDate = sd.cacheDate;
        }
    }
}
