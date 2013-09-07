using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class EveType
    {
        [ProtoMember(1)]public int ID { get; set; }
        [ProtoMember(2)]public string Name { get; set; }
        [ProtoMember(3)]public string Description { get; set; }
        [ProtoMember(4)]public int Group { get; set; }
        [ProtoMember(5)]public int Category { get; set; }
        [ProtoMember(6)]public int MarketGroupID { get; set; }
        [ProtoMember(7)]public bool Published { get; set; }
        [ProtoMember(8)]public double Mass { get; set; }
        [ProtoMember(9)]public double Capacity { get; set; }
        [ProtoMember(10)]public double Volume { get; set; }
        [ProtoMember(11)]public int MetaLevel { get; set; }
        [ProtoMember(12)]public int PortionSize { get; set; }
        [ProtoMember(13)]public double BasePrice { get; set; }
    }
}