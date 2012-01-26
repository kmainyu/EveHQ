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

namespace EveHQ.PosManager
{
    [Serializable]
    public class MoonSiloReactMineral
    {
        public decimal typeID, groupID, reactQty;
        public decimal volume, portionSize, basePrice, mass, mktPrice;
        public string name, description, groupName, icon;

        public MoonSiloReactMineral()
        {
            typeID = 0;
            groupID = 0;
            reactQty = 0;
            volume = 0;
            portionSize = 0;
            basePrice = 0;
            mass = 0;
            mktPrice = 0;
            name = "";
            description = "";
            groupName = "";
            icon = "";
        }

        public MoonSiloReactMineral(MoonSiloReactMineral ms)
        {
            typeID = ms.typeID;
            groupID = ms.groupID;
            reactQty = ms.reactQty;
            volume = ms.volume;
            portionSize = ms.portionSize;
            basePrice = ms.basePrice;
            mass = ms.mass;
            mktPrice = ms.mktPrice;
            name = ms.name;
            description = ms.description;
            groupName = ms.groupName;
            icon = ms.icon;
        }
    }
}
