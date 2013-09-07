using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class EffectType
    {
        [ProtoMember(1)]public int EffectID { get; set; }
        [ProtoMember(2)]public string EffectName { get; set; }
    }
}