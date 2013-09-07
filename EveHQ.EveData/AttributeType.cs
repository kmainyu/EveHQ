using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class AttributeType
    {
        [ProtoMember(1)]public int AttributeID { get; set; }
        [ProtoMember(2)]public string AttributeName { get; set; }
        [ProtoMember(3)]public string DisplayName { get; set; }
        [ProtoMember(4)]public int UnitID { get; set; }
        [ProtoMember(5)]public int CategoryID { get; set; }
    }
}