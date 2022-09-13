using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class Language
    {
        public Language()
        {
            ExerciseAttemp = new HashSet<ExerciseAttemp>();
            LanguageSupport = new HashSet<LanguageSupport>();
        }

        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LạnguageDisplay { get; set; }
        public string LanguageExten { get; set; }
        public string LanguageMode { get; set; }
        public string Version { get; set; }

        public virtual ICollection<ExerciseAttemp> ExerciseAttemp { get; set; }
        public virtual ICollection<LanguageSupport> LanguageSupport { get; set; }
    }
}
