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
    public class FuelType
    {
        public string Name, UsedFor;
        public decimal BaseVol, QtyVol;
        public decimal Cost;
        public decimal Qty, BaseQty, PeriodQty, RunTime;
        public decimal VolForQty, CostForQty;
        public ArrayList Extra;

        public FuelType()
        {
            Name = "";
            UsedFor = "";
            BaseVol = 0;
            QtyVol = 0;
            Cost = 0;
            Qty = 0;
            BaseQty = 0;
            PeriodQty = 0;
            RunTime = 0;
            VolForQty = 0;
            CostForQty = 0;
            Extra = new ArrayList();
        }

        public FuelType(FuelType ft)
        {
            Name = ft.Name;
            UsedFor = ft.UsedFor;
            BaseVol = ft.BaseVol;
            QtyVol = ft.QtyVol;
            Cost = ft.Cost;
            Qty = ft.Qty;
            BaseQty = ft.BaseQty;
            PeriodQty = ft.PeriodQty;
            RunTime = ft.RunTime;
            VolForQty = ft.VolForQty;
            CostForQty = ft.CostForQty;
            Extra = new ArrayList(ft.Extra);
        }

        public void SetFuelRunTime(decimal mod, decimal sov_mod)
        {
            decimal perQ;

            if (mod > 0)
                perQ = Math.Ceiling(PeriodQty * mod);
            else
                perQ = PeriodQty;

            perQ = Math.Ceiling(perQ * sov_mod);

            if (perQ > 0)
                RunTime = (Qty / perQ);
            else
                RunTime = 0;
        }

        public decimal SetAndReturnFuelRunTime(decimal mod, decimal sov_mod)
        {
            decimal perQ;

            if (mod > 0)
                perQ = Math.Ceiling(PeriodQty * mod);
            else
                perQ = PeriodQty;

            perQ = Math.Ceiling(perQ * sov_mod);

            if (perQ > 0)
                RunTime = (Qty / perQ);
            else
                RunTime = 0;

            return RunTime;
        }

        public ArrayList SetAndReturnFuelRunTimeAndName(decimal mod, decimal sov_mod)
        {
            ArrayList retVal = new ArrayList();
            decimal perQ;

            if (mod > 0)
                perQ = Math.Ceiling(PeriodQty * mod);
            else
                perQ = PeriodQty;

            perQ = Math.Ceiling(perQ * sov_mod);

            if (perQ > 0)
                RunTime = (Qty / perQ);
            else
                RunTime = 0;

            retVal.Add(RunTime);
            retVal.Add(Name);

            return retVal;
        }


    }
}
