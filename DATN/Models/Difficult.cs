using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class Difficult
    {
        public Difficult()
        {
            Exercise = new HashSet<Exercise>();
        }

        public int DifficultId { get; set; }
        public string DifficultName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Exercise> Exercise { get; set; }
    }
}
