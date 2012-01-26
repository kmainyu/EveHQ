// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2012  EveHQ Development Team
// 
// This file is part of the "EveHQ RouteMap" plug-in
//
// EveHQ RouteMap derives from copyrighted works licensed under the GNU 
// General Public License originally created by Lhyda Souljacker.
//
// This version has been modified pursuant to the GNU General Public 
// License as of 26 January 20121, and as distributed, it includes or is 
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
    public enum GateType
    {
        Undefined = 0,
        Normal,
        InterConst,
        InterRegion
    }

    [Serializable]
    public class StarGate
    {
        public SolarSystem From;
        public SolarSystem To;
        public GateType Type;
        public double radius;
        public double X, Y, Z;
        public int graphicID;
        public string destSys;
        public int ID;

        public StarGate()
        {
            From = new SolarSystem();
            To = new SolarSystem();
            Type = GateType.Undefined;
            radius = 0;
            X = 0;
            Y = 0;
            Z = 0;
            graphicID = 0;
            destSys = "";
            ID = 0;
        }

        public StarGate(StarGate s)
        {
            From = new SolarSystem(s.From);
            To = new SolarSystem(s.To);
            Type = s.Type;
            radius = s.radius;
            X = s.X;
            Y = s.Y;
            Z = s.Z;
            graphicID = s.graphicID;
            destSys = s.destSys;
            ID = s.ID;
        }

    }
}
