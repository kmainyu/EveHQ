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
    class CategoryItem
    {
        public string CatName;
        public int groupID;

        public CategoryItem()
        {
            CatName = "";
            groupID = 0;
        }

        public CategoryItem(string cn, int gi)
        {
            CatName = cn;
            groupID = gi;
        }

        public CategoryItem(CategoryItem ci)
        {
            CatName = ci.CatName;
            groupID = ci.groupID;
        }
    }
}
