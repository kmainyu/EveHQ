// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
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

namespace EveHQ.RouteMap
{
    [Serializable]
    public class AlternateNode
    {
        public int JumpNum;
        public SolarSystem from, to, curr;
        public string constellation, region, sov, sTic;//, type;
        public double sec, distFrom, distTo, ISOFrom, ISOTo, LOFrom, LOTo, cargoF, cargoT;
        public string Jumps, Kills;
        public bool Station, StationTo;
        public string StationTT, StationToTT, IsoType;
        public string JumpTT, JumpToTT;
        public PlugInData.JumpType JumpTyp, JumpTypTo;
        public JumpBridge Bridge, BridgeTo;

        public AlternateNode()
        {
            from = new SolarSystem();
            to = new SolarSystem();
            curr = new SolarSystem();
            constellation = "";
            region = "";
            sov = "";
            JumpTyp = PlugInData.JumpType.Undefined;
            JumpTypTo = PlugInData.JumpType.Undefined;
            sec = 0;
            distFrom = 0;
            distTo = 0;
            ISOFrom = 0;
            ISOTo = 0;
            LOFrom = 0;
            LOTo = 0;
            cargoF = 0;
            cargoT = 0;
            JumpNum = 0;
            Jumps = "";
            Kills = "";
            Station = false;
            StationTo = false;
            StationTT = "";
            StationToTT = "";
            JumpTT = "";
            JumpToTT = "";
            IsoType = "";
            sTic = "";
            Bridge = new JumpBridge();
            BridgeTo = new JumpBridge();
        }
    }
}
