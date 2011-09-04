﻿// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
// 
// This file is part of the "EveHQ RouteMap" plug-in
//
// EveHQ RouteMap derives from copyrighted works licensed under the GNU 
// General Public License originally created by Lhyda Souljacker.
//
// This version has been modified pursuant to the GNU General Public 
// License as of 4 September 2011, and as distributed, it includes or is 
// derivative of works licensed under the GNU General Public License.
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
using System.Linq;

namespace EveHQ.RouteMap
{
    [Serializable]
    public class Alliance_Data
    {
        public string  name, ticker;
        public int allianceID, execCorpID, members;
        public ArrayList corps;
        public string cacheDate, cacheUntil, startDate;
        public Color AColor;

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
            AColor = Color.Green;
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
            AColor = sd.AColor;
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
            AColor = sd.AColor;
        }

    }
}
