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

namespace EveHQ.PI
{
    [Serializable]
    public class CommandCenter
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
        public int Power;
        public int CPU;
        public int ExportTax;
        public int ptypeID;
        public SortedList<int, string> Items;
        public string Location;
        public int CPU_Used;
        public int Power_Used;
        public Point CLoc;

        public CommandCenter()
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
            Power = 0;
            CPU = 0;
            ExportTax = 0;
            ptypeID = 0;
            Items = new SortedList<int, string>();
            Location = "";
            CPU_Used = 0;
            Power_Used = 0;
            CLoc = new Point(10, 150);
        }

        public CommandCenter(CommandCenter c)
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
            Power = c.Power;
            CPU=c.CPU;
            ExportTax = c.ExportTax;
            ptypeID = c.ptypeID;
            Items = new SortedList<int, string>();
            foreach (var v in c.Items)
                Items.Add(v.Key, v.Value);
            Location = c.Location;
            CPU_Used = c.CPU_Used;
            Power_Used = c.Power_Used;
            CLoc = new Point(c.CLoc.X, c.CLoc.Y);
        }
    }
}
