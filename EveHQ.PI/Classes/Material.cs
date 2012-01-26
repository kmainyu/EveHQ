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
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.PI
{
    [Serializable]
    public class Material
    {
        public string Name;
        public string FacName;
        public string ID;
        public double NeedHour;
        public double ProdHour;
        public double DeltaHour;
        public double UseHour;
        public double Value;
        public double NeedVol, ProdVol, DeltaVol;
        public int Rank;

        public Material()
        {
            Name = "";
            FacName = "";
            ID = "";
            NeedHour = 0;
            ProdHour = 0;
            DeltaHour = 0;
            UseHour = 0;
            Rank = 0;
            Value = 0;
            NeedVol = 0;
            ProdVol = 0;
            DeltaVol = 0;
        }
        public Material(Material m)
        {
            Name = m.Name;
            FacName = m.FacName;
            ID = m.ID;
            NeedHour = m.NeedHour;
            ProdHour = m.ProdHour;
            DeltaHour = m.DeltaHour;
            Rank = m.Rank;
            UseHour = m.UseHour;
            Value = m.Value;
            NeedVol = m.NeedVol;
            ProdVol = m.ProdVol;
            DeltaVol = m.DeltaVol;
        }
        public Material(string nm, string id, double nh, double ph, double dh, int r, string fn, double uh, double vl)
        {
            Name = nm;
            ID = id;
            FacName = fn;
            NeedHour = nh;
            ProdHour = ph;
            DeltaHour = dh;
            Rank = r;
            UseHour = uh;
            Value = vl;
        }
    }
}
