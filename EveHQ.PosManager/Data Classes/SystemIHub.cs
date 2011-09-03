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

namespace EveHQ.PosManager
{
    [Serializable]
    public class SystemIHub
    {
        public long ID;
        public long ownerID;
        public long locID;
        public bool online;
        public string owner;
        public string itmName;
        public string location;
        public double milLev;
        public double indLev;
        public double strLev;
        public SortedList<string, InfrastructureUG> OreUG;
        public SortedList<string, InfrastructureUG> SrvUG;
        public SortedList<string, InfrastructureUG> EntUG;
        public SortedList<string, InfrastructureUG> PrtUG;
        public SortedList<string, InfrastructureUG> FlxUG;
        public SortedList<string, InfrastructureUG> StratUG;

        public SystemIHub()
        {
            ID = 0;
            ownerID = 0;
            locID = 0;
            online = false;
            owner = "";
            location = "";
            milLev = 0;
            indLev = 0;
            strLev = 0;
            OreUG = new SortedList<string, InfrastructureUG>();
            SrvUG = new SortedList<string, InfrastructureUG>();
            EntUG = new SortedList<string, InfrastructureUG>();
            PrtUG = new SortedList<string, InfrastructureUG>();
            FlxUG = new SortedList<string, InfrastructureUG>();
            StratUG = new SortedList<string, InfrastructureUG>();
        }
    }
}
