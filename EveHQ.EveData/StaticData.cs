using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;

namespace EveHQ.EveData
{
    public class StaticData
    {

        // Data Availability
        public bool DataAvailable = false;

        // Attributes
        public SortedList<int, AttributeType> AttributeTypes = new SortedList<int, AttributeType>(); // attributeID, AttributeType
        public List<TypeAttribute> TypeAttributes = new List<TypeAttribute>();
        public SortedList<int, string> AttributeUnits = new SortedList<int, string>(); // unitID, DisplayName

        // Effects
        public SortedList<int, EffectType> EffectTypes = new SortedList<int, EffectType>(); // effectID, EffectType
        public List<TypeEffect> TypeEffects = new List<TypeEffect>();
        
        // Types
        public SortedList<string, string> TypeNames = new SortedList<string, string>(); // typeName, typeID
        public SortedList<string, EveType> Types = new SortedList<string, EveType>();  // typeID, EveType
        public SortedList<int, string> TypeGroups = new SortedList<int, string>(); // groupID, groupName
        public SortedList<int, string> TypeCats = new SortedList<int, string>();  // catID, catName
        public SortedList<int, int> GroupCats = new SortedList<int, int>();   // groupID, catID

        // Market Groups
        public SortedList<int, MarketGroup> MarketGroups = new SortedList<int, MarketGroup>();  // typeID, MarketGroup
        public SortedList<string, string> ItemMarketGroups = new SortedList<string, string>();  // typeID, marketGroupID

        // Meta Items
        public SortedList<int, string> MetaGroups = new SortedList<int, string>(); // metaGroupID, metaGroupName
        public SortedList<int, MetaType> MetaTypes = new SortedList<int, MetaType>();  // typeID, MetaItem

        // Certificates
        public SortedList<string, Certificate> Certificates = new SortedList<string, Certificate>();
        public SortedList<string, CertificateCategory> CertificateCategories = new SortedList<string, CertificateCategory>();
        public SortedList<string, CertificateClass> CertificateClasses = new SortedList<string, CertificateClass>();
        public List<CertificateRecommendation> CertificateRecommendations = new List<CertificateRecommendation>();
        
        // Unlocks
        public SortedList<string, List<string>> SkillUnlocks = new SortedList<string, List<string>>();
        public SortedList<string, List<string>> ItemUnlocks = new SortedList<string, List<string>>();
        public SortedList<string, List<string>> CertUnlockCerts = new SortedList<string, List<string>>();
        public SortedList<string, List<string>> CertUnlockSkills = new SortedList<string, List<string>>();
        
        // Locations
        public Dictionary<int, string> Regions = new Dictionary<int, string>();
        public Dictionary<int, string> Constellations = new Dictionary<int, string>();
        public Dictionary<int, SolarSystem> SolarSystems = new Dictionary<int, SolarSystem>();
        public Dictionary<int, Station> Stations = new Dictionary<int, Station>();
        public Dictionary<int, Agent> Agents = new Dictionary<int, Agent>();
        public Dictionary<int, string> Divisions = new Dictionary<int, string>();

        // Blueprints, Materials and Industry
        public SortedList<int, Blueprint> Blueprints = new SortedList<int, Blueprint>();  // typeID, BlueprintType
        public SortedList<string, AssemblyArray> AssemblyArrays = new SortedList<string, AssemblyArray>();  // typeName, AssemblyArray
        public SortedList<int, string> NPCCorps = new SortedList<int, string>(); // corpID, corpName
        public SortedList<int, string> ItemFlags = new SortedList<int, string>();  // flagID, flagName

        /// <summary>
        /// Initialises a set of blank data for use
        /// </summary>
        public StaticData()
        {
            // Default constructor
        }

        /// <summary>
        /// Initialises a set of EveData from pre-generated cache files.
        /// </summary>
        /// <param name="coreCacheFolder">The full path to the folder containing the EveHQ cache files.</param>
        public StaticData(string coreCacheFolder)
        {
            //TODO: Constructor should have a parameter containing a reference to the log file so it can be written to
            if (LoadCoreCache(coreCacheFolder))
            {
                // Mark data as being available
                DataAvailable = true;
            }
        }

        /// <summary>
        /// Loads the core cache data from storage.
        /// </summary>
        /// <param name="coreCacheFolder">The full path to the folder containing the EveHQ cache files.</param>
        /// <returns>A boolean value indicating whether the load procedure was successful.</returns>
        private bool LoadCoreCache(string coreCacheFolder)
        {

            try
            {
                // Check for existence of a core cache folder in the application directory
                if (!Directory.Exists(coreCacheFolder)) return false;

                //HQ.WriteLogEvent(" *** Start of Cache Loading...");

                // Get files from dump
                   
                // Item List
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemList.dat"), FileMode.Open))
                {
                    TypeNames = Serializer.Deserialize<SortedList<string, string>>(s);
                }
                //HQ.WriteLogEvent(" *** Item List Finished Loading");

                // Item Data
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Items.dat"), FileMode.Open))
                {
                    Types = Serializer.Deserialize<SortedList<string, EveType>>(s);
                }
                //HQ.WriteLogEvent(" *** Items Finished Loading");

                // Item Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemGroups.dat"), FileMode.Open))
                {
                    TypeGroups = Serializer.Deserialize<SortedList<int, string>>(s); 
                }
                //HQ.WriteLogEvent(" *** Item Groups Finished Loading");

                // Items Cats
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemCats.dat"), FileMode.Open))
                {
                    TypeCats = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //HQ.WriteLogEvent(" *** Item Categories Finished Loading");

                // Group Cats
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "GroupCats.dat"), FileMode.Open))
                {
                    GroupCats = Serializer.Deserialize<SortedList<int, int>>(s);
                }
                //HQ.WriteLogEvent(" *** Group Categories Finished Loading");

                // Market Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "MarketGroups.dat"), FileMode.Open))
                {
                    MarketGroups = Serializer.Deserialize<SortedList<int, MarketGroup>>(s);
                }
                //HQ.WriteLogEvent(" *** Market Groups Finished Loading");

                // Item Market Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemMarketGroups.dat"), FileMode.Open))
                {
                    ItemMarketGroups = Serializer.Deserialize<SortedList<string, string>>(s);
                }
                //HQ.WriteLogEvent(" *** Market Groups Finished Loading");

                // Cert Categories
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertCats.dat"), FileMode.Open))
                {
                    CertificateCategories = Serializer.Deserialize<SortedList<string, CertificateCategory>>(s); 
                }
                //HQ.WriteLogEvent(" *** Certificate Categories Finished Loading");

                // Cert Classes
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertClasses.dat"), FileMode.Open))
                {
                    CertificateClasses = Serializer.Deserialize<SortedList<string, CertificateClass>>(s);
                }
                //HQ.WriteLogEvent(" *** Certificate Classes Finished Loading");

                // Certs
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Certs.dat"), FileMode.Open))
                {
                    Certificates = Serializer.Deserialize<SortedList<string, Certificate>>(s);
                }
                //HQ.WriteLogEvent(" *** Certificates Finished Loading");

                // Cert Recommendations
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertRec.dat"), FileMode.Open))
                {
                    CertificateRecommendations = Serializer.Deserialize<List<CertificateRecommendation>>(s); 
                }
                //HQ.WriteLogEvent(" *** Certificate Recommendations Finished Loading");

                // Unlocks
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemUnlocks.dat"), FileMode.Open))
                {
                    ItemUnlocks = Serializer.Deserialize<SortedList<string, List<string>>>(s); 
                }
                //HQ.WriteLogEvent(" *** Item Unlocks Finished Loading");

                // SkillUnlocks
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "SkillUnlocks.dat"), FileMode.Open))
                {
                    SkillUnlocks = Serializer.Deserialize<SortedList<string, List<string>>>(s);
                }
                //HQ.WriteLogEvent(" *** Skill Unlocks Finished Loading");

                // CertCerts
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertCerts.dat"), FileMode.Open))
                {
                    CertUnlockCerts = Serializer.Deserialize<SortedList<string, List<string>>>(s);
                }
                //HQ.WriteLogEvent(" *** Certificate Unlocks Finished Loading");

                // CertSkills
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertSkills.dat"), FileMode.Open))
                {
                    CertUnlockSkills = Serializer.Deserialize<SortedList<string, List<string>>>(s);
                }
                //HQ.WriteLogEvent(" *** Certificate Skills Finished Loading");

                // Regions
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Regions.dat"), FileMode.Open))
                {
                    Regions = Serializer.Deserialize<Dictionary<int, string>>(s);
                }
                //HQ.WriteLogEvent(" *** Regions Finished Loading");

                // Constellations
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Constellations.dat"), FileMode.Open))
                {
                    Constellations = Serializer.Deserialize<Dictionary<int, string>>(s);
                }
                //HQ.WriteLogEvent(" *** Constellations Finished Loading");

                // SolarSystems
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Systems.dat"), FileMode.Open))
                {
                    SolarSystems = Serializer.Deserialize<Dictionary<int, SolarSystem>>(s);
                }
                //HQ.WriteLogEvent(" *** Solar Systems Finished Loading");

                // Stations
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Stations.dat"), FileMode.Open))
                {
                    Stations = Serializer.Deserialize<Dictionary<int, Station>>(s);
                }
                //HQ.WriteLogEvent(" *** Stations Finished Loading");

                // Divisions
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Divisions.dat"), FileMode.Open))
                {
                    Divisions = Serializer.Deserialize<Dictionary<int, string>>(s);
                }
                //HQ.WriteLogEvent(" *** Divisions Finished Loading");

                // Agents
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Agents.dat"), FileMode.Open))
                {
                    Agents = Serializer.Deserialize<Dictionary<int, Agent>>(s);
                }
                //HQ.WriteLogEvent(" *** Agents Finished Loading");

                // Attribute Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "AttributeTypes.dat"), FileMode.Open))
                {
                    AttributeTypes = Serializer.Deserialize<SortedList<int, AttributeType>>(s);
                }
                //HQ.WriteLogEvent(" *** Attribute Types Finished Loading");

                // Type Attributes
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "TypeAttributes.dat"), FileMode.Open))
                {
                    TypeAttributes = Serializer.Deserialize<List<TypeAttribute>>(s);
                }
                //HQ.WriteLogEvent(" *** Type Attributes Finished Loading");

                // Attribute Units
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Units.dat"), FileMode.Open))
                {
                    AttributeUnits = Serializer.Deserialize<SortedList<int, string>>(s);    
                }
                //HQ.WriteLogEvent(" *** Units Finished Loading");

                // Effect Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "EffectTypes.dat"), FileMode.Open))
                {
                    EffectTypes = Serializer.Deserialize<SortedList<int, EffectType>>(s);
                }
                //HQ.WriteLogEvent(" *** Effect Types Finished Loading");

                // Type Effects
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "TypeEffects.dat"), FileMode.Open))
                {
                    TypeEffects = Serializer.Deserialize<List<TypeEffect>>(s);
                }
                //HQ.WriteLogEvent(" *** Type Effects Finished Loading");

                // Meta Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "MetaGroups.dat"), FileMode.Open))
                {
                    MetaGroups = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //HQ.WriteLogEvent(" *** Meta Groups Finished Loading");

                // Meta Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "MetaTypes.dat"), FileMode.Open))
                {
                    MetaTypes = Serializer.Deserialize<SortedList<int, MetaType>>(s);
                }
                //HQ.WriteLogEvent(" *** Meta Types Finished Loading");

                // Blueprint Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Blueprints.dat"), FileMode.Open))
                {
                    Blueprints = Serializer.Deserialize<SortedList<int, Blueprint>>(s);
                }
                //HQ.WriteLogEvent(" *** Blueprints Finished Loading");

                // Assembly Arrays
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "AssemblyArrays.dat"), FileMode.Open))
                {
                    AssemblyArrays = Serializer.Deserialize<SortedList<string, AssemblyArray>>(s);
                }
                //HQ.WriteLogEvent(" *** Assembly Arrays Finished Loading");

                // NPC Corps
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "NPCCorps.dat"), FileMode.Open))
                {
                    NPCCorps = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //HQ.WriteLogEvent(" *** NPC Corps Finished Loading");

                // Item Flags
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemFlags.dat"), FileMode.Open))
                {
                    ItemFlags = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //HQ.WriteLogEvent(" *** Item Flags Finished Loading");

                return true;
            }
            catch (Exception)
            {
                // Load Core Cache failed
                return false;
            }

        }

        //''' <summary>
        //''' Returns the type ID of a blueprint given a productID
        //''' </summary>
        //''' <param name="productID">The type ID of the product</param>
        //''' <returns>An integer representing the blueprint type ID</returns>
        //''' <remarks></remarks>
        public int GetBPTypeID(int productID)
        {
            var itemIDs = (from bt in Blueprints.Values where bt.ProductID == productID select bt.ID).ToList();

            if (itemIDs.Count > 0)
            {
                return itemIDs[0];
            }

            return -1;
        }
        
        /// <summary>
        /// Returns the type ID of the product made from the blueprint
        /// </summary>
        /// <param name="bpTypeID">The type ID of the blueprint</param>
        /// <returns>An integer representing the typeID of the product</returns>
        /// <remarks></remarks>
        public int GetTypeID(int bpTypeID)
        {
            if (Blueprints.ContainsKey(bpTypeID))
            {
                return Blueprints[bpTypeID].ProductID;
            }
           
            return -1;
        }

        /// <summary>
        /// Function to return a list of items in a specific database group
        /// </summary>
        /// <param name="groupID">The groupID of the items</param>
        /// <returns>An IEnumerable(Of EveItem) containing the items in the requested group</returns>
        /// <remarks></remarks>
        public IEnumerable<EveType> GetItemsInGroup(int groupID)
        {
            return Types.Values.Where(item => item.Group == groupID);
        }

        /// <summary>
        /// Function to return a list of groups in a specific category
        /// </summary>
        /// <param name="categoryID">The categoryID of the groups</param>
        /// <returns>An IEnumerable(Of String) containing the IDs of the groups in the requested category</returns>
        /// <remarks></remarks>
        public IEnumerable<int> GetGroupsInCategory(int categoryID)
        {
            return GroupCats.Keys.Where(groupID => GroupCats[groupID] == categoryID);
        }

        /// <summary>
        /// Function to return a sorted list of items with IDs
        /// </summary>
        /// <param name="groupID">The groupID of the items</param>
        /// <returns>A SortedList(Of String, Integer) containing the names and IDs of items in the requested group</returns>
        /// <remarks></remarks>
        public SortedList<string, int> GetSortedItemListInGroup(int groupID)
        {
            var items = new SortedList<string, int>();
            foreach (var item in GetItemsInGroup(groupID))
            {
                items.Add(item.Name, item.ID);
            }
            return items;
        }


        /// <summary>
        /// Function to return all the attributes of a specific item
        /// </summary>
        /// <param name="typeID">The typeID of the item</param>
        /// <returns>A SortedList(Of Integer, ItemAttributeData) containing the detailed attributes</returns>
        /// <remarks></remarks>
        public SortedList<int, ItemAttributeData> GetAttributeDataForItem(int typeID)
        {
            

            //' Fetch the attributes for the item
            var atts = (from ta in TypeAttributes where ta.TypeID == typeID select new ItemAttribute {ID = ta.AttributeID, Value = ta.Value}).ToList();
            
            // Prepare the attribute data
            var attributeList = new SortedList<int, ItemAttributeData>();

            foreach (var att in atts)
            {
                attributeList.Add(att.ID, new ItemAttributeData(att.ID, att.Value, AttributeTypes[att.ID].DisplayName, " " + AttributeUnits[AttributeTypes[att.ID].UnitID]));
            }

            // Process attribute data
            var attributesToAdd = new SortedList<int, ItemAttributeData>();

            foreach (var att in attributeList.Values)
            {
                CorrectAttributeValue(att);

                // Adjust for skills
                switch (att.ID)
                {
                    case 182:
                    case 183:
                    case 184:
                    case 1285:
                    case 1289:
                    case 1290:
                        var skillLevelAttribute = 0;
                        switch (att.ID)
                        {
                            case 182:
                                skillLevelAttribute = 277;
                                break;
                            case 183:
                                skillLevelAttribute = 278;
                                break;
                            case 184:
                                skillLevelAttribute = 279;
                                break;
                            case 1285:
                                skillLevelAttribute = 1286;
                                break;
                            case 1289:
                                skillLevelAttribute = 1287;
                                break;
                            case 1290:
                                skillLevelAttribute = 1288;
                                break;
                        }
                        // Fix cases where there is no skill level data for the skill in the CCP data
                        if (attributeList.ContainsKey(skillLevelAttribute) == false)
                        {
                            attributesToAdd.Add(skillLevelAttribute, new ItemAttributeData(skillLevelAttribute, 0, "", ""));
                            att.DisplayValue = Types[Convert.ToString(att.Value)].Name + " (Level " + attributesToAdd[skillLevelAttribute].Value.ToString("N0") + ")";
                        }
                        else
                        {
                            att.DisplayValue = Types[Convert.ToString(att.Value)].Name + " (Level " + attributeList[skillLevelAttribute].Value.ToString("N0") + ")";
                        }
                        break;
                }

            }

            // Add in new attributes we need for skill levels
            foreach (var iba in attributesToAdd.Values)
            {
                attributeList.Add(iba.ID, iba);
            }

            return attributeList;
        }


        /// <summary>
        /// Corrects attribute values and display values
        /// </summary>
        /// <param name="att">The instance of the ItemAttributeData class to correct</param>
        /// <remarks></remarks>
        public void CorrectAttributeValue(ItemAttributeData att)
        {
            // Alter data based on unit ID
            switch (AttributeTypes[att.ID].UnitID)
            {
                case 108:
                    att.Value = 100 - (att.Value * 100);
                    att.DisplayValue = Convert.ToString(att.Value);
                    break;
                case 109:
                    att.Value = (att.Value * 100) - 100;
                    att.DisplayValue = Convert.ToString(att.Value);
                    break;
                case 111:
                    att.Value = (att.Value - 1) * 100;
                    att.DisplayValue = Convert.ToString(att.Value);
                    break;
                case 101:
                    att.Value = att.Value / 1000;
                    att.DisplayValue = Convert.ToString(att.Value);
                    break;
                case 115:
                    // groupID
                    att.DisplayValue = TypeGroups[Convert.ToInt32(att.Value)];
                    att.Unit = "";
                    break;
                case 116:
                    // typeID
                    att.DisplayValue = Types[Convert.ToString(att.Value)].Name;
                    att.Unit = "";
                    break;
                case 119:
                case 140:
                    att.DisplayValue = Convert.ToString(att.Value);
                    att.Unit = "";
                    break;
                default:
                    att.DisplayValue = Convert.ToString(att.Value);
                    break;
            }
        }
        
        ///<summary>
        ///Returns a list of meta variations of a specific item
        ///</summary>
        ///<param name="typeID">The type ID to get the variations of</param>
        ///<returns>A List(Of Integer) containing the typeIDs of the variations</returns>
        ///<remarks></remarks>
       public List<int> GetVariationsForItem(int typeID)
        {

            // Fetch the parent item ID for this item
            var parentTypeID = typeID;
            if (MetaTypes.ContainsKey(typeID))
            {
                parentTypeID = MetaTypes[typeID].ParentID;
            }

            // Fetch all items with this same parent ID
            var itemIDs = (from mt in MetaTypes.Values where mt.ParentID == parentTypeID select mt.ID).ToList();


            // Add the current item if it is the parent item
            if (itemIDs.Contains(parentTypeID) == false)
            {
                itemIDs.Add(parentTypeID);
            }

            return itemIDs; 
        }
    
    }
}
