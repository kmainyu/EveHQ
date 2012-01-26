// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2012  EveHQ Development Team
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

namespace EveHQ.PI
{
    [Serializable]
    public class Link
    {
        public string FromMod;
        public string ToMod;
        public double Distance;
        public int Capacity;
        public int CPU;
        public int Power;
        public int Level;

        public Link()
        {
            FromMod = "";
            ToMod = "";
            Distance = 0;
            Capacity = 0;
            CPU = 0;
            Power = 0;
            Level = 0;
        }

        public Link(Link l)
        {
            FromMod = l.FromMod;
            ToMod = l.ToMod;
            Distance = l.Distance;
            Capacity = l.Capacity;
            CPU = l.CPU;
            Power = l.Power;
            Level = l.Level;
        }

        public Link(string f, string t, double d, int c, int cpu, int p, int l)
        {
            FromMod = f;
            ToMod = t;
            Distance = d;
            Capacity = c;
            CPU = cpu;
            Power = p;
            Level = l;
        }

    }
}
