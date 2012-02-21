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
    public class APITowerData
    {
        public string corpName, towerName, locName;
        public long itemID, towerID, locID, moonID, corpID;
        public long stateV; // AnchoredOrOffline = 1, Onlining = 2, Reinforced = 3, Online = 4
        public string curTime; // Time - data grabbed
        public string cacheUntil; // Time when next API Data grab is available
        public string useFlag;  // Usage Flags - ??
        public string depFlag;  // Deploy Flags - ??
        public bool allowCorp;
        public bool allowAlliance;
        public bool claimSov;
        public bool onStandDrop;
        public decimal standDrop;
        public bool onStatusDrop;
        public decimal statusDrop;
        public bool onAgression;
        public bool onWar;
        public string stateTS;  // Time when tower will have a server computation for fuel run next. If in reinforced mode, time tower will come out of that mode
        public string onlineTS; // Timestamp for when tower was placed online / online status changed
        public decimal EnrUr, Oxygn, MechP, Coolt, Robot, HeIso, N2Iso, H2Iso, O2Iso, LiqOz, HvyWt, Charters, Stront;

        public APITowerData()
        {
            itemID = 0;
            towerID = 0;
            locID = 0;
            moonID = 0;
            stateV = 0;
            corpID = 0;
            corpName = "";
            towerName = "";
            curTime = "";
            cacheUntil = "";
            useFlag = "";
            depFlag = "";
            locName = "";
            allowCorp = false;
            allowAlliance = false;
            claimSov = false;
            onStandDrop = false;
            standDrop = 0;
            onStatusDrop = false;
            statusDrop = 0;
            onAgression = false;
            onWar = false;
            stateTS = "";
            onlineTS = "";
            EnrUr = 0;
            Oxygn = 0;
            MechP = 0;
            Coolt = 0;
            Robot = 0;
            HeIso = 0;
            N2Iso = 0;
            H2Iso = 0;
            O2Iso = 0;
            LiqOz = 0;
            HvyWt = 0;
            Charters = 0;
            Stront = 0;
        }

        public APITowerData(APITowerData apt)
        {
            itemID = apt.itemID;
            towerID = apt.towerID;
            locID = apt.locID;
            moonID = apt.moonID;
            stateV = apt.stateV;
            corpID = apt.corpID;
            corpName = apt.corpName;
            towerName = apt.towerName;
            locName = apt.locName;
            curTime = apt.curTime;
            cacheUntil = apt.cacheUntil;
            useFlag = apt.useFlag;
            depFlag = apt.depFlag;
            allowCorp = apt.allowCorp;
            allowAlliance = apt.allowAlliance;
            claimSov = apt.claimSov;
            onStandDrop = apt.onStandDrop;
            standDrop = apt.standDrop;
            onStatusDrop = apt.onStatusDrop;
            statusDrop = apt.statusDrop;
            onAgression = apt.onAgression;
            onWar = apt.onWar;
            stateTS = apt.stateTS;
            onlineTS = apt.onlineTS;
            EnrUr = apt.EnrUr;
            Oxygn = apt.Oxygn;
            MechP = apt.MechP;
            Coolt = apt.Coolt;
            Robot = apt.Robot;
            HeIso = apt.HeIso;
            N2Iso = apt.N2Iso;
            H2Iso = apt.H2Iso;
            O2Iso = apt.O2Iso;
            LiqOz = apt.LiqOz;
            HvyWt = apt.HvyWt;
            Charters = apt.Charters;
            Stront = apt.Stront;
        }

    }
}
