using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class TypeAttribute
    {
        [ProtoMember(1)]public int TypeID { get; set; }
        [ProtoMember(2)]public int AttributeID { get; set; }
        [ProtoMember(3)]public double Value { get; set; }
    }
}