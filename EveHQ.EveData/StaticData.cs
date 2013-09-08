// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticData.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//  Defines the collection of Eve static data and associated functions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using ProtoBuf;

    /// <summary>
    /// Defines the collection of Eve static data and associated functions.
    /// </summary>
    public class StaticData
    {
        #region "Property Fields"
        /// <summary>
        /// Gets or sets the attribute types.
        /// </summary>
        private SortedList<int, AttributeType> attributeTypes = new SortedList<int, AttributeType>(); // attributeID, AttributeType

        /// <summary>
        /// Gets or sets the type attributes.
        /// </summary>
        private Collection<TypeAttrib> typeAttributes = new Collection<TypeAttrib>();

        /// <summary>
        /// Gets or sets the attribute units.
        /// </summary>
        private SortedList<int, string> attributeUnits = new SortedList<int, string>(); // unitID, DisplayName

        /// <summary>
        /// Gets or sets the effect types.
        /// </summary>
        private SortedList<int, EffectType> effectTypes = new SortedList<int, EffectType>(); // effectID, EffectType

        /// <summary>
        /// Gets or sets the type effects.
        /// </summary>
        private Collection<TypeEffect> typeEffects = new Collection<TypeEffect>();

        /// <summary>
        /// Gets or sets the type names.
        /// </summary>
        private SortedList<string, string> typeNames = new SortedList<string, string>(); // typeName, typeID

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        private SortedList<string, EveType> types = new SortedList<string, EveType>(); // typeID, EveType

        /// <summary>
        /// Gets or sets the type groups.
        /// </summary>
        private SortedList<int, string> typeGroups = new SortedList<int, string>(); // groupID, groupName

        /// <summary>
        /// Gets or sets the type cats.
        /// </summary>
        private SortedList<int, string> typeCats = new SortedList<int, string>(); // catID, catName

        /// <summary>
        /// Gets or sets the group cats.
        /// </summary>
        private SortedList<int, int> groupCats = new SortedList<int, int>();  // groupID, catID

        /// <summary>
        /// Gets or sets the market groups.
        /// </summary>
        private SortedList<int, MarketGroup> marketGroups = new SortedList<int, MarketGroup>(); // typeID, MarketGroup

        /// <summary>
        /// Gets or sets the item market groups.
        /// </summary>
        private SortedList<string, string> itemMarketGroups = new SortedList<string, string>();  // typeID, marketGroupID

        /// <summary>
        /// Gets or sets the meta groups.
        /// </summary>
        private SortedList<int, string> metaGroups = new SortedList<int, string>(); // metaGroupID, metaGroupName

        /// <summary>
        /// Gets or sets the meta types.
        /// </summary>
        private SortedList<int, MetaType> metaTypes = new SortedList<int, MetaType>();  // typeID, MetaItem

        /// <summary>
        /// Gets or sets the certificates.
        /// </summary>
        private SortedList<string, Certificate> certificates = new SortedList<string, Certificate>();

        /// <summary>
        /// Gets or sets the certificate categories.
        /// </summary>
        private SortedList<string, CertificateCategory> certificateCategories = new SortedList<string, CertificateCategory>();

        /// <summary>
        /// Gets or sets the certificate classes.
        /// </summary>
        private SortedList<string, CertificateClass> certificateClasses = new SortedList<string, CertificateClass>();

        /// <summary>
        /// Gets or sets the certificate recommendations.
        /// </summary>
        private Collection<CertificateRecommendation> certificateRecommendations = new Collection<CertificateRecommendation>();

        /// <summary>
        /// Gets or sets the skill unlocks.
        /// </summary>
        private SortedList<string, Collection<string>> skillUnlocks = new SortedList<string, Collection<string>>();

        /// <summary>
        /// Gets or sets the item unlocks.
        /// </summary>
        private SortedList<string, Collection<string>> itemUnlocks = new SortedList<string, Collection<string>>();

        /// <summary>
        /// Gets or sets the cert unlock certs.
        /// </summary>
        private SortedList<string, Collection<string>> certUnlockCertificates = new SortedList<string, Collection<string>>();

        /// <summary>
        /// Gets or sets the cert unlock skills.
        /// </summary>
        private SortedList<string, Collection<string>> certUnlockSkills = new SortedList<string, Collection<string>>();

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        private Dictionary<int, string> regions = new Dictionary<int, string>();

        /// <summary>
        /// Gets or sets the constellations.
        /// </summary>
        private Dictionary<int, string> constellations = new Dictionary<int, string>();

        /// <summary>
        /// Gets or sets the solar systems.
        /// </summary>
        private Dictionary<int, SolarSystem> solarSystems = new Dictionary<int, SolarSystem>();

        /// <summary>
        /// Gets or sets the stations.
        /// </summary>
        private Dictionary<int, Station> stations = new Dictionary<int, Station>();

        /// <summary>
        /// Gets or sets the agents.
        /// </summary>
        private Dictionary<int, Agent> agents = new Dictionary<int, Agent>();

        /// <summary>
        /// Gets or sets the divisions.
        /// </summary>
        private Dictionary<int, string> divisions = new Dictionary<int, string>();

        /// <summary>
        /// Gets or sets the blueprints.
        /// </summary>
        private SortedList<int, Blueprint> blueprints = new SortedList<int, Blueprint>(); // typeID, BlueprintType

        /// <summary>
        /// Gets or sets the assembly arrays.
        /// </summary>
        private SortedList<string, AssemblyArray> assemblyArrays = new SortedList<string, AssemblyArray>();  // typeName, AssemblyArray

        /// <summary>
        /// Gets or sets the NPC corps.
        /// </summary>
        private SortedList<int, string> npcCorps = new SortedList<int, string>(); // corpID, corpName

        /// <summary>
        /// Gets or sets the item flags.
        /// </summary>
        private SortedList<int, string> itemMarkers = new SortedList<int, string>();  // flagID, flagName

        #endregion

        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticData"/> class.
        /// </summary>
        public StaticData()
        {
            // Default constuctor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticData"/> class.
        /// </summary>
        /// <param name="coreCacheFolder">
        /// The full path to the folder containing the EveHQ cache files.
        /// </param>
        public StaticData(string coreCacheFolder)
        {
            // TODO: Constructor should have a parameter containing a reference to the log file so it can be written to
            if (this.LoadCoreCache(coreCacheFolder))
            {
                // Mark data as being available
                this.DataAvailable = true;
            }
        }

        #endregion

        #region "Public Properties"

        /// <summary>
        /// Gets or sets a value indicating whether data is available in the class.
        /// </summary>
        public bool DataAvailable { get; set; }

        /// <summary>
        /// Gets the attribute types.
        /// </summary>
        public SortedList<int, AttributeType> AttributeTypes // attributeID, AttributeType
        {
            get
            {
                return this.attributeTypes;
            }
        }

        /// <summary>
        /// Gets the type attributes.
        /// </summary>
        public Collection<TypeAttrib> TypeAttributes
        {
            get
            {
                return this.typeAttributes;
            }
        }

        /// <summary>
        /// Gets the attribute units.
        /// </summary>
        public SortedList<int, string> AttributeUnits // unitID, DisplayName
        {
            get
            {
                return this.attributeUnits;
            }
        }

        /// <summary>
        /// Gets the effect types.
        /// </summary>
        public SortedList<int, EffectType> EffectTypes // effectID, EffectType
        {
            get
            {
                return this.effectTypes;
            }
        }

        /// <summary>
        /// Gets the type effects.
        /// </summary>
        public Collection<TypeEffect> TypeEffects
        {
            get
            {
                return this.typeEffects;
            }
        }

        /// <summary>
        /// Gets the type names.
        /// </summary>
        public SortedList<string, string> TypeNames // typeName, typeID
        {
            get
            {
                return this.typeNames;
            }
        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        public SortedList<string, EveType> Types // typeID, EveType
        {
            get
            {
                return this.types;
            }
        }

        /// <summary>
        /// Gets the type groups.
        /// </summary>
        public SortedList<int, string> TypeGroups // groupID, groupName
        {
            get
            {
                return this.typeGroups;
            }
        }

        /// <summary>
        /// Gets the type cats.
        /// </summary>
        public SortedList<int, string> TypeCats // catID, catName
        {
            get
            {
                return this.typeCats;
            }
        }

        /// <summary>
        /// Gets the group cats.
        /// </summary>
        public SortedList<int, int> GroupCats  // groupID, catID
        {
            get
            {
                return this.groupCats;
            }
        }

        /// <summary>
        /// Gets the market groups.
        /// </summary>
        public SortedList<int, MarketGroup> MarketGroups // typeID, MarketGroup
        {
            get
            {
                return this.marketGroups;
            }
        }

        /// <summary>
        /// Gets the item market groups.
        /// </summary>
        public SortedList<string, string> ItemMarketGroups  // typeID, marketGroupID
        {
            get
            {
                return this.itemMarketGroups;
            }
        }

        /// <summary>
        /// Gets the meta groups.
        /// </summary>
        public SortedList<int, string> MetaGroups // metaGroupID, metaGroupName
        {
            get
            {
                return this.metaGroups;
            }
        }

        /// <summary>
        /// Gets the meta types.
        /// </summary>
        public SortedList<int, MetaType> MetaTypes  // typeID, MetaItem
        {
            get
            {
                return this.metaTypes;
            }
        }

        /// <summary>
        /// Gets the certificates.
        /// </summary>
        public SortedList<string, Certificate> Certificates
        {
            get
            {
                return this.certificates;
            }
        }

        /// <summary>
        /// Gets the certificate categories.
        /// </summary>
        public SortedList<string, CertificateCategory> CertificateCategories
        {
            get
            {
                return this.certificateCategories;
            }
        }

        /// <summary>
        /// Gets the certificate classes.
        /// </summary>
        public SortedList<string, CertificateClass> CertificateClasses
        {
            get
            {
                return this.certificateClasses;
            }
        }

        /// <summary>
        /// Gets the certificate recommendations.
        /// </summary>
        public Collection<CertificateRecommendation> CertificateRecommendations
        {
            get
            {
                return this.certificateRecommendations;
            }
        }

        /// <summary>
        /// Gets the skill unlocks.
        /// </summary>
        public SortedList<string, Collection<string>> SkillUnlocks
        {
            get
            {
                return this.skillUnlocks;
            }
        }

        /// <summary>
        /// Gets the item unlocks.
        /// </summary>
        public SortedList<string, Collection<string>> ItemUnlocks
        {
            get
            {
                return this.itemUnlocks;
            }
        }

        /// <summary>
        /// Gets the cert unlock certs.
        /// </summary>
        public SortedList<string, Collection<string>> CertUnlockCertificates
        {
            get
            {
                return this.certUnlockCertificates;
            }
        }

        /// <summary>
        /// Gets the cert unlock skills.
        /// </summary>
        public SortedList<string, Collection<string>> CertUnlockSkills
        {
            get
            {
                return this.certUnlockSkills;
            }
        }

        /// <summary>
        /// Gets the regions.
        /// </summary>
        public Dictionary<int, string> Regions
        {
            get
            {
                return this.regions;
            }
        }

        /// <summary>
        /// Gets the constellations.
        /// </summary>
        public Dictionary<int, string> Constellations
        {
            get
            {
                return this.constellations;
            }
        }

        /// <summary>
        /// Gets the solar systems.
        /// </summary>
        public Dictionary<int, SolarSystem> SolarSystems
        {
            get
            {
                return this.solarSystems;
            }
        }

        /// <summary>
        /// Gets the stations.
        /// </summary>
        public Dictionary<int, Station> Stations
        {
            get
            {
                return this.stations;
            }
        }

        /// <summary>
        /// Gets the agents.
        /// </summary>
        public Dictionary<int, Agent> Agents
        {
            get
            {
                return this.agents;
            }
        }

        /// <summary>
        /// Gets the divisions.
        /// </summary>
        public Dictionary<int, string> Divisions
        {
            get
            {
                return this.divisions;
            }
        }

        /// <summary>
        /// Gets the blueprints.
        /// </summary>
        public SortedList<int, Blueprint> Blueprints // typeID, BlueprintType
        {
            get
            {
                return this.blueprints;
            }
        }

        /// <summary>
        /// Gets the assembly arrays.
        /// </summary>
        public SortedList<string, AssemblyArray> AssemblyArrays  // typeName, AssemblyArray
        {
            get
            {
                return this.assemblyArrays;
            }
        }

        /// <summary>
        /// Gets the NPC corps.
        /// </summary>
        public SortedList<int, string> NpcCorps // corpID, corpName
        {
            get
            {
                return this.npcCorps;
            }
        }

        /// <summary>
        /// Gets the item flags.
        /// </summary>
        public SortedList<int, string> ItemMarkers  // flagID, flagName
        {
            get
            {
                return this.itemMarkers;
            }
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// Returns the type ID of a blueprint given a productID
        /// </summary>
        /// <param name="productId">The type ID of the product</param>
        /// <returns>An integer representing the blueprint type ID</returns>
        public int GetBPTypeId(int productId)
        {
            var itemIDs = (from bt in this.Blueprints.Values where bt.ProductId == productId select bt.Id).ToList();

            if (itemIDs.Count > 0)
            {
                return itemIDs[0];
            }

            return -1;
        }

        /// <summary>
        /// Returns the type ID of the product made from the blueprint
        /// </summary>
        /// <param name="blueprintTypeId">The type ID of the blueprint</param>
        /// <returns>An integer representing the typeID of the product</returns>
        public int GetTypeId(int blueprintTypeId)
        {
            if (this.Blueprints.ContainsKey(blueprintTypeId))
            {
                return this.Blueprints[blueprintTypeId].ProductId;
            }

            return -1;
        }

        /// <summary>
        /// Function to return a list of items in a specific database group
        /// </summary>
        /// <param name="groupId">The groupID of the items</param>
        /// <returns>An IEnumerable(Of EveItem) containing the items in the requested group</returns>
        public IEnumerable<EveType> GetItemsInGroup(int groupId)
        {
            return this.Types.Values.Where(item => item.Group == groupId);
        }

        /// <summary>
        /// Function to return a list of groups in a specific category
        /// </summary>
        /// <param name="categoryId">The categoryID of the groups</param>
        /// <returns>An IEnumerable(Of String) containing the IDs of the groups in the requested category</returns>
        public IEnumerable<int> GetGroupsInCategory(int categoryId)
        {
            return this.GroupCats.Keys.Where(groupId => this.GroupCats[groupId] == categoryId);
        }

        /// <summary>
        /// Function to return a sorted list of items with IDs
        /// </summary>
        /// <param name="groupId">The groupID of the items</param>
        /// <returns>A SortedList(Of String, Integer) containing the names and IDs of items in the requested group</returns>
        public SortedList<string, int> GetSortedItemListInGroup(int groupId)
        {
            var items = new SortedList<string, int>();
            foreach (var item in this.GetItemsInGroup(groupId))
            {
                items.Add(item.Name, item.Id);
            }

            return items;
        }

        /// <summary>
        /// Function to return all the attributes of a specific item
        /// </summary>
        /// <param name="typeId">The typeID of the item</param>
        /// <returns>A SortedList(Of Integer, ItemAttributeData) containing the detailed attributes</returns>
        public SortedList<int, ItemAttribData> GetAttributeDataForItem(int typeId)
        {
            // Fetch the attributes for the item
            var atts = (from ta in this.TypeAttributes where ta.TypeId == typeId select new ItemAttrib { Id = ta.AttributeId, Value = ta.Value }).ToList();

            // Prepare the attribute data
            var attributeList = new SortedList<int, ItemAttribData>();

            foreach (var att in atts)
            {
                attributeList.Add(att.Id, new ItemAttribData(att.Id, att.Value, this.AttributeTypes[att.Id].DisplayName, " " + this.AttributeUnits[this.AttributeTypes[att.Id].UnitId]));
            }

            // Process attribute data
            var attributesToAdd = new SortedList<int, ItemAttribData>();

            foreach (var att in attributeList.Values)
            {
                this.CorrectAttributeValue(att);

                // Adjust for skills
                switch (att.Id)
                {
                    case 182:
                    case 183:
                    case 184:
                    case 1285:
                    case 1289:
                    case 1290:
                        var skillLevelAttribute = 0;
                        switch (att.Id)
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
                            attributesToAdd.Add(skillLevelAttribute, new ItemAttribData(skillLevelAttribute, 0, string.Empty, string.Empty));
                            att.DisplayValue = this.Types[Convert.ToString(att.Value, CultureInfo.CurrentCulture)].Name + " (Level " + attributesToAdd[skillLevelAttribute].Value.ToString("N0", CultureInfo.CurrentCulture) + ")";
                        }
                        else
                        {
                            att.DisplayValue = this.Types[Convert.ToString(att.Value, CultureInfo.CurrentCulture)].Name + " (Level " + attributeList[skillLevelAttribute].Value.ToString("N0", CultureInfo.CurrentCulture) + ")";
                        }

                        break;
                }
            }

            // Add in new attributes we need for skill levels
            foreach (var iba in attributesToAdd.Values)
            {
                attributeList.Add(iba.Id, iba);
            }

            return attributeList;
        }

        /// <summary>
        /// Corrects attribute values and display values
        /// </summary>
        /// <param name="att">The instance of the ItemAttributeData class to correct</param>
        public void CorrectAttributeValue(ItemAttribData att)
        {
            if (att == null)
            {
                return;
            }

            // Alter data based on unit ID
            switch (this.AttributeTypes[att.Id].UnitId)
            {
                case 108:
                    att.Value = 100 - (att.Value * 100);
                    att.DisplayValue = Convert.ToString(att.Value, CultureInfo.CurrentCulture);
                    break;
                case 109:
                    att.Value = (att.Value * 100) - 100;
                    att.DisplayValue = Convert.ToString(att.Value, CultureInfo.CurrentCulture);
                    break;
                case 111:
                    att.Value = (att.Value - 1) * 100;
                    att.DisplayValue = Convert.ToString(att.Value, CultureInfo.CurrentCulture);
                    break;
                case 101:
                    att.Value = att.Value / 1000;
                    att.DisplayValue = Convert.ToString(att.Value, CultureInfo.CurrentCulture);
                    break;
                case 115:
                    // groupID
                    att.DisplayValue = this.TypeGroups[Convert.ToInt32(att.Value, CultureInfo.CurrentCulture)];
                    att.Unit = string.Empty;
                    break;
                case 116:
                    // typeID
                    att.DisplayValue = this.Types[Convert.ToString(att.Value, CultureInfo.CurrentCulture)].Name;
                    att.Unit = string.Empty;
                    break;
                case 119:
                case 140:
                    att.DisplayValue = Convert.ToString(att.Value, CultureInfo.CurrentCulture);
                    att.Unit = string.Empty;
                    break;
                default:
                    att.DisplayValue = Convert.ToString(att.Value, CultureInfo.CurrentCulture);
                    break;
            }
        }

        /// <summary>
        /// Returns a list of meta variations of a specific item
        /// </summary>
        /// <param name="typeId">The type ID to get the variations of</param>
        /// <returns>A List(Of Integer) containing the typeIDs of the variations</returns>
        public IEnumerable<int> GetVariationsForItem(int typeId)
        {
            // Fetch the parent item ID for this item
            var parentTypeId = typeId;
            if (this.MetaTypes.ContainsKey(typeId))
            {
                parentTypeId = this.MetaTypes[typeId].ParentId;
            }

            // Fetch all items with this same parent ID
            var itemIDs = (from mt in this.MetaTypes.Values where mt.ParentId == parentTypeId select mt.Id).ToList();

            // Add the current item if it is the parent item
            if (itemIDs.Contains(parentTypeId) == false)
            {
                itemIDs.Add(parentTypeId);
            }

            return itemIDs;
        }

        #endregion

        #region "Private Methods"

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
                if (!Directory.Exists(coreCacheFolder))
                {
                    return false;
                }

                //// HQ.WriteLogEvent(" *** Start of Cache Loading...");

                // Get files from dump

                // Item List
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemList.dat"), FileMode.Open))
                {
                    this.typeNames = Serializer.Deserialize<SortedList<string, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Item List Finished Loading");

                // Item Data
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Items.dat"), FileMode.Open))
                {
                    this.types = Serializer.Deserialize<SortedList<string, EveType>>(s);
                }
                //// HQ.WriteLogEvent(" *** Items Finished Loading");

                // Item Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemGroups.dat"), FileMode.Open))
                {
                    this.typeGroups = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Item Groups Finished Loading");

                // Items Cats
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemCats.dat"), FileMode.Open))
                {
                    this.typeCats = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Item Categories Finished Loading");

                // Group Cats
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "GroupCats.dat"), FileMode.Open))
                {
                    this.groupCats = Serializer.Deserialize<SortedList<int, int>>(s);
                }
                //// HQ.WriteLogEvent(" *** Group Categories Finished Loading");

                // Market Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "MarketGroups.dat"), FileMode.Open))
                {
                    this.marketGroups = Serializer.Deserialize<SortedList<int, MarketGroup>>(s);
                }
                //// HQ.WriteLogEvent(" *** Market Groups Finished Loading");

                // Item Market Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemMarketGroups.dat"), FileMode.Open))
                {
                    this.itemMarketGroups = Serializer.Deserialize<SortedList<string, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Market Groups Finished Loading");

                // Cert Categories
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertCats.dat"), FileMode.Open))
                {
                    this.certificateCategories = Serializer.Deserialize<SortedList<string, CertificateCategory>>(s);
                }
                //// HQ.WriteLogEvent(" *** Certificate Categories Finished Loading");

                // Cert Classes
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertClasses.dat"), FileMode.Open))
                {
                    this.certificateClasses = Serializer.Deserialize<SortedList<string, CertificateClass>>(s);
                }
                //// HQ.WriteLogEvent(" *** Certificate Classes Finished Loading");

                // Certs
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Certs.dat"), FileMode.Open))
                {
                    this.certificates = Serializer.Deserialize<SortedList<string, Certificate>>(s);
                }
                //// HQ.WriteLogEvent(" *** Certificates Finished Loading");

                // Cert Recommendations
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertRec.dat"), FileMode.Open))
                {
                    this.certificateRecommendations = Serializer.Deserialize<Collection<CertificateRecommendation>>(s);
                }
                //// HQ.WriteLogEvent(" *** Certificate Recommendations Finished Loading");

                // Unlocks
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemUnlocks.dat"), FileMode.Open))
                {
                    this.itemUnlocks = Serializer.Deserialize<SortedList<string, Collection<string>>>(s);
                }
                //// HQ.WriteLogEvent(" *** Item Unlocks Finished Loading");

                // SkillUnlocks
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "SkillUnlocks.dat"), FileMode.Open))
                {
                    this.skillUnlocks = Serializer.Deserialize<SortedList<string, Collection<string>>>(s);
                }
                //// HQ.WriteLogEvent(" *** Skill Unlocks Finished Loading");

                // CertCerts
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertCerts.dat"), FileMode.Open))
                {
                    this.certUnlockCertificates = Serializer.Deserialize<SortedList<string, Collection<string>>>(s);
                }
                //// HQ.WriteLogEvent(" *** Certificate Unlocks Finished Loading");

                // CertSkills
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertSkills.dat"), FileMode.Open))
                {
                    this.certUnlockSkills = Serializer.Deserialize<SortedList<string, Collection<string>>>(s);
                }
                //// HQ.WriteLogEvent(" *** Certificate Skills Finished Loading");

                // Regions
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Regions.dat"), FileMode.Open))
                {
                    this.regions = Serializer.Deserialize<Dictionary<int, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Regions Finished Loading");

                // Constellations
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Constellations.dat"), FileMode.Open))
                {
                    this.constellations = Serializer.Deserialize<Dictionary<int, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Constellations Finished Loading");

                // SolarSystems
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Systems.dat"), FileMode.Open))
                {
                    this.solarSystems = Serializer.Deserialize<Dictionary<int, SolarSystem>>(s);
                }
                //// HQ.WriteLogEvent(" *** Solar Systems Finished Loading");

                // Stations
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Stations.dat"), FileMode.Open))
                {
                    this.stations = Serializer.Deserialize<Dictionary<int, Station>>(s);
                }
                //// HQ.WriteLogEvent(" *** Stations Finished Loading");

                // Divisions
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Divisions.dat"), FileMode.Open))
                {
                    this.divisions = Serializer.Deserialize<Dictionary<int, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Divisions Finished Loading");

                // Agents
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Agents.dat"), FileMode.Open))
                {
                    this.agents = Serializer.Deserialize<Dictionary<int, Agent>>(s);
                }
                //// HQ.WriteLogEvent(" *** Agents Finished Loading");

                // Attribute Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "AttributeTypes.dat"), FileMode.Open))
                {
                    this.attributeTypes = Serializer.Deserialize<SortedList<int, AttributeType>>(s);
                }
                //// HQ.WriteLogEvent(" *** Attribute Types Finished Loading");

                // Type Attributes
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "TypeAttributes.dat"), FileMode.Open))
                {
                    this.typeAttributes = Serializer.Deserialize<Collection<TypeAttrib>>(s);
                }
                //// HQ.WriteLogEvent(" *** Type Attributes Finished Loading");

                // Attribute Units
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Units.dat"), FileMode.Open))
                {
                    this.attributeUnits = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Units Finished Loading");

                // Effect Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "EffectTypes.dat"), FileMode.Open))
                {
                    this.effectTypes = Serializer.Deserialize<SortedList<int, EffectType>>(s);
                }
                //// HQ.WriteLogEvent(" *** Effect Types Finished Loading");

                // Type Effects
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "TypeEffects.dat"), FileMode.Open))
                {
                    this.typeEffects = Serializer.Deserialize<Collection<TypeEffect>>(s);
                }
                //// HQ.WriteLogEvent(" *** Type Effects Finished Loading");

                // Meta Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "MetaGroups.dat"), FileMode.Open))
                {
                    this.metaGroups = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Meta Groups Finished Loading");

                // Meta Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "MetaTypes.dat"), FileMode.Open))
                {
                    this.metaTypes = Serializer.Deserialize<SortedList<int, MetaType>>(s);
                }
                //// HQ.WriteLogEvent(" *** Meta Types Finished Loading");

                // Blueprint Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Blueprints.dat"), FileMode.Open))
                {
                    this.blueprints = Serializer.Deserialize<SortedList<int, Blueprint>>(s);
                }
                //// HQ.WriteLogEvent(" *** Blueprints Finished Loading");

                // Assembly Arrays
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "AssemblyArrays.dat"), FileMode.Open))
                {
                    this.assemblyArrays = Serializer.Deserialize<SortedList<string, AssemblyArray>>(s);
                }
                //// HQ.WriteLogEvent(" *** Assembly Arrays Finished Loading");

                // NPC Corps
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "NPCCorps.dat"), FileMode.Open))
                {
                    this.npcCorps = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** NPC Corps Finished Loading");

                // Item Flags
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemFlags.dat"), FileMode.Open))
                {
                    this.itemMarkers = Serializer.Deserialize<SortedList<int, string>>(s);
                }
                //// HQ.WriteLogEvent(" *** Item Flags Finished Loading");

                return true;
            }
            catch (Exception)
            {
                // Load Core Cache failed
                return false;
            }
        }

        #endregion
    }
}
