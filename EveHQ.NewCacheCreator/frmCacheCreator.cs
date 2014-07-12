//  ==============================================================================
//  
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2014  EveHQ Development Team
//    
//  This file is part of EveHQ.
//   
//  The source code for EveHQ is free and you may redistribute 
//  it and/or modify it under the terms of the MIT License. 
//  
//  Refer to the NOTICES file in the root folder of EVEHQ source
//  project for details of 3rd party components that are covered
//  under their own, separate licenses.
//  
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
//  license below for details.
//  
//  ------------------------------------------------------------------------------
//  
//  The MIT License (MIT)
//  
//  Copyright © 2005-2014  EveHQ Development Team
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  
// ==============================================================================

namespace EveHQ.NewCacheCreator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SQLite;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using EveHQ.EveData;
    using EveHQ.HQF;
    using EveHQ.HQF.Classes;
    using EveHQ.NewCacheCreator.Properties;

    using Microsoft.VisualBasic;

    using ProtoBuf;

    using YamlDotNet.RepresentationModel;

    using Attribute = EveHQ.HQF.Attribute;

    public partial class FrmCacheCreator
    {
        private const string CacheFolderName = "StaticData";

        private static readonly SortedList<int, YAMLCert> YamlCerts = new SortedList<int, YAMLCert>();

        // For SDE connection

        private readonly string _sqLiteDb; // = Path.Combine(_sqLiteDBFolder, "EveHQMaster.db3");

        private readonly string _sqLiteDbFolder = Path.Combine(Application.StartupPath, "EveCache");

        private static string coreCacheFolder;

        private static DataSet marketGroupData;

        private static DataSet moduleAttributeData;

        private static DataSet moduleData;

        private static DataSet moduleEffectData;

        private static DataSet shipGroupData;

        private static DataSet shipNameData;

        // Key = iconID, Value = iconFile
        private static SortedList<int, string> yamlIcons;

        private static SortedList<int, YAMLType> yamlTypes;

        private string StaticDBConnection = "Server=localhost\\SQLExpress; Database = {0}; Integrated Security = SSPI;";

        public FrmCacheCreator()
        {
            InitializeComponent();
            Load += frmCacheCreator_Load;
            _sqLiteDb = Path.Combine(_sqLiteDbFolder, "EveHQMaster.db3");
        }

        public void GenerateHQFCacheData()
        {
            LoadAttributes();
            LoadSkillData();
            LoadShipGroupData();
            LoadMarketGroupData();
            LoadShipNameData();
            LoadShipAttributeData();
            PopulateShipLists();
            LoadModuleData();
            LoadModuleEffectData();
            LoadModuleAttributeData();
            LoadModuleMetaTypes();
            BuildModuleData();
            BuildAttributeQuickList();
            BuildModuleEffects();
            BuildImplantEffects();
            BuildShipEffects();
            BuildSubsystemEffects();
            BuildShipMarketGroups();
            BuildItemMarketGroups();
            SaveHQFCacheData();
            CleanUpData();
        }

        private void AddSQLAttributeGroupColumn(SqlConnection connection)
        {
            string strSql = "ALTER TABLE dgmAttributeTypes ADD attributeGroup INTEGER DEFAULT 0;";
            var keyCommand = new SqlCommand(strSql, connection);
            keyCommand.ExecuteNonQuery();
            strSql = "UPDATE dgmAttributeTypes SET attributeGroup=0;";
            keyCommand = new SqlCommand(strSql, connection);
            keyCommand.ExecuteNonQuery();
            string line = Resources.attributeGroups.Replace(ControlChars.CrLf, Strings.Chr(13).ToString());
            string[] lines = line.Split(Strings.Chr(13));
            // Read the first line which is a header line
            foreach (string lineLoopVariable in lines)
            {
                line = lineLoopVariable;
                if (line.StartsWith("attributeID", StringComparison.Ordinal) == false & !string.IsNullOrEmpty(line))
                {
                    string[] fields = line.Split(",".ToCharArray());
                    string strSql2 = "UPDATE dgmAttributeTypes SET attributeGroup=" + fields[1] + " WHERE attributeID=" + fields[0] + ";";
                    var keyCommand2 = new SqlCommand(strSql2, connection);
                    keyCommand2.ExecuteNonQuery();
                }
            }
        }

        private void BuildAttributeQuickList()
        {
            Attributes.AttributeQuickList.Clear();
            foreach (int att in Attributes.AttributeList.Keys)
            {
                Attribute attData = Attributes.AttributeList[att];
                if (!string.IsNullOrEmpty(attData.DisplayName))
                {
                    Attributes.AttributeQuickList.Add(attData.ID, attData.DisplayName);
                }
                else
                {
                    Attributes.AttributeQuickList.Add(attData.ID, attData.Name);
                }
            }
        }

        private bool BuildImplantData()
        {
            try
            {
                // Build the List of implants from the modules?
                foreach (ShipModule impMod in ModuleLists.ModuleList.Values)
                {
                    if (impMod.IsImplant)
                    {
                        Implants.ImplantList.Add(impMod.Name, impMod);
                    }
                    if (impMod.IsBooster)
                    {
                        Boosters.BoosterList.Add(impMod.Name, impMod);
                    }
                }
                // Extract the groups from the included resource file
                string[] implantsList = ResourceHandler.GetResource("ImplantEffects").Split(ControlChars.CrLf.ToCharArray());
                string[] implantData = null;
                string implantName = null;
                string implantGroups = null;
                string[] implantGroup = null;
                foreach (string cImplant in implantsList)
                {
                    if (!string.IsNullOrEmpty(cImplant.Trim()) & cImplant.StartsWith("#", StringComparison.Ordinal) == false)
                    {
                        implantData = cImplant.Split(",".ToCharArray());
                        implantName = implantData[10];
                        implantGroups = implantData[9];
                        implantGroup = implantGroups.Split(";".ToCharArray());
                        if (Implants.ImplantList.ContainsKey(implantName))
                        {
                            ShipModule bImplant = Implants.ImplantList[implantName];
                            foreach (string impGroup in implantGroup)
                            {
                                bImplant.ImplantGroups.Add(impGroup);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error building Implant Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void BuildImplantEffects()
        {
            // Break the Effects down into separate lines
            string[] effectLines = ResourceHandler.GetResource("ImplantEffects").Split(ControlChars.CrLf.ToCharArray());
            // Go through lines and break each one down
            string[] effectData = null;
            ImplantEffect newEffect = default(ImplantEffect);
            string[] ids = null;
            string[] attIDs = null;
            var atts = new ArrayList();
            string affectingName = null;
            foreach (string effectLine in effectLines)
            {
                if (!string.IsNullOrEmpty(effectLine.Trim()) & effectLine.StartsWith("#", StringComparison.Ordinal) == false)
                {
                    effectData = effectLine.Split(",".ToCharArray());
                    atts.Clear();
                    if (effectData[3].Contains(";"))
                    {
                        attIDs = effectData[3].Split(";".ToCharArray());
                        foreach (string attID in attIDs)
                        {
                            atts.Add(attID);
                        }
                    }
                    else
                    {
                        atts.Add(effectData[3]);
                    }
                    foreach (string att in atts)
                    {
                        newEffect = new ImplantEffect();
                        newEffect.ImplantName = Convert.ToString(effectData[10]);
                        newEffect.AffectingAtt = Convert.ToInt32(effectData[0]);
                        newEffect.AffectedAtt = Convert.ToInt32(att);
                        newEffect.AffectedType = (HQFEffectType)int.Parse(effectData[4]);
                        if (effectData[5].Contains(";"))
                        {
                            ids = effectData[5].Split(";".ToCharArray());
                            foreach (string id in ids)
                            {
                                newEffect.AffectedID.Add(Convert.ToInt32(id));
                            }
                        }
                        else
                        {
                            newEffect.AffectedID.Add(Convert.ToInt32(effectData[5]));
                        }
                        newEffect.CalcType = (EffectCalcType)int.Parse(effectData[6]);
                        ShipModule cImplant = Implants.ImplantList[newEffect.ImplantName];
                        newEffect.Value = Convert.ToDouble(cImplant.Attributes[Convert.ToInt32(effectData[0])]);
                        newEffect.IsGang = Convert.ToBoolean(effectData[8]);
                        if (effectData[9].Contains(";"))
                        {
                            ids = effectData[9].Split(";".ToCharArray());
                            foreach (string id in ids)
                            {
                                newEffect.Groups.Add(id);
                            }
                        }
                        else
                        {
                            newEffect.Groups.Add(effectData[9]);
                        }

                        affectingName = StaticData.Types[Convert.ToInt32(effectData[2])].Name + ";Implant;" + Attributes.AttributeQuickList[newEffect.AffectedAtt] + ";";

                        foreach (ShipModule cModule in ModuleLists.ModuleList.Values)
                        {
                            switch (newEffect.AffectedType)
                            {
                                case HQFEffectType.All:
                                    if (Convert.ToInt32(effectData[2]) != 0)
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.Item:
                                    if (newEffect.AffectedID.Contains(cModule.ID))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.Group:
                                    if (newEffect.AffectedID.Contains(cModule.DatabaseGroup))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.Category:
                                    if (newEffect.AffectedID.Contains(cModule.DatabaseCategory))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.MarketGroup:
                                    if (newEffect.AffectedID.Contains(cModule.MarketGroup))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.Attribute:
                                    if (cModule.Attributes.ContainsKey(newEffect.AffectedID[0]))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void BuildItemMarketGroups()
        {
            var tvwItems = new TreeView();
            tvwItems.BeginUpdate();
            tvwItems.Nodes.Clear();
            DataTable marketTable = marketGroupData.Tables[0];
            DataRow[] rootRows = marketTable.Select("ISNULL(parentGroupID, 0) = 0");
            foreach (DataRow rootRow in rootRows)
            {
                var rootNode = new TreeNode(Convert.ToString(rootRow["marketGroupName"]));
                rootNode.Name = rootNode.Text;
                PopulateModuleGroups(Convert.ToInt32(rootRow["marketGroupID"]), ref rootNode, marketTable);
                switch (rootNode.Text)
                {
                    case "Ship Equipment":
                    case "Ammunition & Charges":
                    case "Drones":
                    case "Ship Modifications":
                    case "Implants & Boosters":
                        tvwItems.Nodes.Add(rootNode);
                        break;
                }
            }
            tvwItems.Sorted = true;
            tvwItems.Sorted = false;
            // Add the Favourties Node
            var favNode = new TreeNode("Favourites");
            favNode.Name = "Favourites";
            favNode.Tag = "Favourites";
            tvwItems.Nodes.Add(favNode);
            // Add the Favourties Node
            var mruNode = new TreeNode("Recently Used");
            mruNode.Name = "Recently Used";
            mruNode.Tag = "Recently Used";
            tvwItems.Nodes.Add(mruNode);
            tvwItems.EndUpdate();
            Market.MarketGroupPath.Clear();
            BuildTreePathData(tvwItems);
            WriteItemGroups(tvwItems);
            tvwItems.Dispose();
        }

        private bool BuildModuleAttributeData()
        {
            try
            {
                // Get details of module attributes from already retrieved dataset
                double attValue = 0;
                string pSkillName = "";
                string sSkillName = "";
                string tSkillName = "";
                string lastModName = "";
                foreach (DataRow modRow in moduleAttributeData.Tables[0].Rows)
                {
                    ShipModule attMod = ModuleLists.ModuleList[Convert.ToInt32(modRow["typeID"])];
                    //If attMod IsNot Nothing Then
                    if (lastModName != modRow["typeName"].ToString() && !string.IsNullOrEmpty(lastModName))
                    {
                        pSkillName = "";
                        sSkillName = "";
                        tSkillName = "";
                    }
                    // Now get, modify (if applicable) and add the "attribute"
                    if (Information.IsDBNull(modRow["valueFloat"]) == false)
                    {
                        attValue = Convert.ToDouble(modRow["valueFloat"]);
                    }
                    else
                    {
                        attValue = Convert.ToDouble(modRow["valueInt"]);
                    }

                    switch (modRow["unitID"].ToString())
                    {
                        case "108":
                            attValue = Math.Round(100 - (attValue * 100), 2);
                            break;
                        case "109":
                            attValue = Math.Round((attValue * 100) - 100, 2);
                            break;
                        case "111":
                            attValue = Math.Round((attValue - 1) * 100, 2);
                            break;
                        case "101":
                            // If unit is "ms"
                            if (attValue > 1000)
                            {
                                attValue = Math.Round(attValue / 1000, 2);
                            }
                            break;
                    }

                    // Modify the resists attribute values of damage controls and bastion mods - this is to stack up later on
                    if (attMod.DatabaseGroup == 60 | attMod.ID == 33400)
                    {
                        switch (Convert.ToInt32(modRow["attributeID"]))
                        {
                            case 267:
                            case 268:
                            case 269:
                            case 270:
                            case 271:
                            case 272:
                            case 273:
                            case 274:
                            case 974:
                            case 975:
                            case 976:
                            case 977:
                                attValue = -attValue;
                                break;
                        }
                    }

                    // Do custom attribute changes here!
                    switch (Convert.ToInt32(modRow["attributeID"]))
                    {
                        case 204:
                            if (Convert.ToInt32(attValue) == -100)
                            {
                                break; // TODO: might not be correct. Was : Exit Select
                            }

                            attMod.Attributes.Add(Convert.ToInt32(modRow["attributeID"]), attValue);
                            break;
                        case 51:
                            // ROF
                            if (Convert.ToInt32(attValue) == -100)
                            {
                                break; // TODO: might not be correct. Was : Exit Select
                            }

                            switch (attMod.DatabaseGroup)
                            {
                                case 53:
                                    // Energy Turret 
                                    attMod.Attributes.Add(10011, attValue);
                                    break;
                                case 74:
                                    // Hybrid Turret
                                    attMod.Attributes.Add(10012, attValue);
                                    break;
                                case 55:
                                    // Projectile Turret
                                    attMod.Attributes.Add(10013, attValue);
                                    break;
                                default:
                                    attMod.Attributes.Add(Convert.ToInt32(modRow["attributeID"]), attValue);
                                    break;
                            }
                            break;
                        case 64:
                            // Damage Modifier
                            if (Convert.ToInt32(attValue) == 0)
                            {
                                break; // TODO: might not be correct. Was : Exit Select
                            }

                            switch (attMod.DatabaseGroup)
                            {
                                case 53:
                                    // Energy Turret 
                                    attMod.Attributes.Add(10014, attValue);
                                    break;
                                case 74:
                                    // Hybrid Turret
                                    attMod.Attributes.Add(10015, attValue);
                                    break;
                                case 55:
                                    // Projectile Turret
                                    attMod.Attributes.Add(10016, attValue);
                                    break;
                                default:
                                    attMod.Attributes.Add(Convert.ToInt32(modRow["attributeID"]), attValue);
                                    break;
                            }
                            break;
                        case 306:
                            // Max Velocity Penalty
                            switch (attMod.DatabaseGroup)
                            {
                                case 653:
                                case 654:
                                case 655:
                                case 656:
                                case 657:
                                case 648:
                                    // T2 Missiles
                                    if (Convert.ToInt32(attValue) == -100)
                                    {
                                        attValue = 0;
                                    }
                                    break;
                            }
                            attMod.Attributes.Add(Convert.ToInt32(modRow["attributeID"]), attValue);
                            break;
                        case 144:
                            // Cap Recharge Rate
                            switch (attMod.DatabaseGroup)
                            {
                                case 653:
                                case 654:
                                case 655:
                                case 656:
                                case 657:
                                case 648:
                                    // T2 Missiles
                                    if (Convert.ToInt32(attValue) == -100)
                                    {
                                        attValue = 0;
                                    }
                                    break;
                            }
                            attMod.Attributes.Add(Convert.ToInt32(modRow["attributeID"]), attValue);
                            break;
                        case 267:
                        case 268:
                        case 269:
                        case 270:
                            // Armor resistances
                            // Invert Armor Resistance Shift Hardener values
                            switch (attMod.DatabaseGroup)
                            {
                                case 1150:
                                    attValue = -attValue;
                                    break;
                            }
                            attMod.Attributes.Add(Convert.ToInt32(modRow["attributeID"]), attValue);
                            break;
                        default:
                            attMod.Attributes.Add(Convert.ToInt32(modRow["attributeID"]), attValue);
                            break;
                    }

                    switch (Convert.ToInt32(modRow["attributeID"]))
                    {
                        case 30:
                            attMod.Pg = attValue;
                            break;
                        case 50:
                            attMod.Cpu = attValue;
                            break;
                        case 6:
                            attMod.CapUsage = attValue;
                            break;
                        case 51:
                            if (attMod.Attributes.ContainsKey(6))
                            {
                                attMod.CapUsageRate = attMod.CapUsage / attValue;
                                attMod.Attributes.Add(10032, attMod.CapUsageRate);
                            }
                            break;
                        case 73:
                            attMod.ActivationTime = attValue;
                            attMod.CapUsageRate = attMod.CapUsage / attMod.ActivationTime;
                            attMod.Attributes.Add(10032, attMod.CapUsageRate);
                            break;
                        case 77:
                            switch (Convert.ToInt32(attMod.MarketGroup))
                            {
                                case 1038:
                                    // Ice Mining
                                    attMod.Attributes[10041] = Convert.ToDouble(attMod.Attributes[77]) / Convert.ToDouble(attMod.Attributes[73]);
                                    break;
                                case 1039:
                                case 1040:
                                    // Ore Mining
                                    attMod.Attributes[10039] = Convert.ToDouble(attMod.Attributes[77]) / Convert.ToDouble(attMod.Attributes[73]);
                                    break;
                                case 158:
                                    // Mining Drone
                                    attMod.Attributes[10040] = Convert.ToDouble(attMod.Attributes[77]) / Convert.ToDouble(attMod.Attributes[73]);
                                    break;
                            }
                            break;
                        case 128:
                            attMod.ChargeSize = Convert.ToInt32(attValue);
                            break;
                        case 1153:
                            attMod.Calibration = Convert.ToInt32(attValue);
                            break;
                        case 331:
                            // Slot Type for Implants
                            attMod.ImplantSlot = Convert.ToInt32(attValue);
                            break;
                        case 1087:
                            // Slot Type For Boosters
                            attMod.BoosterSlot = Convert.ToInt32(attValue);
                            break;
                        case 182:
                            if (StaticData.Types.ContainsKey(Convert.ToInt32(attValue)))
                            {
                                EveType pSkill = StaticData.Types[Convert.ToInt32(attValue)];
                                var nSkill = new ItemSkills();
                                nSkill.ID = pSkill.Id;
                                nSkill.Name = pSkill.Name;
                                pSkillName = pSkill.Name;
                                if (attMod.RequiredSkills.ContainsKey(nSkill.Name) == false)
                                {
                                    attMod.RequiredSkills.Add(nSkill.Name, nSkill);
                                }
                            }
                            break;
                        case 183:
                            if (StaticData.Types.ContainsKey(Convert.ToInt32(attValue)))
                            {
                                EveType sSkill = StaticData.Types[Convert.ToInt32(attValue)];
                                var nSkill = new ItemSkills();
                                nSkill.ID = sSkill.Id;
                                nSkill.Name = sSkill.Name;
                                sSkillName = sSkill.Name;
                                if (attMod.RequiredSkills.ContainsKey(nSkill.Name) == false)
                                {
                                    attMod.RequiredSkills.Add(nSkill.Name, nSkill);
                                }
                            }
                            break;
                        case 184:
                            if (StaticData.Types.ContainsKey(Convert.ToInt32(attValue)))
                            {
                                EveType tSkill = StaticData.Types[Convert.ToInt32(attValue)];
                                var nSkill = new ItemSkills();
                                nSkill.ID = tSkill.Id;
                                nSkill.Name = tSkill.Name;
                                tSkillName = tSkill.Name;
                                if (attMod.RequiredSkills.ContainsKey(nSkill.Name) == false)
                                {
                                    attMod.RequiredSkills.Add(nSkill.Name, nSkill);
                                }
                            }
                            break;
                        case 277:
                            if (attMod.RequiredSkills.ContainsKey(pSkillName))
                            {
                                ItemSkills cSkill = attMod.RequiredSkills[pSkillName];
                                if (cSkill != null)
                                {
                                    cSkill.Level = Convert.ToInt32(attValue);
                                }
                            }
                            break;
                        case 278:
                            if (attMod.RequiredSkills.ContainsKey(sSkillName))
                            {
                                ItemSkills cSkill = attMod.RequiredSkills[pSkillName];
                                if (cSkill != null)
                                {
                                    cSkill.Level = Convert.ToInt32(attValue);
                                }
                            }
                            break;
                        case 279:
                            if (attMod.RequiredSkills.ContainsKey(tSkillName))
                            {
                                ItemSkills cSkill = attMod.RequiredSkills[pSkillName];
                                if (cSkill != null)
                                {
                                    cSkill.Level = Convert.ToInt32(attValue);
                                }
                            }
                            break;
                        case 604:
                        case 605:
                        case 606:
                        case 609:
                        case 610:
                            attMod.Charges.Add(Convert.ToInt32(attValue));
                            break;
                        case 633:
                            // MetaLevel
                            attMod.MetaLevel = Convert.ToInt32(attValue);
                            break;
                    }
                    lastModName = modRow["typeName"].ToString();
                    // Add to the ChargeGroups if it doesn't exist and chargesize <> 0
                    //If attMod.IsCharge = True And Charges.ChargeGroups.Contains(attMod.MarketGroup & "_" & attMod.DatabaseGroup & "_" & attMod.Name & "_" & attMod.ChargeSize) = False Then
                    //    Charges.ChargeGroups.Add(attMod.MarketGroup & "_" & attMod.DatabaseGroup & "_" & attMod.Name & "_" & attMod.ChargeSize)
                    //End If
                    //End If
                }
                // Build the metaType data
                foreach (ShipModule cMod in ModuleLists.ModuleList.Values)
                {
                    if (ModuleLists.ModuleMetaGroups.ContainsKey(cMod.ID))
                    {
                        if (ModuleLists.ModuleMetaGroups[cMod.ID] == 0)
                        {
                            if (cMod.Attributes.ContainsKey(422))
                            {
                                switch (Convert.ToInt32(cMod.Attributes[422]))
                                {
                                    case 1:
                                        cMod.MetaType = Convert.ToInt32(Math.Pow(2, 0));
                                        break;
                                    case 2:
                                        cMod.MetaType = Convert.ToInt32(Math.Pow(2, 1));
                                        break;
                                    case 3:
                                        cMod.MetaType = Convert.ToInt32(Math.Pow(2, 13));
                                        break;
                                    default:
                                        cMod.MetaType = Convert.ToInt32(Math.Pow(2, 0));
                                        break;
                                }
                            }
                            else
                            {
                                cMod.MetaType = 1;
                            }
                        }
                        else
                        {
                            cMod.MetaType = Convert.ToInt32(Math.Pow(2, (Convert.ToInt32(ModuleLists.ModuleMetaGroups[cMod.ID]) - 1)));
                        }
                    }
                    else
                    {
                        cMod.MetaType = 1;
                    }
                }
                // Build charge data
                foreach (ShipModule cMod in ModuleLists.ModuleList.Values)
                {
                    if (cMod.IsCharge)
                    {
                        if (Charges.ChargeGroups.ContainsKey(cMod.ID) == false)
                        {
                            Charges.ChargeGroups.Add(cMod.ID, cMod.MarketGroup + "_" + cMod.DatabaseGroup + "_" + cMod.Name + "_" + cMod.ChargeSize);
                        }
                    }
                }
                // Check for drone missiles
                foreach (ShipModule cMod in ModuleLists.ModuleList.Values)
                {
                    if (cMod.IsDrone & cMod.Attributes.ContainsKey(507))
                    {
                        ShipModule chg = ModuleLists.ModuleList[Convert.ToInt32(cMod.Attributes[507])];
                        cMod.LoadedCharge = chg;
                    }
                }
                // Build the implant data
                if (BuildImplantData())
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error building Module Attribute Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void BuildModuleData()
        {
            try
            {
                ModuleLists.ModuleList.Clear();
                ModuleLists.ModuleListName.Clear();
                Implants.ImplantList.Clear();
                Boosters.BoosterList.Clear();
                foreach (DataRow row in moduleData.Tables[0].Rows)
                {
                    var newModule = new ShipModule();
                    newModule.ID = Convert.ToInt32(row["typeID"]);
                    newModule.Description = row["description"].ToString();
                    newModule.Name = row["typeName"].ToString().Trim();
                    newModule.DatabaseGroup = Convert.ToInt32(row["groupID"]);
                    newModule.DatabaseCategory = Convert.ToInt32(row["categoryID"]);
                    newModule.BasePrice = Convert.ToDouble(row["baseprice"]);
                    newModule.Volume = Convert.ToDouble(row["volume"]);
                    newModule.Capacity = Convert.ToDouble(row["capacity"]);
                    newModule.Attributes.Add((int)AttributeEnum.ModuleCapacity, Convert.ToDouble(row["capacity"]));
                    newModule.Attributes.Add((int)AttributeEnum.ModuleMass, Convert.ToDouble(row["mass"]));
                    newModule.MarketPrice = 0;
                    // Get icon from the YAML parsing
                    //newModule.Icon = row["iconFile").ToString
                    if (yamlTypes.ContainsKey(Convert.ToInt32(newModule.ID)))
                    {
                        newModule.Icon = yamlTypes[Convert.ToInt32(newModule.ID)].IconName;
                    }
                    if (Information.IsDBNull(row["marketGroupID"]) == false)
                    {
                        newModule.MarketGroup = Convert.ToInt32(row["marketGroupID"]);
                    }
                    else
                    {
                        newModule.MarketGroup = 0;
                    }
                    newModule.Cpu = 0;
                    newModule.Pg = 0;
                    newModule.Calibration = 0;
                    newModule.CapUsage = 0;
                    newModule.ActivationTime = 0;
                    ModuleLists.ModuleList.Add(newModule.ID, newModule);
                    ModuleLists.ModuleListName.Add(newModule.Name, newModule.ID);

                    // Determine whether implant, drone, charge etc
                    switch (Convert.ToInt32(row["categoryID"]))
                    {
                        case 2:
                            // Container
                            newModule.IsContainer = true;
                            break;
                        case 8:
                            // Charge
                            newModule.IsCharge = true;
                            break;
                        case 18:
                            // Drone
                            newModule.IsDrone = true;
                            break;
                        case 20:
                            // Implant
                            if (Convert.ToInt32(row["groupID"]) != 304)
                            {
                                if (Convert.ToInt32(row["groupID"]) == 303)
                                {
                                    newModule.IsBooster = true;
                                }
                                else
                                {
                                    newModule.IsImplant = true;
                                }
                            }
                            break;
                    }
                }

                // Fill in the blank market groups now the list is complete
                int modID = 0;
                int parentID = 0;
                ShipModule nModule = default(ShipModule);
                ShipModule eModule = default(ShipModule);
                for (int setNo = 0; setNo <= 1; setNo++)
                {
                    foreach (DataRow row in moduleData.Tables[0].Rows)
                    {
                        if (Information.IsDBNull(row["marketGroupID"]))
                        {
                            modID = Convert.ToInt32(row["typeID"]);
                            nModule = ModuleLists.ModuleList[modID];
                            if (ModuleLists.ModuleMetaTypes.ContainsKey(modID))
                            {
                                parentID = ModuleLists.ModuleMetaTypes[modID];
                                eModule = ModuleLists.ModuleList[parentID];
                                nModule.MarketGroup = eModule.MarketGroup;
                            }
                        }
                    }
                }

                // Search for changes/additions to the market groups from resources
                string[] changeLines = ResourceHandler.GetResource("newItemMarketGroup").Split(ControlChars.CrLf.ToCharArray());
                foreach (string marketChange in changeLines)
                {
                    if (!string.IsNullOrEmpty(marketChange.Trim()))
                    {
                        string[] changeData = marketChange.Split(",".ToCharArray());
                        int typeID = Convert.ToInt32(changeData[0]);
                        int marketGroupID = Convert.ToInt32(changeData[1]);
                        int metaTypeID = Convert.ToInt32(changeData[2]);
                        if (ModuleLists.ModuleList.ContainsKey(typeID))
                        {
                            ShipModule mModule = ModuleLists.ModuleList[typeID];
                            mModule.MarketGroup = marketGroupID;
                            if (metaTypeID != 0)
                            {
                                mModule.MetaType = metaTypeID;
                            }
                        }
                    }
                }
                BuildModuleEffectData();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error building Module Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuildModuleEffectData()
        {
            try
            {
                // Get details of module attributes from already retrieved dataset
                foreach (DataRow modRow in moduleEffectData.Tables[0].Rows)
                {
                    ShipModule effMod = ModuleLists.ModuleList[Convert.ToInt32(modRow["typeID"])];
                    if (effMod != null)
                    {
                        switch (Convert.ToInt32(modRow["effectID"]))
                        {
                            case 11:
                                // Low slot
                                effMod.SlotType = SlotTypes.Low;
                                break;
                            case 12:
                                // High slot
                                effMod.SlotType = SlotTypes.High;
                                break;
                            case 13:
                                // Mid slot
                                effMod.SlotType = SlotTypes.Mid;
                                break;
                            case 2663:
                                // Rig slot
                                effMod.SlotType = SlotTypes.Rig;
                                break;
                            case 3772:
                                // Sub slot
                                effMod.SlotType = SlotTypes.Subsystem;
                                break;
                            case 40:
                                if (effMod.DatabaseGroup != 481)
                                {
                                    effMod.IsLauncher = true;
                                }
                                break;
                            case 10:
                            case 34:
                            case 42:
                                effMod.IsTurret = true;
                                break;
                        }
                        // Add custom attributes
                        if (effMod.IsDrone | effMod.IsLauncher | effMod.IsTurret | effMod.DatabaseGroup == 72 | effMod.DatabaseGroup == 862)
                        {
                            if (effMod.Attributes.ContainsKey(10017) == false)
                            {
                                effMod.Attributes.Add(10017, 0);
                                effMod.Attributes.Add(10018, 0);
                                effMod.Attributes.Add(10019, 0);
                                effMod.Attributes.Add(10030, 0);
                                effMod.Attributes.Add(10051, 0);
                                effMod.Attributes.Add(10052, 0);
                                effMod.Attributes.Add(10053, 0);
                                effMod.Attributes.Add(10054, 0);
                            }
                        }
                        switch (Convert.ToInt32(effMod.MarketGroup))
                        {
                            case 1038:
                                // Ice Miners
                                if (effMod.Attributes.ContainsKey(10041) == false)
                                {
                                    effMod.Attributes.Add(10041, 0);
                                }
                                break;
                            case 1039:
                            case 1040:
                                // Ore Miners
                                if (effMod.Attributes.ContainsKey(10039) == false)
                                {
                                    effMod.Attributes.Add(10039, 0);
                                }
                                break;
                            case 158:
                                // Mining Drones
                                if (effMod.Attributes.ContainsKey(10040) == false)
                                {
                                    effMod.Attributes.Add(10040, 0);
                                }
                                break;
                        }
                        switch (Convert.ToInt32(effMod.DatabaseGroup))
                        {
                            case 76:
                                if (effMod.Attributes.ContainsKey(6) == false)
                                {
                                    effMod.Attributes.Add(6, 0);
                                }
                                break;
                        }
                    }
                }
                if (BuildModuleAttributeData())
                {
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error building Module Effect Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuildModuleEffects()
        {
            // Break the Effects down into separate lines
            List<string> effectLines = ResourceHandler.GetResource("Effects").Split(ControlChars.CrLf.ToCharArray()).ToList();
            // Go through lines and break each one down
            List<string> effectData = default(List<string>);
            Effect newEffect = default(Effect);
            List<string> ids = default(List<string>);
            List<string> affectingIDs = default(List<string>);
            string affectingName = "";
            foreach (string effectLine in effectLines)
            {
                if (!string.IsNullOrEmpty(effectLine.Trim()) & effectLine.StartsWith("#", StringComparison.Ordinal) == false)
                {
                    effectData = effectLine.Split(",".ToCharArray()).ToList();
                    affectingIDs = effectData[2].Split(";".ToCharArray()).ToList();

                    foreach (string affectingID in affectingIDs)
                    {
                        newEffect = new Effect();
                        newEffect.AffectingAtt = Convert.ToInt32(effectData[0]);
                        HQFEffectType temp;
                        newEffect.AffectingType = Enum.TryParse(effectData[1], out temp) ? temp : 0;
                        newEffect.AffectingID = Convert.ToInt32(affectingID);
                        newEffect.AffectedAtt = Convert.ToInt32(effectData[3]);
                        newEffect.AffectedType = Enum.TryParse(effectData[4], out temp) ? temp : 0;
                        if (effectData[5].Contains(";"))
                        {
                            ids = effectData[5].Split(";".ToCharArray()).ToList();
                            foreach (string id in ids)
                            {
                                newEffect.AffectedID.Add(Convert.ToInt32(id));
                            }
                        }
                        else
                        {
                            newEffect.AffectedID.Add(Convert.ToInt32(effectData[5]));
                        }
                        EffectStackType tempStack;
                        EffectCalcType tempCalc;
                        newEffect.StackNerf = Enum.TryParse(effectData[6], out tempStack) ? tempStack : 0;
                        newEffect.IsPerLevel = Convert.ToBoolean(effectData[7]);
                        newEffect.CalcType = Enum.TryParse(effectData[8], out tempCalc) ? tempCalc : 0;
                        newEffect.Status = Convert.ToInt32(effectData[9]);

                        switch (newEffect.AffectingType)
                        {
                                // Setup the name as Item;Type;Attribute
                            case HQFEffectType.All:
                                affectingName = "Global;Global;" + Attributes.AttributeQuickList[newEffect.AffectedAtt];
                                break;
                            case HQFEffectType.Item:
                                if (newEffect.AffectingID > 0)
                                {
                                    affectingName = StaticData.Types[newEffect.AffectingID].Name;
                                    if (Core.HQ.SkillListName.ContainsKey(affectingName))
                                    {
                                        affectingName += ";Skill;" + Attributes.AttributeQuickList[newEffect.AffectedAtt];
                                    }
                                    else
                                    {
                                        affectingName += ";Item;" + Attributes.AttributeQuickList[newEffect.AffectedAtt];
                                    }
                                }
                                break;
                            case HQFEffectType.Group:
                                affectingName = StaticData.TypeGroups[newEffect.AffectingID] + ";Group;" + Attributes.AttributeQuickList[newEffect.AffectedAtt];
                                break;
                            case HQFEffectType.Category:
                                affectingName = StaticData.TypeCats[newEffect.AffectingID] + ";Category;" + Attributes.AttributeQuickList[newEffect.AffectedAtt];
                                break;
                            case HQFEffectType.MarketGroup:
                                affectingName = Market.MarketGroupList[newEffect.AffectingID.ToString()] + ";Market Group;" + Attributes.AttributeQuickList[newEffect.AffectedAtt];
                                break;
                        }
                        affectingName += ";";

                        foreach (ShipModule cModule in ModuleLists.ModuleList.Values)
                        {
                            switch (newEffect.AffectedType)
                            {
                                case HQFEffectType.All:
                                    if (newEffect.AffectingID != 0)
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.Item:
                                    if (newEffect.AffectedID.Contains(cModule.ID))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.Group:
                                    if (newEffect.AffectedID.Contains(cModule.DatabaseGroup))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.Category:
                                    if (newEffect.AffectedID.Contains(cModule.DatabaseCategory))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.MarketGroup:
                                    if (newEffect.AffectedID.Contains(cModule.MarketGroup))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.Skill:
                                    if (cModule.RequiredSkills.ContainsKey(StaticData.Types[newEffect.AffectedID[0]].Name))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                                case HQFEffectType.Attribute:
                                    if (cModule.Attributes.ContainsKey(newEffect.AffectedID[0]))
                                    {
                                        cModule.Affects.Add(affectingName);
                                    }
                                    break;
                            }
                        }

                        // Add the skills into the ship
                        if (newEffect.Status < 16)
                        {
                            if (affectingName.Contains(";Skill;"))
                            {
                                foreach (Ship cShip in ShipLists.ShipList.Values)
                                {
                                    switch (newEffect.AffectedType)
                                    {
                                        case HQFEffectType.All:
                                            if (newEffect.AffectingID != 0)
                                            {
                                                cShip.Affects.Add(affectingName);
                                            }
                                            break;
                                        case HQFEffectType.Item:
                                            if (newEffect.AffectedID.Contains(cShip.ID))
                                            {
                                                cShip.Affects.Add(affectingName);
                                            }
                                            break;
                                        case HQFEffectType.Group:
                                            if (newEffect.AffectedID.Contains(cShip.DatabaseGroup))
                                            {
                                                cShip.Affects.Add(affectingName);
                                            }
                                            break;
                                        case HQFEffectType.Category:
                                            if (newEffect.AffectedID.Contains(cShip.DatabaseCategory))
                                            {
                                                cShip.Affects.Add(affectingName);
                                            }
                                            break;
                                        case HQFEffectType.MarketGroup:
                                            if (newEffect.AffectedID.Contains(cShip.MarketGroup))
                                            {
                                                cShip.Affects.Add(affectingName);
                                            }
                                            break;
                                        case HQFEffectType.Attribute:
                                            if (cShip.Attributes.ContainsKey(newEffect.AffectedID[0]))
                                            {
                                                cShip.Affects.Add(affectingName);
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BuildShipEffects()
        {
            var culture = new CultureInfo("en-GB");
            // Break the Effects down into separate lines
            string[] effectLines = ResourceHandler.GetResource("ShipBonuses").Split(ControlChars.CrLf.ToCharArray());
            // Go through lines and break each one down
            string[] effectData = null;

            var shipEffectClassList = new ArrayList();
            ShipEffect newEffect = default(ShipEffect);
            string[] ids = null;
            string affectingName = null;
            foreach (string effectLine in effectLines)
            {
                if (!string.IsNullOrEmpty(effectLine.Trim()) & effectLine.StartsWith("#", StringComparison.Ordinal) == false)
                {
                    effectData = effectLine.Split(",".ToCharArray());
                    newEffect = new ShipEffect();
                    newEffect.ShipID = Convert.ToInt32(effectData[0]);
                    newEffect.AffectingType = (HQFEffectType)int.Parse(effectData[1]);
                    newEffect.AffectingID = Convert.ToInt32(effectData[2]);
                    newEffect.AffectedAtt = Convert.ToInt32(effectData[3]);
                    newEffect.AffectedType = (HQFEffectType)int.Parse(effectData[4]);
                    if (effectData[5].Contains(";"))
                    {
                        ids = effectData[5].Split(";".ToCharArray());
                        foreach (string id in ids)
                        {
                            newEffect.AffectedID.Add(Convert.ToInt32(id));
                        }
                    }
                    else
                    {
                        newEffect.AffectedID.Add(Convert.ToInt32(effectData[5]));
                    }
                    newEffect.StackNerf = (EffectStackType)int.Parse(effectData[6]);
                    newEffect.IsPerLevel = Convert.ToBoolean(effectData[7]);
                    newEffect.CalcType = (EffectCalcType)int.Parse(effectData[8]);
                    newEffect.Value = double.Parse(effectData[9], NumberStyles.Any, culture);
                    newEffect.Status = Convert.ToInt32(effectData[10]);
                    shipEffectClassList.Add(newEffect);

                    affectingName = StaticData.Types[newEffect.ShipID].Name;
                    if (newEffect.IsPerLevel == false)
                    {
                        affectingName += ";Ship Role;";
                    }
                    else
                    {
                        affectingName += ";Ship Bonus;";
                    }
                    affectingName += Attributes.AttributeQuickList[newEffect.AffectedAtt];
                    if (newEffect.IsPerLevel == false)
                    {
                        affectingName += ";";
                    }
                    else
                    {
                        affectingName += ";" + StaticData.Types[newEffect.AffectingID].Name;
                    }

                    // Add the skills into the ship modules
                    foreach (ShipModule cModule in ModuleLists.ModuleList.Values)
                    {
                        switch (newEffect.AffectedType)
                        {
                            case HQFEffectType.All:
                                if (newEffect.AffectingID != 0)
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.Item:
                                if (newEffect.AffectedID.Contains(cModule.ID))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.Group:
                                if (newEffect.AffectedID.Contains(cModule.DatabaseGroup))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.Category:
                                if (newEffect.AffectedID.Contains(cModule.DatabaseCategory))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.MarketGroup:
                                if (newEffect.AffectedID.Contains(cModule.MarketGroup))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.Attribute:
                                if (cModule.Attributes.ContainsKey(newEffect.AffectedID[0]))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                        }
                    }

                    // Add the skills into the ship global skills

                    if (newEffect.Status < 16)
                    {
                        foreach (Ship cShip in ShipLists.ShipList.Values)
                        {
                            if (newEffect.ShipID == Convert.ToInt32(cShip.ID))
                            {
                                if (cShip.GlobalAffects == null)
                                {
                                    cShip.GlobalAffects = new List<string>();
                                }
                                cShip.GlobalAffects.Add(affectingName);
                            }
                        }
                    }
                }
            }
        }

        private void BuildShipMarketGroups()
        {
            var tvwShips = new TreeView();
            tvwShips.BeginUpdate();
            tvwShips.Nodes.Clear();
            DataTable marketTable = marketGroupData.Tables[0];
            DataRow[] rootRows = marketTable.Select("ISNULL(parentGroupID, 0) = 0");
            foreach (DataRow rootRow in rootRows)
            {
                var rootNode = new TreeNode(Convert.ToString(rootRow["marketGroupName"]));
                PopulateShipGroups(Convert.ToInt32(rootRow["marketGroupID"]), ref rootNode, marketTable);
                switch (rootNode.Text)
                {
                    case "Ships":
                        foreach (TreeNode childNode in rootNode.Nodes)
                        {
                            tvwShips.Nodes.Add(childNode);
                        }

                        break;
                }
            }
            // Now check for Faction ships
            string shipGroup = null;
            DataRow[] factionRows = shipNameData.Tables[0].Select("ISNULL(marketGroupID, 0) = 0");
            foreach (DataRow factionRow in factionRows)
            {
                shipGroup = factionRow["groupName"] + "s";
                foreach (TreeNode groupNode in tvwShips.Nodes)
                {
                    if (groupNode.Text == shipGroup)
                    {
                        // Check for "Faction" node
                        if (groupNode.Nodes.ContainsKey("Faction") == false)
                        {
                            groupNode.Nodes.Add("Faction", "Faction");
                        }
                        // Add to the "Faction" node
                        groupNode.Nodes["Faction"].Nodes.Add(factionRow["typeName"].ToString());
                    }
                }
            }
            tvwShips.Sorted = true;
            tvwShips.EndUpdate();
            WriteShipGroups(tvwShips);
            tvwShips.Dispose();
        }

        private void BuildSubsystemEffects()
        {
            var culture = new CultureInfo("en-GB");

            // Break the Effects down into separate lines
            string[] effectLines = ResourceHandler.GetResource("Subsystems").Split(ControlChars.CrLf.ToCharArray());
            // Go through lines and break each one down
            string[] effectData = null;

            var shipEffectClassList = new ArrayList();
            ShipEffect newEffect = default(ShipEffect);
            string[] ids = null;
            string affectingName = null;
            foreach (string effectLine in effectLines)
            {
                if (!string.IsNullOrEmpty(effectLine.Trim()) & effectLine.StartsWith("#", StringComparison.Ordinal) == false)
                {
                    effectData = effectLine.Split(",".ToCharArray());
                    newEffect = new ShipEffect();
                    newEffect.ShipID = Convert.ToInt32(effectData[0]);
                    newEffect.AffectingType = (HQFEffectType)int.Parse(effectData[1]);
                    newEffect.AffectingID = Convert.ToInt32(effectData[2]);
                    newEffect.AffectedAtt = Convert.ToInt32(effectData[3]);
                    newEffect.AffectedType = (HQFEffectType)int.Parse(effectData[4]);
                    if (effectData[5].Contains(";"))
                    {
                        ids = effectData[5].Split(";".ToCharArray());
                        foreach (string id in ids)
                        {
                            newEffect.AffectedID.Add(Convert.ToInt32(id));
                        }
                    }
                    else
                    {
                        newEffect.AffectedID.Add(Convert.ToInt32(effectData[5]));
                    }
                    newEffect.StackNerf = (EffectStackType)int.Parse(effectData[6]);
                    newEffect.IsPerLevel = Convert.ToBoolean(effectData[7]);
                    newEffect.CalcType = (EffectCalcType)int.Parse(effectData[8]);
                    newEffect.Value = double.Parse(effectData[9], NumberStyles.Any, culture);
                    newEffect.Status = Convert.ToInt32(effectData[10]);
                    shipEffectClassList.Add(newEffect);

                    affectingName = StaticData.Types[newEffect.ShipID].Name;
                    if (newEffect.IsPerLevel == false)
                    {
                        affectingName += ";Subsystem Role;";
                    }
                    else
                    {
                        affectingName += ";Subsystem;";
                    }
                    affectingName += Attributes.AttributeQuickList[newEffect.AffectedAtt];
                    if (newEffect.IsPerLevel == false)
                    {
                        affectingName += ";";
                    }
                    else
                    {
                        affectingName += ";" + StaticData.Types[newEffect.AffectingID].Name;
                    }

                    foreach (ShipModule cModule in ModuleLists.ModuleList.Values)
                    {
                        switch (newEffect.AffectedType)
                        {
                            case HQFEffectType.All:
                                if (newEffect.AffectingID != 0)
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.Item:
                                if (newEffect.AffectedID.Contains(cModule.ID))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.Group:
                                if (newEffect.AffectedID.Contains(cModule.DatabaseGroup))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.Category:
                                if (newEffect.AffectedID.Contains(cModule.DatabaseCategory))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.MarketGroup:
                                if (newEffect.AffectedID.Contains(cModule.MarketGroup))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                            case HQFEffectType.Attribute:
                                if (cModule.Attributes.ContainsKey(newEffect.AffectedID[0]))
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                                break;
                        }
                        // Add the skill onto the subsystem
                        if (newEffect.IsPerLevel)
                        {
                            if (cModule.ID == newEffect.ShipID)
                            {
                                affectingName = StaticData.Types[newEffect.AffectingID].Name;
                                affectingName += ";Skill;" + Attributes.AttributeQuickList[newEffect.AffectedAtt];
                                if (cModule.Affects.Contains(affectingName) == false)
                                {
                                    cModule.Affects.Add(affectingName);
                                }
                            }
                        }
                    }

                    // Add the skills into the ship
                    //If newEffect.Status < 16 Then
                    //    If AffectingName.Contains(";Skill;") = True Then
                    //        For Each cShip As Ship In ShipLists.shipList.Values
                    //            Select case newEffect.AffectedType
                    //                case EffectType.All
                    //                    If newEffect.AffectingID <> 0 Then
                    //                        cShip.Affects.Add(AffectingName)
                    //                    End If
                    //                case EffectType.Item
                    //                    If newEffect.AffectedID.Contains(cShip.ID) Then
                    //                        cShip.Affects.Add(AffectingName)
                    //                    End If
                    //                case EffectType.Group
                    //                    If newEffect.AffectedID.Contains(cShip.DatabaseGroup) Then
                    //                        cShip.Affects.Add(AffectingName)
                    //                    End If
                    //                case EffectType.Category
                    //                    If newEffect.AffectedID.Contains(cShip.DatabaseCategory) Then
                    //                        cShip.Affects.Add(AffectingName)
                    //                    End If
                    //                case EffectType.MarketGroup
                    //                    If newEffect.AffectedID.Contains(cShip.MarketGroup) Then
                    //                        cShip.Affects.Add(AffectingName)
                    //                    End If
                    //                case EffectType.Attribute
                    //                    If cShip.Attributes.Contains(newEffect.AffectedID[0].ToString) Then
                    //                        cShip.Affects.Add(AffectingName)
                    //                    End If
                    //            End Select
                    //        Next
                    //    End If
                    //End If
                }
            }
        }

        private void BuildTreePathData(TreeView tvwItems)
        {
            foreach (TreeNode rootNode in tvwItems.Nodes)
            {
                BuildTreePathData2(rootNode);
            }
        }

        private void BuildTreePathData2(TreeNode parentNode)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                if (childNode.Nodes.Count > 0)
                {
                    BuildTreePathData2(childNode);
                }
                else
                {
                    Market.MarketGroupPath.Add(childNode.Tag.ToString(), childNode.FullPath);
                }
            }
        }

        private void CheckSQLDatabase()
        {
            using (DataSet evehqData = GetStaticData("SELECT attributeGroup FROM dgmAttributeTypes"))
            {
                if (evehqData == null)
                {
                    // We seem to be missing the data so lets add it in!
                    var conn = new SqlConnection();
                    conn.ConnectionString = string.Format(StaticDBConnection, EveHQDatabaseName);
                    conn.Open();
                    AddSQLAttributeGroupColumn(conn);
                    CorrectSQLEveUnits(conn);
                    DoSQLQuery(conn);
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void CleanUpData()
        {
            marketGroupData = null;
            shipGroupData = null;
            shipNameData = null;
            moduleData = null;
            moduleEffectData = null;
            moduleAttributeData = null;
            GC.Collect();
        }

        private void CorrectSQLEveUnits(SqlConnection connection)
        {
            const string StrSQL = "UPDATE dgmAttributeTypes SET unitID=122 WHERE unitID IS NULL;";
            var keyCommand = new SqlCommand(StrSQL, connection);
            keyCommand.ExecuteNonQuery();
        }

        private void CreateCoreCache()
        {
            // Dump core data to the folder
            FileStream s = default(FileStream);

            // Item Data
            s = new FileStream(Path.Combine(coreCacheFolder, "Items.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Types);
            s.Flush();
            s.Close();

            // Market Groups
            s = new FileStream(Path.Combine(coreCacheFolder, "MarketGroups.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.MarketGroups);
            s.Flush();
            s.Close();

            // Item Market Groups
            s = new FileStream(Path.Combine(coreCacheFolder, "ItemMarketGroups.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.ItemMarketGroups);
            s.Flush();
            s.Close();

            // Item List
            s = new FileStream(Path.Combine(coreCacheFolder, "ItemList.dat"), FileMode.Create);

            Serializer.Serialize(s, StaticData.TypeNames);
            s.Flush();
            s.Close();

            // Item Groups
            s = new FileStream(Path.Combine(coreCacheFolder, "ItemGroups.dat"), FileMode.Create);

            Serializer.Serialize(s, StaticData.TypeGroups);
            s.Flush();
            s.Close();

            // Items Cats
            s = new FileStream(Path.Combine(coreCacheFolder, "ItemCats.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.TypeCats);
            s.Flush();
            s.Close();

            // Group Cats
            s = new FileStream(Path.Combine(coreCacheFolder, "GroupCats.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.GroupCats);
            s.Flush();
            s.Close();

            // Cert Categories
            s = new FileStream(Path.Combine(coreCacheFolder, "CertCats.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.CertificateCategories);
            s.Flush();
            s.Close();

            // Certs
            s = new FileStream(Path.Combine(coreCacheFolder, "Certs.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Certificates);
            s.Flush();
            s.Close();

            // Cert Recommendations
            s = new FileStream(Path.Combine(coreCacheFolder, "CertRec.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.CertificateRecommendations);
            s.Flush();
            s.Close();

            // Masteries
            s = new FileStream(Path.Combine(coreCacheFolder, "Masteries.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Masteries);
            s.Flush();
            s.Close();

            // Ship Traits
            s = new FileStream(Path.Combine(coreCacheFolder, "Traits.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Traits);
            s.Flush();
            s.Close();

            // Unlocks
            s = new FileStream(Path.Combine(coreCacheFolder, "ItemUnlocks.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.ItemUnlocks);
            s.Flush();
            s.Close();

            // SkillUnlocks
            s = new FileStream(Path.Combine(coreCacheFolder, "SkillUnlocks.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.SkillUnlocks);
            s.Flush();
            s.Close();

            // CertSkills
            s = new FileStream(Path.Combine(coreCacheFolder, "CertSkills.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.CertUnlockSkills);
            s.Flush();
            s.Close();

            // Regions
            s = new FileStream(Path.Combine(coreCacheFolder, "Regions.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Regions);
            s.Flush();
            s.Close();

            // Constellations
            s = new FileStream(Path.Combine(coreCacheFolder, "Constellations.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Constellations);
            s.Flush();
            s.Close();

            // Solar Systems
            s = new FileStream(Path.Combine(coreCacheFolder, "Systems.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.SolarSystems);
            s.Flush();
            s.Close();

            // Stations
            s = new FileStream(Path.Combine(coreCacheFolder, "Stations.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Stations);
            s.Flush();
            s.Close();

            // Divisions
            s = new FileStream(Path.Combine(coreCacheFolder, "Divisions.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Divisions);
            s.Flush();
            s.Close();

            // Agents
            s = new FileStream(Path.Combine(coreCacheFolder, "Agents.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Agents);
            s.Flush();
            s.Close();

            // Attribute Types
            s = new FileStream(Path.Combine(coreCacheFolder, "AttributeTypes.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.AttributeTypes);
            s.Flush();
            s.Close();

            // Type Attributes
            s = new FileStream(Path.Combine(coreCacheFolder, "TypeAttributes.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.TypeAttributes);
            s.Flush();
            s.Close();

            // Attribute Units
            s = new FileStream(Path.Combine(coreCacheFolder, "Units.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.AttributeUnits);
            s.Flush();
            s.Close();

            // Effect Types
            s = new FileStream(Path.Combine(coreCacheFolder, "EffectTypes.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.EffectTypes);
            s.Flush();
            s.Close();

            // Type Effects
            s = new FileStream(Path.Combine(coreCacheFolder, "TypeEffects.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.TypeEffects);
            s.Flush();
            s.Close();

            // Meta Groups
            s = new FileStream(Path.Combine(coreCacheFolder, "MetaGroups.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.MetaGroups);
            s.Flush();
            s.Close();

            // Meta Types
            s = new FileStream(Path.Combine(coreCacheFolder, "MetaTypes.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.MetaTypes);
            s.Flush();
            s.Close();

            // Type Materials
            s = new FileStream(Path.Combine(coreCacheFolder, "TypeMaterials.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.TypeMaterials);
            s.Flush();
            s.Close();

            // Blueprint Types
            s = new FileStream(Path.Combine(coreCacheFolder, "Blueprints.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.Blueprints);
            s.Flush();
            s.Close();

            // Assembly Arrays
            s = new FileStream(Path.Combine(coreCacheFolder, "AssemblyArrays.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.AssemblyArrays);
            s.Flush();
            s.Close();

            // NPC Corps
            s = new FileStream(Path.Combine(coreCacheFolder, "NPCCorps.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.NpcCorps);
            s.Flush();
            s.Close();

            // Item Flags
            s = new FileStream(Path.Combine(coreCacheFolder, "ItemFlags.dat"), FileMode.Create);
            Serializer.Serialize(s, StaticData.ItemMarkers);
            s.Flush();
            s.Close();
        }

        private void DoSQLQuery(SqlConnection connection)
        {
            string strSQL = Resources.SQLQueries;
            var keyCommand = new SqlCommand(strSQL, connection);
            keyCommand.ExecuteNonQuery();
        }

        private DataSet GetStaticData(string strSQL)
        {
            var evehqData = new DataSet();
            var conn = new SqlConnection();
            conn.ConnectionString = string.Format(StaticDBConnection, EveHQDatabaseName);
            try
            {
                conn.Open();
                var da = new SqlDataAdapter(strSQL, conn);
                da.Fill(evehqData, "evehqData");
                conn.Close();
                return evehqData;
            }
            catch (Exception e)
            {
                Core.HQ.DataError = e.Message;
                return null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void LoadAgents()
        {
            // Load the NPC Division data
            using (var connection = new SqlConnection(string.Format(StaticDBConnection, EveHQDatabaseName)))
            {
                var command = new SqlCommand("SELECT * FROM crpNPCDivisions;", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StaticData.Divisions.Add(Convert.ToInt32(reader["divisionID"]), reader["divisionName"].ToString());
                    }
                }

                reader.Close();
            }

            // Load the Agent data
            using (var connection = new SqlConnection(string.Format(StaticDBConnection, EveHQDatabaseName)))
            {
                var command =
                    new SqlCommand(
                        "SELECT agtAgents.agentID, agtAgents.divisionID, agtAgents.corporationID, agtAgents.locationID, agtAgents.[level], agtAgents.quality, agtAgents.agentTypeID, agtAgents.isLocator, invUniqueNames.itemName AS agentName FROM agtAgents INNER JOIN invUniqueNames ON agtAgents.agentID = invUniqueNames.itemID;",
                        connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Agent a = default(Agent);
                    while (reader.Read())
                    {
                        a = new Agent();
                        a.AgentId = Convert.ToInt32(reader["agentID"]);
                        a.AgentName = reader["agentName"].ToString();
                        a.AgentType = Convert.ToInt32(reader["agentTypeID"]);
                        a.CorporationId = Convert.ToInt32(reader["corporationID"]);
                        a.DivisionId = Convert.ToInt32(reader["divisionID"]);
                        a.IsLocator = Convert.ToBoolean(reader["isLocator"]);
                        a.Level = Convert.ToInt32(reader["level"]);
                        a.LocationId = Convert.ToInt32(reader["locationID"]);
                        StaticData.Agents.Add(a.AgentId, a);
                    }
                }

                reader.Close();
            }
        }

        // Key = CertID

        private void LoadAllData()
        {
            LoadItemData();
            LoadMarketGroups();
            LoadItemMarketGroups();
            LoadItemList();
            LoadItemCategories();
            LoadItemGroups();
            LoadGroupCats();

            LoadCertCategories();
            LoadCerts();
            LoadCertRecs();
            LoadMasteries();
            LoadTraits();
            LoadUnlocks();
            // Populates 4 data classes here

            LoadRegions();
            LoadConstellations();
            LoadSolarSystems();
            LoadStations();
            LoadAgents();

            LoadAttributeTypes();
            LoadTypeAttributes();
            LoadUnits();
            LoadEffectTypes();
            LoadTypeEffects();

            LoadMetaGroups();
            LoadMetaTypes();

            LoadTypeMaterials();
            LoadBlueprints();
            LoadAssemblyArrays();
            LoadNPCCorps();
            LoadItemFlags();
        }

        private void LoadAssemblyArrays()
        {
            // Get Data
            const string ArraySQL = "SELECT * FROM ramAssemblyLineTypes WHERE activityID=1 AND (baseTimeMultiplier<>1 OR baseMaterialMultiplier<>1);";
            const string GroupSQL = "SELECT * FROM ramAssemblyLineTypeDetailPerGroup;";
            const string CatSQL = "SELECT * FROM ramAssemblyLineTypeDetailPerCategory;";
            DataSet arrayDataSet = GetStaticData(ArraySQL);
            DataSet groupDataSet = GetStaticData(GroupSQL);
            DataSet catDataSet = GetStaticData(CatSQL);

            // Reset the list
            StaticData.AssemblyArrays.Clear();

            // Populate the arrays
            foreach (DataRow assArray in arrayDataSet.Tables[0].Rows)
            {
                var newArray = new AssemblyArray();
                newArray.Id = Convert.ToString(assArray["assemblyLineTypeID"]);
                newArray.Name = Convert.ToString(assArray["assemblyLineTypeName"]);
                newArray.MaterialMultiplier = Convert.ToDouble(assArray["baseMaterialMultiplier"]);
                newArray.TimeMultiplier = Convert.ToDouble(assArray["baseTimeMultiplier"]);

                DataRow[] groupRows = groupDataSet.Tables[0].Select("assemblyLineTypeID=" + newArray.Id);
                foreach (DataRow @group in groupRows)
                {
                    newArray.AllowableGroups.Add(Convert.ToInt32(@group["groupID"]));
                }
                DataRow[] catRows = catDataSet.Tables[0].Select("assemblyLineTypeID=" + newArray.Id);
                foreach (DataRow cat in catRows)
                {
                    newArray.AllowableCategories.Add(Convert.ToInt32(cat["categoryID"]));
                }
                StaticData.AssemblyArrays.Add(newArray.Name, newArray);
            }

            catDataSet.Dispose();
            groupDataSet.Dispose();
            arrayDataSet.Dispose();
        }

        private void LoadAttributeTypes()
        {
            StaticData.AttributeTypes.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM dgmAttributeTypes;"))
            {
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    var at = new AttributeType();
                    at.AttributeId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["attributeID"]);
                    at.AttributeName = Convert.ToString(evehqData.Tables[0].Rows[item]["attributeName"]).Trim();
                    if (Information.IsDBNull(evehqData.Tables[0].Rows[item]["displayName"]) == false)
                    {
                        at.DisplayName = Convert.ToString(evehqData.Tables[0].Rows[item]["displayName"]).Trim();
                    }
                    else
                    {
                        at.DisplayName = at.AttributeName;
                    }
                    if (Information.IsDBNull(evehqData.Tables[0].Rows[item]["unitID"]) == false)
                    {
                        at.UnitId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["unitID"]);
                    }
                    else
                    {
                        at.UnitId = 0;
                    }
                    if (Information.IsDBNull(evehqData.Tables[0].Rows[item]["categoryID"]) == false)
                    {
                        at.CategoryId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["categoryID"]);
                    }
                    else
                    {
                        at.CategoryId = 0;
                    }
                    StaticData.AttributeTypes.Add(at.AttributeId, at);
                }
            }
        }

        private void LoadAttributes()
        {
            try
            {
                string strSQL = "";
                strSQL +=
                    "SELECT dgmAttributeTypes.attributeID, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName AS dgmAttributeTypes_displayName, dgmAttributeTypes.unitID AS dgmAttributeTypes_unitID, dgmAttributeTypes.attributeGroup, eveUnits.unitName, eveUnits.displayName AS eveUnits_displayName";
                strSQL += " FROM eveUnits RIGHT OUTER JOIN dgmAttributeTypes ON eveUnits.unitID = dgmAttributeTypes.unitID";
                strSQL += " ORDER BY dgmAttributeTypes.attributeID;";
                DataSet attributeData = GetStaticData(strSQL);
                if (attributeData != null)
                {
                    if (attributeData.Tables[0].Rows.Count != 0)
                    {
                        Attributes.AttributeList.Clear();
                        Attribute attData = default(Attribute);
                        foreach (DataRow row in attributeData.Tables[0].Rows)
                        {
                            attData = new Attribute();
                            attData.ID = Convert.ToInt32(row["attributeID"]);
                            attData.Name = row["attributeName"].ToString();
                            attData.DisplayName = row["dgmAttributeTypes_displayName"].ToString();
                            attData.UnitName = row["eveUnits_displayName"].ToString();
                            attData.AttributeGroup = row["attributeGroup"].ToString();
                            if (attData.UnitName == "ms")
                            {
                                attData.UnitName = "s";
                            }
                            Attributes.AttributeList.Add(attData.ID, attData);
                        }
                        LoadCustomAttributes();
                    }
                    MessageBox.Show("Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Attribute Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBlueprints()
        {
            StaticData.Blueprints.Clear();
            DataSet evehqData = GetStaticData("SELECT * FROM invBlueprintTypes;");
            // Populate the main data
            foreach (DataRow bp in evehqData.Tables[0].Rows)
            {
                var bt = new Blueprint();
                bt.Id = Convert.ToInt32(bp["blueprintTypeID"]);
                bt.ProductId = Convert.ToInt32(bp["productTypeID"]);
                bt.TechLevel = Convert.ToInt32(bp["techLevel"]);
                bt.WasteFactor = Convert.ToInt32(bp["wasteFactor"]);
                bt.MaterialModifier = Convert.ToInt32(bp["materialModifier"]);
                bt.ProductivityModifier = Convert.ToInt32(bp["productivityModifier"]);
                bt.MaxProductionLimit = Convert.ToInt32(bp["maxProductionLimit"]);
                bt.ProductionTime = Convert.ToInt64(bp["productionTime"]);
                bt.ResearchMaterialLevelTime = Convert.ToInt64(bp["researchMaterialTime"]);
                bt.ResearchProductionLevelTime = Convert.ToInt64(bp["researchProductivityTime"]);
                bt.ResearchCopyTime = Convert.ToInt64(bp["researchCopyTime"]);
                bt.ResearchTechTime = Convert.ToInt64(bp["researchTechTime"]);
                StaticData.Blueprints.Add(bt.Id, bt);
            }

            // Good so far so let's add the material requirements
            evehqData =
                GetStaticData(
                    "SELECT invBuildMaterials.*, invTypes.typeName, invGroups.groupID, invGroups.categoryID FROM invGroups INNER JOIN (invTypes INNER JOIN invBuildMaterials ON invTypes.typeID = invBuildMaterials.requiredTypeID) ON invGroups.groupID = invTypes.groupID ORDER BY invBuildMaterials.typeID;");

            // Go through each BP and refine the Dataset
            foreach (Blueprint bp in StaticData.Blueprints.Values)
            {
                // Select resource data for the blueprint
                DataRow[] bpRows = evehqData.Tables[0].Select("typeID=" + bp.Id);
                foreach (DataRow req in bpRows)
                {
                    var newReq = new BlueprintResource();
                    newReq.Activity = Convert.ToInt32(req["activityID"]);
                    newReq.DamagePerJob = Convert.ToDouble(req["damagePerJob"]);
                    newReq.TypeId = Convert.ToInt32(req["requiredTypeID"]);
                    newReq.TypeGroup = Convert.ToInt32(req["groupID"]);
                    newReq.TypeCategory = Convert.ToInt32(req["categoryID"]);
                    newReq.Quantity = Convert.ToInt32(req["quantity"]);
                    if (Information.IsDBNull(req["baseMaterial"]) == false)
                    {
                        newReq.BaseMaterial = Convert.ToInt32(req["baseMaterial"]);
                    }
                    else
                    {
                        newReq.BaseMaterial = 0;
                    }
                    // Create activity if required
                    if (bp.Resources.ContainsKey(newReq.Activity) == false)
                    {
                        bp.Resources.Add(newReq.Activity, new Dictionary<int, BlueprintResource>());
                    }
                    if (bp.Resources[newReq.Activity].ContainsKey(newReq.TypeId) == false)
                    {
                        bp.Resources[newReq.Activity].Add(newReq.TypeId, newReq);
                    }
                }
                // Select resource data for the product
                if (bp.ProductId != bp.Id)
                {
                    bpRows = evehqData.Tables[0].Select("typeID=" + bp.ProductId);
                    foreach (DataRow req in bpRows)
                    {
                        var newReq = new BlueprintResource();
                        newReq.TypeId = Convert.ToInt32(req["requiredTypeID"]);
                        newReq.TypeGroup = Convert.ToInt32(req["groupID"]);
                        newReq.TypeCategory = Convert.ToInt32(req["categoryID"]);
                        newReq.Activity = Convert.ToInt32(req["activityID"]);
                        newReq.DamagePerJob = Convert.ToDouble(req["damagePerJob"]);
                        newReq.Quantity = Convert.ToInt32(req["quantity"]);
                        if (Information.IsDBNull(req["baseMaterial"]) == false)
                        {
                            newReq.BaseMaterial = Convert.ToInt32(req["baseMaterial"]);
                        }
                        else
                        {
                            newReq.BaseMaterial = 0;
                        }
                        if (bp.Resources.ContainsKey(newReq.Activity) == false)
                        {
                            bp.Resources.Add(newReq.Activity, new Dictionary<int, BlueprintResource>());
                        }
                        if (bp.Resources[newReq.Activity].ContainsKey(newReq.TypeId) == false)
                        {
                            bp.Resources[newReq.Activity].Add(newReq.TypeId, newReq);
                        }
                    }
                }
            }

            // Fetch the relevant Invention Data
            string strSQL = "SELECT SourceBP.blueprintTypeID AS SBP, InventedBP.blueprintTypeID AS IBP";
            strSQL += " FROM invBlueprintTypes AS SourceBP INNER JOIN";
            strSQL += " invMetaTypes ON SourceBP.productTypeID = invMetaTypes.parentTypeID INNER JOIN";
            strSQL += " invBlueprintTypes AS InventedBP ON invMetaTypes.typeID = InventedBP.productTypeID";
            strSQL += " WHERE (invMetaTypes.metaGroupID = 2);";
            evehqData = GetStaticData(strSQL);
            foreach (DataRow invRow in evehqData.Tables[0].Rows)
            {
                // Add the "Inventable" item
                if (StaticData.Blueprints.ContainsKey(Convert.ToInt32(invRow["SBP"])))
                {
                    StaticData.Blueprints[Convert.ToInt32(invRow["SBP"])].Inventions.Add(Convert.ToInt32(invRow["IBP"]));
                }
                // Add the "Invented From" item
                if (StaticData.Blueprints.ContainsKey(Convert.ToInt32(invRow["IBP"])))
                {
                    StaticData.Blueprints[Convert.ToInt32(invRow["IBP"])].InventFrom.Add(Convert.ToInt32(invRow["SBP"]));
                }
            }

            // Load all the meta level data for invention
            strSQL = "SELECT invBlueprintTypes.blueprintTypeID, invMetaTypes.typeID, invMetaTypes.parentTypeID FROM invBlueprintTypes INNER JOIN";
            strSQL += " invMetaTypes ON invBlueprintTypes.productTypeID = invMetaTypes.parentTypeID";
            strSQL += " WHERE (techLevel = 1)";
            strSQL += " ORDER BY parentTypeID ;";
            evehqData = GetStaticData(strSQL);
            foreach (DataRow invRow in evehqData.Tables[0].Rows)
            {
                if (StaticData.Blueprints[Convert.ToInt32(invRow["blueprintTypeID"])].InventionMetaItems.Contains(Convert.ToInt32(invRow["parentTypeID"])) == false)
                {
                    StaticData.Blueprints[Convert.ToInt32(invRow["blueprintTypeID"])].InventionMetaItems.Add(Convert.ToInt32(invRow["parentTypeID"]));
                }
                if (StaticData.Types[Convert.ToInt32(invRow["typeID"])].MetaLevel < 5)
                {
                    StaticData.Blueprints[Convert.ToInt32(invRow["blueprintTypeID"])].InventionMetaItems.Add(Convert.ToInt32(invRow["typeID"]));
                }
            }

            evehqData.Dispose();
        }

        private void LoadCertCategories()
        {
            StaticData.CertificateCategories.Clear();
            foreach (int groupID_loopVariable in YamlCerts.Values.Select(c => c.GroupID).Distinct())
            {
                int groupID = groupID_loopVariable;
                var newCat = new CertificateCategory();
                newCat.Id = groupID;
                newCat.Name = StaticData.TypeGroups[groupID];
                StaticData.CertificateCategories.Add(newCat.Id.ToString(), newCat);
            }
        }

        private void LoadCertRecs()
        {
            // cert recommendations are now in the cert yaml data
            StaticData.CertificateRecommendations.Clear();

            foreach (YAMLCert certRow_loopVariable in YamlCerts.Values)
            {
                YAMLCert certRow = certRow_loopVariable;
                foreach (int shipRec_loopVariable in certRow.RecommendedFor)
                {
                    int shipRec = shipRec_loopVariable;
                    var certRec = new CertificateRecommendation();
                    certRec.ShipTypeId = shipRec;
                    certRec.CertificateId = certRow.CertID;
                    StaticData.CertificateRecommendations.Add(certRec);
                }
            }
        }

        private void LoadCerts()
        {
            StaticData.Certificates.Clear();
            // With Rubicon 1.0 certs are no longer in the database, but stored in yaml
            foreach (YAMLCert cert_loopVariable in YamlCerts.Values)
            {
                YAMLCert cert = cert_loopVariable;
                var newCert = new Certificate();
                newCert.GradesAndSkills.Add(CertificateGrade.Basic, new SortedList<int, int>());
                newCert.GradesAndSkills.Add(CertificateGrade.Standard, new SortedList<int, int>());
                newCert.GradesAndSkills.Add(CertificateGrade.Improved, new SortedList<int, int>());
                newCert.GradesAndSkills.Add(CertificateGrade.Advanced, new SortedList<int, int>());
                newCert.GradesAndSkills.Add(CertificateGrade.Elite, new SortedList<int, int>());

                newCert.Id = cert.CertID;
                newCert.Description = cert.Description;
                newCert.GroupId = cert.GroupID;
                newCert.Name = cert.Name;
                newCert.RecommendedTypes = cert.RecommendedFor.ToList();
                foreach (CertReqSkill reqSkill_loopVariable in cert.RequiredSkills)
                {
                    CertReqSkill reqSkill = reqSkill_loopVariable;
                    newCert.GradesAndSkills[CertificateGrade.Basic].Add(reqSkill.SkillID, reqSkill.SkillLevels["basic"]);
                    newCert.GradesAndSkills[CertificateGrade.Standard].Add(reqSkill.SkillID, reqSkill.SkillLevels["standard"]);
                    newCert.GradesAndSkills[CertificateGrade.Improved].Add(reqSkill.SkillID, reqSkill.SkillLevels["improved"]);
                    newCert.GradesAndSkills[CertificateGrade.Advanced].Add(reqSkill.SkillID, reqSkill.SkillLevels["advanced"]);
                    newCert.GradesAndSkills[CertificateGrade.Elite].Add(reqSkill.SkillID, reqSkill.SkillLevels["elite"]);
                }
                StaticData.Certificates.Add(newCert.Id, newCert);
            }

            // Trim certs of any skills that have a 0 rank value
            foreach (Certificate cert_loopVariable in StaticData.Certificates.Values)
            {
                Certificate cert = cert_loopVariable;
                foreach (KeyValuePair<CertificateGrade, SortedList<int, int>> gradeAndSkills_loopVariable in cert.GradesAndSkills)
                {
                    KeyValuePair<CertificateGrade, SortedList<int, int>> gradeAndSkills = gradeAndSkills_loopVariable;
                    dynamic skills = gradeAndSkills.Value.ToList();
                    foreach (dynamic skill_loopVariable in skills)
                    {
                        dynamic skill = skill_loopVariable;
                        if (skill.Value == 0)
                        {
                            gradeAndSkills.Value.Remove(skill.Key);
                        }
                    }
                }
            }
        }

        private void LoadConstellations()
        {
            StaticData.Constellations.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM mapConstellations;"))
            {
                foreach (DataRow row in evehqData.Tables[0].Rows)
                {
                    StaticData.Constellations.Add(Convert.ToInt32(row["constellationID"]), row["constellationName"].ToString());
                }
            }
        }

        private void LoadCustomAttributes()
        {
            string[] attributeLines = ResourceHandler.GetResource("Attributes").Split(ControlChars.CrLf.ToCharArray());
            string[] att = null;
            Attribute attData = default(Attribute);
            foreach (string line in attributeLines)
            {
                if (!string.IsNullOrEmpty(line.Trim()) & line.StartsWith("#", StringComparison.Ordinal) == false)
                {
                    att = line.Split(",".ToCharArray());
                    attData = new Attribute();
                    attData.ID = Convert.ToInt32(att[0]);
                    attData.Name = att[1];
                    attData.DisplayName = att[2];
                    attData.UnitName = att[4];
                    attData.AttributeGroup = att[5];
                    Attributes.AttributeList.Add(attData.ID, attData);
                }
            }
        }

        private void LoadEffectTypes()
        {
            StaticData.EffectTypes.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM dgmEffects;"))
            {
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    var et = new EffectType();
                    et.EffectId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["effectID"]);
                    et.EffectName = Convert.ToString(evehqData.Tables[0].Rows[item]["effectName"]).Trim();
                    StaticData.EffectTypes.Add(et.EffectId, et);
                }
            }
        }

        private void LoadEveSkillData()
        {
            // TODO: This is essentially the copy that will be in the Core, so delete it when the Core is updated
            Core.HQ.SkillListName.Clear();
            Core.HQ.SkillListID.Clear();
            Core.HQ.SkillGroups.Clear();

            var skillAttFilter = new List<int>();

            // Get details of skill groups from the database
            IEnumerable<int> groupIDs = StaticData.GetGroupsInCategory(16);
            foreach (int groupID in groupIDs)
            {
                if (groupID != 267)
                {
                    var newSkillGroup = new Core.SkillGroup();
                    newSkillGroup.ID = groupID;
                    newSkillGroup.Name = StaticData.TypeGroups[groupID];
                    Core.HQ.SkillGroups.Add(newSkillGroup.Name, newSkillGroup);

                    // Get the items in this skill group
                    IEnumerable<EveType> items = StaticData.GetItemsInGroup(Convert.ToInt32(groupID));
                    foreach (EveType item in items)
                    {
                        var newSkill = new Core.EveSkill();
                        newSkill.ID = item.Id;
                        newSkill.Description = item.Description;
                        newSkill.GroupID = item.Group;
                        newSkill.Name = item.Name;
                        newSkill.BasePrice = item.BasePrice;
                        // Check for salvage drone op skill in db!
                        if (newSkill.ID == 3440)
                        {
                            newSkill.Published = true;
                        }
                        else
                        {
                            newSkill.Published = item.Published;
                        }
                        Core.HQ.SkillListID.Add(newSkill.ID, newSkill);
                        skillAttFilter.Add(Convert.ToInt32(newSkill.ID));
                    }
                }
            }
            //HQ.WriteLogEvent(" *** Parsed skill groups")

            // Filter attributes to skills for quicker parsing in the loop
            List<TypeAttrib> skillAtts = (from ta in StaticData.TypeAttributes where skillAttFilter.Contains(ta.TypeId) select ta).ToList();

            const int MaxPreReqs = 10;
            foreach (Core.EveSkill newSkill in Core.HQ.SkillListID.Values)
            {
                var preReqSkills = new int[MaxPreReqs + 1];
                var preReqSkillLevels = new int[MaxPreReqs + 1];

                // Fetch the attributes for the item
                int skillID = Convert.ToInt32(newSkill.ID);

                foreach (TypeAttrib att in from ta in skillAtts where ta.TypeId == skillID select ta)
                {
                    switch (att.AttributeId)
                    {
                        case 180:
                            switch ((int)(att.Value))
                            {
                                case 164:
                                    newSkill.Pa = "Charisma";
                                    break;
                                case 165:
                                    newSkill.Pa = "Intelligence";
                                    break;
                                case 166:
                                    newSkill.Pa = "Memory";
                                    break;
                                case 167:
                                    newSkill.Pa = "Perception";
                                    break;
                                case 168:
                                    newSkill.Pa = "Willpower";
                                    break;
                            }
                            break;
                        case 181:
                            switch ((int)(att.Value))
                            {
                                case 164:
                                    newSkill.Sa = "Charisma";
                                    break;
                                case 165:
                                    newSkill.Sa = "Intelligence";
                                    break;
                                case 166:
                                    newSkill.Sa = "Memory";
                                    break;
                                case 167:
                                    newSkill.Sa = "Perception";
                                    break;
                                case 168:
                                    newSkill.Sa = "Willpower";
                                    break;
                            }
                            break;
                        case 275:
                            newSkill.Rank = (int)(att.Value);
                            break;
                        case 182:
                            preReqSkills[1] = (int)(att.Value);
                            break;
                        case 183:
                            preReqSkills[2] = (int)(att.Value);
                            break;
                        case 184:
                            preReqSkills[3] = (int)(att.Value);
                            break;
                        case 1285:
                            preReqSkills[4] = (int)(att.Value);
                            break;
                        case 1289:
                            preReqSkills[5] = (int)(att.Value);
                            break;
                        case 1290:
                            preReqSkills[6] = (int)(att.Value);
                            break;
                        case 277:
                            preReqSkillLevels[1] = (int)(att.Value);
                            break;
                        case 278:
                            preReqSkillLevels[2] = (int)(att.Value);
                            break;
                        case 279:
                            preReqSkillLevels[3] = (int)(att.Value);
                            break;
                        case 1286:
                            preReqSkillLevels[4] = (int)(att.Value);
                            break;
                        case 1287:
                            preReqSkillLevels[5] = (int)(att.Value);
                            break;
                        case 1288:
                            preReqSkillLevels[6] = (int)(att.Value);
                            break;
                    }
                }

                // Add the pre-reqs into the list
                for (int prereq = 1; prereq <= MaxPreReqs; prereq++)
                {
                    if (preReqSkills[prereq] != 0)
                    {
                        newSkill.PreReqSkills.Add(preReqSkills[prereq], preReqSkillLevels[prereq]);
                    }
                }
                // Calculate the levels
                for (int a = 0; a <= 5; a++)
                {
                    newSkill.LevelUp[a] = Convert.ToInt32(Math.Ceiling(Core.SkillFunctions.CalculateSPLevel(newSkill.Rank, a)));
                }
                // Add the currentskill to the name list
                Core.HQ.SkillListName.Add(newSkill.Name, newSkill);
            }
        }

        private void LoadGroupCats()
        {
            StaticData.GroupCats.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM invGroups ORDER BY groupName;"))
            {
                int iKey = 0;
                int iValue = 0;
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    iKey = Convert.ToInt32(evehqData.Tables[0].Rows[item]["groupID"].ToString().Trim());
                    iValue = Convert.ToInt32(evehqData.Tables[0].Rows[item]["categoryID"].ToString().Trim());
                    StaticData.GroupCats.Add(iKey, iValue);
                }
            }
        }

        private void LoadItemCategories()
        {
            StaticData.TypeCats.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM invCategories ORDER BY categoryName;"))
            {
                int iKey = 0;
                string iValue = null;
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    iValue = evehqData.Tables[0].Rows[item]["categoryName"].ToString().Trim();
                    iKey = Convert.ToInt32(evehqData.Tables[0].Rows[item]["categoryID"].ToString().Trim());
                    StaticData.TypeCats.Add(iKey, iValue);
                }
            }
        }

        private void LoadItemData()
        {
            StaticData.Types.Clear();
            DataSet evehqData = default(DataSet);
            string strSQL = "SELECT invTypes.*, invGroups.categoryID FROM invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID;";
            evehqData = GetStaticData(strSQL);
            EveType newItem = default(EveType);
            foreach (DataRow itemRow in evehqData.Tables[0].Rows)
            {
                if (Information.IsDBNull(itemRow["typeName"]) == false)
                {
                    newItem = new EveType();
                    newItem.Id = Convert.ToInt32(itemRow["typeID"]);
                    newItem.Name = Convert.ToString(itemRow["typeName"]);
                    if (Information.IsDBNull(itemRow["description"]) == false)
                    {
                        newItem.Description = Convert.ToString(itemRow["description"]);
                    }
                    else
                    {
                        newItem.Description = "";
                    }
                    newItem.Group = Convert.ToInt32(itemRow["groupID"]);
                    newItem.Published = Convert.ToBoolean(itemRow["published"]);
                    newItem.Category = Convert.ToInt32(itemRow["categoryID"]);
                    if (Information.IsDBNull(itemRow["marketGroupID"]) == false)
                    {
                        newItem.MarketGroupId = Convert.ToInt32(itemRow["marketGroupID"]);
                    }
                    else
                    {
                        newItem.MarketGroupId = 0;
                    }
                    newItem.Mass = Convert.ToDouble(itemRow["mass"]);
                    newItem.Capacity = Convert.ToDouble(itemRow["capacity"]);
                    newItem.Volume = Convert.ToDouble(itemRow["volume"]);
                    newItem.PortionSize = Convert.ToInt32(itemRow["portionSize"]);
                    newItem.BasePrice = Convert.ToDouble(itemRow["basePrice"]);
                    StaticData.Types.Add(newItem.Id, newItem);
                }
            }
            // Get the MetaLevel data
            strSQL = "SELECT * FROM dgmTypeAttributes WHERE attributeID=633;";
            evehqData = GetStaticData(strSQL);
            if (evehqData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow itemRow in evehqData.Tables[0].Rows)
                {
                    if (StaticData.Types.ContainsKey(Convert.ToInt32(itemRow["typeID"])))
                    {
                        newItem = StaticData.Types[Convert.ToInt32(itemRow["typeID"])];
                        if (Information.IsDBNull(itemRow["valueInt"]) == false)
                        {
                            newItem.MetaLevel = Convert.ToInt32(itemRow["valueInt"]);
                        }
                        else
                        {
                            newItem.MetaLevel = Convert.ToInt32(itemRow["valueFloat"]);
                        }
                    }
                }
            }
            evehqData.Dispose();
        }

        private void LoadItemFlags()
        {
            StaticData.ItemMarkers.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM invFlags"))
            {
                foreach (DataRow flagRow in evehqData.Tables[0].Rows)
                {
                    StaticData.ItemMarkers.Add(Convert.ToInt32(flagRow["flagID"]), Convert.ToString(flagRow["flagText"]));
                }
            }
        }

        private void LoadItemGroups()
        {
            StaticData.TypeGroups.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM invGroups ORDER BY groupName;"))
            {
                int iKey = 0;
                string iValue = null;
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    iValue = evehqData.Tables[0].Rows[item]["groupName"].ToString().Trim();
                    iKey = Convert.ToInt32(evehqData.Tables[0].Rows[item]["groupID"].ToString().Trim());
                    StaticData.TypeGroups.Add(iKey, iValue);
                }
            }
        }

        private void LoadItemList()
        {
            StaticData.TypeNames.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM invTypes ORDER BY typeName;"))
            {
                string iKey = null;
                string iValue = null;
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    iKey = evehqData.Tables[0].Rows[item]["typeName"].ToString().Trim();
                    iValue = evehqData.Tables[0].Rows[item]["typeID"].ToString().Trim();
                    if (StaticData.TypeNames.ContainsKey(iKey) == false)
                    {
                        StaticData.TypeNames.Add(iKey, Convert.ToInt32(iValue));
                    }
                }
            }
        }

        private void LoadItemMarketGroups()
        {
            StaticData.ItemMarketGroups.Clear();
            using (DataSet evehqData = GetStaticData("SELECT typeID, marketGroupID FROM invTypes WHERE marketGroupID IS NOT NULL;"))
            {
                if (evehqData != null)
                {
                    if (evehqData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow itemRow in evehqData.Tables[0].Rows)
                        {
                            StaticData.ItemMarketGroups.Add(itemRow["typeID"].ToString(), itemRow["marketGroupID"].ToString());
                        }
                    }
                }
            }
        }

        private void LoadMarketGroupData()
        {
            try
            {
                string strSQL = "";
                strSQL += "SELECT * FROM invMarketGroups ORDER BY parentGroupID;";
                marketGroupData = GetStaticData(strSQL);
                if (marketGroupData != null)
                {
                    if (marketGroupData.Tables[0].Rows.Count != 0)
                    {
                        Market.MarketGroupList.Clear();
                        foreach (DataRow row in marketGroupData.Tables[0].Rows)
                        {
                            Market.MarketGroupList.Add(row["marketGroupID"].ToString(), row["marketGroupName"].ToString());
                        }
                    }
                    MessageBox.Show("Market Group Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Market Group Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Market Group Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMarketGroups()
        {
            StaticData.MarketGroups.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM invMarketGroups;"))
            {
                if (evehqData != null)
                {
                    if (evehqData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow itemRow in evehqData.Tables[0].Rows)
                        {
                            var mg = new MarketGroup();
                            mg.Id = Convert.ToInt32(itemRow["marketGroupID"]);
                            mg.Name = Convert.ToString(itemRow["marketGroupName"]);
                            if (Information.IsDBNull(itemRow["parentGroupID"]) == false)
                            {
                                mg.ParentGroupId = Convert.ToInt32(itemRow["parentGroupID"]);
                            }
                            else
                            {
                                mg.ParentGroupId = 0;
                            }
                            StaticData.MarketGroups.Add(mg.Id, mg);
                        }
                    }
                }
            }
        }

        private void LoadMasteries()
        {
            StaticData.Masteries.Clear();

            foreach (YAMLType type_loopVariable in yamlTypes.Values.Where(i => i.Masteries != null))
            {
                YAMLType type = type_loopVariable;
                var mastery = new Mastery();
                mastery.TypeId = type.TypeID;
                mastery.RequiredCertificates = new SortedList<int, List<int>>();
                foreach (KeyValuePair<int, List<int>> rank_loopVariable in type.Masteries)
                {
                    KeyValuePair<int, List<int>> rank = rank_loopVariable;
                    mastery.RequiredCertificates.Add(rank.Key, new List<int>());
                    foreach (int cert_loopVariable in rank.Value)
                    {
                        int cert = cert_loopVariable;
                        mastery.RequiredCertificates[rank.Key].Add(cert);
                    }
                }
                StaticData.Masteries.Add(mastery.TypeId, mastery);
            }
        }

        private void LoadMetaGroups()
        {
            StaticData.MetaGroups.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM invMetaGroups;"))
            {
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    StaticData.MetaGroups.Add(Convert.ToInt32(evehqData.Tables[0].Rows[item]["metaGroupID"]), Convert.ToString(evehqData.Tables[0].Rows[item]["metaGroupName"]));
                }
            }
        }

        private void LoadMetaTypes()
        {
            StaticData.MetaTypes.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM invMetaTypes;"))
            {
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    var mt = new MetaType();
                    mt.Id = Convert.ToInt32(evehqData.Tables[0].Rows[item]["typeID"]);
                    mt.ParentId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["parentTypeID"]);
                    mt.MetaGroupId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["metaGroupID"]);
                    StaticData.MetaTypes.Add(mt.Id, mt);
                }
            }
        }

        private void LoadModuleAttributeData()
        {
            try
            {
                string strSQL = "";
                strSQL +=
                    "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName";
                strSQL +=
                    " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits RIGHT OUTER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID";
                strSQL += " WHERE (((invCategories.categoryID In (7,8,18,20,22,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010";
                strSQL += " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;";

                moduleAttributeData = GetStaticData(strSQL);
                if (moduleAttributeData != null)
                {
                    if (moduleAttributeData.Tables[0].Rows.Count != 0)
                    {
                    }
                    MessageBox.Show("Module Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Module Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Module Attribute Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadModuleData()
        {
            try
            {
                string strSQL = "";
                strSQL +=
                    "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.raceID, invTypes.marketGroupID";
                strSQL += " FROM invCategories INNER JOIN (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) ON invCategories.categoryID = invGroups.categoryID";
                strSQL += " WHERE (((invCategories.categoryID In (7,8,18,20,22,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010";
                strSQL += " ORDER BY invTypes.typeName;";
                moduleData = GetStaticData(strSQL);
                if (moduleData != null)
                {
                    if (moduleData.Tables[0].Rows.Count != 0)
                    {
                    }
                    MessageBox.Show("Module Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Module Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Module Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadModuleEffectData()
        {
            try
            {
                string strSQL = "";
                strSQL +=
                    "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeEffects.effectID";
                strSQL +=
                    " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeEffects ON invTypes.typeID=dgmTypeEffects.typeID";
                strSQL += " WHERE (((invCategories.categoryID In (7,8,18,20,22,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010";
                strSQL += " ORDER BY typeName, effectID;";
                moduleEffectData = GetStaticData(strSQL);
                if (moduleEffectData != null)
                {
                    if (moduleEffectData.Tables[0].Rows.Count != 0)
                    {
                    }
                    MessageBox.Show("Module Effect Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Module Effect Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Module Effect Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadModuleMetaTypes()
        {
            try
            {
                string strSQL = "";
                strSQL += "SELECT invTypes.typeID AS invTypes_typeID, invMetaTypes.parentTypeID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID";
                strSQL +=
                    " FROM (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (invMetaGroups INNER JOIN invMetaTypes ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID) ON invTypes.typeID = invMetaTypes.typeID";
                strSQL += " WHERE (((invGroups.categoryID) In (7,8,18,20,22,32)) AND (invTypes.published=1))";
                DataSet metaTypeData = GetStaticData(strSQL);
                if (metaTypeData != null)
                {
                    if (metaTypeData.Tables[0].Rows.Count != 0)
                    {
                        ModuleLists.ModuleMetaTypes.Clear();
                        ModuleLists.ModuleMetaGroups.Clear();
                        foreach (DataRow row in metaTypeData.Tables[0].Rows)
                        {
                            if (ModuleLists.ModuleMetaTypes.ContainsKey(Convert.ToInt32(row["invTypes_typeID"])) == false)
                            {
                                ModuleLists.ModuleMetaTypes.Add(Convert.ToInt32(row["invTypes_typeID"]), Convert.ToInt32(row["parentTypeID"]));
                                ModuleLists.ModuleMetaGroups.Add(Convert.ToInt32(row["invTypes_typeID"]), Convert.ToInt32(row["invMetaGroups_metaGroupID"]));
                            }
                            if (ModuleLists.ModuleMetaTypes.ContainsKey(Convert.ToInt32(row["parentTypeID"])) == false)
                            {
                                ModuleLists.ModuleMetaTypes.Add(Convert.ToInt32(row["parentTypeID"]), Convert.ToInt32(row["parentTypeID"]));
                                ModuleLists.ModuleMetaGroups.Add(Convert.ToInt32(row["parentTypeID"]), 0);
                            }
                        }
                    }
                    MessageBox.Show("Module Metatype Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Module Metatype Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Module Metatype Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNPCCorps()
        {
            StaticData.NpcCorps.Clear();
            using (DataSet evehqData = GetStaticData("SELECT itemID, itemName FROM invUniqueNames WHERE groupID=2;"))
            {
                foreach (DataRow corpRow in evehqData.Tables[0].Rows)
                {
                    StaticData.NpcCorps.Add(Convert.ToInt32(corpRow["itemID"]), Convert.ToString(corpRow["itemname"]));
                }
            }
        }

        private void LoadRegions()
        {
            StaticData.Regions.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM mapRegions;"))
            {
                foreach (DataRow row in evehqData.Tables[0].Rows)
                {
                    StaticData.Regions.Add(Convert.ToInt32(row["regionID"]), row["regionName"].ToString());
                }
            }
        }

        private void LoadShipAttributeData()
        {
            try
            {
                // Get details of ship data from database
                string strSQL = "";
                string pSkillName = null;
                string sSkillName = null;
                string tSkillName = null;
                strSQL +=
                    "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.raceID, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat";
                strSQL +=
                    " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID";
                strSQL += " WHERE ((invCategories.categoryID=6 AND invTypes.published=1) OR invTypes.typeID IN (601,596,588,606)) ORDER BY typeName, attributeID;";
                DataSet shipData = GetStaticData(strSQL);
                if (shipData != null)
                {
                    if (shipData.Tables[0].Rows.Count != 0)
                    {
                        ShipLists.ShipList.Clear();
                        string lastShipName = "";
                        var newShip = new Ship();
                        pSkillName = "";
                        sSkillName = "";
                        tSkillName = "";
                        double attValue = 0;
                        foreach (DataRow shipRow in shipData.Tables[0].Rows)
                        {
                            // If the shipName has changed, we need to start a new ship type
                            if (lastShipName != shipRow["typeName"].ToString())
                            {
                                // Add the current ship to the list then reset the ship data
                                if (!string.IsNullOrEmpty(lastShipName))
                                {
                                    newShip.AddCustomShipAttributes();
                                    // Map the attributes
                                    Ship.MapShipAttributes(newShip);
                                    ShipLists.ShipList.Add(newShip.Name, newShip);
                                    newShip = new Ship();
                                    pSkillName = "";
                                    sSkillName = "";
                                    tSkillName = "";
                                }
                                // Create new ship type & non "attribute" data
                                newShip.Name = shipRow["typeName"].ToString();
                                newShip.ID = Convert.ToInt32(shipRow["typeID"]);
                                newShip.Description = shipRow["description"].ToString();
                                newShip.DatabaseGroup = Convert.ToInt32(shipRow["groupID"]);
                                newShip.DatabaseCategory = Convert.ToInt32(shipRow["categoryID"]);
                                if (Information.IsDBNull(shipRow["marketGroupID"]))
                                {
                                    newShip.MarketGroup = 0;
                                }
                                else
                                {
                                    newShip.MarketGroup = Convert.ToInt32(shipRow["marketGroupID"]);
                                }
                                newShip.BasePrice = Convert.ToDouble(shipRow["basePrice"]);
                                newShip.MarketPrice = 0;
                                newShip.Mass = Convert.ToDouble(shipRow["mass"]);
                                newShip.Volume = Convert.ToDouble(shipRow["volume"]);
                                newShip.CargoBay = Convert.ToDouble(shipRow["capacity"]);
                                if (Information.IsDBNull(shipRow["raceID"]) == false)
                                {
                                    newShip.RaceID = Convert.ToInt32(shipRow["raceID"]);
                                }
                                else
                                {
                                    newShip.RaceID = 0;
                                }
                            }

                            // Now get, modify (if applicable) and add the "attribute"

                            if (Information.IsDBNull(shipRow["valueInt"]))
                            {
                                attValue = Convert.ToDouble(shipRow["valueFloat"]);
                            }
                            else
                            {
                                attValue = Convert.ToDouble(shipRow["valueInt"]);
                            }

                            // Do attribute (unit) modifiers
                            switch (Convert.ToInt32(shipRow["attributeID"]))
                            {
                                case 55:
                                case 1034:
                                case 479:
                                    attValue = attValue / 1000;
                                    break;
                                case 113:
                                case 111:
                                case 109:
                                case 110:
                                case 267:
                                case 268:
                                case 269:
                                case 270:
                                case 271:
                                case 272:
                                case 273:
                                case 274:
                                    attValue = (1 - attValue) * 100;
                                    break;
                                case 1154:
                                    // Reset this field to be used as Calibration_Used
                                    attValue = 0;
                                    break;
                            }

                            // Add the attribute to the ship.attributes list
                            newShip.Attributes.Add(Convert.ToInt32(shipRow["attributeID"]), attValue);

                            ItemSkills nSkill;
                            // Map only the skill attributes
                            switch (Convert.ToInt32(shipRow["attributeID"]))
                            {
                                case 182:
                                    EveType pSkill = StaticData.Types[Convert.ToInt32(attValue)];
                                    nSkill = new ItemSkills();
                                    nSkill.ID = pSkill.Id;
                                    nSkill.Name = pSkill.Name;
                                    pSkillName = pSkill.Name;
                                    newShip.RequiredSkills.Add(nSkill.Name, nSkill);
                                    break;
                                case 183:
                                    EveType sSkill = StaticData.Types[Convert.ToInt32(attValue)];
                                    nSkill = new ItemSkills();
                                    nSkill.ID = sSkill.Id;
                                    nSkill.Name = sSkill.Name;
                                    sSkillName = sSkill.Name;
                                    newShip.RequiredSkills.Add(nSkill.Name, nSkill);
                                    break;
                                case 184:
                                    EveType tSkill = StaticData.Types[Convert.ToInt32(attValue)];
                                    nSkill = new ItemSkills();
                                    nSkill.ID = tSkill.Id;
                                    nSkill.Name = tSkill.Name;
                                    tSkillName = tSkill.Name;
                                    newShip.RequiredSkills.Add(nSkill.Name, nSkill);
                                    break;
                                case 277:
                                    if (newShip.RequiredSkills.ContainsKey(pSkillName))
                                    {
                                        ItemSkills cSkill = newShip.RequiredSkills[pSkillName];
                                        cSkill.Level = Convert.ToInt32(attValue);
                                    }
                                    break;
                                case 278:
                                    if (newShip.RequiredSkills.ContainsKey(sSkillName))
                                    {
                                        ItemSkills cSkill = newShip.RequiredSkills[sSkillName];
                                        cSkill.Level = Convert.ToInt32(attValue);
                                    }
                                    break;
                                case 279:
                                    if (newShip.RequiredSkills.ContainsKey(tSkillName))
                                    {
                                        ItemSkills cSkill = newShip.RequiredSkills[tSkillName];
                                        cSkill.Level = Convert.ToInt32(attValue);
                                    }
                                    break;
                            }
                            lastShipName = shipRow["typeName"].ToString();
                        }
                        // Add the custom attributes to the list
                        newShip.AddCustomShipAttributes();
                        // Map the remaining attributes for the last ship type
                        Ship.MapShipAttributes(newShip);
                        // Perform the last addition for the last ship type
                        ShipLists.ShipList.Add(newShip.Name, newShip);
                    }
                    MessageBox.Show("Ship Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Ship Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Ship Attribute Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadShipGroupData()
        {
            try
            {
                string strSQL = "";
                strSQL += "SELECT * FROM invGroups WHERE invGroups.categoryID=6 ORDER BY groupName;";
                shipGroupData = GetStaticData(strSQL);
                if (shipGroupData != null)
                {
                    if (shipGroupData.Tables[0].Rows.Count != 0)
                    {
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Ship Group Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadShipNameData()
        {
            try
            {
                string strSQL = "";
                strSQL += "SELECT invCategories.categoryID, invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.published, invTypes.raceID, invTypes.marketGroupID";
                strSQL += " FROM (invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID";
                strSQL += " WHERE (invCategories.categoryID=6 AND invTypes.published=1) ORDER BY typeName;";
                shipNameData = GetStaticData(strSQL);
                if (shipNameData != null)
                {
                    if (shipNameData.Tables[0].Rows.Count != 0)
                    {
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Ship Name Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSkillData()
        {
            //Call Core.SkillFunctions.LoadEveSkillData()
            LoadEveSkillData();
            try
            {
                DataSet skillData = default(DataSet);
                string strSQL = "";
                strSQL +=
                    "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName,  invTypes.basePrice, invTypes.published, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat";
                strSQL +=
                    " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID";
                strSQL += " WHERE invCategories.categoryID=16 ORDER BY typeName;";
                skillData = GetStaticData(strSQL);
                if (skillData != null)
                {
                    if (skillData.Tables[0].Rows.Count != 0)
                    {
                        SkillLists.SkillList.Clear();
                        foreach (DataRow skillRow in skillData.Tables[0].Rows)
                        {
                            // Check if the typeID already exists
                            Skill newSkill = default(Skill);
                            if (SkillLists.SkillList.ContainsKey(Convert.ToInt32(skillRow["typeID"])) == false)
                            {
                                newSkill = new Skill();
                                newSkill.Attributes = new SortedList<int, double>();
                                newSkill.ID = Convert.ToInt32(skillRow["typeID"]);
                                newSkill.GroupID = skillRow["groupID"].ToString().Trim();
                                newSkill.Name = skillRow["typeName"].ToString().Trim();
                                SkillLists.SkillList.Add(newSkill.ID, newSkill);
                            }
                            else
                            {
                                newSkill = SkillLists.SkillList[Convert.ToInt32(skillRow["typeID"])];
                            }
                            if (Information.IsDBNull(skillRow["valueInt"]) == false)
                            {
                                newSkill.Attributes.Add(Convert.ToInt32(skillRow["attributeID"]), Convert.ToDouble(skillRow["valueInt"]));
                            }
                            else
                            {
                                newSkill.Attributes.Add(Convert.ToInt32(skillRow["attributeID"]), Convert.ToDouble(skillRow["valueFloat"]));
                            }
                        }
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Skill Data: " + e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSolarSystems()
        {
            StaticData.SolarSystems.Clear();
            string strSQL =
                "SELECT mapSolarSystems.regionID AS mapSolarSystems_regionID, mapSolarSystems.constellationID AS mapSolarSystems_constellationID, mapSolarSystems.solarSystemID, mapSolarSystems.solarSystemName, mapSolarSystems.x, mapSolarSystems.y, mapSolarSystems.z, mapSolarSystems.xMin, mapSolarSystems.xMax, mapSolarSystems.yMin, mapSolarSystems.yMax, mapSolarSystems.zMin, mapSolarSystems.zMax, mapSolarSystems.luminosity, mapSolarSystems.border, mapSolarSystems.fringe, mapSolarSystems.corridor, mapSolarSystems.hub, mapSolarSystems.international, mapSolarSystems.regional, mapSolarSystems.constellation, mapSolarSystems.security, mapSolarSystems.factionID, mapSolarSystems.radius, mapSolarSystems.sunTypeID, mapSolarSystems.securityClass, mapRegions.regionID AS mapRegions_regionID, mapRegions.regionName, mapConstellations.constellationID AS mapConstellations_constellationID, mapConstellations.constellationName";
            strSQL +=
                " FROM (mapRegions INNER JOIN mapConstellations ON mapRegions.regionID = mapConstellations.regionID) INNER JOIN mapSolarSystems ON mapConstellations.constellationID = mapSolarSystems.constellationID;";
            using (DataSet evehqData = GetStaticData(strSQL))
            {
                SolarSystem cSystem = default(SolarSystem);
                foreach (DataRow solarRow in evehqData.Tables[0].Rows)
                {
                    cSystem = new SolarSystem();
                    cSystem.Id = Convert.ToInt32(solarRow["solarSystemID"]);
                    cSystem.Name = Convert.ToString(solarRow["solarSystemName"]);
                    cSystem.Security = Convert.ToDouble(solarRow["security"]);
                    cSystem.RegionId = Convert.ToInt32(solarRow["mapSolarSystems_regionID"]);
                    cSystem.ConstellationId = Convert.ToInt32(solarRow["mapSolarSystems_constellationID"]);
                    cSystem.X = Convert.ToDouble(solarRow["x"]);
                    cSystem.Y = Convert.ToDouble(solarRow["y"]);
                    cSystem.Z = Convert.ToDouble(solarRow["z"]);
                    StaticData.SolarSystems.Add(cSystem.Id, cSystem);
                }
            }

            // Load the solar system jump data
            using (var connection = new SqlConnection(string.Format(StaticDBConnection, EveHQDatabaseName)))
            {
                var command = new SqlCommand("SELECT * FROM mapSolarSystemJumps;", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (StaticData.SolarSystems.ContainsKey(Convert.ToInt32(reader["fromSolarSystemID"])))
                        {
                            StaticData.SolarSystems[Convert.ToInt32(reader["fromSolarSystemID"])].Gates.Add(Convert.ToInt32(reader["toSolarSystemID"]));
                        }
                    }
                }

                reader.Close();
            }

            // Load the celestial data
            using (var connection = new SqlConnection(string.Format(StaticDBConnection, EveHQDatabaseName)))
            {
                int id = 0;
                var command = new SqlCommand("SELECT * FROM mapDenormalize;", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Information.IsDBNull(reader["solarSystemID"]) == false)
                        {
                            id = Convert.ToInt32(reader["solarSystemID"]);
                            if (StaticData.SolarSystems.ContainsKey(id))
                            {
                                switch (Convert.ToInt32(reader["groupID"]))
                                {
                                    case 7:
                                        // Planet
                                        //MapData.eveSystems(id).Planets.Add(reader["itemName").ToString)
                                        StaticData.SolarSystems[id].PlanetCount += 1;
                                        break;
                                    case 8:
                                        // Moon
                                        //MapData.eveSystems(id).Moons.Add(reader["itemName").ToString)
                                        StaticData.SolarSystems[id].MoonCount += 1;
                                        break;
                                    case 9:
                                        // Belts
                                        switch (Convert.ToInt32(reader["typeID"]))
                                        {
                                            case 15:
                                                // Ore Belt
                                                //MapData.eveSystems(id).OreBelts.Add(reader["itemName").ToString)
                                                StaticData.SolarSystems[id].OreBeltCount += 1;
                                                break;
                                            case 17774:
                                                // Ice Belt
                                                //MapData.eveSystems(id).IceBelts.Add(reader["itemName").ToString)
                                                StaticData.SolarSystems[id].IceBeltCount += 1;
                                                break;
                                        }
                                        break;
                                    case 15:
                                        // Stations
                                        //MapData.eveSystems(id).Stations.Add(reader["itemName").ToString)
                                        StaticData.SolarSystems[id].StationCount += 1;
                                        break;
                                }
                            }
                        }
                    }
                }

                reader.Close();
            }
        }

        private void LoadStations()
        {
            // Load the Operation data
            var operationServices = new Dictionary<int, int>();
            using (var connection = new SqlConnection(string.Format(StaticDBConnection, EveHQDatabaseName)))
            {
                var command = new SqlCommand("SELECT * FROM staOperationServices;", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (operationServices.ContainsKey(Convert.ToInt32(reader["operationID"])) == false)
                        {
                            operationServices.Add(Convert.ToInt32(reader["operationID"]), 0);
                        }
                        operationServices[Convert.ToInt32(reader["operationID"])] += Convert.ToInt32(reader["serviceID"]);
                    }
                }

                reader.Close();
            }

            // Load the Station data
            using (var connection = new SqlConnection(string.Format(StaticDBConnection, EveHQDatabaseName)))
            {
                var command = new SqlCommand("SELECT * FROM staStations;", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Station s = default(Station);
                    while (reader.Read())
                    {
                        s = new Station();
                        s.StationId = Convert.ToInt32(reader["stationID"]);
                        s.StationName = reader["stationName"].ToString();
                        s.CorpId = Convert.ToInt32(reader["corporationID"]);
                        s.SystemId = Convert.ToInt32(reader["solarSystemID"]);
                        s.RefiningEfficiency = Convert.ToDouble(reader["reprocessingEfficiency"]);
                        s.StationTake = Convert.ToDouble(reader["reprocessingStationsTake"]);
                        s.Services = operationServices[Convert.ToInt32(reader["operationID"])];
                        StaticData.Stations.Add(s.StationId, s);
                    }
                }

                reader.Close();
            }
        }

        private void LoadTraits()
        {
            StaticData.Traits.Clear();

            foreach (YAMLType type_loopVariable in yamlTypes.Values.Where(i => i.Traits != null))
            {
                YAMLType type = type_loopVariable;
                StaticData.Traits.Add(type.TypeID, type.Traits);
            }
        }

        private void LoadTypeAttributes()
        {
            StaticData.TypeAttributes.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM dgmTypeAttributes;"))
            {
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    var ta = new TypeAttrib();
                    ta.TypeId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["typeID"]);
                    ta.AttributeId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["attributeID"]);
                    if (Information.IsDBNull(evehqData.Tables[0].Rows[item]["valueInt"]) == false)
                    {
                        ta.Value = Convert.ToDouble(evehqData.Tables[0].Rows[item]["valueInt"]);
                    }
                    else
                    {
                        ta.Value = Convert.ToDouble(evehqData.Tables[0].Rows[item]["valueFloat"]);
                    }
                    StaticData.TypeAttributes.Add(ta);
                }
            }
        }

        private void LoadTypeEffects()
        {
            StaticData.TypeEffects.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM dgmTypeEffects;"))
            {
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    var te = new TypeEffect();
                    te.TypeId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["typeID"]);
                    te.EffectId = Convert.ToInt32(evehqData.Tables[0].Rows[item]["effectID"]);
                    StaticData.TypeEffects.Add(te);
                }
            }
        }

        private void LoadTypeMaterials()
        {
            StaticData.TypeMaterials.Clear();
            using (DataSet evehqData = GetStaticData("SELECT * FROM invTypeMaterials;"))
            {
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    int typeID = Convert.ToInt32(evehqData.Tables[0].Rows[item]["typeID"]);
                    var tm = new TypeMaterial();
                    if (StaticData.TypeMaterials.ContainsKey(typeID))
                    {
                        tm = StaticData.TypeMaterials[typeID];
                    }
                    else
                    {
                        StaticData.TypeMaterials.Add(typeID, tm);
                    }
                    tm.Materials.Add(Convert.ToInt32(evehqData.Tables[0].Rows[item]["materialTypeID"]), Convert.ToInt32(evehqData.Tables[0].Rows[item]["quantity"]));
                }
            }
        }

        private void LoadUnits()
        {
            StaticData.AttributeUnits.Clear();
            StaticData.AttributeUnits.Add(0, "");
            using (DataSet evehqData = GetStaticData("SELECT * FROM eveUnits;"))
            {
                for (int item = 0; item <= evehqData.Tables[0].Rows.Count - 1; item++)
                {
                    if (Information.IsDBNull(evehqData.Tables[0].Rows[item]["displayName"]) == false)
                    {
                        StaticData.AttributeUnits.Add(Convert.ToInt32(evehqData.Tables[0].Rows[item]["unitID"]), Convert.ToString(evehqData.Tables[0].Rows[item]["displayName"]));
                    }
                    else
                    {
                        StaticData.AttributeUnits.Add(Convert.ToInt32(evehqData.Tables[0].Rows[item]["unitID"]), "");
                    }
                }
            }
        }

        private void LoadUnlocks()
        {
            string strSQL = "";
            strSQL += "SELECT invTypes.typeID AS invTypeID, invTypes.groupID, invTypes.typeName, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, invTypes.published";
            strSQL += " FROM invTypes INNER JOIN dgmTypeAttributes ON invTypes.typeID = dgmTypeAttributes.typeID";
            strSQL += " WHERE (((dgmTypeAttributes.attributeID) IN (182,183,184,277,278,279,1285,1286,1287,1288,1289,1290)) AND (invTypes.published=1))";
            strSQL += " ORDER BY invTypes.typeID, dgmTypeAttributes.attributeID;";
            string lastAtt = "0";
            string skillIDLevel = null;
            var itemList = new ArrayList();
            double attValue = 0;
            using (DataSet evehqData = GetStaticData(strSQL))
            {
                for (int row = 0; row <= evehqData.Tables[0].Rows.Count - 1; row++)
                {
                    if (evehqData.Tables[0].Rows[row]["invTypeID"].ToString() != lastAtt)
                    {
                        DataRow[] attRows = evehqData.Tables[0].Select("invTypeID=" + evehqData.Tables[0].Rows[row]["invtypeID"]);
                        const int MaxPreReqs = 10;
                        var preReqSkills = new string[MaxPreReqs + 1];
                        var preReqSkillLevels = new int[MaxPreReqs + 1];
                        foreach (DataRow attRow in attRows)
                        {
                            if (Information.IsDBNull(attRow["valueInt"]) == false)
                            {
                                attValue = Convert.ToDouble(attRow["valueInt"]);
                            }
                            else
                            {
                                attValue = Convert.ToDouble(attRow["valueFloat"]);
                            }
                            switch (Convert.ToInt32(attRow["attributeID"]))
                            {
                                case 182:
                                    preReqSkills[1] = Convert.ToString(attValue);
                                    break;
                                case 183:
                                    preReqSkills[2] = Convert.ToString(attValue);
                                    break;
                                case 184:
                                    preReqSkills[3] = Convert.ToString(attValue);
                                    break;
                                case 1285:
                                    preReqSkills[4] = Convert.ToString(attValue);
                                    break;
                                case 1289:
                                    preReqSkills[5] = Convert.ToString(attValue);
                                    break;
                                case 1290:
                                    preReqSkills[6] = Convert.ToString(attValue);
                                    break;
                                case 277:
                                    preReqSkillLevels[1] = Convert.ToInt32(attValue);
                                    break;
                                case 278:
                                    preReqSkillLevels[2] = Convert.ToInt32(attValue);
                                    break;
                                case 279:
                                    preReqSkillLevels[3] = Convert.ToInt32(attValue);
                                    break;
                                case 1286:
                                    preReqSkillLevels[4] = Convert.ToInt32(attValue);
                                    break;
                                case 1287:
                                    preReqSkillLevels[5] = Convert.ToInt32(attValue);
                                    break;
                                case 1288:
                                    preReqSkillLevels[6] = Convert.ToInt32(attValue);
                                    break;
                            }
                        }
                        for (int prereq = 1; prereq <= MaxPreReqs; prereq++)
                        {
                            if (!string.IsNullOrEmpty(preReqSkills[prereq]))
                            {
                                skillIDLevel = preReqSkills[prereq] + "." + preReqSkillLevels[prereq];
                                itemList.Add(skillIDLevel + "_" + evehqData.Tables[0].Rows[row]["invtypeID"] + "_" + evehqData.Tables[0].Rows[row]["groupID"]);
                            }
                        }
                        lastAtt = Convert.ToString(evehqData.Tables[0].Rows[row]["invtypeID"]);
                    }
                }

                // Place the items into the Shared arrays
                var items = new string[3];
                List<string> itemUnlocked = default(List<string>);
                List<int> certUnlocked = default(List<int>);
                StaticData.SkillUnlocks.Clear();
                StaticData.ItemUnlocks.Clear();
                foreach (string item in itemList)
                {
                    items = item.Split(Convert.ToChar("_"));
                    if (StaticData.SkillUnlocks.ContainsKey(items[0]) == false)
                    {
                        // Create an arraylist and add the item
                        itemUnlocked = new List<string>();
                        itemUnlocked.Add(items[1] + "_" + items[2]);
                        StaticData.SkillUnlocks.Add(items[0], itemUnlocked);
                    }
                    else
                    {
                        // Fetch the item and add the new one
                        itemUnlocked = StaticData.SkillUnlocks[items[0]];
                        itemUnlocked.Add(items[1] + "_" + items[2]);
                    }
                    if (StaticData.ItemUnlocks.ContainsKey(items[1]) == false)
                    {
                        // Create an arraylist and add the item
                        itemUnlocked = new List<string>();
                        itemUnlocked.Add(items[0]);
                        StaticData.ItemUnlocks.Add(items[1], itemUnlocked);
                    }
                    else
                    {
                        // Fetch the item and add the new one
                        itemUnlocked = StaticData.ItemUnlocks[items[1]];
                        itemUnlocked.Add(items[0]);
                    }
                }

                // Add certificates into the skill unlocks?

                foreach (Certificate cert in StaticData.Certificates.Values)
                {
                    foreach (int skill in cert.GradesAndSkills[CertificateGrade.Basic].Keys)
                    {
                        string skillID = skill + "." + cert.GradesAndSkills[CertificateGrade.Basic][skill];
                        if (StaticData.CertUnlockSkills.ContainsKey(skillID) == false)
                        {
                            // Create an arraylist and add the item
                            certUnlocked = new List<int>();
                            certUnlocked.Add(cert.Id);
                            StaticData.CertUnlockSkills.Add(skillID, certUnlocked);
                        }
                        else
                        {
                            // Fetch the item and add the new one
                            certUnlocked = StaticData.CertUnlockSkills[skillID];
                            certUnlocked.Add(cert.Id);
                        }
                    }
                    //For Each certID As Integer In cert.RequiredCertificates.Keys
                    //    If StaticData.CertUnlockCertificates.ContainsKey(certID) = False Then
                    //        ' Create an arraylist and add the item
                    //        certUnlocked = New List(Of Integer)
                    //        certUnlocked.Add(cert.Id)
                    //        StaticData.CertUnlockCertificates.Add(certID, certUnlocked)
                    //    Else
                    //        ' Fetch the item and add the new one
                    //        certUnlocked = StaticData.CertUnlockCertificates(certID)
                    //        certUnlocked.Add(cert.Id)
                    //    End If
                    //Next
                }
            }
        }

        private void ParseCertsYAMLFile()
        {
            using (var dataStream = new MemoryStream(Resources.certificates))
            {
                using (var reader = new StreamReader(dataStream))
                {
                    dynamic yaml = new YamlDotNet.RepresentationModel.YamlStream();
                    yaml.Load(reader);

                    if (yaml.Documents.Count > 0)
                    {
                        // Should only be 1 document so go with the first
                        dynamic yamlDoc = yaml.Documents[0];
                        // Cycle through the keys, which will be the certIDs
                        dynamic root = (YamlMappingNode)yamlDoc.RootNode;
                        foreach (dynamic entry_loopVariable in root.Children)
                        {
                            dynamic entry = entry_loopVariable;
                            dynamic certID = Convert.ToInt32(((YamlScalarNode)entry.Key).Value);
                            var cert = new YAMLCert();
                            cert.RecommendedFor = new List<int>();
                            cert.CertID = certID;
                            // Parse anything underneath
                            dynamic dataItem = entry.Value as YamlMappingNode;
                            if (dataItem != null)
                            {
                                foreach (dynamic subEntry_loopVariable in dataItem.Children)
                                {
                                    dynamic subEntry = subEntry_loopVariable;
                                    // Get the key and value of th sub entry
                                    string keyName = ((YamlScalarNode)subEntry.Key).Value;
                                    // Do something based on the key
                                    switch (keyName)
                                    {
                                        case "description":
                                            // Set the description
                                            cert.Description = ((YamlScalarNode)subEntry.Value).Value;
                                            break;
                                        case "groupID":
                                            // Set the groupID
                                            cert.GroupID = Convert.ToInt32(((YamlScalarNode)subEntry.Value).Value);
                                            break;
                                        case "name":
                                            // Set the name
                                            cert.Name = ((YamlScalarNode)subEntry.Value).Value;
                                            break;
                                        case "recommendedFor":
                                            // Set the type recommendations.
                                            cert.RecommendedFor = ((YamlSequenceNode)subEntry.Value).Children.Select(e => Convert.ToInt32(((YamlScalarNode)e).Value));
                                            break;
                                        case "skillTypes":
                                            // Set the required Skills
                                            dynamic skillMaps = (YamlMappingNode)subEntry.Value;
                                            var reqSkills = new List<CertReqSkill>();
                                            if (skillMaps != null)
                                            {
                                                foreach (dynamic skillMap_loopVariable in skillMaps.Children)
                                                {
                                                    dynamic skillMap = skillMap_loopVariable;
                                                    var reqSkill = new CertReqSkill();
                                                    reqSkill.SkillID = Convert.ToInt32(((YamlScalarNode)skillMap.Key).Value);
                                                    reqSkill.SkillLevels = new Dictionary<string, int>();
                                                    foreach (KeyValuePair<YamlNode, YamlNode> level_loopVariable in ((YamlMappingNode)skillMap.Value).Children)
                                                    {
                                                        KeyValuePair<YamlNode, YamlNode> level = level_loopVariable;
                                                        reqSkill.SkillLevels.Add(((YamlScalarNode)level.Key).Value, Convert.ToInt32(((YamlScalarNode)level.Value).Value));
                                                    }
                                                    reqSkills.Add(reqSkill);
                                                }
                                            }
                                            cert.RequiredSkills = reqSkills;
                                            break;
                                    }
                                }
                                YamlCerts.Add(cert.CertID, cert);
                            }
                        }
                    }
                }
            }
        }

        private void ParseIconsYAMLFile()
        {
            using (var dataStream = new MemoryStream(Resources.iconIDs))
            {
                using (var reader = new StreamReader(dataStream))
                {
                    dynamic yaml = new YamlDotNet.RepresentationModel.YamlStream();
                    yaml.Load(reader);
                    if (yaml.Documents.Any())
                    {
                        // Should only be 1 document so go with the first
                        dynamic yamlDoc = yaml.Documents[0];
                        // Cycle through the keys, which will be the typeIDs
                        dynamic root = (YamlMappingNode)yamlDoc.RootNode;
                        foreach (dynamic entry_loopVariable in root.Children)
                        {
                            dynamic entry = entry_loopVariable;
                            // Parse the typeID
                            int iconID = Convert.ToInt32(((YamlScalarNode)entry.Key).Value);
                            // Parse anything underneath
                            foreach (KeyValuePair<YamlNode, YamlNode> subEntry_loopVariable in ((YamlMappingNode)entry.Value).Children)
                            {
                                KeyValuePair<YamlNode, YamlNode> subEntry = subEntry_loopVariable;
                                // Get the key and value of th sub entry
                                string keyName = ((YamlScalarNode)subEntry.Key).Value;
                                // Do something based on the key
                                switch (keyName)
                                {
                                    case "iconFile":
                                        // Pre-process the icon name to make it easier later on
                                        string iconName = ((YamlScalarNode)subEntry.Value).Value.Trim();
                                        // Get the filename if the fullname starts with "res:"
                                        if (iconName.StartsWith("res", StringComparison.Ordinal))
                                        {
                                            iconName = iconName.Split("/".ToCharArray()).Last();
                                        }
                                        // Set the icon item
                                        yamlIcons.Add(iconID, iconName);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ParseTypesYAMLFile()
        {
            using (var dataStream = new MemoryStream(Resources.typeIDs))
            {
                using (var reader = new StreamReader(dataStream))
                {
                    dynamic yaml = new YamlDotNet.RepresentationModel.YamlStream();
                    yaml.Load(reader);

                    if (yaml.Documents.Any())
                    {
                        // Should only be 1 document so go with the first
                        dynamic yamlDoc = yaml.Documents[0];
                        // Cycle through the keys, which will be the typeIDs
                        dynamic root = (YamlMappingNode)yamlDoc.RootNode;
                        foreach (dynamic entry_loopVariable in root.Children)
                        {
                            dynamic entry = entry_loopVariable;
                            // Parse the typeID
                            int typeID = Convert.ToInt32(((YamlScalarNode)entry.Key).Value);
                            var yamlItem = new YAMLType();
                            yamlItem.TypeID = typeID;
                            // Parse anything underneath
                            dynamic dataItem = entry.Value as YamlMappingNode;
                            if (dataItem != null)
                            {
                                foreach (dynamic subEntry_loopVariable in dataItem.Children)
                                {
                                    dynamic subEntry = subEntry_loopVariable;
                                    // Get the key and value of the sub entry
                                    string keyName = ((YamlScalarNode)subEntry.Key).Value;
                                    // Do something based on the key
                                    switch (keyName)
                                    {
                                        case "iconID":
                                            // Set the icon item
                                            yamlItem.IconID = Convert.ToInt32(((YamlScalarNode)subEntry.Value).Value);
                                            break;
                                        case "masteries":
                                            // Set the various collections of certificates needed for each level of mastery
                                            yamlItem.Masteries = new Dictionary<int, List<int>>();
                                            dynamic masteryLevels = (YamlMappingNode)subEntry.Value;
                                            foreach (dynamic level_loopVariable in masteryLevels.Children)
                                            {
                                                dynamic level = level_loopVariable;
                                                dynamic levelID = Convert.ToInt32(((YamlScalarNode)level.Key).Value);
                                                dynamic certs = ((YamlSequenceNode)level.Value).Children.Select(node => Convert.ToInt32(((YamlScalarNode)node).Value)).ToList();
                                                yamlItem.Masteries.Add(levelID, certs);
                                            }

                                            break;
                                        case "traits":
                                            // Set ship traits texts for each ship skill
                                            yamlItem.Traits = new Dictionary<int, List<string>>();
                                            dynamic traits = (YamlMappingNode)subEntry.Value;
                                            foreach (dynamic skill_loopVariable in traits.Children)
                                            {
                                                dynamic skill = skill_loopVariable;
                                                dynamic skillID = Convert.ToInt32(((YamlScalarNode)skill.Key).Value);
                                                var bonusStrings = new List<string>();
                                                foreach (KeyValuePair<YamlNode, YamlNode> index_loopVariable in ((YamlMappingNode)skill.Value).Children)
                                                {
                                                    KeyValuePair<YamlNode, YamlNode> index = index_loopVariable;
                                                    string partBonus = "";
                                                    string partBonusText = "";
                                                    string partUnitID = "";
                                                    foreach (KeyValuePair<YamlNode, YamlNode> bonusPart_loopVariable in ((YamlMappingNode)index.Value).Children)
                                                    {
                                                        KeyValuePair<YamlNode, YamlNode> bonusPart = bonusPart_loopVariable;
                                                        switch (((YamlScalarNode)bonusPart.Key).Value)
                                                        {
                                                            case "bonus":
                                                                partBonus = ((YamlScalarNode)bonusPart.Value).Value;
                                                                break;
                                                            case "bonusText":
                                                                partBonusText = ((YamlScalarNode)bonusPart.Value).Value;
                                                                break;
                                                            case "unitID":
                                                                switch (((YamlScalarNode)bonusPart.Value).Value)
                                                                {
                                                                    case "105":
                                                                        partUnitID = "%";
                                                                        break;
                                                                    case "139":
                                                                        partUnitID = "+";
                                                                        break;
                                                                    case "9":
                                                                        partUnitID = "m³";
                                                                        break;
                                                                    case "1":
                                                                        partUnitID = "m";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    if (string.IsNullOrEmpty(partBonus + partUnitID))
                                                    {
                                                        partUnitID = "·";
                                                    }
                                                    bonusStrings.Add(partBonus + partUnitID + " " + partBonusText);
                                                }
                                                yamlItem.Traits.Add(skillID, bonusStrings);
                                            }

                                            break;
                                    }
                                }
                            }
                            // Get the iconFile if relevant
                            if (yamlIcons.ContainsKey(yamlItem.IconID))
                            {
                                yamlItem.IconName = yamlIcons[yamlItem.IconID];
                            }
                            // Add the item
                            yamlTypes.Add(yamlItem.TypeID, yamlItem);
                        }
                    }
                }
            }
        }

        private void ParseYAMLFiles()
        {
            yamlTypes = new SortedList<int, YAMLType>();
            yamlIcons = new SortedList<int, string>();

            // Parse the icons file first
            ParseIconsYAMLFile();
            ParseTypesYAMLFile();
            ParseCertsYAMLFile();
        }

        private void PopulateModuleGroups(int inParentID, ref TreeNode inTreeNode, DataTable marketTable)
        {
            DataRow[] parentRows = marketTable.Select("parentGroupID=" + inParentID);
            foreach (DataRow parentRow in parentRows)
            {
                TreeNode parentnode = default(TreeNode);
                parentnode = new TreeNode(Convert.ToString(parentRow["marketGroupName"]));
                parentnode.Name = parentnode.Text;
                inTreeNode.Nodes.Add(parentnode);
                parentnode.Tag = parentRow["marketGroupID"];
                PopulateModuleGroups(Convert.ToInt32(parentnode.Tag), ref parentnode, marketTable);
            }
        }

        private void PopulateShipGroups(int inParentID, ref TreeNode inTreeNode, DataTable marketTable)
        {
            DataRow[] parentRows = marketTable.Select("parentGroupID=" + inParentID);
            foreach (DataRow parentRow in parentRows)
            {
                TreeNode parentnode = default(TreeNode);
                parentnode = new TreeNode(Convert.ToString(parentRow["marketGroupName"]));
                inTreeNode.Nodes.Add(parentnode);
                parentnode.Tag = parentRow["marketGroupID"];
                PopulateShipGroups(Convert.ToInt32(parentnode.Tag), ref parentnode, marketTable);
            }
            DataRow[] groupRows = shipNameData.Tables[0].Select("marketGroupID=" + inParentID);
            foreach (DataRow shipRow in groupRows)
            {
                inTreeNode.Nodes.Add(shipRow["typeName"].ToString());
            }
        }

        private void PopulateShipLists()
        {
            ShipLists.ShipListKeyName.Clear();
            ShipLists.ShipListKeyID.Clear();
            foreach (Ship baseShip in ShipLists.ShipList.Values)
            {
                ShipLists.ShipListKeyName.Add(baseShip.Name, baseShip.ID);
                ShipLists.ShipListKeyID.Add(baseShip.ID, baseShip.Name);
            }
        }

        private void SaveHQFCacheData()
        {
            // Dump HQF data to folder
            FileStream s = default(FileStream);

            // Save ships
            s = new FileStream(Path.Combine(coreCacheFolder, "ships.dat"), FileMode.Create);
            Serializer.Serialize(s, ShipLists.ShipList);
            s.Flush();
            s.Close();

            // Save modules
            s = new FileStream(Path.Combine(coreCacheFolder, "modules.dat"), FileMode.Create);
            Serializer.Serialize(s, ModuleLists.ModuleList);
            s.Flush();
            s.Close();

            // Save implants
            s = new FileStream(Path.Combine(coreCacheFolder, "implants.dat"), FileMode.Create);
            Serializer.Serialize(s, Implants.ImplantList);
            s.Flush();
            s.Close();

            // Save boosters
            s = new FileStream(Path.Combine(coreCacheFolder, "boosters.dat"), FileMode.Create);
            Serializer.Serialize(s, Boosters.BoosterList);
            s.Flush();
            s.Close();

            // Save skills
            s = new FileStream(Path.Combine(coreCacheFolder, "skills.dat"), FileMode.Create);
            Serializer.Serialize(s, SkillLists.SkillList);
            s.Flush();
            s.Close();

            // Save attributes
            s = new FileStream(Path.Combine(coreCacheFolder, "attributes.dat"), FileMode.Create);
            Serializer.Serialize(s, Attributes.AttributeList);
            s.Flush();
            s.Close();
        }

        private void WriteGroupNodes(TreeNode parentNode, StreamWriter sw)
        {
            sw.Write("0," + parentNode.FullPath + ControlChars.CrLf);
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                if (childNode.Nodes.Count > 0)
                {
                    WriteGroupNodes(childNode, sw);
                }
                else
                {
                    sw.Write(childNode.Tag + "," + childNode.FullPath + ControlChars.CrLf);
                }
            }
        }

        private void WriteItemGroups(TreeView tvwItems)
        {
            var sw = new StreamWriter(Path.Combine(coreCacheFolder, "ItemGroups.bin"));
            foreach (TreeNode rootNode in tvwItems.Nodes)
            {
                WriteGroupNodes(rootNode, sw);
            }
            sw.Flush();
            sw.Close();
        }

        private void WriteShipGroups(TreeView tvwShips)
        {
            var sw = new StreamWriter(Path.Combine(coreCacheFolder, "ShipGroups.bin"));
            foreach (TreeNode rootNode in tvwShips.Nodes)
            {
                WriteShipNodes(rootNode, sw);
            }
            sw.Flush();
            sw.Close();
        }

        private void WriteShipNodes(TreeNode parentNode, StreamWriter sw)
        {
            sw.Write(parentNode.FullPath + ControlChars.CrLf);
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                if (childNode.Nodes.Count > 0)
                {
                    WriteShipNodes(childNode, sw);
                }
                else
                {
                    sw.Write(childNode.FullPath + ControlChars.CrLf);
                }
            }
        }

        private void btnCheckDB_Click(object sender, EventArgs e)
        {
            CheckSQLDatabase();
            MessageBox.Show("SQL Database check complete!");
        }

        private void btnCheckMarketGroup_Click(object sender, EventArgs e)
        {
            DataSet evehqData = default(DataSet);
            const string StrSQL = "SELECT * FROM invMarketGroups;";
            evehqData = GetStaticData(StrSQL);

            var marketGroups = new List<int>();
            foreach (DataRow dr in evehqData.Tables[0].Rows)
            {
                if (marketGroups.Contains(Convert.ToInt32(dr["marketGroupID"])) == false)
                {
                    marketGroups.Add(Convert.ToInt32(dr["marketGroupID"]));
                }
            }

            var missingGroups = new List<int>();
            foreach (DataRow dr in evehqData.Tables[0].Rows)
            {
                if (Information.IsDBNull(dr["parentGroupID"]) == false)
                {
                    if (marketGroups.Contains(Convert.ToInt32(dr["parentGroupID"])) == false)
                    {
                        if (missingGroups.Contains(Convert.ToInt32(dr["parentGroupID"])) == false)
                        {
                            missingGroups.Add(Convert.ToInt32(dr["parentGroupID"]));
                        }
                    }
                }
            }

            MessageBox.Show(missingGroups.Count.ToString());
        }

        private void btnGenerateCache_Click(object sender, EventArgs e)
        {
            // Check for existence of a core cache folder in the application directory
            coreCacheFolder = Path.Combine(Application.StartupPath, CacheFolderName);
            if (Directory.Exists(coreCacheFolder) == false)
            {
                // Create the cache folder if it doesn't exist
                Directory.CreateDirectory(coreCacheFolder);
            }

            // Parse the YAML files
            ParseYAMLFiles();

            // Create and write core cache data
            LoadAllData();
            CreateCoreCache();

            // Create and write HQF cache data
            GenerateHQFCacheData();

            MessageBox.Show("Creation of cache data complete!");
        }

        private void frmCacheCreator_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(_sqLiteDbFolder) == false)
            {
                Directory.CreateDirectory(_sqLiteDbFolder);
            }
            if (File.Exists(_sqLiteDb) == false)
            {
                SQLiteConnection.CreateFile(_sqLiteDb);
            }
        }
    }
}