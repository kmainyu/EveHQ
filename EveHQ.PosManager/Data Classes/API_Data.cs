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
    public class API_Data
    {
        public long itemID;
        public long corpID;
        public string corpName, ceoName, towerName, cacheDate, locName, cacheUntil;
        public long towerID, towerLocation;
        public decimal EnrUr, Oxygn, MechP, Coolt, Robot, HeIso, N2Iso, H2Iso, O2Iso, LiqOz, HvyWt, Charters, Stront;

        public API_Data()
        {
            itemID = 0;
            corpID = 0;
            towerID = 0;
            towerLocation = 0;
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
            corpName = "";
            ceoName = "";
            towerName = "";
            cacheDate = "";
            cacheUntil = "";
            locName = "";
        }

        public void CopyData(API_Data ap)
        {
            itemID = ap.itemID;
            corpID = ap.corpID;
            towerID = ap.towerID;
            towerLocation = ap.towerLocation;
            EnrUr = ap.EnrUr;
            Oxygn = ap.Oxygn;
            MechP = ap.MechP;
            Coolt = ap.Coolt;
            Robot = ap.Robot;
            HeIso = ap.HeIso;
            N2Iso = ap.N2Iso;
            H2Iso = ap.H2Iso;
            O2Iso = ap.O2Iso;
            LiqOz = ap.LiqOz;
            HvyWt = ap.HvyWt;
            Charters = ap.Charters;
            Stront = ap.Stront;
            corpName = ap.corpName;
            ceoName = ap.ceoName;
            towerName = ap.towerName;
            cacheDate = ap.cacheDate;
            cacheUntil = ap.cacheUntil;
            locName = ap.locName;
        }

        public API_Data(API_Data ap)
        {
            itemID = ap.itemID;
            corpID = ap.corpID;
            towerID = ap.towerID;
            towerLocation = ap.towerLocation;
            EnrUr = ap.EnrUr;
            Oxygn = ap.Oxygn;
            MechP = ap.MechP;
            Coolt = ap.Coolt;
            Robot = ap.Robot;
            HeIso = ap.HeIso;
            N2Iso = ap.N2Iso;
            H2Iso = ap.H2Iso;
            O2Iso = ap.O2Iso;
            LiqOz = ap.LiqOz;
            HvyWt = ap.HvyWt;
            Charters = ap.Charters;
            Stront = ap.Stront;
            corpName = ap.corpName;
            ceoName = ap.ceoName;
            towerName = ap.towerName;
            cacheDate = ap.cacheDate;
            cacheUntil = ap.cacheUntil;
            locName = ap.locName;
        }

    }
}
