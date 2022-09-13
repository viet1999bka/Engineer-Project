using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class ExerciseAttemp
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ExerciseId { get; set; }
        public int? LanguageId { get; set; }
        public int? Status { get; set; }
        public string CodeSubmit { get; set; }
        public DateTime? TimeAttemp { get; set; }

        public virtual Exercise Exercise { get; set; }
        public virtual Language Language { get; set; }
        public virtual Account User { get; set; }
    }
}
