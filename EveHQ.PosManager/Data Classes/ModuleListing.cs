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
    public class ModuleListing
    {
        public SortedList<long, Module> Modules;
        public DataSet modAttribData, chargeAttribData, moonMinData, simpReactData, compReactData;
        public DataSet reactSimple, reactComplex, reactBioSimp, reactBioComp, reactHybrid;
        public DataSet biochemData, catalystData, generalData, hazChemData, hybPolyData, refMinData;
        enum modA { grpID, grpName, typID, typDesc, typName, vol, cap, price, attID, valI, valF, attNm, dspNm, unID, unNm, unDNm };
        enum chgA { grpID, grpName, typID, typDesc, typName, mass, vol, cap, prtSz, price, attID, valI, valF, attNm, dspNm, unID, unNm, unDNm };
        enum minA { grpID, grpName, typID, typName, typDesc, grphID, mass, vol, pSize, bPrice, attID, vInt, vFlt, icon };
        enum recA { reacID, inpt, inTypID, qty, grpID, catID, grpName, grpDesc, grGID, UBP, allMan, allRec, anch, cAnc, fNS, gPub, tTID, tGID, tName, tDesc, tGrID, tRad, tMass, tVol, tCap, tPrtSz, tRID, tBPrc, tPub, tMktG, tCoD, egID, icon };
        enum silA { grpID, grpName, typID, typName, typDesc, grphID, mass, vol, pSize, bPrice, icon };

        public ModuleListing()
        {
            Modules = new SortedList<long, Module>();
        }

        private void GetItemData(long typeID, long groupID)
        {
        }

        private double GetDoubleFromVariableIA(DataRow dr, long aI_1, long aI_2)
        {
            double retVal = -1;

            if (!dr.ItemArray[aI_1].Equals(System.DBNull.Value))
            {
                retVal = Convert.ToDouble(dr.ItemArray[aI_1]);
            }
            else
            {
                retVal = Convert.ToDouble(dr.ItemArray[aI_2]);
            }

            return retVal;
        }

        private decimal GetDecimalFromVariableIA(DataRow dr, long aI_1, long aI_2)
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

        public void PerformModuleAndChargeSQLQueries()
        {
            string strSQL;
            
            // Module Attribute Data
            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.volume, invTypes.capacity, invTypes.basePrice, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName";
            strSQL += " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID";
            strSQL += " WHERE (invCategories.categoryID=23) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;";
            modAttribData = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Charge Attribute Data
            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.portionSize, invTypes.basePrice, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName";
            strSQL += " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID";
            strSQL += " WHERE (invCategories.categoryID=8) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;";
            chargeAttribData = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Mineral Attribute Data
            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, eveIcons.iconFile";
            strSQL += " FROM invGroups INNER JOIN ((invTypes INNER JOIN dgmTypeAttributes ON dgmTypeAttributes.typeID = invTypes.typeID) INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE (invGroups.groupID=427) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            moonMinData = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Refined Mineral Attribute Data
            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice, eveIcons.iconFile";
            strSQL += " FROM invGroups INNER JOIN (invTypes INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE (invGroups.groupID=18) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            refMinData = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, eveIcons.iconFile";
            strSQL += " FROM invGroups INNER JOIN ((invTypes INNER JOIN dgmTypeAttributes ON dgmTypeAttributes.typeID = invTypes.typeID) INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE (invGroups.groupID=428) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            simpReactData = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, eveIcons.iconFile";
            strSQL += " FROM invGroups INNER JOIN ((invTypes INNER JOIN dgmTypeAttributes ON dgmTypeAttributes.typeID = invTypes.typeID) INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE (invGroups.groupID=429) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            compReactData = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Other Silo Data
            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice, eveIcons.iconFile";
            strSQL += " FROM invGroups INNER JOIN (invTypes INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE (invGroups.groupID=711) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            biochemData = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice, eveIcons.iconFile";
            strSQL += " FROM invGroups INNER JOIN (invTypes INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE (invGroups.groupID=284) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            catalystData = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice, eveIcons.iconFile";
            strSQL += " FROM invGroups INNER JOIN (invTypes INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE (invGroups.groupID=280) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            generalData = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, eveIcons.iconFile";
            strSQL += " FROM invGroups INNER JOIN ((invTypes INNER JOIN dgmTypeAttributes ON dgmTypeAttributes.typeID = invTypes.typeID) INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE (invGroups.groupID=712) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            hazChemData = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, invTypes.description, invTypes.iconID, invTypes.mass, invTypes.volume, invTypes.portionSize, invTypes.basePrice, eveIcons.iconFile";
            strSQL += " FROM invGroups INNER JOIN (invTypes INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invGroups.groupID = invTypes.groupID";
            strSQL += " WHERE (invGroups.groupID=974) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            hybPolyData = EveHQ.Core.DataFunctions.GetData(strSQL);

            // Reaction Data
            strSQL = "SELECT *";
            strSQL += " FROM invTypeReactions INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invTypeReactions.reactionTypeID=invTypes.typeID";
            strSQL += " WHERE (invGroups.groupID=436) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            reactSimple = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT *";
            strSQL += " FROM invTypeReactions INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invTypeReactions.reactionTypeID=invTypes.typeID";
            strSQL += " WHERE (invGroups.groupID=484) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            reactComplex = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT *";
            strSQL += " FROM invTypeReactions INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invTypeReactions.reactionTypeID=invTypes.typeID";
            strSQL += " WHERE (invGroups.groupID=661) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            reactBioSimp = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT *";
            strSQL += " FROM invTypeReactions INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invTypeReactions.reactionTypeID=invTypes.typeID";
            strSQL += " WHERE (invGroups.groupID=662) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            reactBioComp = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT *";
            strSQL += " FROM invTypeReactions INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN eveIcons ON invTypes.iconID=eveIcons.iconID) ON invTypeReactions.reactionTypeID=invTypes.typeID";
            strSQL += " WHERE (invGroups.groupID=977) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName;";
            reactHybrid = EveHQ.Core.DataFunctions.GetData(strSQL);
        }

        public void GetMineralAndReactIcons()
        {
            foreach (DataRow row in moonMinData.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)minA.typID]).ToString());
            foreach (DataRow row in simpReactData.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)minA.typID]).ToString());
            foreach (DataRow row in compReactData.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)minA.typID]).ToString());
            foreach (DataRow row in reactSimple.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)recA.inTypID]).ToString());
            foreach (DataRow row in reactComplex.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)recA.inTypID]).ToString());
            foreach (DataRow row in reactBioSimp.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)recA.inTypID]).ToString());
            foreach (DataRow row in reactBioComp.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)recA.inTypID]).ToString());
            foreach (DataRow row in reactHybrid.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)recA.inTypID]).ToString());
            foreach (DataRow row in hazChemData.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)minA.typID]).ToString());
            foreach (DataRow row in biochemData.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)silA.typID]).ToString());
            foreach (DataRow row in catalystData.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)silA.typID]).ToString());
            foreach (DataRow row in generalData.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)silA.typID]).ToString());
            foreach (DataRow row in hybPolyData.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)silA.typID]).ToString());
            foreach (DataRow row in refMinData.Tables[0].Rows)
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetIcon), (row.ItemArray[(int)silA.typID]).ToString());
        }

        public void PopulateModuleMineralData(Module nt)
        {
            MoonSiloReactMineral msr;

            switch (nt.groupID)
            {
                case 404:
                    // Silo class module
                    if ((nt.Name == "Silo") || (nt.Name == "Coupling Array"))
                    {
                        foreach (DataRow row in refMinData.Tables[0].Rows)
                        {
                            msr = new MoonSiloReactMineral();

                            msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                            msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                            msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                            msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                            msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                            msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                            msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                            msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                            msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                            msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                            msr.reactQty = msr.portionSize;

                            nt.MSRList.Add(msr);
                            nt.InputList.Add(msr);
                            nt.OutputList.Add(msr);
                        }
                        foreach (DataRow row in moonMinData.Tables[0].Rows)
                        {
                            msr = new MoonSiloReactMineral();

                            msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                            msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                            msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                            msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                            msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                            msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                            msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                            msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                            msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                            msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                            msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                            nt.MSRList.Add(msr);
                            nt.InputList.Add(msr);
                            nt.OutputList.Add(msr);
                        }
                        foreach (DataRow row in simpReactData.Tables[0].Rows)
                        {
                            msr = new MoonSiloReactMineral();

                            msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                            msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                            msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                            msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                            msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                            msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                            msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                            msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                            msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                            msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                            msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                            nt.MSRList.Add(msr);
                            nt.InputList.Add(msr);
                            nt.OutputList.Add(msr);
                        }
                        foreach (DataRow row in compReactData.Tables[0].Rows)
                        {
                            msr = new MoonSiloReactMineral();

                            msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                            msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                            msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                            msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                            msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                            msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                            msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                            msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                            msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                            msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                            msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                            nt.MSRList.Add(msr);
                            nt.InputList.Add(msr);
                            nt.OutputList.Add(msr);
                        }
                        if (nt.Name == "Coupling Array")
                        {
                            foreach (DataRow row in biochemData.Tables[0].Rows)
                            {
                                msr = new MoonSiloReactMineral();

                                msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                                msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                                msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                                msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                                msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                                msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                                msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                                msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                                msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                                msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                                msr.reactQty = msr.portionSize; // GetDecimalFromVariableIA(row, 11, 12);

                                nt.MSRList.Add(msr);
                                nt.InputList.Add(msr);
                                nt.OutputList.Add(msr);
                            }
                            foreach (DataRow row in generalData.Tables[0].Rows)
                            {
                                msr = new MoonSiloReactMineral();

                                msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                                msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                                msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                                msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                                msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                                msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                                msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                                msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                                msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                                msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                                msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                                nt.MSRList.Add(msr);
                                nt.InputList.Add(msr);
                                nt.OutputList.Add(msr);
                            }
                            foreach (DataRow row in catalystData.Tables[0].Rows)
                            {
                                msr = new MoonSiloReactMineral();

                                msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                                msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                                msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                                msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                                msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                                msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                                msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                                msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                                msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                                msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                                msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                                nt.MSRList.Add(msr);
                                nt.InputList.Add(msr);
                                nt.OutputList.Add(msr);
                            }
                            foreach (DataRow row in hazChemData.Tables[0].Rows)
                            {
                                msr = new MoonSiloReactMineral();

                                msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                                msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                                msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                                msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                                msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                                msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                                msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                                msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                                msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                                msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                                msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                                nt.MSRList.Add(msr);
                                nt.InputList.Add(msr);
                                nt.OutputList.Add(msr);
                            }
                            foreach (DataRow row in hybPolyData.Tables[0].Rows)
                            {
                                msr = new MoonSiloReactMineral();

                                msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                                msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                                msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                                msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                                msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                                msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                                msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                                msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                                msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                                msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                                msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                                nt.MSRList.Add(msr);
                                nt.InputList.Add(msr);
                                nt.OutputList.Add(msr);
                            }
                        }
                    }
                    else if (nt.Name.Contains("Biochemical"))
                    {
                        foreach (DataRow row in biochemData.Tables[0].Rows)
                        {
                            msr = new MoonSiloReactMineral();

                            msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                            msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                            msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                            msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                            msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                            msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                            msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                            msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                            msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                            msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                            msr.reactQty = msr.portionSize; // GetDecimalFromVariableIA(row, 11, 12);

                            nt.MSRList.Add(msr);
                            nt.InputList.Add(msr);
                            nt.OutputList.Add(msr);
                        }
                    }
                    else if (nt.Name.Contains("Catalyst"))
                    {
                        foreach (DataRow row in catalystData.Tables[0].Rows)
                        {
                            msr = new MoonSiloReactMineral();

                            msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                            msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                            msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                            msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                            msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                            msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                            msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                            msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                            msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                            msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                            msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                            nt.MSRList.Add(msr);
                            nt.InputList.Add(msr);
                            nt.OutputList.Add(msr);
                        }
                    }
                    else if (nt.Name.Contains("General"))
                    {
                        foreach (DataRow row in generalData.Tables[0].Rows)
                        {
                            msr = new MoonSiloReactMineral();

                            msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                            msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                            msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                            msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                            msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                            msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                            msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                            msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                            msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                            msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                            msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                            nt.MSRList.Add(msr);
                            nt.InputList.Add(msr);
                            nt.OutputList.Add(msr);
                        }
                    }
                    else if (nt.Name.Contains("Hazardous"))
                    {
                        foreach (DataRow row in hazChemData.Tables[0].Rows)
                        {
                            msr = new MoonSiloReactMineral();

                            msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                            msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                            msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                            msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                            msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                            msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                            msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                            msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                            msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                            msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                            msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                            nt.MSRList.Add(msr);
                            nt.InputList.Add(msr);
                            nt.OutputList.Add(msr);
                        }
                    }
                    else if (nt.Name.Contains("Hybrid Polymer"))
                    {
                        foreach (DataRow row in hybPolyData.Tables[0].Rows)
                        {
                            msr = new MoonSiloReactMineral();

                            msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                            msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                            msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                            msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                            msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                            msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                            msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                            msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                            msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                            msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                            msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                            nt.MSRList.Add(msr);
                            nt.InputList.Add(msr);
                            nt.OutputList.Add(msr);
                        }
                    }
                    break;
                case 416:
                    // Moon Harvestor Module
                    foreach (DataRow row in moonMinData.Tables[0].Rows)
                    {
                        msr = new MoonSiloReactMineral();

                        msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                        msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                        msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                        msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                        msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                        msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                        msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                        msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                        msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                        msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                        msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                        nt.MSRList.Add(msr);
                        nt.InputList.Add(msr);
                        nt.OutputList.Add(msr);
                    }
                    break;
                default:
                    // Should never be possible to get here, but meh.
                    break;
            }

            Modules.Add(nt.typeID, nt);
        }

        public void PopulateModuleReactionData(Module nt)
        {
            decimal curTypeID = 0;
            MoonSiloReactMineral msr;
            Reaction nr = null;
            InOutData iod;

            if (nt.Name.Contains("Simple"))
            {
                // Do inputs and outputs as well
                foreach (DataRow row in moonMinData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                    msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                    msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                    nt.InputList.Add(msr);
                }
                foreach (DataRow row in simpReactData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                    msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                    msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                    nt.OutputList.Add(msr);
                }
                foreach (DataRow row in reactSimple.Tables[0].Rows)
                {
                    if (curTypeID != Convert.ToDecimal(row.ItemArray[(int)recA.tTID]))
                    {
                        // New Reaction Found - Get common data
                        if (nr != null)
                            nt.ReactList.Add(nr);

                        nr = new Reaction();

                        nr.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.tTID]);
                        curTypeID = nr.typeID;
                        nr.icon = (row.ItemArray[(int)recA.icon]).ToString();
                        nr.groupID = Convert.ToDecimal(row.ItemArray[(int)recA.grpID]);
                        nr.reactGroupName = (row.ItemArray[(int)recA.grpName]).ToString();
                        nr.reactName = (row.ItemArray[(int)recA.tName]).ToString();
                        nr.desc = (row.ItemArray[(int)recA.tDesc]).ToString();

                        iod = new InOutData();
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                nt.ReactList.Add(nr);
            }
            else if (nt.Name.Contains("Complex"))
            {
                // do inputs and outputs
                foreach (DataRow row in simpReactData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                    msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                    msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                    nt.InputList.Add(msr);
                }
                foreach (DataRow row in compReactData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                    msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                    msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                    nt.OutputList.Add(msr);
                }
                foreach (DataRow row in reactComplex.Tables[0].Rows)
                {
                    if (curTypeID != Convert.ToDecimal(row.ItemArray[(int)recA.tTID]))
                    {
                        // New Reaction Found - Get common data
                        if (nr != null)
                            nt.ReactList.Add(nr);

                        nr = new Reaction();

                        nr.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.tTID]);
                        curTypeID = nr.typeID;
                        nr.icon = (row.ItemArray[(int)recA.icon]).ToString();
                        nr.groupID = Convert.ToDecimal(row.ItemArray[(int)recA.grpID]);
                        nr.reactGroupName = (row.ItemArray[(int)recA.grpName]).ToString();
                        nr.reactName = (row.ItemArray[(int)recA.tName]).ToString();
                        nr.desc = (row.ItemArray[(int)recA.tDesc]).ToString();

                        iod = new InOutData();
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                nt.ReactList.Add(nr);
            }
            else if (nt.Name.Contains("Medium"))
            {
                // I need my inputs and outputs here as well. Bio / drug reactions typically
                // have 2 inputs and 2 outputs - the hard part is figuring out WTF they are
                //
                // I will probably have to determine the recipe / input / output crap all in
                // game and write it down to get the information.
                foreach (DataRow row in biochemData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                    msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                    msr.reactQty = msr.portionSize; 

                    nt.InputList.Add(msr);
                }
                foreach (DataRow row in generalData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                    msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                    msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                    nt.InputList.Add(msr);
                    nt.OutputList.Add(msr);
                }
                foreach (DataRow row in hazChemData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                    msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                    msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                    nt.OutputList.Add(msr);
                }

                foreach (DataRow row in reactBioSimp.Tables[0].Rows)
                {
                    if (curTypeID != Convert.ToDecimal(row.ItemArray[(int)recA.tTID]))
                    {
                        // New Reaction Found - Get common data
                        if (nr != null)
                            nt.ReactList.Add(nr);

                        nr = new Reaction();

                        nr.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.tTID]);
                        curTypeID = nr.typeID;
                        nr.icon = (row.ItemArray[(int)recA.icon]).ToString();
                        nr.groupID = Convert.ToDecimal(row.ItemArray[(int)recA.grpID]);
                        nr.reactGroupName = (row.ItemArray[(int)recA.grpName]).ToString();
                        nr.reactName = (row.ItemArray[(int)recA.tName]).ToString();
                        nr.desc = (row.ItemArray[(int)recA.tDesc]).ToString();

                        iod = new InOutData();
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                nt.ReactList.Add(nr);
            }
            else if (nt.Name.Contains("Biochemical"))
            {
                // I need my inputs and outputs here as well. Bio / drug reactions typically
                // have 2 inputs and 2 outputs - the hard part is figuring out WTF they are
                //
                // I will probably have to determine the recipe / input / output crap all in
                // game and write it down to get the information.
                foreach (DataRow row in catalystData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                    msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                    msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                    nt.InputList.Add(msr);
                    nt.OutputList.Add(msr);
                }
                foreach (DataRow row in generalData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                    msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                    msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                    nt.InputList.Add(msr);
                    nt.OutputList.Add(msr);
                }
                foreach (DataRow row in hazChemData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)minA.typID]);
                    msr.icon = (row.ItemArray[(int)minA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)minA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)minA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)minA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)minA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)minA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)minA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)minA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)minA.typDesc]).ToString();
                    msr.reactQty = GetDecimalFromVariableIA(row, 11, 12);

                    nt.InputList.Add(msr);
                    nt.OutputList.Add(msr);
                }
                foreach (DataRow row in reactBioComp.Tables[0].Rows)
                {
                    if (curTypeID != Convert.ToDecimal(row.ItemArray[(int)recA.tTID]))
                    {
                        // New Reaction Found - Get common data
                        if (nr != null)
                            nt.ReactList.Add(nr);

                        nr = new Reaction();

                        nr.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.tTID]);
                        curTypeID = nr.typeID;
                        nr.icon = (row.ItemArray[(int)recA.icon]).ToString();
                        nr.groupID = Convert.ToDecimal(row.ItemArray[(int)recA.grpID]);
                        nr.reactGroupName = (row.ItemArray[(int)recA.grpName]).ToString();
                        nr.reactName = (row.ItemArray[(int)recA.tName]).ToString();
                        nr.desc = (row.ItemArray[(int)recA.tDesc]).ToString();

                        iod = new InOutData();
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                nt.ReactList.Add(nr);
            }
            else if (nt.Name.Contains("Polymer"))
            {
                // I need my inputs and outputs here as well. Bio / drug reactions typically
                // have 2 inputs and 2 outputs - the hard part is figuring out WTF they are
                //
                // I will probably have to determine the recipe / input / output crap all in
                // game and write it down to get the information.

                // Inputs : refMinData, 711
                // Outputs: 974
                foreach (DataRow row in refMinData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                    msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                    msr.reactQty = msr.portionSize;

                    nt.InputList.Add(msr);
                }
                foreach (DataRow row in biochemData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                    msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                    msr.reactQty = msr.portionSize; // GetDecimalFromVariableIA(row, 11, 12);

                    nt.InputList.Add(msr);
                }
                foreach (DataRow row in hybPolyData.Tables[0].Rows)
                {
                    msr = new MoonSiloReactMineral();

                    msr.typeID = Convert.ToDecimal(row.ItemArray[(int)silA.typID]);
                    msr.icon = (row.ItemArray[(int)silA.icon]).ToString();
                    msr.groupID = Convert.ToDecimal(row.ItemArray[(int)silA.grpID]);
                    msr.mass = Convert.ToDecimal(row.ItemArray[(int)silA.mass]);
                    msr.volume = Convert.ToDecimal(row.ItemArray[(int)silA.vol]);
                    msr.portionSize = Convert.ToDecimal(row.ItemArray[(int)silA.pSize]);
                    msr.basePrice = Convert.ToDecimal(row.ItemArray[(int)silA.bPrice]);
                    msr.groupName = (row.ItemArray[(int)silA.grpName]).ToString();
                    msr.name = (row.ItemArray[(int)silA.typName]).ToString();
                    msr.description = (row.ItemArray[(int)silA.typDesc]).ToString();
                    msr.reactQty = msr.portionSize;// GetDecimalFromVariableIA(row, 11, 12);

                    nt.OutputList.Add(msr);
                }
                foreach (DataRow row in reactHybrid.Tables[0].Rows)
                {
                    if (curTypeID != Convert.ToDecimal(row.ItemArray[(int)recA.tTID]))
                    {
                        // New Reaction Found - Get common data
                        if (nr != null)
                            nt.ReactList.Add(nr);

                        nr = new Reaction();

                        nr.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.tTID]);
                        curTypeID = nr.typeID;
                        nr.icon = (row.ItemArray[(int)recA.icon]).ToString();
                        nr.groupID = Convert.ToDecimal(row.ItemArray[(int)recA.grpID]);
                        nr.reactGroupName = (row.ItemArray[(int)recA.grpName]).ToString();
                        nr.reactName = (row.ItemArray[(int)recA.tName]).ToString();
                        nr.desc = (row.ItemArray[(int)recA.tDesc]).ToString();

                        iod = new InOutData();
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                        iod.typeID = Convert.ToDecimal(row.ItemArray[(int)recA.inTypID]);
                        iod.qty = Convert.ToDecimal(row.ItemArray[(int)recA.qty]);
                        if (Convert.ToDecimal(row.ItemArray[(int)recA.inpt]) > 0)
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
                nt.ReactList.Add(nr);
            }
            Modules.Add(nt.typeID, nt);
        }

        public void PopulateModuleChargeData(Module nt)
        {
            string oVal;
            long curTID = 0, typeID = 0, skpID = 0;
            bool first = true;
            Charge ch;

            ch = new Charge();
            foreach (DataRow row in chargeAttribData.Tables[0].Rows)
            {
                if (nt.ChargeGroup != Convert.ToInt32(row.ItemArray[(int)chgA.grpID]))
                    continue;

                typeID = Convert.ToInt32(row.ItemArray[(int)chgA.typID]);

                if ((skpID != 0) && (typeID == skpID))
                    continue;

                if (typeID != curTID)
                {
                    if ((!first) && (skpID == 0))
                    {
                        // This will pick up every module except for the last one.
                        nt.Charges.Add(ch);
                        nt.ChargeList.Add(ch.Name);
                        ch = new Charge();
                    }
                    else if (skpID != 0)
                    {
                        ch = new Charge();
                        skpID = 0;
                    }
                    ch.typeID = Convert.ToInt32(row.ItemArray[(int)chgA.typID]);
                    ch.ChargeVolume = Convert.ToDecimal(row.ItemArray[(int)(chgA.vol)]);
                    curTID = ch.typeID;
                    ch.Name = row.ItemArray[(int)chgA.typName].ToString();
                    first = false;
                }

                switch (Convert.ToInt32(row.ItemArray[(int)chgA.attID]))
                {
                    case 37:             // Max Velocity
                        ch.Velocity = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 64:             // Damage Multiple
                        ch.DmgMult = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 70:             // Agility
                        ch.Agility = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 107:            // Explosion Range
                        ch.ExpRange = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 108:            // Detonation Range
                        ch.DetRange = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 114:            // EM Damage
                        ch.EM_Dmg = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 116:             // Exp Damage
                        ch.Exp_Dmg = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 117:             // Kinetic Damage
                        ch.Kin_Dmg = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 118:             // Thermal Damage
                        ch.Thm_Dmg = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 120:             // Range Mult
                        ch.RangeMult = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 128:             // Charge Size
                        ch.ChargeSize = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        if (nt.ChargeSize > 0)
                            if (nt.ChargeSize != ch.ChargeSize)
                            {
                                skpID = curTID;
                            }
                        break;
                    case 204:             // Speed Mult
                        ch.SpdMult = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 244:             // Track Speed Mult
                        ch.Tracking = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 281:             // Flight Time
                        ch.FlightTime = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 612:             // Base Shield Dmg
                        ch.Base_Shield = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 613:             // Base Armor Dmg
                        ch.Base_Armor = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    case 779:             // Fly Range Mult
                        ch.FlyRangeMult = GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF);
                        break;
                    default:
                        oVal = row.ItemArray[(int)chgA.dspNm].ToString();
                        if (oVal.Length > 0)
                        {
                            oVal += " [" + GetDecimalFromVariableIA(row, (int)chgA.valI, (int)chgA.valF) + "] <" + Convert.ToInt32(row.ItemArray[(int)chgA.attID]) + ">";
                            ch.Extra.Add(oVal);
                        }
                        break;
                }
            }
            if (skpID == 0)
            {
                nt.Charges.Add(ch);
                nt.ChargeList.Add(ch.Name);
            }

            Modules.Add(nt.typeID, nt);
        }

        public void PopulateModuleData()
        {
            decimal timVal;
            string extT = "", extV = "";
            long curTID = 0, typeID = 0;
            bool first = true;
            Module nt;

            nt = new Module();

            foreach (DataRow row in modAttribData.Tables[0].Rows)
            {
                typeID = Convert.ToInt32(row.ItemArray[(int)modA.typID]);

                if (typeID != curTID)
                {
                    if (!first)
                    {
                        // This will pick up every module except for the last one.
                        if ((nt.MaxTarget > 0) && (nt.ChargeGroup > 0))
                        {
                            //Pick up the Charge Data if Applicable
                            ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nt.typeID);
                            PopulateModuleChargeData(nt);
                        }
                        else if ((nt.groupID == 404) || (nt.groupID == 416))
                        {
                            // Pick up the Mineral Data if Applicable
                            ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nt.typeID);
                            PopulateModuleMineralData(nt);
                        }
                        else if (nt.groupID == 438)
                        {
                            // Pick up the Reaction Data if Applicable
                            ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nt.typeID);
                            PopulateModuleReactionData(nt);
                        }
                        else
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nt.typeID);
                            Modules.Add(nt.typeID, nt);
                        }

                        nt = new Module();
                    }
                    curTID = typeID;
                    // Get item specific information - ie: first row data

                    // Place data longo the data table
                    nt.Name = row.ItemArray[(int)modA.typName].ToString();
                    nt.Desc = row.ItemArray[(int)modA.typDesc].ToString();
                    nt.typeID = typeID;
                    nt.groupID = Convert.ToInt32(row.ItemArray[(int)modA.grpID]);
                    nt.Capacity = Convert.ToDecimal(row.ItemArray[(int)modA.cap]);
                    nt.Volume = Convert.ToDecimal(row.ItemArray[(int)modA.vol]);
                    nt.Cost = Convert.ToDecimal(row.ItemArray[(int)modA.price]);
                    nt.Category = row.ItemArray[(int)modA.grpName].ToString();
                    first = false;
                }

                // Now there are many rows with the same ID that just correspond to other data polongs
                switch (Convert.ToInt32(row.ItemArray[(int)modA.attID]))
                {
                    case 9:             // Structure HP - dec
                        nt.Struct.Amount = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 11:            // Power Grid - double
                        nt.Power = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 30:            // Power Need - double
                        nt.Power_Used = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 48:            // CPU - double
                        nt.CPU = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 50:            // CPU Need - double
                        nt.CPU_Used = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 263:           // Shield HP - float
                        nt.Shield.Amount = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 265:           // Armor HP - double
                        nt.Armor.Amount = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 552:           // Signature Radius
                        nt.SigRad = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 556:   // Anchor
                        timVal = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        timVal = timVal / 1000; // convert ms to seconds
                        nt.Anchor_Time = Convert.ToDecimal(timVal);
                        break;
                    case 676:   // UnAnchor
                        timVal = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        timVal = timVal / 1000; // convert ms to seconds
                        nt.UnAnchor_Time = Convert.ToDecimal(timVal);
                        break;
                    case 677:   // Online
                        timVal = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        timVal = timVal / 1000; // convert ms to seconds
                        nt.Online_Time = Convert.ToDecimal(timVal);
                        break;
                    case 113:           // Hull EM Res - float
                        nt.Struct.EMP = (100 - (100 * GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF)));
                        break;
                    case 111:           // Hull Exp Res - float
                        nt.Struct.Explosive = (100 - (100 * GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF)));
                        break;
                    case 109:           // Hull Kin Res - float
                        nt.Struct.Kinetic = (100 - (100 * GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF)));
                        break;
                    case 110:           // Hull Thermal Res - float
                        nt.Struct.Thermal = (100 - (100 * GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF)));
                        break;
                    case 51:            // Rate of Fire
                    case 506:
                        nt.ROF = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 64:            // Damage Modifier
                        nt.DamageMod = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 54:            // Optimal
                    case 98:            // Optimal - Max Neut Range
                    case 103:           // Optimal - Max Warp Scram Range
                        nt.Optimal = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 73:            // Activation Time
                        nt.Activation = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 97:            // Energy Neutralized
                        nt.EnergyNeut = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 128:           // Charge Size
                        nt.ChargeSize = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 154:           // Activation Prox
                        nt.Proximity = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 158:           // FallOff
                        nt.FallOff = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 160:           // Tracking Speed
                        nt.Tracking = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 192:           // Max Locked Targets
                        nt.MaxTarget = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 604:           // Chargegroup
                        nt.ChargeGroup = Convert.ToInt32(GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF));
                        break;
                    case 564:           // Scan resolution
                        nt.ScanRes = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 620:           // Signature Resolution (pref Target Size)
                        nt.SigRes = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 771:           // Cargo, Ammo, Etc... Capacity
                        nt.Capacity = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 212:           // Missile Damage Bonus
                        nt.Bonuses.MslDamage = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 645:           // Missile Velocity Bonus
                        nt.Bonuses.MslVelocity = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 646:           // Missile Flight Time Bonus
                        nt.Bonuses.MslFlightTime = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 858:           // Missile Explosion Radius Bonus
                        nt.Bonuses.MslExplRad = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 859:           // Missile Explosion Velocity Bonus
                        nt.Bonuses.MslExplVel = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 767:           // Turret Tracking speed Bonus
                        nt.Bonuses.TrtTrackSpeed = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 769:           // Turret Optimal Range Bonus
                        nt.Bonuses.TrtOptimal = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 130:           // Thermal Resist Bonus
                        nt.Bonuses.ThermalDmgRes = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 131:           // Kinetic Resist Bonus
                        nt.Bonuses.KineticDmgRes = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 132:           // Explosive Resist Bonus
                        nt.Bonuses.ExplosiveDmgRes = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 133:           // EMP Resist Bonus
                        nt.Bonuses.EmpDmgRes = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 466:           // Combine Fire Chance
                        nt.CombineFire = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 182:           // Primary Skill Required
                        nt.PriSkill = Convert.ToInt32(GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF));
                        break;
                    case 277:           // Primary Skill Level Required
                        nt.PriSkillLvl = Convert.ToInt32(GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF));
                        break;
                    case 697:           // Target Cycle Speed
                        nt.SwitchDelay = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 238:           // Gravitic Jam Strength
                        nt.Grav_JamStr = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 239:           // Ladar Jam Strength
                        nt.Ladr_JamStr = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 240:           // Magnetometric Jam Strength
                        nt.Magn_JamStr = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 241:           // Combine Fire Chance
                        nt.CombineFire = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 1185:           // Sov Level Rquired
                        nt.SovLevel = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 867:           // Max Jump Range
                        nt.MaxJump = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 1032:           // Sec Level - Max Allowed
                        nt.MaxSecLev = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 717:           // Refining Yield
                        nt.RefineYield = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 842:           // Reaction 1
                        nt.React1 = Convert.ToInt32(GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF));
                        break;
                    case 843:           // Reaction 2
                        nt.React2 = Convert.ToInt32(GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF));
                        break;
                    case 866:           // Jump Fuel Type
                        nt.JF_Type = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 868:           // Jump Fuel per LY
                        nt.JF_Per_LY = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 1001:           // Jump Fuel Mass Multiplier
                        nt.JF_MassMult = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 1195:           // Max Module per System
                        nt.MaxPerSystem = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 237:           // Target Range Modifier
                        nt.TargetRangeMod = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 242:           // Target Speed Modifier
                        nt.TargetSpeedMod = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 20:           // Target Max Speed Modifier
                        nt.TargetMaxSpeedMod = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 235:           // Target Max Targets Modifier
                        nt.TargetMaxTargetsMod = GetDoubleFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 105:           // Warp Scram Strength
                        nt.WarpScramStr = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        break;
                    case 479:           // Shield recharge Time
                        nt.ShieldRechTime = Convert.ToDecimal(GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF) / 1000);
                        break;
                    case 691:           // Target Cycling Speed
                        nt.SwitchDelay = Convert.ToDecimal(GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF) / 1000);
                        break;
                    default:            // All other information
                        break;
                }

                extT = row.ItemArray[(int)modA.dspNm].ToString();
                if(extT.Length <= 0)
                    extT = row.ItemArray[(int)modA.attNm].ToString();

                // Build Item Selection Information DAF
                switch (Convert.ToInt32(row.ItemArray[(int)modA.attID]))
                {
                    case 556:   // Anchor
                    case 676:   // UnAnchor
                    case 677:   // Online
                        extV = (GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF) / 1000).ToString();
                        nt.Extra.Add(extT + "\n" + extV + "<" + Convert.ToInt32(row.ItemArray[(int)modA.attID]) + ">\n");
                        nt.OtherInfo += extT + " <" + extV + " s>\n";
                        break;
                    case 9:             // Structure HP - dec
                    case 11:            // Power Grid - double
                    case 30:            // Power Need - double
                    case 48:            // CPU - double
                    case 50:            // CPU Need - double
                    case 263:           // Shield HP - float
                    case 265:           // Armor HP - double
                    case 113:           // Hull EM Res - float
                    case 604:           // Chargegroup
                    case 111:           // Hull Exp Res - float
                    case 109:           // Hull Kin Res - float
                    case 110:           // Hull Thermal Res - float
                    case 182:           // Primary Skill Required
                    case 277:           // Primary Skill Level Required
                        break;
                    case 552:           // Signature Radius
                    case 51:            // Rate of Fire
                    case 64:            // Damage Modifier
                    case 506:
                    case 54:            // Optimal
                    case 98:            // Optimal - Max Neut Range
                    case 103:           // Optimal - Max Warp Scram Range
                    case 73:            // Activation Time
                    case 97:            // Energy Neutralized
                    case 128:           // Charge Size
                    case 154:           // Activation Prox
                    case 158:           // FallOff
                    case 160:           // Tracking Speed
                    case 192:           // Max Locked Targets
                    case 564:           // Scan resolution
                    case 620:           // Signature Resolution (pref Target Size)
                    case 771:           // Cargo, Ammo, Etc... Capacity
                    case 212:           // Missile Damage Bonus
                    case 645:           // Missile Velocity Bonus
                    case 646:           // Missile Flight Time Bonus
                    case 858:           // Missile Explosion Radius Bonus
                    case 859:           // Missile Explosion Velocity Bonus
                    case 767:           // Turret Tracking speed Bonus
                    case 769:           // Turret Optimal Range Bonus
                    case 130:           // Thermal Resist Bonus
                    case 131:           // Kinetic Resist Bonus
                    case 132:           // Explosive Resist Bonus
                    case 133:           // EMP Resist Bonus
                    case 466:           // Combine Fire Chance
                    case 697:           // Target Cycle Speed
                    case 238:           // Gravitic Jam Strength
                    case 239:           // Ladar Jam Strength
                    case 240:           // Magnetometric Jam Strength
                    case 241:           // Combine Fire Chance
                    case 1185:           // Sov Level Rquired
                    case 867:           // Max Jump Range
                    case 1032:           // Sec Level - Max Allowed
                    case 717:           // Refining Yield
                    case 842:           // Reaction 1
                    case 843:           // Reaction 2
                    case 866:           // Jump Fuel Type
                    case 868:           // Jump Fuel per LY
                    case 1001:           // Jump Fuel Mass Multiplier
                    case 1195:           // Max Module per System
                    case 237:           // Target Range Modifier
                    case 242:           // Target Speed Modifier
                    case 20:           // Target Max Speed Modifier
                    case 235:           // Target Max Targets Modifier
                    case 105:           // Warp Scram Strength
                    case 479:           // Shield recharge Time
                    case 691:           // Target Cycling Speed
                    default:            // All other information
                        extV = (GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF)).ToString();
                        nt.Extra.Add(extT + "\n" + extV + "<" + Convert.ToInt32(row.ItemArray[(int)modA.attID]) + ">\n");
                        nt.OtherInfo += extT + " <" + extV + " " + row.ItemArray[(int)modA.unDNm].ToString() + ">\n";
                        break;
                }

            }
            if ((nt.MaxTarget > 0) && (nt.ChargeGroup > 0))
            {
                //Pick up the Charge Data if Applicable
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nt.typeID);
                PopulateModuleChargeData(nt);
            }
            else if ((nt.groupID == 404) || (nt.groupID == 416))
            {
                // Pick up the Mineral Data if Applicable
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nt.typeID);
                PopulateModuleMineralData(nt);
            }
            else if (nt.groupID == 438)
            {
                // Pick up the Reaction Data if Applicable
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nt.typeID);
                PopulateModuleReactionData(nt);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nt.typeID);
                Modules.Add(nt.typeID, nt);
            }
        }

        public void PopulateModuleListing(Object o)
        {
            PerformModuleAndChargeSQLQueries();
            GetMineralAndReactIcons();
            PopulateModuleData();
            SaveModuleListing();
            PlugInData.resetEvents[1].Set();
        }

        public void GetImage(Object o)
        {
            Image img;
            string imgId;

            imgId = o.ToString();

            img = EveHQ.Core.ImageHandler.GetImage(imgId);
        }

        public void GetIcon(Object o)
        {
            Image img;
            string imgId;

            imgId = o.ToString();

            img = EveHQ.Core.ImageHandler.GetImage(imgId);
        }

        public void SaveModuleListing()
        {
            string fname;

            fname = Path.Combine(PlugInData.PoSCache_Path, "Module_List.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Modules);
            pStream.Close();
        }

        public void LoadModuleListing()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(PlugInData.PoSCache_Path, "Module_List.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Modules = (SortedList<long, Module>)myBf.Deserialize(cStr);
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
