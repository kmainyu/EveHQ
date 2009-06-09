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
    class POSMonitorList
    {
        public string PoSName;
        public double FuelTime;
        public double StrontTime;
        public string Status;
        public string Linked;

        public POSMonitorList()
        {
            PoSName = "";
            FuelTime = 0;
            StrontTime = 0;
            Status = "Online";
            Linked = "";
        }
    }
}
