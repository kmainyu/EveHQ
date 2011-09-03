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
    public class ConfigData2
    {
        public SortedList<string, bool> CFlags; 
        public SortedList<string, Color> MapColors;
        public SortedList<string, int> Weights;
        public SortedList<string, int> Sizes;
        public SortedList<string, float> Zooms;
        public SortedList<string, int> Settings;
        public SortedList<string, string> StrSet;
        public SortedList<EveHQ.Core.Pilot, Ship> SelPilotShip;

        public EveHQ.Core.Pilot SelPilot;
        public Ship SelShip;

        public ConfigData2()
        {
            CFlags = new SortedList<string, bool>();
            CFlags.Add("Show Region Names", true);
            CFlags.Add("Show Const Names", true);
            CFlags.Add("Show System Names", true);
            CFlags.Add("Show Detail Text", true);
            CFlags.Add("Show Jump Bridges", true);
            CFlags.Add("Show Cyno Beacons", true);
            CFlags.Add("Show Sovreignty", true);
            CFlags.Add("Show Bridge Details", true);
            CFlags.Add("Recompute Jump Route", true);
            CFlags.Add("Recompute Gate Route", true);
            CFlags.Add("Show Search Results", true);
            CFlags.Add("Show Bridge Ranges", true);
            CFlags.Add("Show Jump From Ranges", true);
            CFlags.Add("Show Jump To Ranges", true);

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
            MapColors.Add("Search Result Highlight", Color.Orange);

            Weights = new SortedList<string, int>();
            Weights.Add("Gate Avoid", 1);
            Weights.Add("Gate High", 1);
            Weights.Add("Gate Bridge", 10);
            Weights.Add("Gate Default", 5);
            Weights.Add("Jump Avoid", 1);
            Weights.Add("Jump High", 1);
            Weights.Add("Jump Cyno", 9);
            Weights.Add("Jump Bridge", 10);
            Weights.Add("Jump Default", 5);
            Weights.Add("Jump Station", 8);
            Weights.Add("Jump Tower", 9);

            Sizes = new SortedList<string, int>();
            Sizes.Add("Sys Marker Diameter", 6);
            Sizes.Add("Sys Marker Thickness", (Sizes["Sys Marker Diameter"] / 2));
            Sizes.Add("Sys Marker Size", (Sizes["Sys Marker Diameter"] * 2));

            SelPilotShip = new SortedList<Core.Pilot, Ship>();

            Settings = new SortedList<string, int>();
            Settings.Add("Detail Text", 0); //None, Belts[Ice], Ore, Stations, Moons, Sov, Rats, Kills, Jumps, Starbase
            Settings.Add("Normal Gate Alpha", 250);
            Settings.Add("Const Gate Alpha", 250);
            Settings.Add("Region Gate Alpha", 250);

            Zooms = new SortedList<string, float>();
            Zooms.Add("Zoom On Search", 0.9f);

            StrSet = new SortedList<string, string>();
            StrSet.Add("ActMon_1", "");
            StrSet.Add("ActMon_2", "");
            StrSet.Add("ActMon_3", "");
            StrSet.Add("ActMon_4", "");

            // Misc - Values needed in several storage areas
            CFlags.Add("Right Panel Expanded", true);
            CFlags.Add("Left Panel Expanded", true);
            CFlags.Add("Bottom Panel Expanded", true);
            Sizes.Add("Right Panel Size", 200);
            Sizes.Add("Left Panel Size", 225);
            Sizes.Add("Bottom Panel Size", 443);

            SelShip = null;
            SelPilot = null;
        }

        public void SetGateWeights(int High, int Bridge, int Default, int Avoid)
        {
            Weights["Gate Avoid"] = Avoid;
            Weights["Gate High"] = High;
            Weights["Gate Bridge"] = Bridge;
            Weights["Gate Default"] = Default;
        }

        public void SetJumpWeights(int High, int Default, int Bridge, int Beacon, int Station, int Avoid, int Tower)
        {
            Weights["Jump Avoid"] = Avoid;
            Weights["Jump High"] = High;
            Weights["Jump Cyno"] = Beacon;
            Weights["Jump Bridge"] = Bridge;
            Weights["Jump Default"] = Default;
            Weights["Jump Station"] = Station;
            Weights["Jump Tower"] = Tower;
        }

        public void ConvertOlderConfigData(ConfigData oc)
        {
            CFlags = new SortedList<string, bool>();
            CFlags.Add("Show Region Names", oc.ShowRegionNames);
            CFlags.Add("Show Const Names", oc.ShowConstNames);
            CFlags.Add("Show System Names", oc.ShowSystemNames);
            CFlags.Add("Show Detail Text", oc.ShowDetailText);
            CFlags.Add("Show Jump Bridges", oc.ShowJumpBridges);
            CFlags.Add("Show Cyno Beacons", oc.ShowCynoBeacons);
            CFlags.Add("Show Sovreignty", oc.ShowSovreignty);
            CFlags.Add("Show Bridge Details", oc.ShowDetailsOnHover);
            CFlags.Add("Recompute Jump Route", oc.RecomputeJump);
            CFlags.Add("Recompute Gate Route", oc.RecomputeGate);
            CFlags.Add("Show Search Results", false);
            CFlags.Add("Show Bridge Ranges", false);
            CFlags.Add("Show Jump From Ranges", false);
            CFlags.Add("Show Jump To Ranges", false);

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
            MapColors.Add("Search Result Highlight", Color.Orange);
            foreach (string sc in oc.MapColors.Keys)
                MapColors[sc] = oc.MapColors[sc];

            Weights = new SortedList<string, int>();
            Weights.Add("Gate Avoid", oc.GateAvoidWeight);
            Weights.Add("Gate High", oc.GateHighWeight);
            Weights.Add("Gate Bridge", oc.GateJBWeight);
            Weights.Add("Gate Default", oc.GateDefaultWeight);
            Weights.Add("Jump Avoid", oc.JumpAvoidWeight);
            Weights.Add("Jump High", oc.JumpHighWeight);
            Weights.Add("Jump Cyno", oc.JumpCynoWeight);
            Weights.Add("Jump Bridge", oc.JumpJBWeight);
            Weights.Add("Jump Default", oc.JumpDefaultWeight);
            Weights.Add("Jump Station", oc.JumpStationWeight);
            Weights.Add("Jump Tower", oc.SafeTowerWeight);

            Sizes = new SortedList<string, int>();
            Sizes.Add("Sys Marker Diameter", oc.SysMarkerDiameter);
            Sizes.Add("Sys Marker Thickness", oc.SysMarkerThickness);
            Sizes.Add("Sys Marker Size", oc.SysMarkerSize);

            SelPilotShip = new SortedList<Core.Pilot, Ship>();

            Settings = new SortedList<string, int>();
            Settings.Add("Detail Text", oc.DetailTextType); //None, Belts[Ice], Ore, Stations, Moons, Sov, Rats, Kills, Jumps, Starbase
            Settings.Add("Normal Gate Alpha", oc.NormalGateAlpha);
            Settings.Add("Const Gate Alpha", oc.ConstGateAlpha);
            Settings.Add("Region Gate Alpha", oc.RegnGateAlpha);

            Zooms = new SortedList<string, float>();
            Zooms.Add("Zoom On Search", oc.ZoomOnSearch);

            StrSet = new SortedList<string, string>();

            // Misc - Values needed in several storage areas
            CFlags.Add("Right Panel Expanded", (bool)oc.Extra[0]);
            CFlags.Add("Left Panel Expanded", (bool)oc.Extra[4]);
            CFlags.Add("Bottom Panel Expanded", (bool)oc.Extra[2]);
            Sizes.Add("Right Panel Size", (int)oc.Extra[1]);
            Sizes.Add("Left Panel Size", (int)oc.Extra[5]);
            Sizes.Add("Bottom Panel Size", (int)oc.Extra[3]);

            SelShip = oc.SelShip;
            SelPilot = oc.SelPilot;
        }


    }
}
