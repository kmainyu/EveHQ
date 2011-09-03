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
    public class IGBSecurity
    {
        public string Option;
        public string Modifier;
        public bool Active;
        public bool AND;

        public IGBSecurity()
        {
            Option = "";
            Modifier = "";
            Active = true;
            AND = false;
        }

        public IGBSecurity(IGBSecurity i)
        {
            Option = i.Option;
            Modifier = i.Modifier;
            Active = i.Active;
            AND = i.AND;
        }

        public IGBSecurity(string o, string m, bool a, bool act)
        {
            Option = o;
            Modifier = m;
            Active = act;
            AND = a;
        }

        public bool IsEqualTo(IGBSecurity i)
        {
            if ((Option == i.Option) && (Modifier == i.Modifier) &&
                (AND == i.AND))
            {
                return true;
            }

            return false;
        }
    }
}
