using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveHQ.EveApi
{
    public sealed class Research
    {
        public int AgentId { get; set; }
        public int SkillTypeId { get; set; }
        public DateTimeOffset ResearchStartDate { get; set; }
        public double PointsPerDay { get; set; }
        public double RemainingPoints { get; set; }
    }
}
