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
    public class Reaction
    {
        public ArrayList inputs;
        public ArrayList outputs;
        public string reactName, desc, reactGroupName, icon;
        public decimal typeID, groupID;

        public Reaction()
        {
            inputs = new ArrayList();
            outputs = new ArrayList();
            typeID = 0;
            groupID = 0;
            reactName = "";
            reactGroupName = "";
            desc = "";
            icon = "";
        }

        public Reaction(Reaction r)
        {
            inputs = new ArrayList(r.inputs);
            outputs = new ArrayList(r.outputs);
            typeID = r.typeID;
            groupID = r.groupID;
            reactName = r.reactName;
            reactGroupName = r.reactGroupName;
            desc = r.desc;
            icon = r.icon;
        }
    }
}
