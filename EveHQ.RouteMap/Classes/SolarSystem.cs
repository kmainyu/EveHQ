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
    public class SolarSystem
    {
        private const float AU = 149597870691; // In meters
        public Point OrgCoord;
        public int ConstID;
        public int RegionID;
        public int ID;
        public string Name;
        public double Security;
        public string SecClass;
        public PointF_3D Coords;
        public SortedList<int, ConqStation> ConqStations;
        public SortedList<int, Station> Stations;
        public int Planets;
        public int Moons;
        public int OreBelts;
        public int IceBelts;
        public decimal XMin, XMax, YMin, YMax, ZMin, ZMax, Luminosity, FactID, Radius, SunTypID;
        public Point FlatCoords;
        public ArrayList Gates;

        public enum RelativePosition
        {
            Top,
            TopLeft,
            TopRight,
            CenterLeft,
            CenterRight,
            Bottom,
            BottomLeft,
            BottomRight,
            Center,
            None
        };
        public RelativePosition LabelLocation;

        public SolarSystem()
        {
            OrgCoord = new Point();
            ConstID = 0;
            RegionID = 0;
            ID = 0;
            Name = "";
            Security = 0;
            SecClass = "";
            Coords = new PointF_3D();
            Planets = 0;
            Moons = 0;
            OreBelts = 0;
            IceBelts = 0;
            Stations = new SortedList<int, Station>();
            ConqStations = null;
            XMin = 0;
            XMax = 0;
            YMax = 0;
            YMin = 0;
            ZMax = 0;
            ZMin = 0;
            Luminosity = 0;
            FactID = 0;
            Radius = 0;
            SunTypID = 0;
            LabelLocation = RelativePosition.Top;
            FlatCoords = Point.Empty;
            Gates = new ArrayList();
        }

        public double GetJumpDistance(SolarSystem to)
        {
            return Coords.Distance(to.Coords);
        }

        public SolarSystem CopySystem(SolarSystem b)
        {
            SolarSystem a = new SolarSystem();
            a.OrgCoord = new Point(b.OrgCoord.X, b.OrgCoord.Y);
            a.ConstID = b.ConstID;
            a.RegionID = b.RegionID;
            a.ID = b.ID;
            a.Name = b.Name;
            a.Security = b.Security;
            a.SecClass = b.SecClass;
            a.Coords = new PointF_3D(b.Coords.X, b.Coords.Y, b.Coords.Z);
            a.Planets = b.Planets;
            a.Moons = b.Moons;
            a.OreBelts = b.OreBelts;
            a.IceBelts = b.IceBelts;
            a.Stations = new SortedList<int, Station>();
            foreach (var pr in b.Stations)
                a.Stations.Add(pr.Key, pr.Value);

            ConqStations = new SortedList<int, ConqStation>();
            foreach (var cs in b.ConqStations)
                a.ConqStations.Add(cs.Key, cs.Value);

            a.XMin = b.XMin;
            a.XMax = b.XMax;
            a.YMax = b.YMax;
            a.YMin = b.YMin;
            a.ZMax = b.ZMax;
            a.ZMin = b.ZMin;
            a.Luminosity = b.Luminosity;
            a.FactID = b.FactID;
            a.Radius = b.Radius;
            a.SunTypID = b.SunTypID;
            a.LabelLocation = b.LabelLocation;
            a.FlatCoords = new Point(b.FlatCoords.X, b.FlatCoords.Y);

            if (Gates != null)
            {
                foreach (int id in Gates)
                    a.Gates.Add(id);
            }
            return a;
        }

        public SolarSystem(SolarSystem b)
        {
            OrgCoord = new Point(b.OrgCoord.X, b.OrgCoord.Y);
            ConstID = b.ConstID;
            RegionID = b.RegionID;
            ID = b.ID;
            Name = b.Name;
            Security = b.Security;
            SecClass = b.SecClass;
            Coords = new PointF_3D(b.Coords.X, b.Coords.Y, b.Coords.Z);

            Planets = b.Planets;
            Moons = b.Moons;
            OreBelts = b.OreBelts;
            IceBelts = b.IceBelts;

            Stations = new SortedList<int, Station>();
            foreach (var pr in b.Stations)
                Stations.Add(pr.Key, new Station(pr.Value));

            if (b.ConqStations != null)
            {
                ConqStations = new SortedList<int, ConqStation>();
                foreach (var cs in b.ConqStations)
                    ConqStations.Add(cs.Key, cs.Value);
            }
            else
                ConqStations = null;

            XMin = b.XMin;
            XMax = b.XMax;
            YMax = b.YMax;
            YMin = b.YMin;
            ZMax = b.ZMax;
            ZMin = b.ZMin;
            Luminosity = b.Luminosity;
            FactID = b.FactID;
            Radius = b.Radius;
            SunTypID = b.SunTypID;
            LabelLocation = b.LabelLocation;
            FlatCoords = new Point(b.FlatCoords.X, b.FlatCoords.Y);

            if (Gates != null)
            {
                foreach (int id in Gates)
                    b.Gates.Add(id);
            }
        }

        public static bool operator ==(SolarSystem a, SolarSystem b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.ID == b.ID;
        }

        public static bool operator !=(SolarSystem a, SolarSystem b)
        {
            return !(a == b);
        }

        public bool Equals(SolarSystem obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.ID == ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(SolarSystem))
                return false;
            return Equals((SolarSystem)obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public string GetName()
        {
            string retStr;

            retStr = Name + " (" + Security;

            if ((Security == 1) || (Security == 0))
                retStr += ".0";

            retStr += ")";

            return retStr;
        }

        public int GetSystemIndex()
        {
            return Convert.ToInt32(Math.Max(Math.Round(Security, 1), 0) * 10);
        }

        public Color GetSystemColor()
        {
            int index = GetSystemIndex();

            if ((index < 0) || (index > 10))
                return Color.Cyan;

            return (Color)Galaxy.SecColor.SecurityColors[index];
        }

        public float GetSystemSize()
        {
            float sysRad;

            sysRad = ((float)Radius / AU) * 2;
            
            return (float)Math.Round(sysRad, 1);
        }

    }
}
