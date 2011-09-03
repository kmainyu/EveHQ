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
    public class Resource
    {
        public long groupID;
        public long typeID;
        public string Name, Desc, icon;
        public double Volume, Cost;
        public int iconID;
        public List<string> Planets;

        public Resource()
        {
            groupID = 0;
            typeID = 0;
            Name = "";
            Desc = "";
            icon = "";
            Volume = 0;
            Cost = 0;
            iconID = 0;
            Planets = new List<string>();
        }

        public Resource(Resource r)
        {
            groupID = r.groupID;
            typeID = r.typeID;
            Name = r.Name;
            Desc = r.Desc;
            icon = r.icon;
            Volume = r.Volume;
            Cost = r.Cost;
            iconID = r.iconID;
            Planets = new List<string>();
            foreach (string s in r.Planets)
                Planets.Add(s);
        }

    }
}
