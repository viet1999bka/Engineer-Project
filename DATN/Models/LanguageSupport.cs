// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class LanguageSupport
    {
        public int LanguageSupId { get; set; }
        public int? ExerciseId { get; set; }
        public int? LanguageId { get; set; }
        public string FileMain { get; set; }
        public string FileFunction { get; set; }
        public string Description { get; set; }

        public virtual Exercise Exercise { get; set; }
        public virtual Language Language { get; set; }
    }
}
