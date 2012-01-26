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
    public class Planet
    {
        private const double AU = 149597870691.0;
        public int ID;
        public double X, Y, Z;
        public double radius, orbitR, orbitP, orbitE;
        public int Belts;
        public int IceFields;
        public int Number;
        public string Name;
        public int CelIndex;
        public int graphicID;

        public Planet()
        {
            ID = 0;
            X = 0;
            Y = 0;
            Z = 0;
            Belts = 0;
            IceFields = 0;
            Number = 0;
            Name = "";
            CelIndex = 0;
            radius = 0;
            orbitR = 0;
            orbitP = 0;
            orbitE = 0;
            graphicID = 0;
        }

        public Planet(Planet p)
        {
            ID = p.ID;
            X = p.X;
            Y = p.Y;
            Z = p.Z;
            Belts = p.Belts;
            IceFields = p.IceFields;
            Number = p.Number;
            Name = p.Name;
            CelIndex = p.CelIndex;
            radius = p.radius;
            orbitR = p.orbitR;
            orbitP = p.orbitP;
            orbitE = p.orbitE;
            graphicID = p.graphicID;
        }

        public string GetPlanetTypeByGraphicID()
        {
            switch (graphicID)
            {
                case 3941:
                    return "Plasma";
                case 3832:
                    return "Temperate";
                case 3834:
                    return "Ice";
                case 3833:
                    return "Gas";
                case 3835:
                    return "Oceanic";
                case 3836:
                    return "Lava";
                case 3837:
                    return "Barren";
                case 3935:
                    return "Storm";
                case 202:
                    return "Shattered";
                default:
                    return "Unknown";
            }
        }

        public int GetPlanetNumber(string s)
        {
            int lastSpace;
            if (s.Contains("("))
            {
                string cutPlanetName = s.Substring(0, s.IndexOf(" ("));
                lastSpace = GetLastSpacePosition(cutPlanetName);
                return PlugInData.RomanToNumber(cutPlanetName.Substring(lastSpace + 1,
                                                             cutPlanetName.Length - lastSpace - 1));
            }
            lastSpace = GetLastSpacePosition(s);
            return PlugInData.RomanToNumber(s.Substring(lastSpace + 1, s.Length - lastSpace - 1));
        }

        private static int GetLastSpacePosition(string s)
        {
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (s[i] == ' ')
                    return i;
            }
            return -1;
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
