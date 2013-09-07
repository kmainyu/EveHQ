using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace EveHQ.EveData
{
    class StaticData
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

    }
}
