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
    class Charge
    {
        public string Name;
        public decimal Optimal, FallOff, Velocity, Size, DmgMult, SpdMult, Tracking;
        public decimal EM_Dmg, Kin_Dmg, Exp_Dmg, Thm_Dmg, Base_Shield, Base_Armor, FlightTime;
        public decimal ChargeSize, RangeMult, FlyRangeMult, Agility, ExpRange, DetRange;
        public int typeID;
        public ArrayList Extra;

        public Charge()
        {
            Name = "";
            Optimal = 0;
            FallOff = 0;
            Velocity = 0;
            EM_Dmg = 0;
            Kin_Dmg = 0;
            Exp_Dmg = 0;
            Thm_Dmg = 0;
            typeID = 0;
            Size = 0;
            DmgMult = 0;
            SpdMult = 0;
            Tracking = 0;
            Base_Armor = 0;
            Base_Shield = 0;
            ChargeSize = 0;
            RangeMult = 0;
            FlyRangeMult = 0;
            Agility = 0;
            ExpRange = 0;
            DetRange = 0;
            FlightTime = 0;
            Extra = new ArrayList();
        }

        public Charge(Charge c)
        {
            Name = c.Name;
            Optimal = c.Optimal;
            FallOff = c.FallOff;
            Velocity = c.Velocity;
            EM_Dmg = c.EM_Dmg;
            Kin_Dmg = c.Kin_Dmg;
            Exp_Dmg = c.Exp_Dmg;
            Thm_Dmg = c.Thm_Dmg;
            typeID = c.typeID;
            Size = c.Size;
            DmgMult = c.DmgMult;
            SpdMult = c.SpdMult;
            Tracking = c.Tracking;
            Base_Armor = c.Base_Armor;
            Base_Shield = c.Base_Shield;
            ChargeSize = c.ChargeSize;
            RangeMult = c.RangeMult;
            FlyRangeMult = c.FlyRangeMult;
            Agility = c.Agility;
            ExpRange = c.ExpRange;
            DetRange = c.DetRange;
            FlightTime = c.FlightTime;
            Extra = new ArrayList(c.Extra);
        }
    }
}
