using System;
using System.Collections.Generic;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class Blueprint
    {
        [ProtoMember(1)]public int ID { get; set; }
        [ProtoMember(2)]public long AssetID { get; set; }
        [ProtoMember(3)]public int ProductID { get; set; }
        [ProtoMember(4)]public int TechLevel { get; set; }
        [ProtoMember(5)]public int WasteFactor { get; set; }
        [ProtoMember(6)]public int MaterialModifier { get; set; }
        [ProtoMember(7)]public int ProductivityModifier { get; set; }
        [ProtoMember(8)]public int MaxProductionLimit { get; set; }
        [ProtoMember(9)]public long ProductionTime { get; set; }
        [ProtoMember(10)]public long ResearchMaterialLevelTime { get; set; }
        [ProtoMember(11)]public long ResearchProductionLevelTime { get; set; }
        [ProtoMember(12)]public long ResearchCopyTime { get; set; }
        [ProtoMember(13)]public long ResearchTechTime { get; set; }
        [ProtoMember(14)]public Dictionary<int, Dictionary<int, BlueprintResource>> Resources { get; set; }
        [ProtoMember(15)]public List<int> Inventions { get; set; }
        [ProtoMember(16)]public List<int> InventionMetaItems { get; set; }
        [ProtoMember(17)]public List<int> InventFrom { get; set; }
    }
}