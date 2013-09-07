using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class CertificateRecommendation
    {
        [ProtoMember(1)]public int ShipTypeID { get; set; }
        [ProtoMember(2)]public int CertificateID { get; set; }
    }
}