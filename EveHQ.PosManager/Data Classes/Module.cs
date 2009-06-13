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
    public class Module
    {
        public Defense Armor, Shield, Struct;
        public string State, Name, Desc, OtherInfo;
        public string Location, Category, Charge;
        public int typeID, groupID, ChargeGroup, React1, React2;
        public decimal Qty, SovLevel, ScanRes, SigRad, ChargeSize;
        public decimal CPU, Power, CPU_Used, Power_Used, Optimal, FallOff;
        public decimal Anchor_Time, Online_Time, UnAnchor_Time;
        public decimal Volume, Cost, Capacity, Cycle_Period, Activation;
        public decimal SwitchDelay, Proximity, MaxTarget, SigRes;
        public decimal PriSkill, PriSkillLvl, WarpScramStr, ShieldRechTime;
        public decimal JF_Type, JF_Per_LY, JF_MassMult, MaxPerSystem;
        public decimal MissileRange;
        public double Grav_JamStr, Ladr_JamStr, Magn_JamStr, Radr_JamStr;
        public double ROF, MaxJump, DamageMod, Tracking, MaxSecLev, RefineYield;
        public double EnergyNeut, CombineFire, TargetRangeMod, TargetSpeedMod;
        public double TargetMaxSpeedMod, TargetMaxTargetsMod;
        public ArrayList Charges;
        public ArrayList ChargeList;
        public MT_Bonus Bonuses;
        public ArrayList Extra;

        public Module()
        {
            Armor = new Defense(0, 0, 0, 0, 0);
            Shield = new Defense(0, 0, 0, 0, 0);
            Struct = new Defense(0, 0, 0, 0, 0);
            Name = "";
            typeID = 0;
            groupID = 0;
            Qty = 1;
            State = "Online";  // Online, Offline, Reinforced
            Desc = "";
            OtherInfo = "";
            Location = "";
            Category = "";
            Charge = "";
            CPU = 0;
            CPU_Used = 0;
            Power = 0;
            Power_Used = 0;
            Anchor_Time = 0;
            Online_Time = 0;
            UnAnchor_Time = 0;
            Volume = 0;
            Cost = 0;
            Capacity = 0;
            Cycle_Period = 0;
            ROF = 0;
            SwitchDelay = 0;
            Proximity = 0;
            Bonuses = new MT_Bonus();
            SovLevel = 0;
            ScanRes = 0;
            SigRad = 0;
            MaxJump = 0;
            MaxSecLev = 0;
            MaxTarget = 0;
            DamageMod = 0;
            ChargeSize = 0;
            ChargeGroup = 0;
            Tracking = 0;
            React1 = 0;
            React2 = 0;
            RefineYield = 0;
            Optimal = 0;
            FallOff = 0;
            Activation = 0;
            EnergyNeut = 0;
            SigRes = 0;
            CombineFire = 0;
            PriSkill = 0;
            PriSkillLvl = 0;
            Grav_JamStr = 0;
            Magn_JamStr = 0;
            Ladr_JamStr = 0;
            Radr_JamStr = 0;
            WarpScramStr = 0;
            ShieldRechTime = 0;
            TargetMaxSpeedMod = 0;
            TargetMaxTargetsMod = 0;
            TargetRangeMod = 0;
            TargetSpeedMod = 0;
            JF_MassMult = 0;
            JF_Per_LY = 0;
            JF_Type = 0;
            MaxPerSystem = 0;
            MissileRange = 0;
            Charges = new ArrayList();
            ChargeList = new ArrayList();
            Extra = new ArrayList();
        }

        public Module(Module m)
        {
            Armor = new Defense(m.Armor);
            Shield = new Defense(m.Shield);
            Struct = new Defense(m.Struct);
            Name = m.Name;
            typeID = m.typeID;
            groupID = m.groupID;
            Qty = m.Qty;
            State = m.State;  // Online, Offline, Reinforced
            Desc = m.Desc;
            Charge = m.Charge;
            OtherInfo = m.OtherInfo;
            Location = m.Location;
            Category = m.Category;
            CPU = m.CPU;
            CPU_Used = m.CPU_Used;
            Power = m.Power;
            Power_Used = m.Power_Used;
            Anchor_Time = m.Anchor_Time;
            Online_Time = m.Online_Time;
            UnAnchor_Time = m.UnAnchor_Time;
            Volume = m.Volume;
            Cost = m.Cost;
            Capacity = m.Capacity;
            Cycle_Period = m.Cycle_Period;
            ROF = m.ROF;
            SwitchDelay = m.SwitchDelay;
            Proximity = m.Proximity;
            Bonuses = new MT_Bonus(m.Bonuses);
            SovLevel = m.SovLevel;
            ScanRes = m.ScanRes;
            SigRad = m.SigRad;
            MaxJump = m.MaxJump;
            MaxSecLev = m.MaxSecLev;
            MaxTarget = m.MaxTarget;
            DamageMod = m.DamageMod;
            ChargeSize = m.ChargeSize;
            ChargeGroup = m.ChargeGroup;
            Tracking = m.Tracking;
            React1 = m.React1;
            React2 = m.React2;
            RefineYield = m.RefineYield;
            Optimal = m.Optimal;
            FallOff = m.FallOff;
            Activation = m.Activation;
            EnergyNeut = m.EnergyNeut;
            SigRes = m.SigRes;
            CombineFire = m.CombineFire;
            PriSkill = m.PriSkill;
            PriSkillLvl = m.PriSkillLvl;
            Grav_JamStr = m.Grav_JamStr;
            Magn_JamStr = m.Magn_JamStr;
            Ladr_JamStr = m.Ladr_JamStr;
            Radr_JamStr = m.Radr_JamStr;
            WarpScramStr = m.WarpScramStr;
            ShieldRechTime = m.ShieldRechTime;
            TargetMaxSpeedMod = m.TargetMaxSpeedMod;
            TargetMaxTargetsMod = m.TargetMaxTargetsMod;
            TargetRangeMod = m.TargetRangeMod;
            TargetSpeedMod = m.TargetSpeedMod;
            JF_MassMult = m.JF_MassMult;
            JF_Per_LY = m.JF_Per_LY;
            JF_Type = m.JF_Type;
            MaxPerSystem = m.MaxPerSystem;
            MissileRange = m.MissileRange;
            Charges = new ArrayList(m.Charges);
            ChargeList = new ArrayList(m.ChargeList);
            Extra = new ArrayList(m.Extra);
        }

    }
}
