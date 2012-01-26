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
    public class MT_Bonus
    {
        public double MslVelocity, MslFlightTime, MslExplRad, MslExplVel, MslDamage;
        public double TrtTrackSpeed, TrtOptimal;
        public double ThermalDmgRes, KineticDmgRes, ExplosiveDmgRes, EmpDmgRes;
        public ArrayList Extra;

        public MT_Bonus()
        {
            MslExplRad = 0;
            MslExplVel = 0;
            MslFlightTime = 0;
            MslVelocity = 0;
            MslDamage = 0;
            TrtOptimal = 0;
            TrtTrackSpeed = 0;
            ThermalDmgRes = 0;
            KineticDmgRes = 0;
            ExplosiveDmgRes = 0;
            EmpDmgRes = 0;
            Extra = new ArrayList();
        }

        public MT_Bonus(MT_Bonus m)
        {
            MslExplRad = m.MslExplRad;
            MslExplVel = m.MslExplVel;
            MslFlightTime = m.MslFlightTime;
            MslVelocity = m.MslVelocity;
            MslDamage = m.MslDamage;
            TrtOptimal = m.TrtOptimal;
            TrtTrackSpeed = m.TrtTrackSpeed;
            ThermalDmgRes = m.ThermalDmgRes;
            KineticDmgRes = m.KineticDmgRes;
            ExplosiveDmgRes = m.ExplosiveDmgRes;
            EmpDmgRes = m.EmpDmgRes;
            Extra = new ArrayList(m.Extra);
        }
    }
}
