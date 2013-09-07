using System;
using System.Collections.Generic;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class Certificate
    {
        [ProtoMember(1)]public int ID { get; set; }
        [ProtoMember(2)]public int Grade { get; set; }
        [ProtoMember(3)]public int ClassID { get; set; }
        [ProtoMember(4)]public int CategoryID { get; set; }
        [ProtoMember(5)]public long CorpID { get; set; }
        [ProtoMember(6)]public string Description { get; set; }
        [ProtoMember(7)]public SortedList<string, int> RequiredSkills { get; set; }
        [ProtoMember(8)]public SortedList<string, int> RequiredCerts { get; set; }
    }
}