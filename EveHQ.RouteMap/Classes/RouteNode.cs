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
    public class RouteNode
    {
        public int JumpNum;
        public SolarSystem from, to;
        public string constellation, region, sov, sTic;//, type;
        public double sec, distance, ISO, LO, cargo;
        public string Jumps, Kills;
        public bool Station, Dest, AltNode;
        public string StationTT, IsoType;
        public string JumpTT, CynoSafeMoon;
        public PlugInData.JumpType JumpTyp;

        public RouteNode()
        {
            from = new SolarSystem();
            to = new SolarSystem();
            constellation = "";
            region = "";
            sov = "";
            JumpTyp = PlugInData.JumpType.Undefined;
            sec = 0;
            distance = 0;
            ISO = 0;
            LO = 0;
            cargo = 0;
            JumpNum = 0;
            Jumps = "";
            Kills = "";
            Station = false;
            Dest = false;
            AltNode = true;
            StationTT = "";
            JumpTT = "";
            IsoType = "";
            sTic = "";
            CynoSafeMoon = "";
        }

        public RouteNode(RouteNode rn)
        {
            from = new SolarSystem(rn.from);
            to = new SolarSystem(rn.to);
            constellation = rn.constellation;
            region = rn.region;
            sov = rn.sov;
            JumpTyp = rn.JumpTyp;
            sec = rn.sec;
            distance = rn.distance;
            ISO = rn.ISO;
            LO = rn.LO;
            cargo = rn.cargo;
            JumpNum = rn.JumpNum;
            Jumps = rn.Jumps;
            Kills = rn.Kills;
            Station = rn.Station;
            Dest = rn.Dest;
            AltNode = rn.AltNode;
            StationTT = rn.StationTT;
            JumpTT = rn.JumpTT;
            IsoType = rn.IsoType;
            sTic = rn.sTic;
            CynoSafeMoon = rn.CynoSafeMoon;
        }

    }

}
