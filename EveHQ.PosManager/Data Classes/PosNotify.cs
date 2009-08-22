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
    public class PosNotify
    {
        public string Tower;
        public string Type, Initial, Frequency;
        public decimal InitQty, FreqQty;
        public DateTime Notify_Sent;
        public bool Notify_Active;
        public PlayerList PList;

        public PosNotify()
        {
            Tower = "";
            Type = "";
            Initial = "";
            Frequency = "";
            InitQty = 1;
            FreqQty = 1;
            Notify_Active = false;
            Notify_Sent = DateTime.Now;
            PList = new PlayerList();
        }

        public PosNotify(PosNotify pn)
        {
            Tower = pn.Tower;
            Type = pn.Type;
            Initial = pn.Initial;
            Frequency = pn.Frequency;
            InitQty = pn.InitQty;
            FreqQty = pn.FreqQty;
            Notify_Active = pn.Notify_Active;
            Notify_Sent = pn.Notify_Sent;
            PList = new PlayerList(pn.PList);
        }

    }
}
