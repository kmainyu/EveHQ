using System;
using System.Collections.Generic;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class SolarSystem
    {
        [ProtoMember(1)]public int ID { get; set; }
        [ProtoMember(2)]public string Name { get; set; }
        [ProtoMember(3)]public double Security { get; set; }
        [ProtoMember(4)]public int ConstellationID { get; set; }
        [ProtoMember(5)]public int RegionID { get; set; }
        [ProtoMember(6)]public int PlanetCount { get; set; }
        [ProtoMember(7)]public int MoonCount { get; set; }
        [ProtoMember(8)]public int StationCount { get; set; }
        [ProtoMember(9)]public int OreBeltCount { get; set; }
        [ProtoMember(10)]public int IceBeltCount { get; set; }
        [ProtoMember(11)]public List<int> Gates { get; set; }
        [ProtoMember(12)]public double X { get; set; }
        [ProtoMember(13)]public double Y { get; set; }
        [ProtoMember(14)]public double Z { get; set; }
    }
}