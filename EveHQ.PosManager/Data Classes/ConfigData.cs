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
        public bool maintChart, maintStront, noNegs;
        public ArrayList dgMonBool, dgDesBool;
        public decimal maintTP, maintPV;
        // Extra - Storage and Values

        public ConfigData()
        {
            FuelCosts = new FuelBay();
            Extra = new ArrayList();
            dgMonBool = new ArrayList();
            dgDesBool = new ArrayList();
            SortedColumnIndex = 3;
            MonSelIndex = 0;
            SelPos = "";
            MonSortOrder = SortOrder.Ascending;
            maintChart = true;
            maintStront = true;
            noNegs = false;
            maintTP = 0;
            maintPV = 1;
        }
    }
}
