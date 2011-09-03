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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveHQ.PosManager
{
    public class ReactMod : IComparable
    {
        public string name;
        public string timeS;
        public decimal runT;
        public int capQ;
        public int maxQ;

        public int CompareTo(object obj)
        {
            ReactMod rm = (ReactMod)obj;

            return this.runT.CompareTo(rm.runT);
        }

        public ReactMod(string n, string t, decimal r, int c, int m)
        {
            name = n;
            timeS = t;
            runT = r;
            capQ = c;
            maxQ = m;
        }
    }
}
