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
    public class Player
    {
        public string Name, Email1, Email2, Email3;
        public string Corp;
        public string Ally;
        public long Roles;
        public long UserID;
        public long CorpID;
        public long AllyID;

        public Player()
        {
            Name = "";
            Corp = "";
            Ally = "";
            Roles = 0;
            UserID = 0;
            CorpID = 0;
            AllyID = 0;
            Email1 = "";
            Email2 = "";
            Email3 = "";
        }

        public Player(Player p)
        {
            Name = p.Name;
            Corp = p.Corp;
            Ally = p.Ally;
            Roles = p.Roles;
            UserID = p.UserID;
            CorpID = p.CorpID;
            AllyID = p.AllyID;
            Email1 = p.Email1;
            Email2 = p.Email2;
            Email3 = p.Email3;
        }

        public Player(string nm, string e1, string e2, string e3)
        {
            Name = nm;
            Email1 = e1;
            Email2 = e2;
            Email3 = e3;
        }
    }
}
