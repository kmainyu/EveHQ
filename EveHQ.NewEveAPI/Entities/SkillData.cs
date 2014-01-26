using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveHQ.EveApi
{
    public class SkillData
    {
        public int GroupId { get; set; }

        public bool Published { get; set; }

        public int TypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Rank { get; set; }

        public IEnumerable<ReqSkillData> RequiredSkills { get; set; }

        public string PrimaryAttribute { get; set; }

        public string SecondaryAttribute { get; set; }

        public bool CannotBeTrainedOnTrial { get; set; }
    }

    public class ReqSkillData
    {
        public int Level { get; set; }
        public int TypeId { get; set; }
    }


    public class SkillGroup
    {
        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<SkillData> Skills { get; set; }
    }
}
