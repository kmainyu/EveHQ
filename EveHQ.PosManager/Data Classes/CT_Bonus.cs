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
    public class CT_Bonus
    {
        public double LaserDmg, LaserOpt, LaserProxRange;
        public double HybOpt, HybProxRange, HybDmg;
        public double EWRof, EWTargetSwitch;
        public double PrjOpt, PrjFallOff, PrjROF, PrjProxRange;
        public double MslROF, MslVel;
        public double MoonHvstCPU, SiloCap;
        public ArrayList Extra;

        public CT_Bonus()
        {
            LaserDmg = 0;
            LaserOpt = 0;
            LaserProxRange = 0;
            HybDmg = 0;
            HybOpt = 0;
            HybProxRange = 0;
            EWRof = 0;
            EWTargetSwitch = 0;
            PrjFallOff = 0;
            PrjOpt = 0;
            PrjProxRange = 0;
            PrjROF = 0;
            MslROF = 0;
            MslVel = 0;
            MoonHvstCPU = 0;
            SiloCap = 0;
            Extra = new ArrayList();
        }

        public CT_Bonus(CT_Bonus c)
        {
            LaserDmg = c.LaserDmg;
            LaserOpt = c.LaserOpt;
            LaserProxRange = c.LaserProxRange;
            HybDmg = c.HybDmg;
            HybOpt = c.HybOpt;
            HybProxRange = c.HybProxRange;
            EWRof = c.EWRof;
            EWTargetSwitch = c.EWTargetSwitch;
            PrjFallOff = c.PrjFallOff;
            PrjOpt = c.PrjOpt;
            PrjProxRange = c.PrjProxRange;
            PrjROF = c.PrjROF;
            MslROF = c.MslROF;
            MslVel = c.MslVel;
            MoonHvstCPU = c.MoonHvstCPU;
            SiloCap = c.SiloCap;
            Extra = new ArrayList(c.Extra);
        }
    }
}
