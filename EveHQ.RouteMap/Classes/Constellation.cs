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
    public class Constellation
    {
        public Dictionary<int, SolarSystem> Systems;
        public List<int> AdjConstJump;
        public int ID;
        public string Name;
        public int RegionID;
        public Color CColor;
        private Rectangle _bounds;
        public decimal X, Y, Z, XMin, XMax, YMin, YMax, ZMin, ZMax, Radius;
        public int FactID;
        public Rectangle FlatArea;

        public Constellation()
        {
            AdjConstJump = new List<int>();
            ID = 0;
            Name = "";
            RegionID = 0;
            Systems = new Dictionary<int, SolarSystem>();
            CColor = Color.Azure;
            _bounds = new Rectangle();
            X = 0;
            Y = 0;
            Z = 0;
            XMin = 0;
            XMax = 0;
            YMin = 0;
            YMax = 0;
            ZMin = 0;
            ZMax = 0;
            Radius = 0;
            FactID = 0;
            FlatArea = new Rectangle();
        }

        public Constellation CopyConstellation(Constellation b)
        {
            Constellation a = new Constellation();
            AdjConstJump = new List<int>();
            foreach (int i in b.AdjConstJump)
                a.AdjConstJump.Add(i);

            a.ID = b.ID;
            a.Name = b.Name;
            a.RegionID = b.RegionID;
            a.Systems = new Dictionary<int, SolarSystem>();
            foreach (var sys in b.Systems)
                a.Systems.Add(sys.Key, sys.Value.CopySystem(sys.Value));

            a.CColor = b.CColor;
            _bounds = new Rectangle();
            a._bounds = new Rectangle(b._bounds.X, b._bounds.Y, b._bounds.Width, b._bounds.Height);
            a.X = b.X;
            a.Y = b.Y;
            a.Z = b.Z;
            a.XMin = b.XMin;
            a.XMax = b.XMax;
            a.YMin = b.YMin;
            a.YMax = b.YMax;
            a.ZMin = b.ZMin;
            a.ZMax = b.ZMax;
            a.Radius = b.Radius;
            a.FactID = b.FactID;
            a.FlatArea = new Rectangle(b.FlatArea.X, b.FlatArea.Y, b.FlatArea.Width, b.FlatArea.Height);

            return a;
        }

        public Constellation(Constellation b)
        {
            AdjConstJump = new List<int>();
            foreach (int i in b.AdjConstJump)
                AdjConstJump.Add(i);

            ID = b.ID;
            Name = b.Name;
            RegionID = b.RegionID;
            Systems = new Dictionary<int, SolarSystem>();
            foreach (var sys in b.Systems)
                Systems.Add(sys.Key, sys.Value.CopySystem(sys.Value));

            CColor = b.CColor;
            _bounds = new Rectangle(b._bounds.X, b._bounds.Y, b._bounds.Width, b._bounds.Height);
            X = b.X;
            Y = b.Y;
            Z = b.Z;
            XMin = b.XMin;
            XMax = b.XMax;
            YMin = b.YMin;
            YMax = b.YMax;
            ZMin = b.ZMin;
            ZMax = b.ZMax;
            Radius = b.Radius;
            FactID = b.FactID;
            FlatArea = new Rectangle(b.FlatArea.X, b.FlatArea.Y, b.FlatArea.Width, b.FlatArea.Height);
        }

        public Rectangle Bounds
        {
            get
            {
                return GetBounds();
            }
        }

        public Rectangle GetBounds()
        {
            SystemCoordinates sys;

            if ((_bounds.Size == Size.Empty) || (_bounds.X == int.MaxValue))
            {
                Point Min = new Point(int.MaxValue, int.MaxValue);
                Point Max = new Point(int.MinValue, int.MinValue);
                foreach (SolarSystem ss in Systems.Values)
                {
                    if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(ss.ID)))
                    {
                        sys = PlugInData.SystemCoords[Convert.ToInt64(ss.ID)];

                        if (sys.OrgCoord.X < Min.X)
                            Min.X = sys.OrgCoord.X;
                        if (sys.OrgCoord.Y < Min.Y)
                            Min.Y = sys.OrgCoord.Y;
                        if (sys.OrgCoord.X > Max.X)
                            Max.X = sys.OrgCoord.X;
                        if (sys.OrgCoord.Y > Max.Y)
                            Max.Y = sys.OrgCoord.Y;
                    }
                    else
                    {
                        if (ss.OrgCoord.X < Min.X)
                            Min.X = ss.OrgCoord.X;
                        if (ss.OrgCoord.Y < Min.Y)
                            Min.Y = ss.OrgCoord.Y;
                        if (ss.OrgCoord.X > Max.X)
                            Max.X = ss.OrgCoord.X;
                        if (ss.OrgCoord.Y > Max.Y)
                            Max.Y = ss.OrgCoord.Y;
                    }
                }
                _bounds = new Rectangle(Min.X, Min.Y, Max.X - Min.X, Max.Y - Min.Y);
            }
            return _bounds;
        }

        public Rectangle GetCurrentBounds()
        {
            SystemCoordinates sys;

            Point Min = new Point(int.MaxValue, int.MaxValue);
            Point Max = new Point(int.MinValue, int.MinValue);
            foreach (SolarSystem ss in Systems.Values)
            {
                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(ss.ID)))
                {
                    sys = PlugInData.SystemCoords[Convert.ToInt64(ss.ID)];

                    if (sys.OrgCoord.X < Min.X)
                        Min.X = sys.OrgCoord.X;
                    if (sys.OrgCoord.Y < Min.Y)
                        Min.Y = sys.OrgCoord.Y;
                    if (sys.OrgCoord.X > Max.X)
                        Max.X = sys.OrgCoord.X;
                    if (sys.OrgCoord.Y > Max.Y)
                        Max.Y = sys.OrgCoord.Y;
                }
                else
                {
                    if (ss.OrgCoord.X < Min.X)
                        Min.X = ss.OrgCoord.X;
                    if (ss.OrgCoord.Y < Min.Y)
                        Min.Y = ss.OrgCoord.Y;
                    if (ss.OrgCoord.X > Max.X)
                        Max.X = ss.OrgCoord.X;
                    if (ss.OrgCoord.Y > Max.Y)
                        Max.Y = ss.OrgCoord.Y;
                }
                _bounds = new Rectangle(Min.X, Min.Y, Max.X - Min.X, Max.Y - Min.Y);
            }
            return _bounds;
        }


        public static bool operator ==(Constellation a, Constellation b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (((object)a == null) || ((object)b == null))
                return false;
            return a.ID == b.ID;
        }

        public static bool operator !=(Constellation a, Constellation b)
        {
            return !(a == b);
        }

        public bool Equals(Constellation obj)
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
            if (obj.GetType() != typeof(Constellation))
                return false;
            return Equals((Constellation)obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public void FillFlatBoundaries()
        {
            Point FlatMin = new Point(int.MaxValue, int.MaxValue);
            Point FlatMax = new Point(int.MinValue, int.MinValue);

            foreach (var syst in Systems)
            {
                if (syst.Value.FlatCoords.X < FlatMin.X) 
                    FlatMin.X = syst.Value.FlatCoords.X;
                if (syst.Value.FlatCoords.X > FlatMax.X) 
                    FlatMax.X = syst.Value.FlatCoords.X;
                if (syst.Value.FlatCoords.Y < FlatMin.Y) 
                    FlatMin.Y = syst.Value.FlatCoords.Y;
                if (syst.Value.FlatCoords.Y > FlatMax.Y) 
                    FlatMax.Y = syst.Value.FlatCoords.Y;
            }

            FlatArea = Rectangle.FromLTRB(FlatMin.X, FlatMin.Y, FlatMax.X, FlatMax.Y);
        }

    }
}
