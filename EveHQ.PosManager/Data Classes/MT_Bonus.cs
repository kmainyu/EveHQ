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
