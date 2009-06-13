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
    public class DamageProfile
    {
        public string Name;
        public decimal EMP, Exp, Kin, Thm;

        public DamageProfile()
        {
            Name = "";
            EMP = 0;
            Exp = 0;
            Kin = 0;
            Thm = 0;
        }
        public DamageProfile(string n, decimal em, decimal ex, decimal k, decimal t)
        {
            Name = n;
            EMP = em;
            Exp = ex;
            Kin = k;
            Thm = t;
        }
        public DamageProfile(DamageProfile dp)
        {
            Name = dp.Name;
            EMP = dp.EMP;
            Exp = dp.Exp;
            Kin = dp.Kin;
            Thm = dp.Thm;
        }
    }
}
