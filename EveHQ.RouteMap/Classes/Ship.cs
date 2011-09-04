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
    public class Ship
    {
        public int typeID;
        public int groupID;
        public int raceID;
        public string Name;
        public string Desc;
        public string Category;
        public double Mass;
        public double FuelConsumption;
        public double JumpDistance;
        public int fuelID;
        public bool CanGate;
        public double FuelBayCap;

        public Ship()
        {
            typeID = 0;
            groupID = 0;
            raceID = 0;
            Name = "";
            Desc = "";
            Category = "";
            Mass = 0.0;
            FuelConsumption = 0.0;
            JumpDistance = 0.0;
            fuelID = 0;
            CanGate = true;
            FuelBayCap = 0.0;
        }

        public Ship(Ship ns)
        {
            typeID = ns.typeID;
            groupID = ns.groupID;
            raceID = ns.raceID;
            Name = ns.Name;
            Desc = ns.Desc;
            Category = ns.Category;
            Mass = ns.Mass;
            FuelConsumption = ns.FuelConsumption;
            JumpDistance = ns.JumpDistance;
            fuelID = ns.fuelID;
            CanGate = ns.CanGate;
            FuelBayCap = ns.FuelBayCap;
        }

        public static double GetFuel(double dist, Route rParm, int typ)
        {
            double retVal;

            switch (typ)
            {
                case 0: // Cyno
                    retVal = dist * rParm.ShipFuelPerLY;
                    break;
                case 1: // Bridge
                    retVal = (rParm.JumpShip.Mass/1000000000) * 500 * dist;
                    break;
                default:
                    retVal = 0;
                    break;
            }

            return retVal;
        }

        public string GetIsoType()
        {
            switch (fuelID)
            {
                case 17887:
                    return " O2";
                case 17888:
                    return " N2";
                case 17889:
                    return " H2";
                case 16274:
                    return " He";
                default:
                    return " na";
            }
        }
    }
}
