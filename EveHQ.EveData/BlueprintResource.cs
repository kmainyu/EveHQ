using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class BlueprintResource
    {
        [ProtoMember(1)]public int TypeID;
        [ProtoMember(2)]public int TypeGroup;
        [ProtoMember(3)]public int TypeCategory;
        [ProtoMember(4)]public int Activity;
        [ProtoMember(5)]public int Quantity;
        [ProtoMember(6)]public double DamagePerJob;
        [ProtoMember(7)]public int BaseMaterial;
    }
}