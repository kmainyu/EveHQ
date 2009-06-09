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
    public class FuelBay
    {
        public FuelType EnrUran, Oxygen, MechPart, Coolant, Robotics;
        public FuelType HeIso, H2Iso, N2Iso, O2Iso;
        public FuelType HvyWater, LiqOzone, Charters, Strontium;
        public decimal   FuelCap, FuelUsed, StrontCap, StrontUsed;
        public ArrayList Extra;

        public FuelBay()
        {
            EnrUran = new FuelType();
            Oxygen = new FuelType();
            MechPart = new FuelType();
            Coolant = new FuelType();
            Robotics = new FuelType();
            HeIso = new FuelType();
            H2Iso = new FuelType();
            N2Iso = new FuelType();
            O2Iso = new FuelType();
            HvyWater = new FuelType();
            LiqOzone = new FuelType();
            Charters = new FuelType();
            Strontium = new FuelType();
            FuelCap = 0;
            FuelUsed = 0;
            StrontCap = 0;
            StrontUsed = 0;
            Extra = new ArrayList();
        }

        public FuelBay(FuelBay fb)
        {
            EnrUran = new FuelType(fb.EnrUran);
            Oxygen = new FuelType(fb.Oxygen);
            MechPart = new FuelType(fb.MechPart);
            Coolant = new FuelType(fb.Coolant);
            Robotics = new FuelType(fb.Robotics);
            HeIso = new FuelType(fb.HeIso);
            H2Iso = new FuelType(fb.H2Iso);
            N2Iso = new FuelType(fb.N2Iso);
            O2Iso = new FuelType(fb.O2Iso);
            HvyWater = new FuelType(fb.HvyWater);
            LiqOzone = new FuelType(fb.LiqOzone);
            Charters = new FuelType(fb.Charters);
            Strontium = new FuelType(fb.Strontium);
            FuelCap = fb.FuelCap;
            FuelUsed = fb.FuelUsed;
            StrontCap = fb.StrontCap;
            StrontUsed = fb.StrontUsed;
            Extra = new ArrayList(fb.Extra);
        }

    }
}
