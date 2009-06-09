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
    class ConfigData
    {
        public FuelBay FuelCosts;
        public int SortedColumnIndex;
        public int MonSelIndex;
        public SortOrder MonSortOrder;
        public ArrayList Extra;
        public string SelPos;

        public ConfigData()
        {
            FuelCosts = new FuelBay();
            Extra = new ArrayList();
            SortedColumnIndex = 3;
            MonSelIndex = 0;
            SelPos = "";
            MonSortOrder = SortOrder.Ascending;
        }
    }
}
