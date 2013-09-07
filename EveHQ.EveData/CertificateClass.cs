using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class CertificateClass
    {
        [ProtoMember(1)]public int ID { get; set; }
        [ProtoMember(2)]public string Name { get; set; }
    }
}