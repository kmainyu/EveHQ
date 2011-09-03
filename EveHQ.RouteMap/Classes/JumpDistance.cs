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
using System.Linq;

namespace EveHQ.RouteMap
{
    class JumpDistance : IComparable
    {
        public SolarSystem DestSystem;
        public double Distance;

        public JumpDistance()
        {
            DestSystem = new SolarSystem();
            Distance = 0.0;
        }

        public JumpDistance(SolarSystem ss, double dist)
        {
            DestSystem = new SolarSystem(ss);
            Distance = dist;
        }

        public int CompareTo(Object o)
        {
            JumpDistance jd = (JumpDistance)o;

            return DestSystem.Name.CompareTo(jd.DestSystem.Name);
            //    return 0;
            //if (this.Distance > jd.Distance)
            //    return 1;
            //else
            //    return -1;
        }
        public int Compare(Object o1, object o2)
        {
            JumpDistance jd = (JumpDistance)o1;
            JumpDistance jd2 = (JumpDistance)o2;

            return jd.CompareTo(jd2);
        }
    }
}
