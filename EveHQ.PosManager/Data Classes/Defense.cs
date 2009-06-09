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
