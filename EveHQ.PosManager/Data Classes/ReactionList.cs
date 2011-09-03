// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
// 
// This file is part of EveHQ.
//
// EveHQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
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
    public class ReactionList
    {
        public ArrayList Reactions;
        public DataSet reactionData;
        enum minA { reacID, inpt, inTypID, qty, grpID, catID, grpName, grpDesc, grGID, UBP, allMan, allRec, anch, cAnc, fNS, gPub, tTID, tGID, tName, tDesc, tGrID, tRad, tMass, tVol, tCap, tPrtSz, tRID, tBPrc, tPub, tMktG, tCoD };

        public ReactionList()
        {
            Reactions = new ArrayList();
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

            //Module Attribute Data

            //strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.basePrice, invTypeReactions.input, invTypeReactions.typeID, invTypeReactions.quantity";
            //strSQL += " FROM invTypeReactions INNER JOIN (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) ON invTypes.typeID=invTypeReactions.reactionTypeID";
            //strSQL += " WHERE ((invGroups.groupID=436) OR (invGroups.groupID=484) OR (invGroups.groupID=661) OR (invGroups.groupID=662) OR (invGroups.groupID=977)) AND (invTypes.published=1);";
            //strSQL += " ORDER BY invTypes.typeName;";

            strSQL = "SELECT *";
            strSQL += " FROM invTypeReactions INNER JOIN (invGroups INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) ON invTypeReactions.reactionTypeID=invTypes.typeID";
            strSQL += " WHERE ((invGroups.groupID=436) OR (invGroups.groupID=484) OR (invGroups.groupID=661) OR (invGroups.groupID=662) OR (invGroups.groupID=977)) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            reactionData = EveHQ.Core.DataFunctions.GetData(strSQL);
         }

        public void PopulateReactionData()
        {
            decimal curTypeID = 0;
            Reaction nr = null;
            InOutData iod;

            foreach (DataRow row in reactionData.Tables[0].Rows)
            {
                if (curTypeID != Convert.ToDecimal(row.ItemArray[(int)minA.tTID]))
                {
                    // New Reaction Found - Get common data
                    if (nr != null)
                        Reactions.Add(nr);
    
                    nr = new Reaction();

                    nr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.tTID]);
                    curTypeID = nr.typeID;

                    ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nr.typeID);

                    nr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                    nr.reactGroupName = (row.ItemArray[(int)minA.grpName]).ToString();
                    nr.reactName = (row.ItemArray[(int)minA.tName]).ToString();
                    nr.desc = (row.ItemArray[(int)minA.tDesc]).ToString();

                    iod = new InOutData();
                    iod.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.inTypID]);
                    iod.qty = Convert.ToDecimal(row.ItemArray[(int)minA.qty]);
                    if (Convert.ToDecimal(row.ItemArray[(int)minA.inpt]) > 0)
                    {
                        // input
                        nr.inputs.Add(iod);
                    }
                    else
                    {
                        // output
                        nr.outputs.Add(iod);
                    }
                }
                else
                {
                    // Current Reaction Found - add other data
                    iod = new InOutData();
                    iod.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.inTypID]);
                    iod.qty = Convert.ToDecimal(row.ItemArray[(int)minA.qty]);
                    if (Convert.ToDecimal(row.ItemArray[(int)minA.inpt]) > 0)
                    {
                        // input
                        nr.inputs.Add(iod);
                    }
                    else
                    {
                        // output
                        nr.outputs.Add(iod);
                    }
                }
            }
            Reactions.Add(nr);
        }

        public void PopulateReactionListing(Object o)
        {
            PerformSQLQuery();
            PopulateReactionData();
            SaveReactionListing();
           
            PlugInData.resetEvents[7].Set();
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

        public void SaveReactionListing()
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
            pBF.Serialize(pStream, Reactions);
            pStream.Close();
        }

        public void LoadReactionListing()
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
                    Reactions = (ArrayList)myBf.Deserialize(cStr);
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
