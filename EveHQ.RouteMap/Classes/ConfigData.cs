// ========================================================================
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

namespace EveHQ.RouteMap
{
    [Serializable]
    public class ConfigData
    {
        public bool ShowRegionNames;
        public bool ShowConstNames;
        public bool ShowSystemNames;
        public bool ShowDetailText;
        public bool ShowJumpBridges;
        public bool ShowCynoBeacons;
        public bool ShowSovreignty;
        public bool ShowDetailsOnHover;
        public bool RecomputeJump;
        public bool RecomputeGate;

        public SortedList<string, Color> MapColors;

        public int GateAvoidWeight;
        public int GateJBWeight;
        public int GateDefaultWeight;
        public int GateHighWeight;
        public int JumpAvoidWeight;
        public int JumpJBWeight;
        public int JumpCynoWeight;
        public int JumpStationWeight;
        public int JumpDefaultWeight;
        public int JumpHighWeight;
        public int SafeTowerWeight;
        public int SysMarkerDiameter;
        public int SysMarkerThickness;
        public int SysMarkerSize;
        public int DetailTextType;
        public int NormalGateAlpha;
        public int ConstGateAlpha;
        public int RegnGateAlpha;

        public float ZoomOnSearch;

        public EveHQ.Core.Pilot SelPilot;
        public Ship SelShip;

        public ArrayList Extra;
        // 0 - Right Panel Expanded State, Bool
        // 1 - Right Panel Expanded Width, Int
        // 2 - Bottom Panel Expanded State, Bool
        // 3 - Bottom Panel Expanded Height, Int
        // 4 - Left Panel Expaned State, Bool
        // 5 - Left Panel Expaned Width, Int
        // 6 - Activity Monitor 1 System Name
        // 7 - Activity Monitor 2 System Name
        // 8 - Activity Monitor 3 System Name
        // 9 - Activity Monitor 4 System Name

        public ConfigData()
        {
            ShowRegionNames = true;
            ShowConstNames = true;
            ShowSystemNames = true;
            ShowDetailText = true;
            ShowJumpBridges = true;
            ShowCynoBeacons = true;
            ShowSovreignty = true;
            ShowDetailsOnHover = true;
            RecomputeJump = false;
            RecomputeGate = false;

            MapColors = new SortedList<string, Color>();
            MapColors.Add("Background", Color.Black);
            MapColors.Add("System", Color.LightBlue);
            MapColors.Add("System Details", Color.Orange);
            MapColors.Add("Cyno Beacon", Color.LimeGreen);
            MapColors.Add("Normal Gate", Color.Blue);
            MapColors.Add("Constellation Gate", Color.DarkMagenta);
            MapColors.Add("Region Gate", Color.DarkRed);
            MapColors.Add("Jump Bridge Link", Color.Silver);
            MapColors.Add("Gate Route", Color.White);
            MapColors.Add("Jump Route", Color.LimeGreen);
            MapColors.Add("Null Sec Details", Color.Red);
            MapColors.Add("Low Sec Details", Color.Gold);
            MapColors.Add("High Sec Details", Color.Green);
            MapColors.Add("Bridge Range Highlight", Color.Lime);
            MapColors.Add("Ship Jump Range Highlight", Color.LightBlue);
            MapColors.Add("Ship Jump To Range Highlight", Color.GreenYellow);
            MapColors.Add("Search Result Highlight", Color.OrangeRed);

            GateAvoidWeight = 1;
            GateHighWeight = 1;
            GateJBWeight = 10;
            GateDefaultWeight = 5;
            JumpAvoidWeight = 1;
            JumpHighWeight = 1;
            JumpCynoWeight = 9;
            JumpJBWeight = 10;
            JumpDefaultWeight = 5;
            JumpStationWeight = 8;
            SafeTowerWeight = 9;

            SysMarkerDiameter = 6;
            SysMarkerThickness = SysMarkerDiameter / 2;
            SysMarkerSize = 2 * SysMarkerDiameter;

            SelPilot = null;
            SelShip = null;

            DetailTextType = 0; //None, Belts[Ice], Ore, Stations, Moons, Sov, Rats, Kills, Jumps, Starbase

            ZoomOnSearch = 0.9f;
            NormalGateAlpha = 250;
            ConstGateAlpha = 250;
            RegnGateAlpha = 250;

            Extra = new ArrayList();
            Extra.Add(true);
            Extra.Add((int)200);
            Extra.Add(true);
            Extra.Add((int)225);
            Extra.Add(true);
            Extra.Add((int)443);
            Extra.Add("");
            Extra.Add("");
            Extra.Add("");
            Extra.Add("");
        }

        public void SetGateWeights(int High, int Bridge, int Default)
        {
            GateHighWeight = High;
            GateJBWeight = Bridge;
            GateDefaultWeight = Default;
        }

        public void SetJumpWeights(int High, int Default, int Bridge, int Beacon, int Station)
        {
            JumpHighWeight = High;
            JumpJBWeight = Bridge;
            JumpDefaultWeight = Default;
            JumpCynoWeight = Beacon;
            JumpStationWeight = Station;
        }

    }
}
