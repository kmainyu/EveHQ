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
    public class Launchpad
    {
        public int ID;
        public long typeID;
        public int graphicID;
        public string Name;
        public string Desc;
        public int Mass;
        public int Volume;
        public int Capacity;
        public int Cost;
        public int CPU;
        public int Power;
        public int ptypeID;
        public int InpTax;
        public int ExpTax;
        public SortedList<int, string> Items;
        public string Location;
        public Point CLoc;
        public string facID;

        public Launchpad()
        {
            ID = 0;
            typeID = 0;
            graphicID = 0;
            Name = "";
            Desc = "";
            Mass = 0;
            Volume = 0;
            Capacity = 0;
            Cost = 0;
            CPU = 0;
            Power = 0;
            ptypeID = 0;
            InpTax = 0;
            ExpTax = 0;
            Items = new SortedList<int, string>();
            Location = "";
            CLoc = new Point(0, 0);
            facID = "";
        }

        public Launchpad(Launchpad c)
        {
            ID = c.ID;
            typeID = c.typeID;
            graphicID = c.graphicID;
            Name = c.Name;
            Desc = c.Desc;
            Mass = c.Mass;
            Volume = c.Volume;
            Capacity = c.Capacity;
            Cost = c.Cost;
            CPU = c.CPU;
            Power = c.Power;
            ptypeID = c.ptypeID;
            InpTax = c.InpTax;
            ExpTax = c.ExpTax;
            Items = new SortedList<int, string>();

            foreach (var v in c.Items)
                Items.Add(v.Key, v.Value);

            Location = c.Location;
            CLoc = new Point(c.CLoc.X, c.CLoc.Y);
            facID = c.facID;
        }

    }
}
