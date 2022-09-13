using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class Experience
    {
        public int ExperienceId { get; set; }
        public int UserId { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string JobLocation { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Account User { get; set; }
    }
}
