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
    class CategoryList
    {
        public ArrayList Cats;

        public CategoryList()
        {
            Cats = new ArrayList();
        }

        public void PopulateCategoryList(Object o)
        {
            string strSQL;
            DataSet cd;
            CategoryItem ci;

             // Get Category Listing
            strSQL = "SELECT invGroups.groupID,invGroups.groupName FROM invGroups WHERE invGroups.categoryID=23 AND invGroups.published=1 ORDER BY invGroups.groupName;";
            cd = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Go through the Catagory Listing, 1 at a time, and Build the resultant Data Table Set
            // For use during program processing
            if (cd.Tables.Count > 0)
            {
                Cats.Clear();

                // For Each Category
                foreach (DataRow dr in cd.Tables[0].Rows)
                {
                    ci = new CategoryItem(dr[1].ToString(), Convert.ToInt32(dr[0]));
                    Cats.Add(ci);
                }
            }
            SaveCategoryList();
            PlugInData.resetEvents[2].Set();
        }

        public void LoadCategoryList()
        {
            string PoSBase_Path, PoSManage_Path, PoSCache_Path, fname;
            Stream cStr;
            BinaryFormatter myBf;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = PoSBase_Path + @"\PoSManage";
            PoSCache_Path = PoSManage_Path + @"\Cache";

            if (!Directory.Exists(PoSManage_Path))
                return;
            if (!Directory.Exists(PoSCache_Path))
                return;

            fname = PoSCache_Path + @"\Cat_List.bin";
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Cats = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public void SaveCategoryList()
        {
            string PoSBase_Path, PoSManage_Path, PoSCache_Path, fname;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = PoSBase_Path + @"\PoSManage";
            PoSCache_Path = PoSManage_Path + @"\Cache";

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = PoSCache_Path + @"\Cat_List.bin";

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Cats);
            pStream.Close();
        }
    }
}
