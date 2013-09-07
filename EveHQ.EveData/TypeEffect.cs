using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class TypeEffect
    {
        [ProtoMember(1)]public int TypeID { get; set; }
        [ProtoMember(2)]public int EffectID { get; set; }
    }
}