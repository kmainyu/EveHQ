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
    public class MSRMineralList
    {
        public SortedList msrMineral;
        public DataSet mineralData, qtyData;
        enum minA { grpID, grpName, typID, typName, typDesc, grphID, mass, vol, pSize, bPrice };

        public MSRMineralList()
        {
            msrMineral = new SortedList();
        }

        private decimal GetDecimalFromVariableIA(DataRow dr, int aI_1, int aI_2)
        {
            decimal retVal = 0;

            if (!dr.ItemArray[aI_1].Equals(System.DBNull.Value))
            {
                retVal = Convert.ToDecimal(dr.ItemArray[aI_1]);
            }
            else
            {
                retVal = Convert.ToDecimal(dr.ItemArray[aI_2]);
            }

            return retVal;
        }

        public void PerformSQLQuery()
        {
            string strSQL;

            //Mineral Attribute Data
            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.graphicID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice";
            strSQL += " FROM invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE ((invGroups.groupID=427) OR (invGroups.groupID=428) OR (invGroups.groupID=429)) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            mineralData = EveHQ.Core.DataFunctions.GetData(strSQL);
        }

        public void PopulateMSRData()
        {
            string strSQL;
            MoonSiloReactMineral msr;

            foreach (DataRow row in mineralData.Tables[0].Rows)
            {
                msr = new MoonSiloReactMineral();

                msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                
                GetImage(msr.typeID);

                msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();

                strSQL = "SELECT *";
                strSQL += " FROM dgmTypeAttributes";
                strSQL += " WHERE (dgmTypeAttributes.attributeID=726) AND (dgmTypeAttributes.typeID=" + msr.typeID + ");";
                qtyData = EveHQ.Core.DataFunctions.GetData(strSQL);

                msr.reactQty = GetDecimalFromVariableIA(qtyData.Tables[0].Rows[0], 2, 3);

                msrMineral.Add(msr.typeID, msr);
            }
        }

        public void PopulateMineralListing(Object o)
        {
            PerformSQLQuery();
            PopulateMSRData();
            SaveMSRListing();
            PlugInData.resetEvents[6].Set();
        }

        public void GetImage(Object o)
        {
            Bitmap bmp;
            string imgLoc, imgId;

            
            imgId = o.ToString();

            imgLoc = EveHQ.Core.ImageHandler.GetImageLocation(imgId, Convert.ToInt32(EveHQ.Core.ImageHandler.ImageType.Blueprints));

            try
            {
                bmp = new Bitmap(Image.FromFile(imgLoc));
            }
            catch
            {
            }

        }

        public void SaveMSRListing()
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
            PoSManage_Path = Path.Combine(PoSBase_Path, "PoSManage");
            PoSCache_Path = Path.Combine(PoSManage_Path, "Cache");

            if (!Directory.Exists(PoSManage_Path))
                Directory.CreateDirectory(PoSManage_Path);

            if (!Directory.Exists(PoSCache_Path))
                Directory.CreateDirectory(PoSCache_Path);

            fname = Path.Combine(PoSCache_Path, "MSR_List.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, msrMineral);
            pStream.Close();
        }

        public void LoadMSRListing()
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
            PoSManage_Path = Path.Combine(PoSBase_Path, "PoSManage");
            PoSCache_Path = Path.Combine(PoSManage_Path, "Cache");

            if (!Directory.Exists(PoSManage_Path))
                return;
            if (!Directory.Exists(PoSCache_Path))
                return;

            fname = Path.Combine(PoSCache_Path, "MSR_List.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    msrMineral = (SortedList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }


    }
}
