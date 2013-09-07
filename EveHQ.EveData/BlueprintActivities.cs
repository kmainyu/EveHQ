using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public enum BlueprintActivities
    {
        Manufacturing = 1,
        ResearchTech = 2,
        ResearchProductionLevel = 3,
        ResearchMaterialLevel = 4,
        Copying = 5,
        Recycling = 6,
        ReverseEng = 7,
        Invention = 8
    }
}