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
    public class Player
    {
        public string Name, Email1, Email2, Email3;

        public Player()
        {
            Name = "";
            Email1 = "";
            Email2 = "";
            Email3 = "";
        }

        public Player(Player p)
        {
            Name = p.Name;
            Email1 = p.Email1;
            Email2 = p.Email2;
            Email3 = p.Email3;
        }

        public Player(string nm, string e1, string e2, string e3)
        {
            Name = nm;
            Email1 = e1;
            Email2 = e2;
            Email3 = e3;
        }
    }
}
