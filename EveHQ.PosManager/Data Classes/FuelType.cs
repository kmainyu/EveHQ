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
            Extra = new ArrayList();
        }

        public FuelType(string n, string uf, decimal v, decimal qv, decimal c, decimal q, decimal bq, decimal pq, decimal rt, ArrayList ext)
        {
            Name = n;
            UsedFor = uf;
            BaseVol = v;
            QtyVol = qv;
            Cost = c;
            Qty = q;
            BaseQty = bq;
            PeriodQty = pq;
            RunTime = rt;
            Extra = new ArrayList(ext);
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
            Extra = new ArrayList(ft.Extra);
        }
    }
}
