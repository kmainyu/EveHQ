using System;
using System.Collections.Generic;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class AssemblyArray
    {
        [ProtoMember(1)]public string ID;
        [ProtoMember(2)]public string Name;
        [ProtoMember(3)]public double TimeMultiplier;
        [ProtoMember(4)]public double MaterialMultiplier;
        [ProtoMember(5)]public List<int> AllowableGroups = new List<int>();
        [ProtoMember(6)]public List<int> AllowableCategories = new List<int>();

        public AssemblyArray()
        {
            AllowableGroups = new List<int>();
            AllowableCategories = new List<int>();
        }
    }
}