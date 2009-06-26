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

        public void SetFuelQtyForPeriod(decimal sov, decimal cap, decimal used, decimal period)
        {
            Qty = GetFuelQtyForPeriod(sov, cap, used) * period;
        }

        public void DecrementFuelQtyForPeriod(decimal period, decimal sov, decimal cap, decimal used)
        {
            Qty = Math.Max((Qty - (GetFuelQtyForPeriod(sov, cap, used) * period)), 0);
        }

        public void IncrementFuelQtyForPeriod(decimal period, decimal sov, decimal cap, decimal used)
        {
            Qty += (GetFuelQtyForPeriod(sov, cap, used) * period);
        }

        public decimal GetFuelQtyForPeriod(decimal sov, decimal cap, decimal used)
        {
            decimal ret, pcMult;

            if ((cap == 1) && (used == 1))
            {
                ret = Math.Ceiling(sov * PeriodQty);
            }
            else
            {
                if(cap > 0)
                    pcMult = (used / cap);
                else
                    pcMult = 1;

                if(used > 0)
                    ret = Math.Floor((pcMult * sov * PeriodQty) + 1);
                else
                    ret = Math.Floor(pcMult * sov * PeriodQty);
            }


            return ret;
        }

        public void SetFuelRunTime(decimal sov, decimal cap, decimal used)
        {
            decimal perQ;
            perQ = GetFuelQtyForPeriod(sov, cap, used);

            if (perQ > 0)
                RunTime = (Qty / perQ);
            else
                RunTime = 999999;
        }

        public decimal SetAndReturnFuelRunTime(decimal sov, decimal cap, decimal used)
        {
            decimal perQ;
            perQ = GetFuelQtyForPeriod(sov, cap, used);

            if (perQ > 0)
                RunTime = (Qty / perQ);
            else
                RunTime = 999999;

            return RunTime;
        }

        public ArrayList SetAndReturnFuelRunTimeAndName(decimal sov, decimal cap, decimal used)
        {
            ArrayList retVal = new ArrayList();
            decimal perQ;
            perQ = GetFuelQtyForPeriod(sov, cap, used);


            if (perQ > 0)
                RunTime = (Qty / perQ);
            else
                RunTime = 999999;

            retVal.Add(RunTime);
            retVal.Add(Name);

            return retVal;
        }


    }
}
