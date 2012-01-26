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
    public class Defense
    {
        public double EMP, Explosive, Kinetic, Thermal;
        public double EMP_M, Explosive_M, Kinetic_M, Thermal_M;
        public decimal Amount;
        public ArrayList Extra;

        public Defense()
        {
            EMP = 0;
            Explosive = 0;
            Kinetic = 0;
            Thermal = 0;
            Amount = 0;
            Extra = new ArrayList();
        }

        public Defense(double em, double ex, double k, double th, decimal a)
        {
            EMP = em;
            Explosive = ex;
            Kinetic = k;
            Thermal = th;
            Amount = a;
            Extra = new ArrayList();
        }

        public Defense(Defense d)
        {
            EMP = d.EMP;
            Explosive = d.Explosive;
            Kinetic = d.Kinetic;
            Thermal = d.Thermal;
            Amount = d.Amount;
            Extra = new ArrayList(d.Extra);
        }

    }
}
