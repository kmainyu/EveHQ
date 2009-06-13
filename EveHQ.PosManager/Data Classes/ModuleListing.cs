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
        public ArrayList Modules;
        public DataSet modAttribData, chargeAttribData;
        enum modA { grpID, grpName, typID, typDesc, typName, vol, cap, price, attID, valI, valF, attNm, dspNm, unID, unNm, unDNm };
        enum chgA { grpID, grpName, typID, typDesc, typName, mass, vol, cap, prtSz, price, attID, valI, valF, attNm, dspNm, unID, unNm, unDNm };

        public ModuleListing()
        {
            Modules = new ArrayList();
        }

        private void GetItemData(int typeID, int groupID)
        {
            //string strSQL;

            //// Get Table With Tower or Tower Item Information
            //strSQL = "SELECT * FROM invTypes WHERE invTypes.typeID=" + typeID + ";";
            //invTypeData = EveHQ.Core.DataFunctions.GetData(strSQL);

            //// Get Table With Tower or Tower Item Details
            //strSQL = "SELECT * FROM dgmTypeAttributes INNER JOIN dgmAttributeTypes ON dgmTypeAttributes.attributeID = dgmAttributeTypes.attributeID WHERE dgmTypeAttributes.typeID=" + typeID + ";";
            //towerStatData = EveHQ.Core.DataFunctions.GetData(strSQL);

            //// Get Table with Tower Fuel Usage Information
            //if (invTypeData.Tables[0].Rows[0].ItemArray[2].ToString().Contains("Tower"))
            //{
            //    strSQL = "SELECT * FROM ((invControlTowerResources INNER JOIN invControlTowerResourcePurposes" +
            //                              " ON invControlTowerResources.purpose = invControlTowerResourcePurposes.purpose)" +
            //                              " INNER JOIN invTypes ON invControlTowerResources.resourceTypeID = invTypes.typeID)" +
            //                              " WHERE invControlTowerResources.controlTowerTypeID=" + typeID + ";";
            //    towerFuelData = EveHQ.Core.DataFunctions.GetData(strSQL);
            //}
            //else
            //{
            //    if (towerFuelData != null)
            //    {
            //        towerFuelData.Clear();
            //        towerFuelData.Dispose();
            //        towerFuelData = null;
            //    }
            //}
        }

        private double GetDoubleFromVariableIA(DataRow dr, int aI_1, int aI_2)
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

        public void PermormModuleAndChargeSQLQueries()
        {
            string strSQL;
            
            //Module Attribute Data
            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.volume, invTypes.capacity, invTypes.basePrice, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName";
            strSQL += " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID";
            strSQL += " WHERE (invCategories.categoryID=23) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;";
            modAttribData = EveHQ.Core.DataFunctions.GetData(strSQL);

            //Charge Attribute Data
            strSQL = "SELECT invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.portionSize, invTypes.basePrice, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName";
            strSQL += " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID";
            strSQL += " WHERE (invCategories.categoryID=8) AND (invTypes.published=1)";
            strSQL += " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;";
            chargeAttribData = EveHQ.Core.DataFunctions.GetData(strSQL);

        }

        public void PopulateModuleChargeData(Module nt)
        {
            string oVal;
            int curTID = 0, typeID = 0, skpID = 0;
            bool first = true;
            Charge ch;

            // Start a thread to retrieve the Module Image for Selection ListView
            // No real rush, but might speed things up just a little bit
            ThreadPool.QueueUserWorkItem(new WaitCallback(GetImage), nt.typeID);

            if ((nt.MaxTarget > 0) && (nt.ChargeGroup > 0))
            {
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
            }                
            Modules.Add(nt);
        }

        public void PopulateModuleData()
        {
            decimal timVal;
            string oVal, oiLin, oinf = "";
            int curTID = 0, typeID = 0;
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
                        PopulateModuleChargeData(nt);
                        nt = new Module();
                        oinf = "";
                    }
                    curTID = typeID;
                    // Get item specific information - ie: first row data

                    // Place data into the data table
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

                // Now there are many rows with the same ID that just correspond to other data points
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

                        oVal = row.ItemArray[(int)modA.dspNm].ToString();
                        if (oVal.Length > 0)
                        {
                            oiLin = oVal + "\n";
                            oiLin += String.Format("{0:0,0.#}", timVal) + "s\n";
                            oinf += oiLin;
                        }
                        nt.OtherInfo = oinf;
                        break;
                    case 676:   // UnAnchor
                        timVal = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        timVal = timVal / 1000; // convert ms to seconds
                        nt.UnAnchor_Time = Convert.ToDecimal(timVal);

                        oVal = row.ItemArray[(int)modA.dspNm].ToString();
                        if (oVal.Length > 0)
                        {
                            oiLin = oVal + "\n";
                            oiLin += String.Format("{0:0,0.#}", timVal) + "s\n";
                            oinf += oiLin;
                        }
                        nt.OtherInfo = oinf;
                        break;
                    case 677:   // Online
                        timVal = GetDecimalFromVariableIA(row, (int)modA.valI, (int)modA.valF);
                        timVal = timVal / 1000; // convert ms to seconds
                        nt.Online_Time = Convert.ToDecimal(timVal);

                        oVal = row.ItemArray[(int)modA.dspNm].ToString();
                        if (oVal.Length > 0)
                        {
                            oiLin = oVal + "\n";
                            oiLin += String.Format("{0:0,0.#}", timVal) + "s\n";
                            oinf += oiLin;
                        }
                        nt.OtherInfo = oinf;
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
                        oVal = row.ItemArray[(int)modA.dspNm].ToString();
                        if (oVal.Length > 0)
                        {
                            oiLin = oVal + " --> " + Convert.ToInt32(row.ItemArray[(int)modA.attID]) + "\n";
                            //oiLin = oVal + "\n";
                            oVal = row.ItemArray[(int)modA.valI].ToString();
                            if (oVal.Length > 0)
                                oiLin += oVal + "\n";
                            oVal = row.ItemArray[(int)modA.valF].ToString();
                            if (oVal.Length > 0)
                                oiLin += oVal + "\n";

                            if (oiLin.Length > 0)
                            {
                                nt.Extra.Add(oiLin);
                                oinf += oiLin;
                            }
                        }
                        nt.OtherInfo = oinf;
                        break;
                }
            }
            // Pick up the last module
            PopulateModuleChargeData(nt);
        }

        public void PopulateModuleListing(Object o)
        {
            PermormModuleAndChargeSQLQueries();
            PopulateModuleData();
            SaveModuleListing();
            PlugInData.resetEvents[1].Set();
        }

        public void GetImage(Object o)
        {
            Bitmap bmp;
            string imgLoc, imgId;

            imgId = Convert.ToString((int)o);

            imgLoc = EveHQ.Core.ImageHandler.GetImageLocation(imgId, Convert.ToInt32(EveHQ.Core.ImageHandler.ImageType.Types));

            try
            {
                bmp = new Bitmap(Image.FromFile(imgLoc));
            }
            catch
            {
            }

        }

        public void SaveModuleListing()
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

            fname = PoSCache_Path + @"\Module_List.bin";

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Modules);
            pStream.Close();
        }

        public void LoadModuleListing()
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

            fname = PoSCache_Path + @"\Module_List.bin";
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Modules = (ArrayList)myBf.Deserialize(cStr);
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
