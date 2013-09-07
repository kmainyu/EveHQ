using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class MetaType
    {
        [ProtoMember(1)]public int ID { get; set; }
        [ProtoMember(2)]public int ParentID { get; set; }
        [ProtoMember(3)]public int MetaGroupID { get; set; }
    }
}