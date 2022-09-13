using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class Exercise
    {
        public Exercise()
        {
            ExerciseAttemp = new HashSet<ExerciseAttemp>();
            LanguageSupport = new HashSet<LanguageSupport>();
            TestCase = new HashSet<TestCase>();
        }

        public int ExcerciseId { get; set; }
        public string ExcerciseName { get; set; }
        public int? LevelId { get; set; }
        public int? ChallengeId { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public string TimeLimit { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Challenge Challenge { get; set; }
        public virtual Difficult Level { get; set; }
        public virtual ICollection<ExerciseAttemp> ExerciseAttemp { get; set; }
        public virtual ICollection<LanguageSupport> LanguageSupport { get; set; }
        public virtual ICollection<TestCase> TestCase { get; set; }
    }
}
