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
    public class Station
    {
        public long SSysID;
        public long ID, stationTypeID;
        public string Name;
        public string Owner;
        public bool isConquerable;
        public long Faction;
        public long corpID, regionID, constID;
        public long iconID;
        public double X, Y, Z, orbitR;

        public Station()
        {
            SSysID = 0;
            ID = 0;
            stationTypeID = 0;
            Name = "";
            Owner = "";
            isConquerable = false;
            Faction = 0;
            corpID = 0;
            regionID = 0;
            constID = 0;
            orbitR = 0;
            iconID = 0;
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Station CopyStation(Station b)
        {
            Station a = new Station();

            a.SSysID = b.SSysID;
            a.ID = b.ID;
            stationTypeID = b.stationTypeID;
            a.Name = b.Name;
            a.Owner = b.Owner;

            a.Faction = b.Faction;

            a.corpID = b.corpID;
            a.regionID = b.regionID;
            a.constID = b.constID;
            a.orbitR = b.orbitR;
            a.iconID = b.iconID;
            a.X = b.X;
            a.Y = b.Y;
            a.Z = b.Z;

            return a;
        }

        public Station(Station b)
        {
            SSysID = b.SSysID;
            ID = b.ID;
            stationTypeID = b.stationTypeID;
            Name = b.Name;
            Owner = b.Owner;

            Faction = b.Faction;

            corpID = b.corpID;
            regionID = b.regionID;
            constID = b.constID;
            orbitR = b.orbitR;
            iconID = b.iconID;
            X = b.X;
            Y = b.Y;
            Z = b.Z;
        }

        public static bool operator ==(Station a, Station b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.ID == b.ID;
        }

        public static bool operator !=(Station a, Station b)
        {
            return !(a == b);
        }

        public bool Equals(Station obj)
        {
            if(ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.ID == ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(Station))
                return false;
            return Equals((Station) obj);
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(ID);
        }
    }
}
