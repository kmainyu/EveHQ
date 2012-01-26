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
    public class PointF_3D
    {
        public const double LY = 9.4605284E+15;
        public double X;
        public double Y;
        public double Z;

        public PointF_3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public PointF_3D()
        {
        }

        public static implicit operator PointF(PointF_3D a)
        {
            return new PointF((float)a.X, (float)a.Y);
        }

        public Point Convert2D()
        {
            return new Point(Convert.ToInt32(Math.Round(X)), Convert.ToInt32(Math.Round(Y)));
        }

        public double Distance(PointF_3D s1)
        {
            double dx, dy, dz, dd;

            dx = ((X - s1.X) / LY);
            dy = ((Y - s1.Y) / LY);
            dz = ((Z - s1.Z) / LY);

            dd = Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));

            return dd;
        }

        public double DistanceMeters(PointF_3D s1)
        {
            double dx, dy, dz, dd;

            dx = (X - s1.X);
            dy = (Y - s1.Y);
            dz = (Z - s1.Z);

            dd = Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));

            return dd;
        }
    }
}
