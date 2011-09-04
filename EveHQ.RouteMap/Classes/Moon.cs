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
    public class Moon
    {
        private const double AU = 149597870691.0;
        public string Name;
        public double radius, orbitR, orbitP;
        public double X, Y, Z;
        public int ID;
        public int CelIndex, OrbitIndex;
        public int graphicID;

        public Moon()
        {
            Name = "";
            X = 0;
            Y = 0;
            Z = 0;
            ID = 0;
            CelIndex = 0;       // Planet Number
            OrbitIndex = 0;     // Moon Number
            radius = 0;
            orbitR = 0;
            orbitP = 0;
            graphicID = 0;
        }

        public Moon(Moon m)
        {
            Name = m.Name;
            X = m.X;
            Y = m.Y;
            Z = m.Z;
            ID = m.ID;
            CelIndex = m.CelIndex;       // Planet Number
            OrbitIndex = m.OrbitIndex;     // Moon Number
            radius = m.radius;
            orbitR = m.orbitR;
            orbitP = m.orbitP;
            graphicID = m.graphicID;
        }

        public double GetDistanceFromSun()
        {
            double dx, dy, dz, dd;

            dx = (X / AU);
            dy = (Y / AU);
            dz = (Z / AU);

            dd = Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));

            return dd;
        }

        public double GetMeterDistanceFromObjectPosition(double x1, double y1, double z1)
        {
            double dx, dy, dz, dd;

            dx = (X - x1);
            dy = (Y - y1);
            dz = (Z - z1);

            dd = Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));

            return dd;
        }

        public double GetKilometerDistanceFromObjectPosition(double x1, double y1, double z1)
        {
            double dx, dy, dz, dd;

            dx = (X - x1) / 1000;
            dy = (Y - y1) / 1000;
            dz = (Z - z1) / 1000;

            dd = Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));

            return dd;
        }

        public double GetAUDistanceFromObjectPosition(double x1, double y1, double z1)
        {
            double dx, dy, dz, dd;

            dx = (X - x1) / AU;
            dy = (Y - y1) / AU;
            dz = (Z - z1) / AU;

            dd = Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));

            return dd;
        }


    }
}
