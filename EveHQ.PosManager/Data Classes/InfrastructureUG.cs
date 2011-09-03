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
    public class InfrastructureUG
    {
        public long ID;
        public long typeID;
        public bool online;
        public string name;
        public string colNm;
        public double volume;
        public int cell;

        public InfrastructureUG()
        {
            ID = 0;
            typeID = 0;
            online = false;
            name = "";
            colNm = "";
            volume = 0;
            cell = 0;
        }

        public InfrastructureUG(InfrastructureUG I)
        {
            ID = I.ID;
            typeID = I.typeID;
            online = I.online;
            name = I.name;
            colNm = I.colNm;
            volume = I.volume;
            cell = I.cell;
        }

    }
}
