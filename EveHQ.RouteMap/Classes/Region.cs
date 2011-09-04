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
    public class Region
    {
        public int ID;
        public string Name;
        public Dictionary<int, Constellation> Constellations;
        private Rectangle _bounds;
        public Color RColor;
        public decimal X, Y, Z, XMin, XMax, YMin, YMax, ZMin, ZMax;
        public int FactID;
        public Rectangle FlatArea;

        public enum EveRatType
        {
            Angel,
            Drone,
            Sansha,
            Gurista,
            Blood,
            Serpentis,
            None
        };

        public Region()
        {
            ID = 0;
            Name = "";
            Constellations = new Dictionary<int, Constellation>();
            RColor = Color.LimeGreen;
            X = 0;
            Y = 0;
            Z = 0;
            XMin = 0;
            XMax = 0;
            YMin = 0;
            YMax = 0;
            ZMin = 0;
            ZMax = 0;
            FactID = 0;
            FlatArea = new Rectangle();
        }

        public void FillFlatBoundaries()
        {
            Point FlatMin = new Point(int.MaxValue, int.MaxValue);
            Point FlatMax = new Point(int.MinValue, int.MinValue);

            foreach (var constellation in Constellations)
            {
                constellation.Value.FillFlatBoundaries();

                if (constellation.Value.FlatArea.Left < FlatMin.X) FlatMin.X = constellation.Value.FlatArea.Left;
                if (constellation.Value.FlatArea.Right > FlatMax.X) FlatMax.X = constellation.Value.FlatArea.Right;
                if (constellation.Value.FlatArea.Top < FlatMin.Y) FlatMin.Y = constellation.Value.FlatArea.Top;
                if (constellation.Value.FlatArea.Bottom > FlatMax.Y) FlatMax.Y = constellation.Value.FlatArea.Bottom;
            }

            FlatArea = Rectangle.FromLTRB(FlatMin.X, FlatMin.Y, FlatMax.X, FlatMax.Y);
        }

        public Rectangle Bounds
        {
            get
            {
                if (_bounds.Size != SizeF.Empty)
                    return _bounds;

                return GetBounds();
            }
        }

        public Rectangle GetBounds()
        {
            Point min = new Point(int.MaxValue, int.MaxValue);
            Point max = new Point(int.MinValue, int.MinValue);
            foreach (var cnst in Constellations)
            {
                Rectangle bnds = cnst.Value.Bounds;
                if (bnds.Left < min.X)
                    min.X = bnds.Left;
                if (bnds.Top < min.Y)
                    min.Y = bnds.Top;
                if ((bnds.Left + bnds.Width) > max.X)
                    max.X = (bnds.Left + bnds.Width);
                if ((bnds.Top + bnds.Height) > max.Y)
                    max.Y = (bnds.Top + bnds.Height);
            }
            _bounds = new Rectangle(min.X, min.Y, max.X - min.X, max.Y - min.Y);
            return _bounds;
        }

        public Rectangle GetCurrentBounds()
        {
            Point min = new Point(int.MaxValue, int.MaxValue);
            Point max = new Point(int.MinValue, int.MinValue);
            foreach (var cnst in Constellations)
            {
                Rectangle bnds = cnst.Value.GetCurrentBounds();
                if (bnds.Left < min.X)
                    min.X = bnds.Left;
                if (bnds.Top < min.Y)
                    min.Y = bnds.Top;
                if ((bnds.Left + bnds.Width) > max.X)
                    max.X = (bnds.Left + bnds.Width);
                if ((bnds.Top + bnds.Height) > max.Y)
                    max.Y = (bnds.Top + bnds.Height);
            }
            _bounds = new Rectangle(min.X, min.Y, max.X - min.X, max.Y - min.Y);
            return _bounds;
        }

        public string GetRatType()
        {
            switch (Name)
            {
                case "Cache":
                case "Curse":
                case "Detorid":
                case "Feythabolis":
                case "Great Wildlands":
                case "Heimatar":
                case "Immensea":
                case "Insmother":
                case "Metropolis":
                case "Molden Heath":
                case "Omist":
                case "Scalding Pass":
                case "Tenerifis":
                case "Wicked Creek":
                case "Impass":
                    return "Angel";
                case "Cobalt Edge":
                case "Etherium Reach":
                case "Malpais":
                case "Oasa":
                case "Perrigen Falls":
                case "The Kalevale Expanse":
                case "The Spire":
                    return "Drone";
                case "Catch":
                case "Esoteria":
                case "Paragon Soul":
                case "Providence":
                case "Stain":
                    return "Sansha";
                case "Branch":
                case "Deklein":
                case "Geminate":
                case "Lonetrek":
                case "Tenal":
                case "The Citadel":
                case "The Forge":
                case "Tribute":
                case "Vale of the Silent":
                case "Venal":
                case "Black Rise":
                    return "Gurista";
                case "Delve":
                case "Period Basis":
                case "Querious":
                case "Tash-Murkon":
                case "Kador":
                case "Khanid":
                case "Kor-Azor":
                case "Aridia":
                case "Derelik":
                case "Devoid":
                case "Domain":
                case "Genesis":
                case "The Bleak Lands":
                    return "Blood";
                case "Cloud Ring":
                case "Essence":
                case "Everyshore":
                case "Fade":
                case "Fountain":
                case "Outer Ring":
                case "Placid":
                case "Sinq Laison":
                case "Solitude":
                case "Syndicate":
                case "Verge Vendor":
                case "Pure Blind":
                    return "Serp";
                default:
                    return "None";
            }
        }

    }
}
