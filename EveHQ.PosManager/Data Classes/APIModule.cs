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

namespace EveHQ.PosManager
{
    [Serializable]
    public class APIModule
    {
        public long typeID;      // Module type
        public long systemID;    // Must ensure it is a system and not a station :)
        public long modID;       // Note: Repackaging an item will change it's ID
        public long corpID;      // Owning Corporation
        public long charID;      // Owning Character
        public long Qty;
        public bool InStation;
        public bool InShip;
        public bool InSpace;
        public string name;
        public string corpName;
        public string updateTime;
        public string ownerName;
        public string locName;
        public SortedList<long, PlugInData.ModuleItem> Items; // Items contained inside the module

        public APIModule()
        {
            typeID = 0;
            systemID = 0;
            modID = 0;
            corpID = 0;
            charID = 0;
            Qty = 0;
            InStation = false;
            InSpace = false;
            InShip = false;
            name = "";
            corpName = "";
            updateTime="";
            ownerName = "";
            locName = "";
            Items = new SortedList<long, PlugInData.ModuleItem>();
        }

        public long GetQtyOfItemInModule(string itmName)
        {
            long retV = 0;

            foreach (PlugInData.ModuleItem mi in Items.Values)
            {
                if (mi.name == itmName)
                    return mi.qty;
            }

            return retV;
        }

        public long GetQtyOfItemInModule(long itmID)
        {
            long retV = 0;

            if (Items.ContainsKey(itmID))
                return Items[itmID].qty;

            return retV;
        }
    }
}
