using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.PosManager
{
    [Serializable]
    class InOutData
    {
        public decimal typeID, qty;

        public InOutData()
        {
            typeID = 0;
            qty = 0;
        }

        public InOutData(decimal tid, decimal q)
        {
            typeID = tid;
            qty = q;
        }

        public InOutData(InOutData io)
        {
            typeID = io.typeID;
            qty = io.qty;
        }

    }
}
