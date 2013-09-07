using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class CertificateCategory
    {
        [ProtoMember(1)]public int ID { get; set; }
        [ProtoMember(2)]public string Name { get; set; }
    }
}