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
    public class LinkData
    {
        public string Name;
        public int typeID;
        public int Power;
        public int CPU;
        public int MCPerHour;
        public double MWPerKM;
        public double CPUPerKM;
        public double CPULoadMult;
        public double MWLoadMult;

        public LinkData()
        {
            Name = "";
            typeID = 0;
            Power = 0;
            CPU = 0;
            MCPerHour = 0;
            MWPerKM = 0;
            CPUPerKM = 0;
            CPULoadMult = 0;
            MWLoadMult = 0;
        }

        public LinkData(LinkData ld)
        {
            Name = ld.Name;
            typeID = ld.typeID;
            Power = ld.Power;
            CPU = ld.CPU;
            MCPerHour = ld.MCPerHour;
            MWPerKM = ld.MWPerKM;
            CPUPerKM = ld.CPUPerKM;
            CPULoadMult = ld.CPULoadMult;
            MWLoadMult = ld.MWLoadMult;
        }
    }
}
