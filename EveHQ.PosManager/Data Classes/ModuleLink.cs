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
    public class ModuleLink
    {        
        public long modID, sysID, typID;
        public string twrName;
        public string modLoc;
        public Color LinkColor;

        public ModuleLink()
        {
            modID = 0;
            sysID = 0;
            typID = 0;
            twrName = "";
            modLoc = "";
            LinkColor = Color.Silver;
        }

        public ModuleLink(ModuleLink ml)
        {
            modID = ml.modID;
            sysID = ml.sysID;
            typID = ml.typID;
            twrName = ml.twrName;
            modLoc = ml.modLoc;
            LinkColor = ml.LinkColor;
        }

        public ModuleLink(long mid, long sid, long tid, Color lc, string tn, string ml)
        {
            modID = mid;
            sysID = sid;
            typID = tid;
            twrName = tn;
            modLoc = ml;
            LinkColor = lc;
        }

    }
}
