using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public enum CertificateGrades
    {
        Basic = 1,
        Standard = 2,
        Improved = 3,
        Advanced = 4,
        Elite = 5
    }
}