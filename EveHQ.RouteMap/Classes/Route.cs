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
    public class Route
    {
        public string RouteName;
        public Dictionary<SolarSystem, bool> AvoidSystems;   // ssData, bool
        public bool UseCynoBeacon;
        public bool UseJumpBridge;
        public bool PreferStation;
        public bool UseCynoSafeTwr;
        public SolarSystem Start;
        public SolarSystem Dest;
        public List<SolarSystem> WayPoints;                 
        public EveHQ.Core.Pilot EvePilot;
        public double ShipJumpRange;
        public double ShipFuelPerLY;
        public double minSec, maxSec;
        public Ship JumpShip;
        public List<RouteNode> RouteSystems;

        public Route()
        {
            RouteName = "";
            Start = new SolarSystem();
            Dest = new SolarSystem();
            AvoidSystems = new Dictionary<SolarSystem, bool>();
            WayPoints = new List<SolarSystem>();
            UseCynoBeacon = true;
            UseJumpBridge = true;
            PreferStation = true;
            UseCynoSafeTwr = true;
            EvePilot = new EveHQ.Core.Pilot();
            ShipJumpRange = 0;
            ShipFuelPerLY = 0;
            minSec = 0;
            maxSec = 1;
            JumpShip = new Ship();
            RouteSystems = new List<RouteNode>();
        }

        public Route(SolarSystem st, SolarSystem ds, ArrayList AS, ArrayList WPL, EveHQ.Core.Pilot P, Ship js, double mns, double mxs)
        {
            RouteName = "";
            Start = new SolarSystem(st);
            Dest = new SolarSystem(ds);
            AvoidSystems = new Dictionary<SolarSystem, bool>();
            WayPoints = new List<SolarSystem>();
            UseCynoBeacon = true;
            UseJumpBridge = true;
            PreferStation = true;
            UseCynoSafeTwr = true;
            EvePilot = P;
            ShipJumpRange = 0;
            ShipFuelPerLY = 0;
            JumpShip = js;
            RouteSystems = new List<RouteNode>();
            minSec = mns;
            maxSec = mxs;

            BuildAvoidListing(AS);
            BuildWayPointListing(WPL);
        }

        public void BuildAvoidListing(ArrayList AS)
        {
            SolarSystem ss;

            if (AS == null)
                AS = new ArrayList();

            AvoidSystems.Clear();
            foreach (string s in AS)
            {
                ss = new SolarSystem(PlugInData.GalMap.GetSystemByName(s));
                AvoidSystems.Add(ss, true);
            }
        }

        public void BuildWayPointListing(ArrayList WPL)
        {
            SolarSystem ss;

            if (WPL == null)
                WPL = new ArrayList();

            WayPoints.Clear();
            foreach (string s in WPL)
            {
                ss = new SolarSystem(PlugInData.GalMap.GetSystemByName(s));
                WayPoints.Add(ss);
            }
        }


    }
}
