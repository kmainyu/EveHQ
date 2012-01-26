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
    public class Reaction
    {
        public SortedList<int,Component> inputs;
        public SortedList<int, Component> outputs;
        public string reactName, desc, reactGroupName, icon;
        public int typeID, cycleTime, level;

        public Reaction()
        {
            inputs = new SortedList<int, Component>();
            outputs = new SortedList<int, Component>();
            typeID = 0;
            cycleTime = 0;
            reactName = "";
            reactGroupName = "";
            desc = "";
            icon = "";
            level = 0;
        }

        public Reaction(Reaction r)
        {
            inputs = new SortedList<int, Component>();
            foreach(var v in r.inputs)
                inputs.Add(v.Key, v.Value);

            outputs = new SortedList<int, Component>();
            foreach(var v in r.outputs)
                outputs.Add(v.Key, v.Value);

            typeID = r.typeID;
            cycleTime = r.cycleTime;
            reactName = r.reactName;
            reactGroupName = r.reactGroupName;
            desc = r.desc;
            icon = r.icon;
            level = r.level;
        }
    }
}
