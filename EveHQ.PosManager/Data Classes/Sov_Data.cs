﻿using System;
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
        public decimal  systemID, allianceID, secLevel, corpID, factionID;
        public string   systemName;
        public string   cacheDate, cacheUntil;
        public ArrayList Moons;

        public Sov_Data()
        {
            systemID = 0;
            allianceID = 0;
            secLevel = 0;
            corpID = 0;
            factionID = 0;
            systemName = "";
            cacheDate = "";
            cacheUntil = "";
            Moons = new ArrayList();
        }

        public Sov_Data(Sov_Data sd)
        {
            systemID = sd.systemID;
            allianceID = sd.allianceID;
            corpID = sd.corpID;
            factionID = sd.factionID;
            systemName = sd.systemName;
            cacheUntil = sd.cacheUntil;
            cacheDate = sd.cacheDate;
            secLevel = sd.secLevel;
            Moons = new ArrayList(sd.Moons);
        }

        public void CopyData(Sov_Data sd)
        {
            systemID = sd.systemID;
            allianceID = sd.allianceID;
            corpID = sd.corpID;
            factionID = sd.factionID;
            systemName = sd.systemName;
            cacheUntil = sd.cacheUntil;
            cacheDate = sd.cacheDate;
            secLevel = sd.secLevel;
            Moons = new ArrayList(sd.Moons);
        }
    }
}
