using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class Station
    {
        [ProtoMember(1)]public int StationID { get; set; }
        [ProtoMember(2)]public string StationName { get; set; }
        [ProtoMember(3)]public int SystemID { get; set; }
        [ProtoMember(4)]public int CorpID { get; set; }
        [ProtoMember(5)]public double RefiningEff { get; set; }
        [ProtoMember(6)]public double StationTake { get; set; }
        [ProtoMember(7)]public int Services { get; set; }
    }
}