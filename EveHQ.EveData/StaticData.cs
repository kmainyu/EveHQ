// ===========================================================================
// <copyright file="StaticData.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (StaticData.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================
namespace EveHQ.EveData
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using EveHQ.Common.Extensions;

    using ProtoBuf;

    // TODO: using static classes does not promote a good separation of concerns or DI coding pattern. Refactor this into an instance that can be passed around at a later date.

    /// <summary>
    ///     Defines the collection of Eve static data and associated functions.
    /// </summary>
    public static class StaticData
    {
        #region Static Fields

        /// <summary>
        ///     The agents.
        /// </summary>
        private static Dictionary<int, Agent> agents = new Dictionary<int, Agent>();

        /// <summary>
        ///     The assembly arrays.
        /// </summary>
        private static SortedList<string, AssemblyArray> assemblyArrays = new SortedList<string, AssemblyArray>(); // typeName, AssemblyArray

        /// <summary>
        ///     The attribute types.
        /// </summary>
        private static SortedList<int, AttributeType> attributeTypes = new SortedList<int, AttributeType>(); // attributeID, AttributeType

        /// <summary>
        ///     The attribute units.
        /// </summary>
        private static SortedList<int, string> attributeUnits = new SortedList<int, string>(); // unitID, DisplayName

        /// <summary>
        ///     The blueprints.
        /// </summary>
        private static SortedList<int, Blueprint> blueprints = new SortedList<int, Blueprint>(); // typeID, BlueprintType

        /// <summary>
        ///     The cert unlock skills.
        /// </summary>
        private static SortedList<string, List<int>> certUnlockSkills = new SortedList<string, List<int>>();

        /// <summary>
        ///     The certificate categories.
        /// </summary>
        private static SortedList<string, CertificateCategory> certificateCategories = new SortedList<string, CertificateCategory>();

        /// <summary>
        ///     The certificate recommendations.
        /// </summary>
        private static List<CertificateRecommendation> certificateRecommendations = new List<CertificateRecommendation>();

        /// <summary>
        ///     The certificates.
        /// </summary>
        private static SortedList<int, Certificate> certificates = new SortedList<int, Certificate>();

        /// <summary>
        ///     The constellations.
        /// </summary>
        private static Dictionary<int, string> constellations = new Dictionary<int, string>();

        /// <summary>
        ///     The divisions.
        /// </summary>
        private static Dictionary<int, string> divisions = new Dictionary<int, string>();

        /// <summary>
        ///     The effect types.
        /// </summary>
        private static SortedList<int, EffectType> effectTypes = new SortedList<int, EffectType>(); // effectID, EffectType

        /// <summary>
        ///     The group cats.
        /// </summary>
        private static SortedList<int, int> groupCats = new SortedList<int, int>(); // groupID, catID

        /// <summary>
        ///     The item markers.
        /// </summary>
        private static SortedList<int, string> itemMarkers = new SortedList<int, string>(); // flagID, flagName

        /// <summary>
        ///     The item market groups.
        /// </summary>
        private static SortedList<string, string> itemMarketGroups = new SortedList<string, string>(); // typeID, marketGroupID

        /// <summary>
        ///     The item unlocks.
        /// </summary>
        private static SortedList<string, List<string>> itemUnlocks = new SortedList<string, List<string>>();

        /// <summary>
        ///     The market groups.
        /// </summary>
        private static SortedList<int, MarketGroup> marketGroups = new SortedList<int, MarketGroup>(); // typeID, MarketGroup

        /// <summary>
        ///  The item masteries
        /// </summary>
        private static SortedList<int, Mastery> masteries = new SortedList<int, Mastery>(); // typeID, masteryRank,requiredCerts

        /// <summary>
        ///     The meta groups.
        /// </summary>
        private static SortedList<int, string> metaGroups = new SortedList<int, string>(); // metaGroupID, metaGroupName

        /// <summary>
        ///     The meta types.
        /// </summary>
        private static SortedList<int, MetaType> metaTypes = new SortedList<int, MetaType>(); // typeID, MetaItem

        /// <summary>
        ///     The NPC corps.
        /// </summary>
        private static SortedList<int, string> npcCorps = new SortedList<int, string>(); // corpID, corpName

        /// <summary>
        ///     The regions.
        /// </summary>
        private static Dictionary<int, string> regions = new Dictionary<int, string>();

        /// <summary>
        ///     The skill unlocks.
        /// </summary>
        private static SortedList<string, List<string>> skillUnlocks = new SortedList<string, List<string>>();

        /// <summary>
        ///     The solar systems.
        /// </summary>
        private static Dictionary<int, SolarSystem> solarSystems = new Dictionary<int, SolarSystem>();

        /// <summary>
        ///     The stations.
        /// </summary>
        private static Dictionary<int, Station> stations = new Dictionary<int, Station>();

        /// <summary>
        ///     The type attributes.
        /// </summary>
        private static List<TypeAttrib> typeAttributes = new List<TypeAttrib>();

        /// <summary>
        ///     The type cats.
        /// </summary>
        private static SortedList<int, string> typeCats = new SortedList<int, string>(); // catID, catName

        /// <summary>
        ///     The type effects.
        /// </summary>
        private static List<TypeEffect> typeEffects = new List<TypeEffect>();

        /// <summary>
        ///     The type groups.
        /// </summary>
        private static SortedList<int, string> typeGroups = new SortedList<int, string>(); // groupID, groupName

        /// <summary>
        ///     The type names.
        /// </summary>
        private static SortedList<string, int> typeNames = new SortedList<string, int>(); // typeName, typeID

        /// <summary>
        ///     The types.
        /// </summary>
        private static SortedList<int, EveType> types = new SortedList<int, EveType>(); // typeID, EveType

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the agents.
        /// </summary>
        public static Dictionary<int, Agent> Agents
        {
            get
            {
                return agents;
            }
        }

        /// <summary>
        ///     Gets the assembly arrays.
        /// </summary>
        public static SortedList<string, AssemblyArray> AssemblyArrays
        {
            // typeName, AssemblyArray
            get
            {
                return assemblyArrays;
            }
        }

        /// <summary>
        ///     Gets a collection of the available attribute types.
        /// </summary>
        public static SortedList<int, AttributeType> AttributeTypes
        {
            // attributeID, AttributeType
            get
            {
                return attributeTypes;
            }
        }

        /// <summary>
        ///     Gets the attribute units.
        /// </summary>
        public static SortedList<int, string> AttributeUnits
        {
            // unitID, DisplayName
            get
            {
                return attributeUnits;
            }
        }

        /// <summary>
        ///     Gets the blueprints.
        /// </summary>
        public static SortedList<int, Blueprint> Blueprints
        {
            // typeID, BlueprintType
            get
            {
                return blueprints;
            }
        }

        /// <summary>
        ///     Gets the cert unlock skills.
        /// </summary>
        public static SortedList<string, List<int>> CertUnlockSkills
        {
            get
            {
                return certUnlockSkills;
            }
        }

        /// <summary>
        ///     Gets the certificate categories.
        /// </summary>
        public static SortedList<string, CertificateCategory> CertificateCategories
        {
            get
            {
                return certificateCategories;
            }
        }

        /// <summary>
        ///     Gets the certificate recommendations.
        /// </summary>
        public static List<CertificateRecommendation> CertificateRecommendations
        {
            get
            {
                return certificateRecommendations;
            }
        }

        /// <summary>
        ///     Gets the certificates.
        /// </summary>
        public static SortedList<int, Certificate> Certificates
        {
            get
            {
                return certificates;
            }
        }

        /// <summary>
        ///     Gets the constellations.
        /// </summary>
        public static Dictionary<int, string> Constellations
        {
            get
            {
                return constellations;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether data is available in the class.
        /// </summary>
        public static bool DataAvailable { get; set; }

        /// <summary>
        ///     Gets the divisions.
        /// </summary>
        public static Dictionary<int, string> Divisions
        {
            get
            {
                return divisions;
            }
        }

        /// <summary>
        ///     Gets the effect types.
        /// </summary>
        public static SortedList<int, EffectType> EffectTypes
        {
            // effectID, EffectType
            get
            {
                return effectTypes;
            }
        }

        /// <summary>
        ///     Gets the group cats.
        /// </summary>
        public static SortedList<int, int> GroupCats
        {
            // groupID, catID
            get
            {
                return groupCats;
            }
        }

        /// <summary>
        ///     Gets the item flags.
        /// </summary>
        public static SortedList<int, string> ItemMarkers
        {
            // flagID, flagName
            get
            {
                return itemMarkers;
            }
        }

        /// <summary>
        ///     Gets the item market groups.
        /// </summary>
        public static SortedList<string, string> ItemMarketGroups
        {
            // typeID, marketGroupID
            get
            {
                return itemMarketGroups;
            }
        }

        /// <summary>
        ///     Gets the item unlocks.
        /// </summary>
        public static SortedList<string, List<string>> ItemUnlocks
        {
            get
            {
                return itemUnlocks;
            }
        }

        /// <summary>
        ///     Gets the market groups.
        /// </summary>
        public static SortedList<int, MarketGroup> MarketGroups
        {
            // typeID, MarketGroup
            get
            {
                return marketGroups;
            }
        }

        /// <summary>
        /// Gets the masteries.
        /// </summary>
        public static SortedList<int, Mastery> Masteries
        {
            // typeID, rank, requiredCerts
            get
            {
                return masteries;
            }
        }

        /// <summary>
        ///     Gets the meta groups.
        /// </summary>
        public static SortedList<int, string> MetaGroups
        {
            // metaGroupID, metaGroupName
            get
            {
                return metaGroups;
            }
        }

        /// <summary>
        ///     Gets the meta types.
        /// </summary>
        public static SortedList<int, MetaType> MetaTypes
        {
            // typeID, MetaItem
            get
            {
                return metaTypes;
            }
        }

        /// <summary>
        ///     Gets the NPC corps.
        /// </summary>
        public static SortedList<int, string> NpcCorps
        {
            // corpID, corpName
            get
            {
                return npcCorps;
            }
        }

        /// <summary>
        ///     Gets the regions.
        /// </summary>
        public static Dictionary<int, string> Regions
        {
            get
            {
                return regions;
            }
        }

        /// <summary>
        ///     Gets the skill unlocks.
        /// </summary>
        public static SortedList<string, List<string>> SkillUnlocks
        {
            get
            {
                return skillUnlocks;
            }
        }

        /// <summary>
        ///     Gets the solar systems.
        /// </summary>
        public static Dictionary<int, SolarSystem> SolarSystems
        {
            get
            {
                return solarSystems;
            }
        }

        /// <summary>
        ///     Gets the stations.
        /// </summary>
        public static Dictionary<int, Station> Stations
        {
            get
            {
                return stations;
            }
        }

        /// <summary>
        ///     Gets the attributes for each EveType.
        /// </summary>
        public static List<TypeAttrib> TypeAttributes
        {
            get
            {
                return typeAttributes;
            }
        }

        /// <summary>
        ///     Gets the type cats.
        /// </summary>
        public static SortedList<int, string> TypeCats
        {
            // catID, catName
            get
            {
                return typeCats;
            }
        }

        /// <summary>
        ///     Gets the type effects.
        /// </summary>
        public static List<TypeEffect> TypeEffects
        {
            get
            {
                return typeEffects;
            }
        }

        /// <summary>
        ///     Gets the type groups.
        /// </summary>
        public static SortedList<int, string> TypeGroups
        {
            // groupID, groupName
            get
            {
                return typeGroups;
            }
        }

        /// <summary>
        ///     Gets a collection of type names as key, with typeIDs as values.
        /// </summary>
        public static SortedList<string, int> TypeNames
        {
            // typeName, typeID
            get
            {
                return typeNames;
            }
        }

        /// <summary>
        ///     Gets the types.
        /// </summary>
        public static SortedList<int, EveType> Types
        {
            // typeID, EveType
            get
            {
                return types;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Corrects attribute values and display values</summary>
        /// <param name="att">The instance of the ItemAttributeData class to correct</param>
        public static void CorrectAttributeValue(ItemAttribData att)
        {
            if (att == null)
            {
                return;
            }

            // Alter data based on unit ID
            switch (AttributeTypes[att.Id].UnitId)
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
                    att.DisplayValue = TypeGroups[Convert.ToInt32(att.Value, CultureInfo.CurrentCulture)];
                    att.Unit = string.Empty;
                    break;
                case 116:

                    // typeID
                    att.DisplayValue = Types[Convert.ToInt32(att.Value, CultureInfo.CurrentCulture)].Name;
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

        /// <summary>Function to return all the attributes of a specific item</summary>
        /// <param name="typeId">The typeID of the item</param>
        /// <returns>A SortedList(Of Integer, ItemAttributeData) containing the detailed attributes</returns>
        public static SortedList<int, ItemAttribData> GetAttributeDataForItem(int typeId)
        {
            // Fetch the attributes for the item
            var atts = (from ta in TypeAttributes where ta.TypeId == typeId select new ItemAttrib { Id = ta.AttributeId, Value = ta.Value }).ToList();

            // Prepare the attribute data
            var attributeList = new SortedList<int, ItemAttribData>();

            foreach (var att in atts)
            {
                attributeList.Add(att.Id, new ItemAttribData(att.Id, att.Value, AttributeTypes[att.Id].DisplayName, " " + AttributeUnits[AttributeTypes[att.Id].UnitId]));
            }

            // Process attribute data
            var attributesToAdd = new SortedList<int, ItemAttribData>();

            foreach (var att in attributeList.Values)
            {
                CorrectAttributeValue(att);

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
                            att.DisplayValue = Types[Convert.ToInt32(att.Value, CultureInfo.CurrentCulture)].Name + " (Level " + attributesToAdd[skillLevelAttribute].Value.ToString("N0", CultureInfo.CurrentCulture)
                                               + ")";
                        }
                        else
                        {
                            att.DisplayValue = Types[Convert.ToInt32(att.Value, CultureInfo.CurrentCulture)].Name + " (Level " + attributeList[skillLevelAttribute].Value.ToString("N0", CultureInfo.CurrentCulture) + ")";
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

        /// <summary>Returns the type ID of a blueprint given a productID</summary>
        /// <param name="productId">The type ID of the product</param>
        /// <returns>An integer representing the blueprint type ID</returns>
        public static int GetBPTypeId(int productId)
        {
            var itemIDs = (from bt in Blueprints.Values where bt.ProductId == productId select bt.Id).ToList();

            if (itemIDs.Count > 0)
            {
                return itemIDs[0];
            }

            return -1;
        }

        /// <summary>Function to return a list of groups in a specific category</summary>
        /// <param name="categoryId">The categoryID of the groups</param>
        /// <returns>An IEnumerable(Of String) containing the IDs of the groups in the requested category</returns>
        public static IEnumerable<int> GetGroupsInCategory(int categoryId)
        {
            return GroupCats.Keys.Where(groupId => GroupCats[groupId] == categoryId);
        }

        /// <summary>Function to return a list of items in a specific database group</summary>
        /// <param name="groupId">The groupID of the items</param>
        /// <returns>An IEnumerable(Of EveItem) containing the items in the requested group</returns>
        public static IEnumerable<EveType> GetItemsInGroup(int groupId)
        {
            return Types.Values.Where(item => item.Group == groupId);
        }

        /// <summary>Gets the name of an Eve location from the location ID</summary>
        /// <param name="locationId">The location id.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetLocationName(int locationId)
        {
            if (locationId >= 66000000)
            {
                if (locationId < 66014933)
                {
                    locationId = locationId - 6000001;
                }
                else
                {
                    locationId = locationId - 6000000;
                }
            }

            if (Convert.ToDouble(locationId) >= 61000000 & Convert.ToDouble(locationId) <= 61999999)
            {
                if (Stations.ContainsKey(locationId))
                {
                    // Known Outpost
                    return Stations[locationId].StationName;
                }

                // Unknown outpost!
                return "Unknown Outpost";
            }

            if (Convert.ToDouble(locationId) < 60000000)
            {
                if (SolarSystems.ContainsKey(locationId))
                {
                    // Known solar system
                    return SolarSystems[locationId].Name;
                }

                // Unknown solar system
                return "Unknown System";
            }

            if (Stations.ContainsKey(locationId))
            {
                // Known station
                return Stations[locationId].StationName;
            }

            // Unknown station
            return "Unknown Station";
        }

        /// <summary>Function to return a sorted list of items with IDs</summary>
        /// <param name="groupId">The groupID of the items</param>
        /// <returns>A SortedList(Of String, Integer) containing the names and IDs of items in the requested group</returns>
        public static SortedList<string, int> GetSortedItemListInGroup(int groupId)
        {
            var items = new SortedList<string, int>();
            foreach (var item in GetItemsInGroup(groupId))
            {
                items.Add(item.Name, item.Id);
            }

            return items;
        }

        /// <summary>Returns the type ID of the product made from the blueprint</summary>
        /// <param name="blueprintTypeId">The type ID of the blueprint</param>
        /// <returns>An integer representing the typeID of the product</returns>
        public static int GetTypeId(int blueprintTypeId)
        {
            Blueprint item;
            if (Blueprints.TryGetValue(blueprintTypeId, out item))
            {
                return item.ProductId;
            }

            return -1;
        }

        /// <summary>Returns a list of meta variations of a specific item</summary>
        /// <param name="typeId">The type ID to get the variations of</param>
        /// <returns>A List(Of Integer) containing the typeIDs of the variations</returns>
        public static List<int> GetVariationsForItem(int typeId)
        {
            // Fetch the parent item ID for this item
            var parentTypeId = typeId;
            MetaType metaType;
            if (MetaTypes.TryGetValue(typeId, out metaType))
            {
                parentTypeId = metaType.ParentId;
            }

            // Fetch all items with this same parent ID
            var itemIDs = (from mt in MetaTypes.Values where mt.ParentId == parentTypeId select mt.Id).ToList();

            // Add the current item if it is the parent item
            if (itemIDs.Contains(parentTypeId) == false)
            {
                itemIDs.Add(parentTypeId);
            }

            return itemIDs;
        }

        /// <summary>Loads the core cache data from storage.</summary>
        /// <param name="coreCacheFolder">The full path to the folder containing the EveHQ cache files.</param>
        /// <returns>A boolean value indicating whether the load procedure was successful.</returns>
        public static bool LoadCoreCache(string coreCacheFolder)
        {
            try
            {
                // Check for existence of a core cache folder in the application directory
                if (!Directory.Exists(coreCacheFolder))
                {
                    return false;
                }

                Trace.TraceInformation(" *** Start of Cache Loading...");

                // Get files from dump

                // Item List
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemList.dat"), FileMode.Open,FileAccess.Read))
                {
                    typeNames = Serializer.Deserialize<SortedList<string, int>>(s);
                }

                Trace.TraceInformation(" *** Item List Finished Loading");

                // Item Data
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Items.dat"), FileMode.Open, FileAccess.Read))
                {
                    types = Serializer.Deserialize<SortedList<int, EveType>>(s);
                }

                Trace.TraceInformation(" *** Items Finished Loading");

                // Item Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemGroups.dat"), FileMode.Open, FileAccess.Read))
                {
                    typeGroups = Serializer.Deserialize<SortedList<int, string>>(s);
                }

                Trace.TraceInformation(" *** Item Groups Finished Loading");

                // Items Cats
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemCats.dat"), FileMode.Open, FileAccess.Read))
                {
                    typeCats = Serializer.Deserialize<SortedList<int, string>>(s);
                }

                Trace.TraceInformation(" *** Item Categories Finished Loading");

                // Group Cats
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "GroupCats.dat"), FileMode.Open, FileAccess.Read))
                {
                    groupCats = Serializer.Deserialize<SortedList<int, int>>(s);
                }

                Trace.TraceInformation(" *** Group Categories Finished Loading");

                // Market Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "MarketGroups.dat"), FileMode.Open, FileAccess.Read))
                {
                    marketGroups = Serializer.Deserialize<SortedList<int, MarketGroup>>(s);
                }

                Trace.TraceInformation(" *** Market Groups Finished Loading");

                // Item Market Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemMarketGroups.dat"), FileMode.Open, FileAccess.Read))
                {
                    itemMarketGroups = Serializer.Deserialize<SortedList<string, string>>(s);
                }

                Trace.TraceInformation(" *** Market Groups Finished Loading");

                // Cert Categories
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertCats.dat"), FileMode.Open, FileAccess.Read))
                {
                    certificateCategories = Serializer.Deserialize<SortedList<string, CertificateCategory>>(s);
                }

                Trace.TraceInformation(" *** Certificate Categories Finished Loading");

                // Certs
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Certs.dat"), FileMode.Open, FileAccess.Read))
                {
                    certificates = Serializer.Deserialize<SortedList<int, Certificate>>(s);
                }

                Trace.TraceInformation(" *** Certificates Finished Loading");

                // Cert Recommendations
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertRec.dat"), FileMode.Open, FileAccess.Read))
                {
                    certificateRecommendations = Serializer.Deserialize<List<CertificateRecommendation>>(s);
                }

                Trace.TraceInformation(" *** Certificate Recommendations Finished Loading");

                // Unlocks
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemUnlocks.dat"), FileMode.Open, FileAccess.Read))
                {
                    itemUnlocks = Serializer.Deserialize<SortedList<string, List<string>>>(s);
                }

                Trace.TraceInformation(" *** Item Unlocks Finished Loading");

                // SkillUnlocks
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "SkillUnlocks.dat"), FileMode.Open, FileAccess.Read))
                {
                    skillUnlocks = Serializer.Deserialize<SortedList<string, List<string>>>(s);
                }

                Trace.TraceInformation(" *** Skill Unlocks Finished Loading");

                // CertSkills
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "CertSkills.dat"), FileMode.Open, FileAccess.Read))
                {
                    certUnlockSkills = Serializer.Deserialize<SortedList<string, List<int>>>(s);
                }

                Trace.TraceInformation(" *** Certificate Skills Finished Loading");

                // Masteries
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Masteries.dat"), FileMode.Open, FileAccess.Read))
                {
                    masteries = Serializer.Deserialize<SortedList<int, Mastery>>(s);
                }

                Trace.TraceInformation(" *** Masteries Finished Loading");

                // Regions
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Regions.dat"), FileMode.Open, FileAccess.Read))
                {
                    regions = Serializer.Deserialize<Dictionary<int, string>>(s);
                }

                Trace.TraceInformation(" *** Regions Finished Loading");

                // Constellations
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Constellations.dat"), FileMode.Open, FileAccess.Read))
                {
                    constellations = Serializer.Deserialize<Dictionary<int, string>>(s);
                }

                Trace.TraceInformation(" *** Constellations Finished Loading");

                // SolarSystems
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Systems.dat"), FileMode.Open, FileAccess.Read))
                {
                    solarSystems = Serializer.Deserialize<Dictionary<int, SolarSystem>>(s);
                }

                Trace.TraceInformation(" *** Solar Systems Finished Loading");

                // Stations
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Stations.dat"), FileMode.Open, FileAccess.Read))
                {
                    stations = Serializer.Deserialize<Dictionary<int, Station>>(s);
                }

                Trace.TraceInformation(" *** Stations Finished Loading");

                // Divisions
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Divisions.dat"), FileMode.Open, FileAccess.Read))
                {
                    divisions = Serializer.Deserialize<Dictionary<int, string>>(s);
                }

                Trace.TraceInformation(" *** Divisions Finished Loading");

                // Agents
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Agents.dat"), FileMode.Open, FileAccess.Read))
                {
                    agents = Serializer.Deserialize<Dictionary<int, Agent>>(s);
                }

                Trace.TraceInformation(" *** Agents Finished Loading");

                // Attribute Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "AttributeTypes.dat"), FileMode.Open, FileAccess.Read))
                {
                    attributeTypes = Serializer.Deserialize<SortedList<int, AttributeType>>(s);
                }

                Trace.TraceInformation(" *** Attribute Types Finished Loading");

                // Type Attributes
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "TypeAttributes.dat"), FileMode.Open, FileAccess.Read))
                {
                    typeAttributes = Serializer.Deserialize<List<TypeAttrib>>(s);
                }

                Trace.TraceInformation(" *** Type Attributes Finished Loading");

                // Attribute Units
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Units.dat"), FileMode.Open, FileAccess.Read))
                {
                    attributeUnits = Serializer.Deserialize<SortedList<int, string>>(s);
                }

                Trace.TraceInformation(" *** Units Finished Loading");

                // Effect Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "EffectTypes.dat"), FileMode.Open, FileAccess.Read))
                {
                    effectTypes = Serializer.Deserialize<SortedList<int, EffectType>>(s);
                }

                Trace.TraceInformation(" *** Effect Types Finished Loading");

                // Type Effects
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "TypeEffects.dat"), FileMode.Open, FileAccess.Read))
                {
                    typeEffects = Serializer.Deserialize<List<TypeEffect>>(s);
                }

                Trace.TraceInformation(" *** Type Effects Finished Loading");

                // Meta Groups
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "MetaGroups.dat"), FileMode.Open, FileAccess.Read))
                {
                    metaGroups = Serializer.Deserialize<SortedList<int, string>>(s);
                }

                Trace.TraceInformation(" *** Meta Groups Finished Loading");

                // Meta Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "MetaTypes.dat"), FileMode.Open, FileAccess.Read))
                {
                    metaTypes = Serializer.Deserialize<SortedList<int, MetaType>>(s);
                }

                Trace.TraceInformation(" *** Meta Types Finished Loading");

                // Blueprint Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Blueprints.dat"), FileMode.Open, FileAccess.Read))
                {
                    blueprints = Serializer.Deserialize<SortedList<int, Blueprint>>(s);
                }

                Trace.TraceInformation(" *** Blueprints Finished Loading");

                // Assembly Arrays
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "AssemblyArrays.dat"), FileMode.Open, FileAccess.Read))
                {
                    assemblyArrays = Serializer.Deserialize<SortedList<string, AssemblyArray>>(s);
                }

                Trace.TraceInformation(" *** Assembly Arrays Finished Loading");

                // NPC Corps
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "NPCCorps.dat"), FileMode.Open, FileAccess.Read))
                {
                    npcCorps = Serializer.Deserialize<SortedList<int, string>>(s);
                }

                Trace.TraceInformation(" *** NPC Corps Finished Loading");

                // Item Flags
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "ItemFlags.dat"), FileMode.Open, FileAccess.Read))
                {
                    itemMarkers = Serializer.Deserialize<SortedList<int, string>>(s);
                }

                Trace.TraceInformation(" *** Item Flags Finished Loading");
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.FormatException());
                return false;
            }
        }

        /// <summary>Loads the core cache for data from storage for conversion purposes.</summary>
        /// <param name="coreCacheFolder">The full path to the folder containing the EveHQ cache files.</param>
        /// <returns>A boolean value indicating whether the load procedure was successful.</returns>
        public static bool LoadCoreCacheForConversion(string coreCacheFolder)
        {
            try
            {
                // Check for existence of a core cache folder in the application directory
                if (!Directory.Exists(coreCacheFolder))
                {
                    return false;
                }

                // Blueprint Types
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "Blueprints.dat"), FileMode.Open))
                {
                    blueprints = Serializer.Deserialize<SortedList<int, Blueprint>>(s);
                }

                // Assembly Arrays
                using (var s = new FileStream(Path.Combine(coreCacheFolder, "AssemblyArrays.dat"), FileMode.Open))
                {
                    assemblyArrays = Serializer.Deserialize<SortedList<string, AssemblyArray>>(s);
                }

                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.FormatException());
                return false;
            }
        }

        #endregion
    }
}