using System;
using ProtoBuf;

namespace EveHQ.EveData
{
    [ProtoContract][Serializable]public class Agent
    {
        [ProtoMember(1)]public int AgentID { get; set; }
        [ProtoMember(2)]public string AgentName { get; set; }
        [ProtoMember(3)]public int DivisionID { get; set; }
        [ProtoMember(5)]public int CorporationID { get; set; }
        [ProtoMember(6)]public int LocationID { get; set; }
        [ProtoMember(7)]public int Level { get; set; }
        [ProtoMember(8)]public int AgentType { get; set; }
        [ProtoMember(9)]public bool IsLocator { get; set; }
    }
}