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
    public class MoonSiloReactMineral
    {
        public decimal typeID, groupID, reactQty;
        public decimal volume, portionSize, basePrice, mass, mktPrice;
        public string name, description, groupName, icon;

        public MoonSiloReactMineral()
        {
            typeID = 0;
            groupID = 0;
            reactQty = 0;
            volume = 0;
            portionSize = 0;
            basePrice = 0;
            mass = 0;
            mktPrice = 0;
            name = "";
            description = "";
            groupName = "";
            icon = "";
        }

        public MoonSiloReactMineral(MoonSiloReactMineral ms)
        {
            typeID = ms.typeID;
            groupID = ms.groupID;
            reactQty = ms.reactQty;
            volume = ms.volume;
            portionSize = ms.portionSize;
            basePrice = ms.basePrice;
            mass = ms.mass;
            mktPrice = ms.mktPrice;
            name = ms.name;
            description = ms.description;
            groupName = ms.groupName;
            icon = ms.icon;
        }
    }
}
