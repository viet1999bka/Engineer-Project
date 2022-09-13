using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class Challenge
    {
        public Challenge()
        {
            Exercise = new HashSet<Exercise>();
        }

        public int ChallengeId { get; set; }
        public string ChallengeName { get; set; }
        public string Description { get; set; }
        public string Thumb { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Exercise> Exercise { get; set; }
    }
}
